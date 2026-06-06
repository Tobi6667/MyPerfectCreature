using Game.Minigames;
using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

namespace Game.Minigames
{
    public class TorsoGameplayPhase : IMinigamePhase
    {

        private readonly TorsoGameRoundData _round;
        private bool _roundFinished = false;
        private bool _injuryRequested = false;

        public TorsoGameplayPhase(TorsoGameRoundData round)
        {
            _round = round;
        }

        public IEnumerator Execute(MinigameContext context)
        {
            context.State = EMinigameState.Playing;
            context.Receiver.Injected += OnInjury;

            _roundFinished = false;

            float timer = _round.duration;

            try
            {

                while (!_roundFinished && timer > 0f)
                {
                    if (_injuryRequested)
                    {
                        _injuryRequested = false;

                        yield return context.RunPhase(new InjuryPhase());
                    }

                    timer -= Time.deltaTime;

                    context.UI.UpdateTimer(timer);


                    yield return null;
                }
                _roundFinished = true;
                if (!_roundFinished)
                    context.Cancelled = true;
            }
            finally
            {
                context.Receiver.Injected -= OnInjury;
            }
        }

        private void OnInjury()
        {
            throw new NotImplementedException();
        }
    }
}
