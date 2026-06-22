using DG.Tweening;
using Game.Minigames;
using System;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.AI;

namespace Game.Body
{
    [RequireComponent(typeof(HopComponent))]
    [RequireComponent(typeof(NavMeshAgent))]

    public abstract class BodyPartBase : MonoBehaviour
    {

        [Header("Minigames")]
        [SerializeField]
        private List<MinigameBase> _minigamePrefabs = new();
        [SerializeField] private NavMeshAgent _agent;
        [SerializeField] private Vector3 _offsetAtTarget;
        [SerializeField] private Vector3 _offsetRotationAtTarget;
        [SerializeField] protected HopComponent _hopComponent;
        [SerializeField] private EBodyRegion _region;
        [SerializeField] private EBodyPartType _type;
        private int _gameIndex = 0;
        public HopComponent HopComponent => _hopComponent;
        public EBodyRegion Region => _region;
        public IReadOnlyList<MinigameBase> MinigamePrefabs => _minigamePrefabs;

        public EBodyPartType Type => _type;
        public MinigameBase GetMinigame(int index = 0)
        {
            Debug.Log("ask for game" + _gameIndex);
            if (_gameIndex >= _minigamePrefabs.Count)
            {
                return null;
            }
            Debug.Log(_minigamePrefabs[_gameIndex]);
            _gameIndex += 1;
            return _minigamePrefabs[_gameIndex - 1];
        }

        public abstract void Initialize();
        public abstract void MoveToObject(Transform target, Action onReached, float speed = 4f, float arriveDistance = 0f);

        public abstract void OnInject(IInjuryData injury);

        public void MoveToInteractionPoint(Vector3 target, Action onArrived)
        {
            if (_agent)
                _agent.enabled = false;
            if (_hopComponent) _hopComponent.StopHopping();
            DG.Tweening.Sequence seq = DOTween.Sequence();
            seq.Join(transform.DOMove(target + _offsetAtTarget, 2f));
            seq.Join(transform.DORotateQuaternion(Quaternion.Euler(_offsetRotationAtTarget), 2f).SetEase(Ease.InOutSine));
            seq.OnComplete(() => onArrived?.Invoke());
        }


        public abstract CinemachineCamera GetTransitionCam();
    }
}
