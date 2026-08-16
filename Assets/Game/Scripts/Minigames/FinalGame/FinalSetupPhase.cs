using Game.Body;
using Game.Input;
using Game.Main;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Game.Minigames
{

    public class FinalSetUpPhase : IMinigamePhase
    {
        private List<BodyPartBase> bodies;
        

        public IEnumerator Execute(MinigameContext context)
        {
            Debug.Log($"[SetUpPhase] Starting. BodyPart: {context.BodyPart}, StartTransform: {context.StartTransform}");

            bool finished = false;

            bodies = new List<BodyPartBase>();
            bodies = GameManager.Instance.BodypartsList;


            foreach(var body in bodies)
            {
                body.MoveToInteractionPoint(context.StartTransform.position, () =>
                {
                    finished = true;
                });
            }


            CameraMinigameManager.Instance.ChangeTo(context.Minigame.GameCam);


            while (!finished)
                yield return null;

            Debug.Log("[SetUpPhase] Done.");
        }
    }
}
