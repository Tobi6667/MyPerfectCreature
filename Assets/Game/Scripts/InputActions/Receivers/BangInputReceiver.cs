using Game.Body;
using Game.Input;
using System;
using UnityEngine;
namespace Game.Input
{

    public class BangInputReceiver : MonoBehaviour, IInputReceiver
    {
        public event Action Confirmed;
        public event Action Injected;
        private TorsoController _torsoController;
        private bool _isActive = false;

        public void Bind(BodyPartBase bodypart)
        {
            _torsoController = bodypart as TorsoController;
           _isActive = true;
        }

        public void Initialize()
        {
        }

        public void OnConfirm()
        {
            Confirmed?.Invoke();
        }

        public void OnDefault()
        {
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
        }

        public void OnMousePosition(Vector2 mouse)
        {
        }

        public void OnMove(Vector2 input)
        {
            if (!_isActive) return;
            _torsoController.Move(input);
            //_torsoController.Move
        }

        public void OnOne()
        {
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void OnJump()
        {
            if (!_isActive) return;
            _torsoController.BangHead(true);
        }

        public void Deactivate()
        {
            _isActive = false;
        }
    }

}