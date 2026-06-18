using Game.Body;
using System;
using Unity.Cinemachine;
using UnityEngine;

public class EnemyHandController : BodyPartBase
{
    public override CinemachineCamera GetTransitionCam()
    {
        throw new NotImplementedException();
    }


    public override void MoveToObject(Transform target, Action onReached, float speed = 4, float arriveDistance = 0)
    {
        throw new NotImplementedException();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_ball == null) return;

        if (_stunTimer > 0f)
        {
            _stunTimer -= Time.deltaTime;
            return;
        }

        FollowBall();
        UpdateVisualPunch();
        CheckStrike();
    }

    [SerializeField] private FingerController _fingerController;

    [Header("Lane (Local Z)")]
    [SerializeField] private Transform minX;
    [SerializeField] private Transform maxX;

    //private PingPongGameModule _pingpongModule;

    [Header("Movement")]
    [SerializeField] private float reactionSpeed = 12f;

    [Header("Strike")]
    [SerializeField] private float strikeDistance = 1.2f;
    [SerializeField] private float strikePower = 10f;
    [SerializeField] private float strikeArcHeight = 4f; // upward angle component so ball arcs over net

    [Header("AI")]
    [SerializeField] private Transform opponentTarget;
    [SerializeField] private float predictionTime = 0.25f;
    [SerializeField] private float skillAccuracy = 0.85f;
    [SerializeField] private float aimNoise = 0.3f;

    [Header("Visual Punch")]
    [SerializeField] private float strikePushDistance = 0.25f;
    [SerializeField] private float strikeReturnSpeed = 18f;
    //[SerializeField] private EBodyInjuryType _bodyinjuryType;
    //public override EBodyInjuryType EBodyInjuryType => _bodyinjuryType;

    // Cached starting position (local space) so visual punch always has a stable origin
    private Vector3 _originalLocalPos;

    // Visual punch state — tracked independently of lane-follow so they don't fight
    private bool _isPushing;

    private float _stunTimer;
    private bool _hasStruck;

    private Transform _ball;

    // BodyPartBase requires this — return a sensible default or wire up properly
   // public override BodyPartType Type => BodyPartType.LeftArm;

    public override void Initialize()
    {
        _originalLocalPos = transform.localPosition;
        _fingerController.Initialize(null);
    }

    internal void SetBallTarget(Transform ball) => _ball = ball;
   // internal void SetPingPongModule(PingPongGameModule module) => _pingpongModule = module;

    /// <summary>Briefly stuns the hand (e.g. after being hit back).</summary>
    public void PushBack(float force) => _stunTimer = 0.15f;



    // -------------------------------------------------------------------------
    // Movement
    // -------------------------------------------------------------------------

    private void FollowBall()
    {
        Transform parent = transform.parent;
        Vector3 localBall = parent.InverseTransformPoint(_ball.position);

        float targetZ = Mathf.Clamp(
            localBall.z,
            minX.localPosition.z,
            maxX.localPosition.z
        );

        // Only adjust Z — visual punch drives X, so we never touch it here
        Vector3 lp = transform.localPosition;
        lp.z = Mathf.Lerp(lp.z, targetZ, Time.deltaTime * reactionSpeed);
        transform.localPosition = lp;
    }

    // -------------------------------------------------------------------------
    // Strike
    // -------------------------------------------------------------------------

    private void CheckStrike()
    {
        float dist = Mathf.Abs(transform.position.z - _ball.position.z);

        if (dist > strikeDistance)
        {
            _hasStruck = false;
            return;
        }

        if (_hasStruck) return;

        Strike();
    }

    private void Strike()
    {
        if (!_ball.TryGetComponent<Rigidbody>(out var rb)) return;

        _hasStruck = true;
        _isPushing = true;

        // Predict where the ball will be after predictionTime
        Vector3 predicted = _ball.position + rb.linearVelocity * predictionTime;

        // Blend between predicted landing and the explicit target (if set)
        Vector3 baseAim = opponentTarget != null
            ? Vector3.Lerp(predicted, opponentTarget.position, skillAccuracy)
            : predicted;

        // Apply skill-based positional noise
        float error = (1f - skillAccuracy) * aimNoise;
        Vector3 noise = new Vector3(
            UnityEngine.Random.Range(-error, error),
            0f,
            UnityEngine.Random.Range(-error, error)
        );

        Vector3 aimPoint = baseAim + noise;

        // Build a launch direction that includes a Y component so the ball arcs
        Vector3 toTarget = aimPoint - transform.position;
        toTarget.y = 0f;
        Vector3 dir = (toTarget.normalized + Vector3.up * strikeArcHeight).normalized;

        rb.linearVelocity = dir * strikePower;

        //_pingpongModule?.EnemyHitBall();
    }

    // -------------------------------------------------------------------------
    // Visual punch — operates only on the X axis (local forward of the hand)
    // so it never interferes with the Z lane-follow above.
    // -------------------------------------------------------------------------

    private void UpdateVisualPunch()
    {
        // Compute the punch target in local space using localRight so rotation is respected
        float currentX = transform.localPosition.x;
        float targetX = _isPushing
            ? _originalLocalPos.x + strikePushDistance
            : _originalLocalPos.x;

        float speed = _isPushing ? 25f : strikeReturnSpeed;
        float newX = Mathf.Lerp(currentX, targetX, Time.deltaTime * speed);

        // Write back only the X axis — leave Y and Z as they are
        Vector3 lp = transform.localPosition;
        lp.x = newX;
        transform.localPosition = lp;

        if (_isPushing && Mathf.Abs(newX - targetX) < 0.02f)
            _isPushing = false;
    }

    // -------------------------------------------------------------------------
    // Round data / gesture
    // -------------------------------------------------------------------------

    internal void SetRoundData(PingPongRoundData data)
    {
        reactionSpeed = data.reactionSpeed;
        strikeDistance = data.strikeDistance;
        strikePower = data.strikeSpeed;
        _stunTimer = data.stunTime;
    }

    internal void SetGesture(HandPositions gesture)
    {
        _fingerController.SetGesturePosition(gesture);
    }

    public override void OnInject(IInjuryData injury)
    {
        throw new NotImplementedException();
    }
}
