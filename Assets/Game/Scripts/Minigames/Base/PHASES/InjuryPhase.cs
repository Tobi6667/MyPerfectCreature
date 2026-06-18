using Game.Body;
using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

namespace Game.Minigames
{
    public class InjuryPhase : IMinigamePhase
    {
        public IEnumerator Execute(MinigameContext context)
        {
            Debug.Log("show injury");

            IInjuryData inj = GetInjury(context.BodyPart.Region);
            context.UI.ShowInjuryPanel(inj);
            context.BodyPart.OnInject(inj);

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

        private IInjuryData GetInjury(EBodyRegion region)
        {
            var database = GodInjuryDatabase.Get(region);
            var injury = database.GetRandomInjury();
            return injury;
        }

    }
    
}
