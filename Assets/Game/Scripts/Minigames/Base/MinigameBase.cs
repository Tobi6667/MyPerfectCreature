using Game.Input;
using Game.Minigames;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Game.Minigames
{
    public abstract class MinigameBase : MonoBehaviour
    {
        protected MinigameContext Context;
        protected MinigamePipeline Pipeline;
        public event Action Completed;
        protected virtual void SetContext(IInputReceiver _receiver)
        {
            Context = new MinigameContext
            {

                Receiver = _receiver,
                UI = UIMinigameManager.Instance,
                Audio = AudioMinigameManager.Instance
            };
        }

        public void StartMinigame()
        {
            BuildPipeline();
            StartCoroutine(Pipeline.Run(Context));
        }

        protected void Finish()
        {
            Completed?.Invoke();
        }

        protected abstract void BuildPipeline();
    }

}