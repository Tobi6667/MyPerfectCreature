using Game.Body;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Input
{
    public class BalanceInputReceiver : MonoBehaviour, IInputReceiver
    {
        public event Action Confirmed;
        public event Action Injected;
       private LegController _controller;
        private bool _isActive = false;
        private BodyPartBase _activeBodypart;
        public void Initialize()
        {

        }
        public void Bind(BodyPartBase part)
        {
            _controller = part as LegController;
            _isActive = true;
        }
        public void OnConfirm()
        {
            Confirmed?.Invoke();
        }

        public void Activate()
        {
            _isActive = true;
        }

        public void OnInject()
        {
            Injected?.Invoke();
        }

        public void OnInteract()
        {

        }

        public void OnLook(Vector2 input)
        {
            if (!_isActive) return;
            
        }

        public void OnMove(Vector2 input)
        {

        }

        public void OnMousePosition(Vector2 mouse)
        {
            if (!_isActive) return;
            _controller.MoveLegRoot(mouse);
        }

        public void OnOne()
        {
            throw new NotImplementedException();
        }

        public void OnDefault()
        {
            throw new NotImplementedException();
        }

        public void OnJump()
        {
            throw new NotImplementedException();
        }

        public void Deactivate()
        {
            _isActive = false;
        }
    }
}
