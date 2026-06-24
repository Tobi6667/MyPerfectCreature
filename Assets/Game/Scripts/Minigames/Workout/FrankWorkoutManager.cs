using DG.Tweening;
using Game.Main;
using System;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.UIElements;

namespace Game.Minigames
{
    public class FrankWorkoutManager : MinigameBase
    {
        public static FrankWorkoutManager Instance;

        [Header("Data")]
        private List<FrankensteinWorkoutData> workoutDataList;

        [Header("UI")]
        [SerializeField] private VisualTreeAsset _uiprefab;
        [SerializeField] private UIDocument _uiDoc;
        [SerializeField] private CinemachineCamera _yogamatCam;

        [Header("Body")]
        [SerializeField] private FrankensteinController _frankensteinBody;
        [SerializeField] private WorkoutReceiver _workoutReceiver;
        private FrankensteinWorkoutData activeWorkout;

        private int FAT = 60;
        private int VAS = 40;

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
            workoutDataList = WorkoutDatabase.Instance.GetWorkout();
            Pipeline.Add(new WorkoutSelectionPhase(
                _uiprefab,
                _uiDoc,
                workoutDataList,
                (_FrankensteinWorkoutData) =>
                {
                    activeWorkout = _FrankensteinWorkoutData;
                    Debug.Log(_FrankensteinWorkoutData);

                    FAT -= _FrankensteinWorkoutData.fatigueCosts;
                    VAS -= _FrankensteinWorkoutData.vasCosts;

                    if(_FrankensteinWorkoutData.workoutType == WorkoutType.Default)
                    {

                        StartWorkout(_FrankensteinWorkoutData);
                    }
                    if(_FrankensteinWorkoutData.workoutType == WorkoutType.Game)
                    {
                        StartMiniGame(_FrankensteinWorkoutData);
                    }
                    UIMinigameManager.Instance.UpdateFatVas(FAT, VAS);

                }
            ));

        }
        private void StartWorkout(FrankensteinWorkoutData workdata)
        {
            Debug.Log("Starting workout");
            _frankensteinBody = Context.BodyPart as FrankensteinController;
            //CameraMinigameManager.Instance.ChangeTo(_frankensteinBody.GetTransitionCam());
            _frankensteinBody.FrankensteinMovementModule.MoveToTarget(workdata.workoutPosition, () =>
            {
                CameraMinigameManager.Instance.ChangeTo(_yogamatCam);
               WorkoutDatabase.Instance.SetIK(workdata);
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
            _frankensteinBody = Context.BodyPart as FrankensteinController;
            _frankensteinBody.FrankensteinMovementModule.SetIKAnimator(true);

            Debug.Log("MiniGame placeholder");
            MinigameManager.Instance.StartMinigame(
    workdata._minigame,
    Context.BodyPart
);

        }

        // ---------------------------------------------------
        // IK SYSTEM (UNCHANGED)
        // ---------------------------------------------------


    }
}