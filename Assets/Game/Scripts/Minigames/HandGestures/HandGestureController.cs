using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Minigames
{
    public class HandGestureController : MonoBehaviour
    {
        public event Action RoundCompleted;
        public event Action RoundFailed;

        [Header("Gesture Settings")]
        [SerializeField] private EHandGestures _resetGesture = EHandGestures.Open;
        [SerializeField] private float _resetTime = 0.5f;
        [SerializeField] private float _gestureMatchThreshold = 1.5f;
       [SerializeField] private EnemyHandController _enemyHandController;
        private List<EHandGestures> _availableGestures;

        private Coroutine _gestureRoutine;

        private EHandGestures _currentGesture;

        private bool _active;
        private bool _waitingForGesture;
        private bool _gestureResolved;
        private bool _inResetPhase;

        private float _gestureMatchTimer;

        private int _wrongGestureCount;
        private int _maxWrongCount;

        private int _correctGestureCount;
        private int _requiredCorrectGestures;

        private void Update()
        {
            if (!_active)
                return;

            if (!_waitingForGesture)
                return;

            _gestureMatchTimer += Time.deltaTime;

            if (_gestureMatchTimer >= _gestureMatchThreshold)
            {
                HandleWrongGesture();
            }
        }

        public void StartRound(HandGesturesRoundData data)
        {
            _resetTime = data.resetTime;
            _gestureMatchThreshold = data.gestureMatchThreshold;
            _maxWrongCount = data.maxWrongCount;

            _availableGestures = data.handGestures;

            // add this field to your data if needed
           _requiredCorrectGestures = data.requiredCorrectGestures;

            _correctGestureCount = 0;
            _wrongGestureCount = 0;

            StartGestures();
        }

        public void StopRound()
        {
            StopGestures();
        }

        public void PlayerDidGesture(EHandGestures gesture)
        {
            if (!_active)
                return;

            if (_inResetPhase)
                return;

            if (!_waitingForGesture)
                return;

            if (_gestureResolved)
                return;

            if (gesture == EHandGestures.Open)
                return;

            if (gesture != _currentGesture)
            {
                HandleWrongGesture();
                return;
            }

            HandleCorrectGesture();
        }

        private void HandleCorrectGesture()
        {
            _gestureResolved = true;
            _waitingForGesture = false;

            _correctGestureCount++;

            Debug.Log($"Correct gesture ({_correctGestureCount}/{_requiredCorrectGestures})");

            if (_correctGestureCount >= _requiredCorrectGestures)
            {
                StopGestures();
                
                //RoundCompleted?.Invoke();
                return;
            }

            RestartGestureCycle();
        }

        private void HandleWrongGesture()
        {
            _waitingForGesture = false;
            _wrongGestureCount++;

            Debug.Log($"Wrong gesture ({_wrongGestureCount}/{_maxWrongCount})");

            if (_wrongGestureCount >= _maxWrongCount)
            {
                StopGestures();

                RoundFailed?.Invoke();
                return;
            }

            RestartGestureCycle();
        }

        private void RestartGestureCycle()
        {
            if (_gestureRoutine != null)
                StopCoroutine(_gestureRoutine);

            _gestureRoutine = StartCoroutine(NextGestureCycle());
        }

        internal void StartGestures()
        {
            _active = true;

            _gestureResolved = false;
            _waitingForGesture = false;
            _inResetPhase = false;

            RestartGestureCycle();
        }

        internal void StopGestures()
        {
            _active = false;
            _waitingForGesture = false;

            if (_gestureRoutine != null)
            {
                StopCoroutine(_gestureRoutine);
                _gestureRoutine = null;
            }
        }

        private IEnumerator NextGestureCycle()
        {
            _inResetPhase = true;
            _waitingForGesture = false;
            _gestureResolved = false;

            _gestureMatchTimer = 0f;

            ShowEnemyGesture(_resetGesture);

            yield return new WaitForSeconds(_resetTime);

            PickRandomGesture();

            ShowEnemyGesture(_currentGesture);
            _gestureMatchTimer = 0f;

            _inResetPhase = false;
            _waitingForGesture = true;
        }

        private void PickRandomGesture()
        {
            if (_availableGestures == null || _availableGestures.Count == 0)
            {
                Debug.LogWarning("No gestures configured.");
                return;
            }

            _currentGesture = _availableGestures[
                UnityEngine.Random.Range(0, _availableGestures.Count)
            ];

            Debug.Log($"Enemy Gesture: {_currentGesture}");
        }

        private void ShowEnemyGesture(EHandGestures gesture)
        {
            var inst = GestureDatabase.Instance.GetGesture(gesture);
            UIMinigameManager.Instance.ShowInstruction(inst.GestureKey);
            _enemyHandController.SetGesture(inst);
        }


        private void Start()
        {

            _enemyHandController.Initialize();
        }
    }
}