using Game.Input;
using Game.Minigames;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;


namespace Game.Minigames
{
    public abstract class MinigameBase : MonoBehaviour
    {
        protected MinigameContext Context;
        protected MinigamePipeline Pipeline;
        public event Action Completed;
        [SerializeField] private Transform _startTransform;
        [SerializeField] private SoWorkoutSettings _soWorkoutSettings;
        [SerializeField] private AudioClip _gameAudio;
        [SerializeField] private CinemachineCamera _gameCam;

        public CinemachineCamera GameCam => _gameCam;
        public AudioClip GameMusic => _gameAudio;

        public void Initialize(MinigameContext context)
        {
            context.Minigame = this;
            Context = context;

        }

        public Transform GetStartTrans()
        {
            return _startTransform;
        }


        public void StartMinigame()
        {
            BuildPipeline();

            StartCoroutine(Pipeline.Run(Context));
        }

        protected void Finish()
        {
            Completed?.Invoke();
            DestroyMinigame();
        }

        protected void DestroyMinigame()
        {
            Destroy(this.gameObject);
        }

        protected abstract void BuildPipeline();

        public void Complete()
        {
            Completed?.Invoke();
        }
    }

}