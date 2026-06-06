using UnityEngine;

public class TorsoBangComponent : MonoBehaviour
{




    [Header("Animators")]
    [SerializeField] private Animator _animatorOne;
    [SerializeField] private Animator _animatorTwo;
    [Header("Spine")]
    [SerializeField] private Transform[] spineBones;
    //private TorsoInjuryData _currentInjury;
    [Header("Bang")]
    [SerializeField] private float impactFreezeTime = 0.25f;
    [SerializeField] private float _bangCooldownTime = 0.3f;
    private float _bangCooldown;


    private Quaternion[] _rest;
    private Quaternion[] _frozenPose;

    private bool _idle;
    private bool _isImpactLocked;

    private float _impactRecover = 0f;
    [SerializeField] private float _bangTarget;
    private float _bangCurrent;
    private float _bangAmount = 5f;

    [SerializeField] private float _impactRecoverSpeed = 6f;

    private void Start()
    {
        InitializeModule();
    }

    internal void InitializeModule()
    {
        _rest = new Quaternion[spineBones.Length];
        _frozenPose = new Quaternion[spineBones.Length];

        for (int i = 0; i < spineBones.Length; i++)
            _rest[i] = spineBones[i].localRotation;

        //headColliderModule.InitializeModule(this);
    }

    private void LateUpdate()
    {
        //if (!_idle) return;

        if (_bangCooldown > 0f)
            _bangCooldown -= Time.deltaTime;

        // ── Impact freeze: hold the hit pose then slerp back to rest ─────────
        if (_isImpactLocked)
        {
            _impactRecover += Time.deltaTime * _impactRecoverSpeed;

            for (int i = 0; i < spineBones.Length; i++)
            {
                spineBones[i].localRotation = Quaternion.Slerp(
                    _frozenPose[i],
                    _rest[i],
                    _impactRecover
                );
            }

            if (_impactRecover >= 1f)
                _isImpactLocked = false;

            return;
        }


        _bangCurrent = Mathf.Lerp(_bangCurrent, _bangTarget, Time.deltaTime * 10f);

        int last = Mathf.Max(spineBones.Length - 1, 1);

        for (int i = 0; i < spineBones.Length; i++)
        {
            float t = (float)i / last;  // 0 = root, 1 = tip


            float sway = Mathf.Sin(Time.time * 1.0f + i) * 6f * t;
            float twist = Mathf.Sin(Time.time * 0.7f + i) * 10f * t;
            float breathe = Mathf.Sin(Time.time * 1.2f) * 3f * t;

            Quaternion idleRot = Quaternion.Euler(breathe, twist, sway);


            Quaternion injuryRot = Quaternion.identity;
            float bangMask = 1f;     

            /*

            if (_currentInjury != null)
            {
                injuryRot = Quaternion.Euler(_currentInjury.spineTilt, _currentInjury.twistOffset,
                    Random.Range(-_currentInjury.idleShake, _currentInjury.idleShake));
                bangMask = 0f;
            }
            */
            float rootWeight = (1f - t) * (1f - t);
            float bang = _bangCurrent * _bangAmount * rootWeight * bangMask;

            Quaternion bangRot = Quaternion.AngleAxis(bang, Vector3.right);

            spineBones[i].localRotation = _rest[i] * idleRot *  injuryRot * bangRot;
        }
    }
}
