using DG.Tweening;
using Game.Minigames;
using System;
using System.Collections.Generic;
using Unity.Cinemachine;
using Unity.VisualScripting;
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
        [SerializeField] protected HopComponent _hopComponent;

        public IReadOnlyList<MinigameBase> MinigamePrefabs => _minigamePrefabs;

        public MinigameBase GetMinigame(int index = 0)
        {
            if (_minigamePrefabs == null || _minigamePrefabs.Count == 0)
            {
                Debug.LogError($"{name} has no minigames assigned!", this);
                return null;
            }

            index = Mathf.Clamp(index, 0, _minigamePrefabs.Count - 1);
            return _minigamePrefabs[index];
        }

        public abstract void Initialize();
        public abstract void MoveToObject(Transform target, Action onReached,float speed = 4f, float arriveDistance = 0f);

        public void MoveToInteractionPoint(Vector3 target, Action onArrived)
        {
            if (_agent)
                _agent.enabled = false;
            _hopComponent.StopHopping();
            DG.Tweening.Sequence seq = DOTween.Sequence();
            seq.Join(transform.DOMove(target + _offsetAtTarget, 2f));
            seq.Join(transform.DORotateQuaternion(Quaternion.Euler(_offsetAtTarget), 2f).SetEase(Ease.InOutSine));
            seq.OnComplete(() => onArrived?.Invoke());
        }


        public abstract CinemachineCamera GetTransitionCam();
    }
}
