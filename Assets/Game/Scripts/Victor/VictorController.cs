using System;
using UnityEngine;
using UnityEngine.InputSystem.XR;

namespace Game.Input
{
    [RequireComponent(typeof(CharacterController))]
    public class VictorController : MonoBehaviour, IInputReceiver
    {

        [SerializeField] private Transform _cameraReference;
        [SerializeField] private float _speed = 5f; 
        [SerializeField] private PlayerInputModule _inputModule;


        //[SerializeField] private Animator _animator;


        [SerializeField] private float _gravity = -25f;
        private float _verticalVelocity;
        private CharacterController _controller;

        public event Action Confirmed;
        public event Action Injected;

        public void Initialize()
        {
            _controller = GetComponent<CharacterController>();
            _inputModule.SetReceiver(this);
        }

        public void OnConfirm()
        {
            throw new System.NotImplementedException();
        }

        public void OnInject()
        {
            throw new NotImplementedException();
        }

        public void OnInteract()
        {
            throw new System.NotImplementedException();
        }

        public void OnLook(Vector2 input)
        {
            throw new System.NotImplementedException();
        }

        public void OnMove(Vector2 moveInput)
        {
            Vector3 forward = _cameraReference.forward;
            Vector3 right = _cameraReference.right;

            forward.y = 0;
            right.y = 0;

            forward.Normalize();
            right.Normalize();

            Vector3 dir = forward * moveInput.y + right * moveInput.x;

            if (_controller.isGrounded && _verticalVelocity < 0f)
            {
                _verticalVelocity = -10f;
            }

            _verticalVelocity += _gravity * Time.deltaTime;

            Vector3 velocity = dir * _speed;
            velocity.y = _verticalVelocity;

            _controller.Move(velocity * Time.deltaTime);

            //_animator.SetBool("isWalking",dir.sqrMagnitude > 0.001f);
        }

        public void OnSecondary()
        {
            throw new System.NotImplementedException();
        }
    }
}
