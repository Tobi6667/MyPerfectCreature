using Game.Body;
using Game.Input;
using UnityEngine;

namespace Game.Minigames
{
    public class LegGameManager : MinigameBase
    {

        [SerializeField] private SoLegGameRounds _data;
        [SerializeField] private Transform _footTarget;

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
                    .Add(new LegGameplayPhase(round, _footTarget));
            }
        }
    }
}
