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
        private bool _injured = false;
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

            if(_injured)
            {
                _controller.MoveCam(input);

            }

        }

        public void OnMove(Vector2 input)
        {

        }

        public void OnMousePosition(Vector2 mouse)
        {
            if (!_isActive) return;

            if (_injured)
            {
                return;
            }
            _controller.MoveLegRoot(mouse);
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

        internal void ChangeToInjuryInput(bool inj)
        {
            _injured = inj;
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
