using System.Collections;
using UnityEngine;

namespace Game.Minigames
{
    public class ResultPhase : IMinigamePhase
    {
        public IEnumerator Execute(MinigameContext context)
        {
            Debug.Log("FINISHED");
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
