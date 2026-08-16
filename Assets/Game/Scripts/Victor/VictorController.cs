using DG.Tweening;
using Game.Main;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;


namespace Game.Input
{
    [RequireComponent(typeof(CharacterController))]
    public class VictorController : MonoBehaviour, IInteractable
    {
        [SerializeField] private NavMeshAgent _navMeshAgent;
        [SerializeField] private float _rotationSpeed = 720f;
        [SerializeField] private Transform _defaultPosition;
        [SerializeField] private Transform _cameraReference;
        [SerializeField] private PlayerCameraModule _playerCameraModule;
        [SerializeField] private CharacterController _characterController;
        [SerializeField] private float _moveSpeed = 2f;
        [SerializeField] private float _speed = 5f; 
        [SerializeField] private PlayerInputModule _inputModule;
        private Action<IInteractable> onInteract;

        [SerializeField] private Animator _animator;


        [SerializeField] private float _gravity = -25f;
        private float _verticalVelocity;
        private CharacterController _controller;

        public event Action Confirmed;
        public event Action Injected;

       

        public void Initialize(Action<IInteractable> onInteractCallback)
        {
            _controller = GetComponent<CharacterController>();
            onInteract = onInteractCallback;
            
        }


        public void OnConfirm()
        {

        }

        public void OnInject()
        {

        }

        public void OnInteract()
        {
           

            Collider[] hits = Physics.OverlapSphere(transform.position, 2f);

            foreach (var hit in hits)
            {
                if (hit.gameObject == gameObject)
                    continue;

                if (hit.TryGetComponent(out IInteractable interactable))
                {
                    Debug.Log(interactable);
                    onInteract?.Invoke(interactable);
                    //_animator.SetBool("isWalking", false);
                    break;
                }
            }

        }

        public void OnLook(Vector2 input)
        {
           _playerCameraModule.ManualLook(input);
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

            _animator.SetBool("isWalking",dir.sqrMagnitude > 0.001f);
        }


        public void MoveToDefault()
        {
            _navMeshAgent.enabled = true;
            _animator.SetBool("isWalking", true);

            _navMeshAgent.SetDestination(_defaultPosition.position);
            StartCoroutine(WaitUntilDestinationReached());
        }

        private IEnumerator WaitUntilDestinationReached()
        {
            while (_navMeshAgent.pathPending)
                yield return null;

            while (_navMeshAgent.remainingDistance > _navMeshAgent.stoppingDistance)
            {
                Vector3 direction = _navMeshAgent.desiredVelocity;

                direction.y = 0f;

                if (direction.sqrMagnitude > 0.01f)
                {
                    Quaternion targetRotation = Quaternion.LookRotation(direction);

                    transform.rotation = Quaternion.RotateTowards(
                        transform.rotation,
                        targetRotation,
                        _rotationSpeed * Time.deltaTime
                    );
                }

                yield return null;
            }
            _navMeshAgent.enabled=false;
            _animator.SetBool("isWalking", false);
        }

        public void OnSecondary()
        {
        }
    }
}
