using Game.Minigames;
using System.Collections;
using UnityEngine;


namespace Game.Minigames
{
    public class WalkAvoidGameplayPhase : IMinigamePhase
    {
        private readonly WalkAvoidRoundData _round;

        private bool _roundFinished;
        private int _currentHits;
        private bool _injuryRequested = false;
        private bool _success = false;
        public bool IsPaused { get; private set; }

        public void Pause() => IsPaused = true;
        public void Resume() => IsPaused = false;

        public WalkAvoidGameplayPhase(WalkAvoidRoundData round)
        {
            _round = round;
        }

        public IEnumerator Execute(MinigameContext context)
        {
            context.State = EMinigameState.Playing;
            context.Receiver.Injected += OnInjury;
            _currentHits = 0;
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

                /*    if (_currentHits >= _round.requiredHits)
                    {
                        _success = true;
                        break;
                    }*/

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
            _injuryRequested = true;
        }


    }
}
