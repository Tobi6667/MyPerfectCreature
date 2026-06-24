using Game.Body;
using Game.Input;
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
            Pipeline = new MinigamePipeline();

            foreach (var round in _data.rounds)
            {
                Pipeline
                    .Add(new SetUpPhase())
                    .Add(new TutorialPhase(round.instruction))
                    .Add(new ReadyPhase(2f))
                    .Add(new CountdownPhase(3))
                    .Add(new WalkAvoidGameplayPhase(round, _workoutSettings, this));
            }
        }
    }
}
