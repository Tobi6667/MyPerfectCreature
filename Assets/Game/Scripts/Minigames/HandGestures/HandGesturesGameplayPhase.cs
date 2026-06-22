using Game.Body;
using System.Collections;
using UnityEngine;

namespace Game.Minigames
{
    public class HandGestureGameplayPhase : IMinigamePhase
    {
        private readonly HandGesturesRoundData _round;
        private readonly HandGestureController _gestureController;

        private bool _roundCompleted;
        private bool _roundFailed;
        private bool _injuryRequested;


        public HandGestureGameplayPhase(
            HandGesturesRoundData round,
            HandGestureController gestureController)
        {
            _round = round;
            _gestureController = gestureController;
        }

        public IEnumerator Execute(MinigameContext context)
        {
            context.State = EMinigameState.Playing;

            float timer = _round.duration;
            _gestureController.RoundCompleted += OnRoundCompleted;
            _gestureController.RoundFailed += OnRoundFailed;

            _gestureController.StartRound(_round);
            context.Receiver.Bind(context.BodyPart);
            context.Receiver.Injected += OnInjury;

            try
            {
                while (timer > 0f)
                {

                    if(context.State == EMinigameState.Paused)
                    {
                        context.State = EMinigameState.Playing;
                       _gestureController.StartGestures();

                    }

                    if (_roundFailed)
                    {
                        context.Cancelled = true;
                        yield break;
                    }

                    if (_roundCompleted)
                    {
                        yield break;
                    }

                    if (_injuryRequested)
                    {
                        _injuryRequested = false;
                        context.State = EMinigameState.Paused;
                        yield return context.RunPhase(
                            new InjuryPhase());
                    }

                    timer -= Time.deltaTime;
                    context.UI.UpdateTimer(timer);

                    yield return null;
                }

                // Timer ran out without completion = failure
                context.Cancelled = true;
            }
            finally
            {
                _gestureController.StopRound();
                _gestureController.RoundCompleted -= OnRoundCompleted;
                _gestureController.RoundFailed -= OnRoundFailed;
                context.Receiver.Injected -= OnInjury;

            }
        }

        private void OnRoundCompleted() => _roundCompleted = true;
        private void OnRoundFailed() => _injuryRequested = true;

        private void OnInjury()
        {
            _injuryRequested = true;
        }
    }
}