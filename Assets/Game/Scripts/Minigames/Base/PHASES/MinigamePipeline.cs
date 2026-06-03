using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Minigames
{
    public class MinigamePipeline
    {
        private readonly List<IMinigamePhase> _phases = new();

        public MinigamePipeline Add(IMinigamePhase phase)
        {
            _phases.Add(phase);
            return this;
        }

        public IEnumerator Run(MinigameContext context)
        {
            foreach (var phase in _phases)
            {
                if (context.Cancelled)
                    yield break;

                yield return phase.Execute(context);
            }
        }
    }
}
