using System.Collections;
using UnityEngine;

namespace Game.Minigames
{
    public class InjuryPhase : IMinigamePhase
    {
        public IEnumerator Execute(MinigameContext context)
        {
            context.UI.ShowInjuryPanel();

            bool confirmed = false;

            void OnConfirm()
            {
                confirmed = true;
            }

            context.Receiver.Confirmed += OnConfirm;

            try
            {
                while (!confirmed)
                    yield return null;
            }
            finally
            {
                context.Receiver.Confirmed -= OnConfirm;
            }

            context.UI.HideInjuryPanel();
        }
    }
    
}
