using UnityEngine;

namespace Game.Minigames
{

    public class AudioMinigameManager : MonoBehaviour
    {
        [Header("Sources")]
        [SerializeField] private AudioSource _musicSource;
        [SerializeField] private AudioSource _sfxSource;

        [Header("State Clips")]
        [SerializeField] private AudioClip _introClip;
        [SerializeField] private AudioClip _countdownClip;     // tick per number
        [SerializeField] private AudioClip _countdownGoClip;   // "GO!" sting

        [SerializeField] private AudioClip _finishedClip;

        public static AudioMinigameManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
            }
        }


        public void PlayState(EMinigameState state)
        {
            switch (state)
            {
                case EMinigameState.Intro:
                    PlayMusic(_introClip, loop: false);
                    break;
             /*   case EMinigameState.Playing:
                    PlayMusic(_playingClip, loop: true);
                    break;
                case EMinigameState.RoundEnded:
                    StopMusic();
                    PlaySfx(_roundEndClip);
                    break;*/
                case EMinigameState.Finished:
                    StopMusic();
                    PlaySfx(_finishedClip);
                    break;
                case EMinigameState.Paused:
                    StopAll();
                    break;
            }
        }

        public void PlayCountdownTick() => PlaySfx(_countdownClip);
        public void PlayCountdownGo() => PlaySfx(_countdownGoClip);
       // public void PlayRoundFail() => PlaySfx(_roundFailClip);

        public void PlayMusic(AudioClip clip, bool loop)
        {
            if (clip == null) return;
            _musicSource.loop = loop;
            _musicSource.clip = clip;
            _musicSource.Play();
        }

        public void StopMusic() => _musicSource.Stop();

        private void PlaySfx(AudioClip clip)
        {
            if (clip == null) return;
            _sfxSource.PlayOneShot(clip);
        }

        public void StopAll()
        {
            _musicSource.Stop();
            _sfxSource.Stop();
        }
    }
}