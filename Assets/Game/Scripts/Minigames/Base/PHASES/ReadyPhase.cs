using System.Collections;
using UnityEngine;

namespace Game.Minigames
{
    public class ReadyPhase : IMinigamePhase
    {
        private float _time;

        public ReadyPhase(float time)
        {
            _time = time;
        }

        IEnumerator IMinigamePhase.Execute(MinigameContext context)
        {
            context.State = EMinigameState.Ready;

            // optional UI hook
           // context.UI?.ShowReady();

            // optional: lock input during ready phase
           // context.InputLocked = true;

            yield return new WaitForSeconds(_time);

            //context.InputLocked = false;
        }
    }
}