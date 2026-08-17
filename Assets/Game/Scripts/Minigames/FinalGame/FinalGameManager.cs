using Game.Minigames;
using UnityEngine;

public class FinalGameManager : MinigameBase
{
    [SerializeField] private SoFinalGameRoundData _data;
    [SerializeField] private FinalGameQuizController _quizController;
    protected override void BuildPipeline()
    {

        Pipeline = new MinigamePipeline();
        
        foreach (var round in _data.rounds)
        {
            Pipeline
                .Add(new FinalSetUpPhase())
                .Add(new TutorialPhase(round.instruction))
                .Add(new ReadyPhase(1f))
                .Add(new CountdownPhase(3))
                .Add(new FinalGameGameplayPhase(round, _quizController));
        }
    }
}
