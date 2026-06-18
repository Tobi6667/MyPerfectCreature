using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.AI;

namespace Game.Body
{

    public class HopComponent : ComponentBase
    {

        [Header("Movement")]
        [SerializeField] private float _hopHeight = 0.6f;
        [SerializeField] private float _hopDuration = 0.35f;
        [SerializeField] private float _moveRadius = 1.5f;

        [Header("NavMesh")]
        [SerializeField] private float _navMeshSearchRadius = 2f;
        [SerializeField] private int _maxNavMeshAttempts = 5;
        [Header("Target Steering")]
        [SerializeField] private Transform _moveTarget;

        [Range(0f, 1f)]
        [SerializeField] private float _targetBias = 0.6f;

        [Header("Grounding")]
        [SerializeField] private float _rayHeight = 2f;
        [SerializeField] private float _rayDistance = 5f;
        [SerializeField] private LayerMask _groundMask;

        [SerializeField] private float _arriveDistance = 1.2f;

        private Action _onReachedTarget;
        private bool _isMovingToTarget;

        [Header("Squash")]
        [SerializeField] private float _squashAmount = 1.01f;
        [SerializeField] private float _squashDuration = 0.08f;

        [Header("Pause")]
        [SerializeField] private float _minPause = 0.05f;
        [SerializeField] private float _maxPause = 0.4f;

        [Header("Rotation")]
        [SerializeField] private float _rotationSpeed = 8f;

        private Quaternion _originalRotation;
        private Vector3 _safeTransformHelper;
        private Vector3 baseScale;
        private DG.Tweening.Sequence _currentSequence;
        private Tween _delayedCall;
        private UnityEngine.AI.NavMeshAgent _agent;
        private bool _isHopping = false;

        public override void Initialize()
        {
            _agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
            _isHopping = true;
            HopLoop();
            baseScale = transform.localScale;
        }


        internal void HopLoop()
        {
            _currentSequence?.Kill();
            if (!_isHopping) return;
            Vector3 start = transform.position;
            Vector3 target = start;
            bool foundValidPosition = false;

            for (int i = 0; i < _maxNavMeshAttempts; i++)
            {
                Vector3 randomOffset = new Vector3(
                    UnityEngine.Random.Range(-_moveRadius, _moveRadius),
                    0,
                    UnityEngine.Random.Range(-_moveRadius, _moveRadius)
                );

                Vector3 toTarget = Vector3.zero;

                if (_moveTarget != null)
                {
                    toTarget = (_moveTarget.position - start);
                    toTarget.y = 0;
                    toTarget = toTarget.normalized * _moveRadius;
                }

                Vector3 desiredPos = start + Vector3.Lerp(randomOffset, toTarget, _targetBias);

                if (UnityEngine.AI.NavMesh.SamplePosition(desiredPos, out UnityEngine.AI.NavMeshHit navHit, _navMeshSearchRadius, UnityEngine.AI.NavMesh.AllAreas))
                {
                    Vector3 navPos = navHit.position;
                    Ray ray = new Ray(navPos + Vector3.up * _rayHeight, Vector3.down);
                    float groundY = navPos.y;

                    if (Physics.Raycast(ray, out RaycastHit hit, _rayDistance, _groundMask))
                        groundY = hit.point.y;

                    target = new Vector3(navPos.x, groundY, navPos.z);
                    foundValidPosition = true;
                    break;
                }
            }

            if (!foundValidPosition)
            {
                _delayedCall = DOVirtual.DelayedCall(0.2f, HopLoop);
                return;
            }

            Vector3 hopDir = target - start;
            hopDir.y = 0f;

            if (hopDir.sqrMagnitude > 0.001f)
            {
                Quaternion targetRot = Quaternion.LookRotation(hopDir.normalized, Vector3.up);
                transform.DORotateQuaternion(targetRot, _hopDuration * 0.5f).SetEase(Ease.OutQuad);
            }

            SetAgentEnabled(false);

            _currentSequence = DOTween.Sequence();
            _currentSequence.Append(
                DOTween.To(
                    () => 0f,
                    t =>
                    {
                        float x = Mathf.Lerp(start.x, target.x, t);
                        float z = Mathf.Lerp(start.z, target.z, t);
                        float baseY = Mathf.Lerp(start.y, target.y, t);
                        float arc = Mathf.Sin(t * Mathf.PI) * _hopHeight;
                        transform.position = new Vector3(x, baseY + arc, z);
                    },
                    1f,
                    _hopDuration
                ).SetEase(Ease.Linear)
            );

            _currentSequence.OnComplete(() =>
            {
                transform.position = target;
                SetAgentEnabled(true);

                if (_isMovingToTarget && _moveTarget != null)
                {
                    Vector3 flatCurrent = transform.position;
                    Vector3 flatTarget = _moveTarget.position;
                    flatCurrent.y = 0;
                    flatTarget.y = 0;

                    if (Vector3.Distance(flatCurrent, flatTarget) <= _arriveDistance)
                    {
                        _isMovingToTarget = false;
                        StopHopping();
                        _onReachedTarget?.Invoke();
                        return;
                    }
                }

                transform.DOScale(
                    new Vector3(baseScale.x * _squashAmount, baseScale.y * (1f / _squashAmount), baseScale.z * _squashAmount),
                    _squashDuration
                ).SetLoops(2, LoopType.Yoyo);

                float delay = UnityEngine.Random.Range(_minPause, _maxPause);
                _delayedCall = DOVirtual.DelayedCall(delay, HopLoop);
            });
        }

