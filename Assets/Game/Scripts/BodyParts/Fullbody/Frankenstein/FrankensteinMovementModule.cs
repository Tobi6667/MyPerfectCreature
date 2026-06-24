using DG.Tweening;
using Game.Minigames;
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
    [SerializeField] private Animator _animatorIK;
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

    private Coroutine _moveRoutine;
    private int _workoutHash;

    private bool _isMoving;
    private bool _isWorking;

    private Vector3 _pelvisOffset;
    private Vector3 _spineOffset;

    private void Awake()
    {
        _overrideController =
            new AnimatorOverrideController(_animatorFrank.runtimeAnimatorController);


        _animatorFrank.runtimeAnimatorController = _overrideController;

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

        _animatorFrank.SetBool("isWorkingout", true);

        _animatorFrank.CrossFade(_workoutHash, transition);


        //DOVirtual.DelayedCall(transition, () => SetIK(1f));
    }


    internal void StopWorkout()
    {
        EnterIdleState();
    }
    
    private void EnterMoveState()
    {
        _isMoving = true;
        _isWorking = false;

        _activeWorkout = null;

        _animatorFrank.SetBool("isWorkingout", false);
    }

    internal void SetIKAnimator(bool state)
    {
        _animatorIK.enabled = state;
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

       _animatorFrank.SetBool("isWalking", false);

       // _animatorIK.enabled = true;


    }




    private void EnterIdleState()
    {
        _isMoving = false;
        _isWorking = false;
        _animatorIK.enabled = false;
        if (_moveRoutine != null)
        {
            StopCoroutine(_moveRoutine);
            _moveRoutine = null;
        }
        if (_agent.isActiveAndEnabled)
        { 
            _agent.isStopped = true;
    }
        _animatorFrank.SetBool("isWalking", false);
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

            ik.target.localPosition = Vector3.Lerp(Vector3.zero, pos, w);

            ik.target.localRotation = Quaternion.Slerp(Quaternion.identity, Quaternion.Euler(ik.localRotationOffset), w);
        }
    }

    private void OnAnimatorIK(int layerIndex)
    {
        if (!_isWorking) return;

        if (!_animatorFrank) return;


        Vector3 basePos = _animatorFrank.bodyPosition;

        Vector3 worldOffset = transform.TransformVector(_pelvisOffset);

        _animatorFrank.bodyPosition = basePos + worldOffset;

        Quaternion spineRot = Quaternion.Euler(_spineOffset);
        _animatorFrank.bodyRotation = spineRot * _animatorFrank.bodyRotation;
    }



}