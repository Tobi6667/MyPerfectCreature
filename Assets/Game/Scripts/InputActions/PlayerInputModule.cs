using GameInput;
using System;
using UnityEngine;

namespace Game.Input
{
    public class PlayerInputModule : MonoBehaviour
    {
        private InputActionMain _input;
        private IInputReceiver _activeInputReceiver;
        private Vector2 _moveInput;
        private Vector2 _mouseInput;
        public event Action OnConfirmed;
        public Vector2 _mousePosition;

        private void Awake()
        {
            _input = new InputActionMain();


            _input.Gameplay.Move.performed += ctx =>
            {
                _moveInput = ctx.ReadValue<Vector2>();
            };

            _input.Gameplay.Move.canceled += ctx =>
            {
                _moveInput = Vector2.zero;
            };


            _input.Gameplay.Look.performed += ctx =>
            {
                _mouseInput = ctx.ReadValue<Vector2>();

            };

            _input.Gameplay.MousePosition.performed += ctx =>
            {
                _mousePosition = ctx.ReadValue<Vector2>();
            };



            _input.Gameplay.Look.canceled += ctx =>
            {
                _mouseInput = Vector2.zero;


            };



            _input.Gameplay.Interact.performed += ctx =>
            {
                Debug.Log("Interact pressed. Receiver = " + _activeInputReceiver);
                _activeInputReceiver?.OnInteract();
            };

            _input.Gameplay.Confirm.performed += _ =>
            {
                _activeInputReceiver?.OnConfirm();
            };

            _input.Gameplay.Inject.performed += _ =>
            {
                _activeInputReceiver?.OnInject();
            };

            _input.Gameplay.One.performed += _ =>
            {
                _activeInputReceiver?.OnOne();
            };


            _input.Gameplay.One.canceled += _ =>
            {
                _activeInputReceiver?.OnDefault();
            };

            _input.Gameplay.Two.performed += _ =>
            {
                _activeInputReceiver?.OnTwo();
            };


            _input.Gameplay.Two.canceled += _ =>
            {
                _activeInputReceiver?.OnDefault();
            };

            _input.Gameplay.T.performed += _ =>
            {
                _activeInputReceiver?.OnT();
            };


            _input.Gameplay.T.canceled += _ =>
            {
                _activeInputReceiver?.OnDefault();
            };


            _input.Gameplay.Jump.performed += _ =>
            {
                _activeInputReceiver?.OnJump();
            };




            EnableGameplay();
        }


        public void SetReceiver(IInputReceiver receiver)
        {
            Debug.Log("Setting receiver: " + receiver);
            _activeInputReceiver = receiver;
        }

        private void Update()
        {
            _activeInputReceiver?.OnMove(_moveInput);
            _activeInputReceiver?.OnLook(_mouseInput);
            _activeInputReceiver?.OnMousePosition(_mousePosition);
        }

       


        public void EnableGameplay()
        {
            _input.UI.Disable();
            _input.Gameplay.Enable();
        }

        public void EnableUI()
        {
            _input.Gameplay.Disable();
            _input.UI.Enable();
        }



        private void OnDisable()
        {
            _input.Disable();
        }
    }
}
