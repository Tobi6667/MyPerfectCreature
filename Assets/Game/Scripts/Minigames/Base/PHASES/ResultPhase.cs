using DG.Tweening;
using Game.Main;
using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Minigames
{
    public class ResultPhase : IMinigamePhase
    {
        public IEnumerator Execute(MinigameContext context)
        {
            Debug.Log("FINISHED");
            context.Receiver.Deactivate();
            context.Minigame.Complete();
            //context.BodyPart.MoveT





            yield return new WaitForSeconds(2f);

            //Object.Destroy(context.Minigame.gameObject);
            yield break;
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
