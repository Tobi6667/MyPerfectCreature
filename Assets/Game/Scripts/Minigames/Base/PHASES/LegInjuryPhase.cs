using Game.Body;
using Game.Input;
using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

namespace Game.Minigames
{
    public class LegInjuryPhase : IMinigamePhase
    {
        public IEnumerator Execute(MinigameContext context)
        {
            Debug.Log("show injury");

            IInjuryData inj = GetInjury(context.BodyPart.Region);
            context.UI.ShowInjuryPanel(inj);
            context.BodyPart.OnInject(inj);
            var rec = context.Receiver as BalanceInputReceiver;
            rec.ChangeToInjuryInput(true);
            bool confirmed = false;

            void OnConfirm()
            {
                Debug.Log("conf");
                rec.ChangeToInjuryInput(false);

                var leg = context.BodyPart as LegController;
                leg.ResetLeg(() =>
                {
                    confirmed = true;
                    CameraMinigameManager.Instance.ChangeTo(context.Minigame.GameCam);
                });

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