        internal void MoveToTarget(Transform target, Action onReached, float dist = 0f)
        {
            _moveTarget = target;
            _targetBias = 1f;
            _arriveDistance = dist;
            _onReachedTarget = onReached;
            _isMovingToTarget = true;
        }

        internal void StopHopping()
        {
            _currentSequence?.Kill();
            _delayedCall?.Kill();
            _isMovingToTarget = false;
        }

        internal void HopOnObject(Vector3 hopPoint, Action onReached)
        {
            StopHopping();
            SetAgentEnabled(false);

            _originalRotation = transform.rotation;
            _safeTransformHelper = transform.position;

            Vector3 start = transform.position;
            Vector3 dir = hopPoint - start;
            dir.y = 0f;

            if (dir.sqrMagnitude > 0.001f)
            {
                transform.DORotateQuaternion(
                    Quaternion.LookRotation(dir.normalized, Vector3.up), 0.15f
                ).SetEase(Ease.OutQuad);
            }

            DOTween.To(
                () => 0f,
                t =>
                {
                    float x = Mathf.Lerp(start.x, hopPoint.x, t);
                    float z = Mathf.Lerp(start.z, hopPoint.z, t);
                    float baseY = Mathf.Lerp(start.y, hopPoint.y, t);
                    float arc = Mathf.Sin(t * Mathf.PI) * (_hopHeight + 0.7f);
                    transform.position = new Vector3(x, baseY + arc, z);
                },
                1f,
                _hopDuration * 2f
            )
            .SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                transform.position = hopPoint;
                onReached?.Invoke();
            });
        }

        // Now takes an optional callback — fires AFTER landing, before ContinueHopping
        internal void HopOffObject(Action onLanded = null)
        {
            StopHopping();

            // Find nearest valid NavMesh point from current position
            Vector3 landPos = transform.position;

            if (UnityEngine.AI.NavMesh.SamplePosition(transform.position, out UnityEngine.AI.NavMeshHit hit, 5f, UnityEngine.AI.NavMesh.AllAreas))
                landPos = hit.position;

            Vector3 dir = landPos - transform.position;
            dir.y = 0f;

            Sequence s = DOTween.Sequence();

            if (dir.sqrMagnitude > 0.01f)
                s.Append(transform.DOMove(landPos, 0.4f).SetEase(Ease.OutQuad));


            s.AppendCallback(() =>
            {
                transform.position = landPos;
                SetAgentEnabled(true);
                onLanded?.Invoke();
            });
        }


        private void SetAgentEnabled(bool on)
        {
            if (_agent == null) _agent = GetComponent<NavMeshAgent>();
            if (_agent == null) return;

            if (on)
            {
                _agent.Warp(transform.position);
                _agent.enabled = true;
            }
            else
            {
                _agent.enabled = false;
            }
        }


    }
}
