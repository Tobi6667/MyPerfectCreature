using System.Collections;
using UnityEngine;

namespace Game.Minigames
{

    public class AudioMinigameManager : MonoBehaviour
    {
        [Header("Sources")]

        [SerializeField] private AudioSource _defaultSource;

        [SerializeField] private AudioSource _musicSource;
        [SerializeField] private AudioSource _sfxSource;

        [Header("State Clips")]
        [SerializeField] private AudioClip _introClip;
        [SerializeField] private AudioClip _countdownClip;     // tick per number
        [SerializeField] private AudioClip _countdownGoClip;   // "GO!" sting

        [SerializeField] private AudioClip _finishedClip;

        [Header("Fade")]
        [SerializeField] private float _fadeDuration = 1f;

        public static AudioMinigameManager Instance { get; private set; }

        private float _defaultTargetVolume;
        private float _musicTargetVolume;
        private Coroutine _fadeCoroutine;

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

            _defaultTargetVolume = _defaultSource.volume;
            _musicTargetVolume = _musicSource.volume;
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

            // already playing this clip -> do nothing
            if (_musicSource.clip == clip && _musicSource.isPlaying)
            {
                _musicSource.loop = loop;
                return;
            }

            _musicSource.loop = loop;
            _musicSource.clip = clip;
            _musicSource.Play();

            StartFade(fadeInDefault: false);
        }

        public void PlayDefault()
        {
            if (_defaultSource.isPlaying && !_musicSource.isPlaying) return;

            if (!_defaultSource.isPlaying)
                _defaultSource.Play();

            StartFade(fadeInDefault: true);
        }

        public void StopMusic() => _musicSource.Stop();

        private void PlaySfx(AudioClip clip)
        {
            if (clip == null) return;
            _sfxSource.PlayOneShot(clip);
        }

        public void StopAll()
        {
            if (_fadeCoroutine != null) StopCoroutine(_fadeCoroutine);
            _musicSource.Stop();
            _sfxSource.Stop();
            _defaultSource.Stop();
        }

        private void StartFade(bool fadeInDefault)
        {
            if (_fadeCoroutine != null) StopCoroutine(_fadeCoroutine);
            _fadeCoroutine = StartCoroutine(fadeInDefault ? FadeToDefault() : FadeToMusic());
        }

        private IEnumerator FadeToMusic()
        {
            float startDefaultVolume = _defaultSource.volume;
            float startMusicVolume = _musicSource.volume;
            float t = 0f;

            while (t < _fadeDuration)
            {
                t += Time.deltaTime;
                float lerp = t / _fadeDuration;
                _defaultSource.volume = Mathf.Lerp(startDefaultVolume, 0f, lerp);
                _musicSource.volume = Mathf.Lerp(startMusicVolume, _musicTargetVolume, lerp);
                yield return null;
            }

            _defaultSource.volume = 0f;
            _musicSource.volume = _musicTargetVolume;
            _defaultSource.Pause();
            _fadeCoroutine = null;
        }

        private IEnumerator FadeToDefault()
        {
            float startDefaultVolume = _defaultSource.volume;
            float startMusicVolume = _musicSource.volume;
            float t = 0f;

            while (t < _fadeDuration)
            {
                t += Time.deltaTime;
                float lerp = t / _fadeDuration;
                _defaultSource.volume = Mathf.Lerp(startDefaultVolume, _defaultTargetVolume, lerp);
                _musicSource.volume = Mathf.Lerp(startMusicVolume, 0f, lerp);
                yield return null;
            }

            _defaultSource.volume = _defaultTargetVolume;
            _musicSource.volume = 0f;
            _musicSource.Stop();
            _fadeCoroutine = null;
        }
    }
}