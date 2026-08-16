using Game.Minigames;
using UnityEngine;

public class FinalGameManager : MinigameBase
{
    [SerializeField] private SoFinalGameRoundData _data;
    protected override void BuildPipeline()
    {
        Pipeline = new MinigamePipeline();
        
        foreach (var round in _data.rounds)
        {
            Pipeline
                .Add(new FinalSetUpPhase())
                .Add(new TutorialPhase(round.instruction))
                .Add(new ReadyPhase(2f))
                .Add(new CountdownPhase(3))
                .Add(new FinalGameGameplayPhase(round));
        }
    }
}
