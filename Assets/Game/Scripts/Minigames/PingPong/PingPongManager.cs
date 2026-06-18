using Game.Body;
using Game.Input;
using System.Collections;
using UnityEngine;


namespace Game.Minigames
{
    public class PingPongManager : MinigameBase
    {
        [SerializeField] private SoPingPongRound _data;
        [SerializeField] private PingPongInputReceiver _inputReceiver;
        private int _currentRound;
        private int _currentHits;
        private bool _roundFinished;


        public void RegisterHit()
        {
            _currentHits++;
        }

        private void SpawnBall(PingPongRoundData round)
        {
            // instantiate ball
            // initialize speed
        }

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
                    .Add(new PingPongGameplayPhase(round));
            }
            Pipeline.Add(new ResultPhase());
        }
    }
}
