using Game.Main;
using System;
using Unity.Cinemachine;
using UnityEngine;

namespace Game.Body
{
    public class HandController : BodyPartBase, IInteractable
    {
        public Action OnFailed;

        [SerializeField] private FingerController _fingerController;
        [SerializeField] private PingPongHandComponent _pingPongHandComponent;
        [SerializeField] private CinemachineCamera _camera;

        public override CinemachineCamera GetTransitionCam()
        {
            return _camera;
        }

        public override void Initialize()
        {
            _hopComponent = GetComponent<HopComponent>();
            _hopComponent.Initialize();
            _fingerController.Initialize(null);
            _pingPongHandComponent.Initialize();            
        }

        internal void ShowGesture(HandPositions gesture)
        {
            _fingerController.SetGesturePosition(gesture);
        }

        public override void MoveToObject(Transform target, Action onReached, float speed = 4, float arriveDistance = 0)
        {
            
        }

        public void OnInteract()
        {
            throw new System.NotImplementedException();
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            //_hopComponent.Initialize();
           // _pingPongHandComponent.Initialize();
           // _fingerController.Initialize(null);
            
        }

        // Update is called once per frame
        void Update()
        {

        }

        public override void OnInject(IInjuryData injury)
        {
            _fingerController.InjectInjury(injury as HandInjuryTypes);
        }
    }
}
