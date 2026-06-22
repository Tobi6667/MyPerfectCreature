using Game.Minigames;
using UnityEngine;


namespace Game.Minigames
{

    public class TorsoGameManager : MinigameBase
    {
        [SerializeField] private SoTorsoGameRoundData _data;
        [SerializeField] private Transform _spawnPoint;
        [SerializeField] private Transform _spawnEnd;
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
                    .Add(new TorsoGameplayPhase(round, _spawnPoint, _spawnEnd));
            }


        }

    }
}
