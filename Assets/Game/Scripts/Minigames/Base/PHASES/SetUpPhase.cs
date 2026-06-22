using Game.Body;
using Game.Input;
using System.Collections;
using UnityEditor;
using UnityEngine;


namespace Game.Minigames
{

    public class SetUpPhase : IMinigamePhase
    {

        public IEnumerator Execute(MinigameContext context)
        {
            Debug.Log($"[SetUpPhase] Starting. BodyPart: {context.BodyPart}, StartTransform: {context.StartTransform}");
             
            bool finished = false;

            context.BodyPart.MoveToInteractionPoint(
                context.StartTransform.position,
                () => finished = true);
            CameraMinigameManager.Instance.ChangeTo(context.Minigame.GameCam);
            Debug.Log("[SetUpPhase] Waiting for MoveToInteractionPoint to complete...");

            while (!finished)
                yield return null;

            Debug.Log("[SetUpPhase] Done.");
        }
    }
}
