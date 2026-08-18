using Game.Body;
using Game.Input;
using Game.Main;
using System;
using UnityEngine;

namespace Game.Minigames
{
    public class WalkAvoidMinigameManager : MinigameBase
    {
        [SerializeField] private SoWalkAvoidRoundData _data;
        [SerializeField] private SoWorkoutSettings _workoutSettings;
        [SerializeField] private Transform _spawnSpiderPoint;
        [SerializeField] private Transform _spawnBulletPoint;
        [SerializeField] private Transform _endPoint;

        public Transform SpawnSpiderPoint => _spawnSpiderPoint;
        public Transform SpawnBulletPoint => _spawnBulletPoint;
        public Transform EndPoint => _endPoint;

        protected override void BuildPipeline()
        {
            UIMinigameManager.Instance.ShowHUD();
            Completed += OnDone;
            Pipeline = new MinigamePipeline();

            foreach (var round in _data.rounds)
            {
                Pipeline
                    .Add(new SetUpPhase())
                    .Add(new TutorialPhase(round.instruction))
                    .Add(new ReadyPhase(1f))
                    .Add(new CountdownPhase(3))
                    .Add(new WalkAvoidGameplayPhase(round, _workoutSettings, this));
            }

        }

        private void OnDone()
        {
            Debug.Log("done 11");
            WorkoutDatabase.Instance.ResetIK();
            GameManager.Instance.ChangeToDefaultReceiver();
            CameraMinigameManager.Instance.VictorCam();
        }

    }
}
