using Game.Body;
using Game.Input;
using Game.Minigames;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Input
{

    public class PingPongInputReceiver : MonoBehaviour, IInputReceiver
    {
        [SerializeField] private PingPongHandComponent _pingPongHand;
        private HandController _handController;
        [SerializeField] private PingPongManager _pingManager;
        public event Action Confirmed;
        public event Action Injected;
        private bool _isActive = false;
        public void Initialize()
        {

        }

        public void OnInteract()
        {
            Debug.Log("Interact");
            _pingManager.StartMinigame();
        }

        public void OnMove(Vector2 input)
        {

        }


        public void OnLook(Vector2 input)
        {
            if (!_isActive) return;
            _pingPongHand.Slide(Mouse.current.position.ReadValue());
        }


        public void OnConfirm()
        {
            Confirmed?.Invoke();
        }

        public void OnInject()
        {
            Debug.Log("Inject");
            Injected?.Invoke();
        }

        public void Bind(BodyPartBase bodypart)
        {
            _handController = bodypart as HandController;
            _pingPongHand = _handController.GetComponent<PingPongHandComponent>();
            _isActive = true;
            
        }

        public void OnMousePosition(Vector2 mouse)
        {
        }

        public void OnOne()
        {
        }

        public void OnDefault()
        {
        }

        public void OnJump()
        {
        }

        public void Deactivate()
        {
            _isActive = false;
        }

        public void OnTwo()
        {
            throw new NotImplementedException();
        }

        public void OnT()
        {
            throw new NotImplementedException();
        }
    }

}
