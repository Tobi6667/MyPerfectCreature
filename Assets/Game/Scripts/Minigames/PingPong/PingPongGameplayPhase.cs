using System;
using System.Collections;
using UnityEngine;

namespace Game.Minigames
{
    public class PingPongGameplayPhase : IMinigamePhase
    {
        private readonly PingPongRoundData _round;

        private bool _roundFinished;
        private int _currentHits;
        private bool _injuryRequested = false;
        private bool _success = false;
        public bool IsPaused { get; private set; }

        public void Pause() => IsPaused = true;
        public void Resume() => IsPaused = false;

        public PingPongGameplayPhase(PingPongRoundData round)
        {
            _round = round;
        }

        public IEnumerator Execute(MinigameContext context)
        {
            context.State = EMinigameState.Playing;
            context.Receiver.Injected += OnInjury;
            _currentHits = 0;
            _roundFinished = false;
            context.Receiver.Bind(context.BodyPart);
            float timer = _round.duration;

            SpawnBall(_round, context);
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

                    if (_currentHits >= _round.requiredHits)
                    {
                        _success = true;
                        break;
                    }

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

        public void RegisterHit()
        {
            _currentHits++;
        }

        private void SpawnBall(PingPongRoundData round, MinigameContext context)
        {
            // TODO
        }
    }
}