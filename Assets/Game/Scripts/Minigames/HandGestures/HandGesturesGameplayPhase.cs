using Game.Minigames;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;


namespace Game.Minigames
{

    public class HandGestureGameplayPhase : IMinigamePhase
    {
        private readonly HandGesturesRoundData _data;
        private readonly HandGestureController _controller;

        public HandGestureGameplayPhase(
            HandGesturesRoundData data,
            HandGestureController controller)
        {
            _data = data;
            _controller = controller;
        }

        public IEnumerator Execute(MinigameContext context)
        {
            bool completed = false;
            bool failed = false;

            void OnCompleted() => completed = true;
            void OnFailed() => failed = true;

            _controller.RoundCompleted += OnCompleted;
            _controller.RoundFailed += OnFailed;

            try
            {
                _controller.StartRound(_data);
                _controller.StartGestures();

                while (!completed && !failed)
                    yield return null;

                if (failed)
                    context.Cancelled = true;
            }
            finally
            {
                _controller.StopGestures();

                _controller.RoundCompleted -= OnCompleted;
                _controller.RoundFailed -= OnFailed;
            }
        }
    }
}
