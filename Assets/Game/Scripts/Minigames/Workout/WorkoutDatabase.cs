using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class WorkoutDatabase : MonoBehaviour
{
    public static WorkoutDatabase Instance;


    [SerializeField] private List<WorkoutDataList> workoutDataLists;

    private int workoutIndex = 0;
    [SerializeField] private List<IKTarget> iks;
    [SerializeField] private Animator animator;

    private void Awake()
    {
        Instance = this;
    }


    public List<FrankensteinWorkoutData> GetWorkout()
    {

        var l = workoutDataLists[workoutIndex].WorkoutList;
        workoutIndex++;

        return l;
    }
    public void SetIK(FrankensteinWorkoutData activeWorkout)
    {
        Debug.Log("set ik");

        foreach (var ik in iks)
        {
            TweenIKWeight(ik, 0f);
        }

        if (activeWorkout == null) return;

        foreach (var ik in activeWorkout.ikTargets)
        {
            TweenIKWeight(ik.ikConstraint, ik.currentWeight);
        }

        //animator.enabled = true;
    }

    private void TweenIKWeight(IKTarget ikTarget, float targetWeight)
    {
        ikTarget.tween?.Kill();

        if (ikTarget.constraint is UnityEngine.Animations.Rigging.TwoBoneIKConstraint twoBone)
        {
            ikTarget.tween = DOTween.To(
                () => twoBone.weight,
                x => twoBone.weight = x,
                targetWeight,
                0.35f
            ).SetEase(Ease.OutSine);
        }
        else if (ikTarget.constraint is UnityEngine.Animations.Rigging.ChainIKConstraint chain)
        {
            ikTarget.tween = DOTween.To(
                () => chain.weight,
                x => chain.weight = x,
                targetWeight,
                0.35f
            ).SetEase(Ease.OutSine);
        }
        else if (ikTarget.constraint is UnityEngine.Animations.Rigging.MultiAimConstraint multiAim)
        {
            ikTarget.tween = DOTween.To(
                () => multiAim.weight,
                x => multiAim.weight = x,
                targetWeight,
                0.35f
            ).SetEase(Ease.OutSine);
        }
        else if (ikTarget.constraint is UnityEngine.Animations.Rigging.MultiRotationConstraint multiRotation)
        {
            ikTarget.tween = DOTween.To(
                () => multiRotation.weight,
                x => multiRotation.weight = x,
                targetWeight,
                0.35f
            ).SetEase(Ease.OutSine);
        }
        else if (ikTarget.constraint is UnityEngine.Animations.Rigging.MultiParentConstraint multiParent)
        {
            ikTarget.tween = DOTween.To(
                () => multiParent.weight,
                x => multiParent.weight = x,
                targetWeight,
                0.35f
            ).SetEase(Ease.OutSine);
        }
    }


}
