using Game.Minigames;
using System.Collections;
using UnityEngine;


namespace Game.Minigames
{
    public class TutorialPhase : IMinigamePhase
    {
        private readonly string _text;

        public TutorialPhase(string text)
        {
            _text = text;
        }

        public IEnumerator Execute(MinigameContext context)
        {
            context.State = EMinigameState.Tutorial;

            context.UI.ShowTutorial(_text);

            bool confirmed = false;

            void Handler()
            {
                confirmed = true;
            }

            context.Receiver.Confirmed += Handler;

            try
            {
                yield return new WaitUntil(() => confirmed);
            }
            finally
            {
                context.Receiver.Confirmed -= Handler;
            }
        }
    }
}