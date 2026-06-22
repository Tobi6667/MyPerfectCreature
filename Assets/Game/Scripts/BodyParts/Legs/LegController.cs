using DG.Tweening;
using Game.Minigames;
using System;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace Game.Body
{
    [RequireComponent(typeof(HopComponent))]
    public class LegController : BodyPartBase, IInteractable
    {


        private BodypartMusclesComponent _musclesComp;
        [SerializeField] private CinemachineCamera _followCam;


        public override void Initialize()
        {
            _hopComponent = GetComponent<HopComponent>();
            _musclesComp = GetComponent<BodypartMusclesComponent>();
            _musclesComp.InitializeModule();
            _hopComponent.Initialize();
        }

        [SerializeField] private TwoBoneIKConstraint _ikLeg;
        [SerializeField] private TwoBoneIKConstraint _gameikLeg;

        [Header("Failure")]
        [SerializeField] private float failDistance = 1.5f;

        [Header("Physics Feel")]
        [SerializeField] private float gravity = 8f;
        [SerializeField] private float stiffness = 22f;
        [SerializeField] private float damping = 7f;
        [SerializeField] private float maxOffset = 1.5f;
        [SerializeField] private float controlResponsiveness = 1f;
        public event Action FellOver;
        private Vector3 _idleOffset;
        private Vector3 _inputSmoothed;
        private bool _movetoLabObject = false;
        [Header("References")]
        [SerializeField] private Transform hip;
        [SerializeField] private Transform foot;

        private Vector3 _startPosition;
        private Quaternion _startRotation;
        private Vector2 _idleInput;
        [SerializeField] private Rigidbody _rb;
        [SerializeField] private Collider _collider;

        private bool _ikChanged = false;
        private bool _hasFallen;
        [SerializeField] private float randomPushForce = 2f;
        [SerializeField] private float randomPushIntervalMin = 0.4f;
        [SerializeField] private float randomPushIntervalMax = 1.2f;

        private float _nextRandomPushTime;
        private Vector3 _velocity;
        [SerializeField] private bool _active;
        private bool _activeGame = false;

        private Vector3 _injuryJitterOffset;
        private float _injuryDelayTimer;
        private Vector3 _delayedDesiredPos;

        private LegInjuryInstance _activeInjury;





        internal void MoveLegRoot(Vector3 screenPos)
        {
            //Debug.Log(screenPos);
            if (!_active) return;
            //Debug.Log(screenPos);
            Camera cam = Camera.main;
            if (cam == null) return;

            // 1. Screen → world
            Ray ray = cam.ScreenPointToRay(screenPos);

            Vector3 inputWorld = foot.position;

            if (Physics.Raycast(ray, out RaycastHit hit, 100f))
                inputWorld = hit.point;

            // 2. Base offset
            Vector3 footPos = foot.position;

            Vector3 offset = inputWorld - footPos;
            offset.y = 0f;
            offset = Vector3.ClampMagnitude(offset, maxOffset);

            Vector3 desiredPos = footPos + offset;

            // 3. Injury modifiers
            if (_activeInjury != null)
            {
                if (_activeInjury.range < 1f)
                {
                    float clampedMax = Mathf.Lerp(maxOffset, maxOffset * 0.1f, 1f - _activeInjury.range);
                    offset = Vector3.ClampMagnitude(offset, clampedMax);
                    desiredPos = footPos + offset;
                }

                if (_activeInjury.delay > 0f)
                {
                    float lerpSpeed = Mathf.Lerp(25f, 1f, _activeInjury.delay);
                    _delayedDesiredPos = Vector3.Lerp(_delayedDesiredPos, desiredPos, Time.deltaTime * lerpSpeed);
                    desiredPos = _delayedDesiredPos;
                }
            }

            // 4. Physics force
            Vector3 toTarget = desiredPos - hip.position;

            float effectiveStiffness =
                _activeInjury != null
                    ? Mathf.Lerp(stiffness, stiffness * 0.1f, _activeInjury.stiffness)
                    : stiffness;

            float effectiveDamping =
                _activeInjury != null
                    ? Mathf.Lerp(damping, damping * 0.3f, _activeInjury.stiffness)
                    : damping;

            Vector3 gravityForce = Vector3.down * gravity;

            _velocity += (toTarget * effectiveStiffness + gravityForce) * Time.deltaTime;
            _velocity *= Mathf.Exp(-effectiveDamping * Time.deltaTime);

            ApplyRandomPush();

            // 5. Jitter
            Vector3 jitter = Vector3.zero;

            if (_activeInjury != null && _activeInjury.jitter > 0f)
            {
                float t = Time.time * 10f;

                jitter = new Vector3(
                    Mathf.PerlinNoise(t, 0f) - 0.5f,
                    0f,
                    Mathf.PerlinNoise(0f, t) - 0.5f
                ) * _activeInjury.jitter * 0.15f;
            }

            hip.position += (_velocity * Time.deltaTime) + jitter;

            CheckFailure();
        }

        private void Update()
        {
            if (!_active)
                return;

            float t = Time.time * 2f;

            float injuryFactor = _activeInjury != null
                ? Mathf.Clamp01(
                    (_activeInjury.stiffness +
                     _activeInjury.jitter +
                     (1f - _activeInjury.range)) * 0.33f
                  )
                : 0.2f;

            Vector3 targetIdle = new Vector3(
                Mathf.PerlinNoise(t, 0.1f) - 0.5f,
                0f,
                Mathf.PerlinNoise(0.1f, t) - 0.5f
            ) * 0.15f * injuryFactor;

            _idleOffset = Vector3.Lerp(_idleOffset, targetIdle, Time.deltaTime * 5f);

            // idle APPLY (IMPORTANT FIX: must actually influence pose)
            hip.position += _idleOffset;
        }

        private void CheckFailure()
        {
            if (!_activeGame) return;
            if (_hasFallen) return;

            float dist = Vector3.Distance(hip.position, foot.position);

            if (dist > failDistance)
            {
                _hasFallen = true;
                FellOver?.Invoke();
                _active = false;
            }
        }

        internal void SetIdleInput(Vector2 input)
        {
            _idleInput = input;
        }


        internal void ChangeIK()
        {
            if (_ikChanged)
            {
                _ikLeg.weight = 1f;
                _gameikLeg.weight = 0f;
                _ikChanged = false;
            }
            else
            {
                _ikChanged = true;
                _ikLeg.weight = 0f;
                _gameikLeg.weight = 1f;
                _startPosition = transform.position;
                _startRotation = transform.rotation;
            }
        }


        internal void SetRoundData(LegGameRoundData data)
        {
            gravity = data.gravity;
            stiffness = data.stiffness;
            damping = data.damping;
            maxOffset = data.maxOffset;
            controlResponsiveness = data.controlResponsiveness;
            randomPushForce = data.randomPushForce;
            randomPushIntervalMin = data.randomPushIntervalMin;
            randomPushIntervalMax = data.randomPushIntervalMax;
        }

        internal void ResetLeg(Action onReset)
        {
            _activeGame = false;
            _hasFallen = false;

            _rb.linearVelocity = Vector3.zero;
            _rb.angularVelocity = Vector3.zero;

            _rb.isKinematic = true;
            _rb.useGravity = false;

            //_collider.isTrigger = true;


            _musclesComp.HideMuscles(() =>
            {




                DG.Tweening.Sequence seq = DOTween.Sequence();

                seq.Join(hip.DOLocalMove(Vector3.zero, 2f));

                seq.Join(
                    transform.DOLocalRotateQuaternion(
                        Quaternion.identity,
                        2f
                    ).SetEase(Ease.InOutSine)
                );

                seq.OnComplete(() =>
                {
                    transform.localPosition = _startPosition;
                    transform.localRotation = _startRotation;



                    _velocity = Vector3.zero;
                    onReset?.Invoke();
                    Activate();
                });
            });

        }

        internal void Activate()
        {
            _startPosition = transform.position;
            _startRotation = transform.rotation;

            _hasFallen = false;
            _activeGame = true;
            _active = true;
            _ikLeg.weight = 0;
            _gameikLeg.weight = 1;

        }

        internal void Deactivate()
        {
            _activeGame = false;
            _active = false;
            _ikLeg.weight = 1;
            _gameikLeg.weight = 0;
        }

        private void ApplyRandomPush()
        {
            if (!_active) return;

            if (Time.time < _nextRandomPushTime) return;

            _nextRandomPushTime = Time.time + UnityEngine.Random.Range(
                randomPushIntervalMin,
                randomPushIntervalMax
            );

            Vector3 randomDir = new Vector3(
                UnityEngine.Random.Range(-1f, 1f),
                0f,
                UnityEngine.Random.Range(-1f, 1f)
            ).normalized;

            _velocity += randomDir * randomPushForce;
        }

        public override void MoveToObject(Transform target, Action onReached, float speed = 4, float arriveDistance = 0)
        {
            throw new NotImplementedException();
        }

        public void OnInteract()
        {
            throw new NotImplementedException();
        }

        public override CinemachineCamera GetTransitionCam()
        {
            return _followCam;
        }

        public override void OnInject(IInjuryData injury)
        {

            var inj = injury as LegInjuryInstance;
            _musclesComp.ShowMuscles(inj.affectedMuscles);
        }
    }
}