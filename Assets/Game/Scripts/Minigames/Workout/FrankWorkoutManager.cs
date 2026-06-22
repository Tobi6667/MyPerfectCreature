using DG.Tweening;
using Game.Main;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Game.Minigames
{
    public class FrankWorkoutManager : MinigameBase
    {
        public static FrankWorkoutManager Instance;

        [Header("Data")]
        [SerializeField] private List<FrankensteinWorkoutData> workoutDataList;

        [Header("UI")]
        [SerializeField] private VisualTreeAsset _uiprefab;
        [SerializeField] private UIDocument _uiDoc;

        [Header("Body")]
        [SerializeField] private FrankensteinController _frankensteinBody;
        [SerializeField] private WorkoutReceiver _workoutReceiver;
        private ActiveWorkoutRuntime activeWorkout;

        private void Awake()
        {
            Instance = this;
        }




        // ---------------------------------------------------
        // PIPELINE
        // ---------------------------------------------------

        protected override void BuildPipeline()
        {
            Pipeline = new MinigamePipeline();

            Pipeline.Add(new WorkoutSelectionPhase(
                _uiprefab,
                _uiDoc,
                workoutDataList,
                (_FrankensteinWorkoutData) =>
                {
                    Debug.Log(_FrankensteinWorkoutData);
                    if(_FrankensteinWorkoutData.workoutType == WorkoutType.Default)
                    {

                        StartWorkout(_FrankensteinWorkoutData);
                    }
                    if(_FrankensteinWorkoutData.workoutType == WorkoutType.Game)
                    {
                        StartMiniGame(_FrankensteinWorkoutData);
                    }
                }
            ));

        }

        // ---------------------------------------------------
        // SELECTION RESULT
        // ---------------------------------------------------



        // ---------------------------------------------------
        // WORKOUT START
        // ---------------------------------------------------

        private void StartWorkout(FrankensteinWorkoutData workdata)
        {
            Debug.Log("Starting workout");
            _frankensteinBody = Context.BodyPart as FrankensteinController;

            _frankensteinBody.FrankensteinMovementModule.MoveToTarget(workdata.workoutPosition, () =>
            {
                _frankensteinBody.FrankensteinMovementModule.PlayWorkout(workdata);
                GameManager.Instance.ChangeReceiver(_workoutReceiver);
            });
            _workoutReceiver.Confirmed += OnConfirm;

            //OnReachedWorkout();
        }

        private void OnConfirm()
        {
            _frankensteinBody.FrankensteinMovementModule.StopWorkout();
            GameManager.Instance.ChangeToDefaultReceiver();
            Destroy(this.gameObject);
        }

        private void StartMiniGame(FrankensteinWorkoutData workdata)
        {
            Debug.Log("MiniGame placeholder");
            MinigameManager.Instance.StartMinigame(
    workdata._minigame,
    Context.BodyPart
);

        }

        // ---------------------------------------------------
        // IK SYSTEM (UNCHANGED)
        // ---------------------------------------------------

        private void OnReachedWorkout()
        {
            SetIK();
        }

        private void SetIK()
        {
            Debug.Log("set ik");

            if (activeWorkout == null) return;

            foreach (var ik in activeWorkout.workoutData.ikTargets)
            {
                ik.ikConstraint.tween?.Kill();

                var targetWeight = ik.currentWeight;

                if (ik.ikConstraint.constraint is UnityEngine.Animations.Rigging.TwoBoneIKConstraint twoBone)
                {
                    ik.ikConstraint.tween = DOTween.To(
                        () => twoBone.weight,
                        x => twoBone.weight = x,
                        targetWeight,
                        0.35f
                    ).SetEase(Ease.OutSine);
                }
                else if (ik.ikConstraint.constraint is UnityEngine.Animations.Rigging.ChainIKConstraint chain)
                {
                    ik.ikConstraint.tween = DOTween.To(
                        () => chain.weight,
                        x => chain.weight = x,
                        targetWeight,
                        0.35f
                    ).SetEase(Ease.OutSine);
                }
                else if (ik.ikConstraint.constraint is UnityEngine.Animations.Rigging.MultiAimConstraint multiAim)
                {
                    ik.ikConstraint.tween = DOTween.To(
                        () => multiAim.weight,
                        x => multiAim.weight = x,
                        targetWeight,
                        0.35f
                    ).SetEase(Ease.OutSine);
                }
                else if (ik.ikConstraint.constraint is UnityEngine.Animations.Rigging.MultiRotationConstraint multiRotation)
                {
                    ik.ikConstraint.tween = DOTween.To(
                        () => multiRotation.weight,
                        x => multiRotation.weight = x,
                        targetWeight,
                        0.35f
                    ).SetEase(Ease.OutSine);
                }
                else if (ik.ikConstraint.constraint is UnityEngine.Animations.Rigging.MultiParentConstraint multiParent)
                {
                    ik.ikConstraint.tween = DOTween.To(
                        () => multiParent.weight,
                        x => multiParent.weight = x,
                        targetWeight,
                        0.35f
                    ).SetEase(Ease.OutSine);
                }
            }
        }
    }
}