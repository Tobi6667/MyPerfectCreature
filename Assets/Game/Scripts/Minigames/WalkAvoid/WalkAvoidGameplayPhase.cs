using Game.Main;
using Game.Minigames;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Minigames
{
    public class WalkAvoidGameplayPhase : IMinigamePhase
    {
        private readonly WalkAvoidRoundData _round;
        private readonly MinigameBase _runner;
        private readonly SoWorkoutSettings _workoutSettings;
        private bool _roundFinished;
        private int _currentHits;
        private bool _injuryRequested;
        private Transform _spawn;
        private Transform _end;

        public bool IsPaused { get; private set; }

        public void Pause() => IsPaused = true;
        public void Resume() => IsPaused = false;

        private List<MovingObjectBase> _spawnedObjects = new();
        private Coroutine _spawnRoutine;

        public WalkAvoidGameplayPhase(WalkAvoidRoundData round, SoWorkoutSettings settings, MinigameBase runner)
        {
            _round = round;
            _runner = runner;
            _workoutSettings = settings;

        }

        public IEnumerator Execute(MinigameContext context)
        {
            var b = context.BodyPart as FrankensteinController;
            b.IkBlendController.SetStartData(_workoutSettings);
            var t = _runner as WalkAvoidMinigameManager;
            _spawn = t.SpawnPoint;
            _end = t.EndPoint;
            context.Receiver.Bind(context.BodyPart);
            GameManager.Instance.ChangeReceiver(context.Receiver);
            context.State = EMinigameState.Playing;
            context.OnInjuryInjected += OnInjury;

            _currentHits = 0;
            _roundFinished = false;
            _injuryRequested = false;

            float timer = _round.roundDuration;

            StartSpawnLoop();

            try
            {
                while (!_roundFinished && timer > 0f)
                {
                    if (_injuryRequested)
                    {
                        _injuryRequested = false;

                        StopSpawnLoop();
                        DestroyAllSpawned();

                        yield return context.RunPhase(new InjuryPhase());

                        StartSpawnLoop();
                    }

                    if (!IsPaused)
                    {
                        timer -= Time.deltaTime;
                        context.UI.UpdateTimer(timer);
                    }

                    yield return null;
                }

                _roundFinished = true;
            }
            finally
            {
                context.OnInjuryInjected -= OnInjury;
                StopSpawnLoop();
                DestroyAllSpawned();
            }
        }

        // =====================================================
        // SPAWN
        // =====================================================

        private void StartSpawnLoop()
        {
            _spawnRoutine = _runner.StartCoroutine(SpawnLoop());
        }

        private void StopSpawnLoop()
        {
            if (_spawnRoutine != null)
            {
                _runner.StopCoroutine(_spawnRoutine);
                _spawnRoutine = null;
            }
        }

        private IEnumerator SpawnLoop()
        {
            while (!_roundFinished)
            {
                if (!IsPaused)
                    SpawnSpider();

                yield return new WaitForSeconds(_round.spawnInterval);
            }
        }

        private void SpawnSpider()
        {
           // if (_round.spiderPrefab == null) return;
           
            float randomX = _spawn.position.x + UnityEngine.Random.Range(-4f, 4f);
            Vector3 spawnPoint = new Vector3(
                randomX, _spawn.position.y, _spawn.position.z
            );

            SpiderController spider = UnityEngine.Object.Instantiate(
                _round.spiderPrefab, spawnPoint, Quaternion.identity
            );

            Vector3 direction = (_end.position - spawnPoint).normalized;

            spider.Initialize(
                5f * _round.difficultyMultiplier,
                direction,
                _end,
                null,
                (MovingObjectBase hit) =>
                {
                    if (_roundFinished) return;

                    _currentHits++;
                    _spawnedObjects.Remove(hit);
                    UnityEngine.Object.Destroy(hit.gameObject);
                    CheckHitLimit();
                }
            );

            _spawnedObjects.Add(spider);
        }

        // =====================================================
        // HIT
        // =====================================================

        private void CheckHitLimit()
        {
            if (_currentHits < _round.cangetHits) return;

            _currentHits = 0;
            OnInjury();
        }

        private void DestroyAllSpawned()
        {
            for (int i = _spawnedObjects.Count - 1; i >= 0; i--)
            {
                if (_spawnedObjects[i] != null)
                    UnityEngine.Object.Destroy(_spawnedObjects[i].gameObject);
            }
            _spawnedObjects.Clear();
        }

        private void OnInjury()
        {
            _injuryRequested = true;
        }
    }
}