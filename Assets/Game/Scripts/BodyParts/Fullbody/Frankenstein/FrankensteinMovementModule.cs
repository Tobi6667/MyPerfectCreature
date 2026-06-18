using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class FrankensteinMovementModule : MonoBehaviour
{
    [Header("Test")]
    [SerializeField] private Transform _testTarget;
    public event System.Action OnWorkoutFinished;
    [Header("References")]
    [SerializeField] private Animator _animator;
    [SerializeField] private Animator _animatorFrank;
    [SerializeField] private NavMeshAgent _agent;

    [Header("Animator")]
    [SerializeField] private string _workoutStateName = "Base Layer.Workout";
    [SerializeField] private string _workoutSlotName = "Walking";

    [Header("INJURY")]
    [SerializeField] private float injuryIntensity = 0f;
    [SerializeField] private float limpFrequency = 6f;
    [SerializeField] private float limpAmplitude = 0.05f;
    [SerializeField] private float pelvisWobble = 0.03f;

    private Transform _currentTarget;
    private FrankensteinWorkoutData _activeWorkout;

    private AnimatorOverrideController _overrideController;
    private AnimatorOverrideController _overrideControllerFrank;

    private Coroutine _moveRoutine;
    private int _workoutHash;

    private bool _isMoving;
    private bool _isWorking;

    private Vector3 _pelvisOffset;
    private Vector3 _spineOffset;

    private void Awake()
    {
        _overrideController =
            new AnimatorOverrideController(_animator.runtimeAnimatorController);

        _overrideControllerFrank =
            new AnimatorOverrideController(_animatorFrank.runtimeAnimatorController);

        _animator.runtimeAnimatorController = _overrideController;
        _animatorFrank.runtimeAnimatorController = _overrideControllerFrank;

        _workoutHash = Animator.StringToHash(_workoutStateName);
    }

    private void Start()
    {

        _agent.updateRotation = false;
      //  if (_testTarget != null)          //  MoveToTarget(_testTarget, null);
    }

    // =========================================================
    // MOVE (UNCHANGED BEHAVIOR)
    // =========================================================
    internal void MoveToTarget(Transform target, Action onReached)
    {
        if (target == null) return;

        EnterMoveState();

        _currentTarget = target;

        _agent.enabled = true;
        _agent.isStopped = false;
        _agent.ResetPath();
        _agent.SetDestination(target.position);

      //  _animator.SetBool("isWalking", true);
       _animatorFrank.SetBool("isWalking", true);

        Vector3 look = target.position;
        look.y = transform.position.y;

        transform.DOKill();
        transform.DOLookAt(look, 0.35f)
                 .SetEase(Ease.OutSine);

        if (_moveRoutine != null)
            StopCoroutine(_moveRoutine);

        _moveRoutine = StartCoroutine(CheckReached(onReached));
    }
    private IEnumerator CheckReached(Action onReached)
    {
        while (_isMoving)
        {
            if (!_agent.pathPending &&
                _agent.remainingDistance <= _agent.stoppingDistance &&
                _agent.velocity.sqrMagnitude <= 0.01f)
            {
                EnterIdleState();

                if (_currentTarget != null)
                {
                    transform.DOKill();

                    transform.DORotateQuaternion(_currentTarget.rotation, 0.15f)
                        .SetEase(Ease.OutSine)
                        .OnComplete(() =>
                        {
                            _agent.enabled = false;
                            onReached?.Invoke();
                        });

                    yield break;
                }

            }

            yield return null;
        }
    }
    // =========================================================
    // WORKOUT (RESTORED PROPERLY)
    // =========================================================
    public void PlayWorkout(FrankensteinWorkoutData workout, float transition = 0.25f)
    {
        if (workout == null) return;
        if (_activeWorkout == workout) return;

        EnterWorkoutState();

        _activeWorkout = workout;

        _overrideController[_workoutSlotName] = workout.workoutClip;
        _overrideControllerFrank[_workoutSlotName] = workout.workoutClip;

       // _animator.SetBool("isWorkingout", true);
        _animatorFrank.SetBool("isWorkingout", true);

       // _animator.CrossFade(_workoutHash, transition);
        _animatorFrank.CrossFade(_workoutHash, transition);


        //DOVirtual.DelayedCall(transition, () => SetIK(1f));
    }

    // =========================================================
    // STATES (UNCHANGED LOGIC)
    // =========================================================
    private void EnterMoveState()
    {
        _isMoving = true;
        _isWorking = false;

        _activeWorkout = null;

       // _animator.SetBool("isWorkingout", false);
        _animatorFrank.SetBool("isWorkingout", false);
    }

    private void EnterWorkoutState()
    {
        _isMoving = false;
        _isWorking = true;

        if (_moveRoutine != null)
        {
            StopCoroutine(_moveRoutine);
            _moveRoutine = null;
        }

        if (_agent.enabled)
        {
            _agent.isStopped = true;
            _agent.ResetPath();
        }

       // _animator.SetBool("isWalking", false);
       _animatorFrank.SetBool("isWalking", false);
    }

    private void EnterIdleState()
    {
        _isMoving = false;
        _isWorking = false;

        if (_moveRoutine != null)
        {
            StopCoroutine(_moveRoutine);
            _moveRoutine = null;
        }

        _agent.isStopped = true;

    //    _animator.SetBool("isWalking", false);
        _animatorFrank.SetBool("isWalking", false);
    //    _animator.SetBool("isWorkingout", false);
      _animatorFrank.SetBool("isWorkingout", false);


        _activeWorkout = null;
    }

    // =========================================================
    // INJURY (DATA ONLY — SAFE NOW)
    // =========================================================
    private void Update()
    {
        float t = Time.time;

        float limp = Mathf.Sin(t * limpFrequency) * limpAmplitude * injuryIntensity;
        float wobble = Mathf.Sin(t * limpFrequency * 0.5f) * pelvisWobble * injuryIntensity;

        _pelvisOffset = new Vector3(limp, 0f, wobble);

        _spineOffset = new Vector3(
            limp * 10f,
            0f,
            wobble * 6f
        );

        // KEEP YOUR IK SYSTEM EXACTLY AS YOU HAD IT
        if (!_isWorking || _activeWorkout == null)
            return;

        for (int i = 0; i < _activeWorkout.ikTargets.Count; i++)
        {
            var ik = _activeWorkout.ikTargets[i];
            if (!ik.animateTarget || ik.target == null) continue;

            float w = ik.currentWeight;

          //  if (ik._ikConstraintSkeleton) ik._ikConstraintSkeleton.weight = w;
          //  if (ik._ikConstraintFrank) ik._ikConstraintFrank.weight = w;

            float time = Time.time * ik.speed;

            Vector3 baseOffset =
                ik.moveDirection *
                Mathf.Sin(time + ik.delay) *
                ik.moveAmount;

            Vector3 pos = ik.localOffset + baseOffset;

            ik.target.localPosition =
                Vector3.Lerp(Vector3.zero, pos, w);

            ik.target.localRotation =
                Quaternion.Slerp(
                    Quaternion.identity,
                    Quaternion.Euler(ik.localRotationOffset),
                    w
                );
        }
    }

    // =========================================================
    // THIS IS NOW CORRECTLY PLACED (ONLY ADDITIVE VISUAL LAYER)
    // =========================================================
    private void OnAnimatorIK(int layerIndex)
    {
        if (!_isWorking) return;

        if (!_animator) return;

        // IMPORTANT: base pose first
        Vector3 basePos = _animator.bodyPosition;

        // apply offset in world space directly (NO +=)
        Vector3 worldOffset = transform.TransformVector(_pelvisOffset);

        _animator.bodyPosition = basePos + worldOffset;

        // spine rotation (safe multiplicative blend)
        Quaternion spineRot = Quaternion.Euler(_spineOffset);
        _animator.bodyRotation = spineRot * _animator.bodyRotation;
    }



}