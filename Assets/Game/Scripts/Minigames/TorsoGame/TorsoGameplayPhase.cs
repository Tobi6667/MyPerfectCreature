using Game.Minigames;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Minigames
{
    public class TorsoGameplayPhase : IMinigamePhase
    {
        private TorsoController _torsoController;
        private readonly TorsoGameRoundData _round;
        private bool _roundFinished = false;
        private bool _injuryRequested = false;
        private bool _imagesPaused = false;
        private int _bangCount = 0;
        private readonly List<MoveImagesObject> _activeImages = new List<MoveImagesObject>();
        private float _dropTimer = 0f;
        private Vector3 _spawnPos;
        private Transform _endPoint;

       

        public TorsoGameplayPhase(TorsoGameRoundData round, Transform spawnPoint, Transform endP)
        {
            _round = round;
            _spawnPos = spawnPoint.position;
            _endPoint = endP;
        }

        public IEnumerator Execute(MinigameContext context)
        {
            context.State = EMinigameState.Playing;

            _roundFinished = false;
            _dropTimer = 0f; // spawn first image immediately on entry

            float timer = _round.duration;
            context.Receiver.Bind(context.BodyPart);
            _torsoController = context.BodyPart as TorsoController;
            _torsoController.OnBangHead += OnHeadBanged;
            AudioMinigameManager.Instance.PlayMusic(context.Minigame.GameMusic, true);

            try
            {
                while (!_roundFinished && timer > 0f)
                {
                    if (_injuryRequested)
                    {
                        _injuryRequested = false;
                        _bangCount = 0;
                        PauseImages(true);
                        yield return context.RunPhase(new InjuryPhase());
                        PauseImages(false);
                    }

                    // ---- spawn tick ----
                    _dropTimer -= Time.deltaTime;
                    if (_dropTimer <= 0f)
                    {
                        SpawnImage(context);

                        float rspeed = UnityEngine.Random.Range(0.7f, 1.5f);
                        _dropTimer = _round.dropInterval * _round.dropSpeedMultiplier * rspeed;
                    }

                    timer -= Time.deltaTime;
                    context.UI.UpdateTimer(timer);

                    yield return null;
                }

                _roundFinished = true;
            }
            finally
            {

                ClearDrops();
            }
        }

        private void OnHeadBanged()
        {
            Debug.Log("bang");
            _bangCount++;
            if (_bangCount > _round.maxBangAmount)
            {

                _injuryRequested = true;
                _bangCount = 0;
            }
        }

        private void SpawnImage(MinigameContext context)
        {
            float randomZ = UnityEngine.Random.Range(-5f, 5f);

            Vector3 spawnPoint = new Vector3(
                _spawnPos.x,
                _spawnPos.y,
                _spawnPos.z + randomZ
            );

            var img = UnityEngine.Object.Instantiate(
                _round.imageObject,
                spawnPoint,
                Quaternion.Euler(0f, 90f, 0f)
            );

            _activeImages.Add(img);

            // if an injury popup happened mid-spawn-tick, keep new images paused too
            /*    if (_imagesPaused)
                    img.SetPaused(true);
            */
            Vector3 direction = Vector3.down;

            img.Initialize(
                5f * _round.difficultyMultiplier,
                direction,
                _endPoint,
                null ,
                (MovingObjectBase movingObject) =>
                {
                    if (_roundFinished) return;
                    Debug.Log("Hit Frankenstein");
                    _activeImages.Remove(movingObject as MoveImagesObject);
                    movingObject.DestroyObject();
                }
            );
        }

        private void PauseImages(bool paused)
        {
            _imagesPaused = paused;

            foreach (var img in _activeImages)
            {
                /* if (img != null)
                     img.SetPaused(paused);*/
            }
        }

        private void ClearDrops()
        {
            foreach (var drop in _activeImages)
            {
                if (drop != null)
                    UnityEngine.Object.Destroy(drop.gameObject);
            }

            _activeImages.Clear();
        }


    }
}