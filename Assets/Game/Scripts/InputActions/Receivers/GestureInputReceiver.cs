using Game.Body;
using Game.Input;
using Game.Minigames;
using System;
using UnityEngine;


namespace Game.Input
{
    public class GestureInputReceiver : MonoBehaviour, IInputReceiver
    {
        public event Action Confirmed;
        public event Action Injected;
        private HandController _controller;
        private HandGestureController _gestureController;
        private bool _isActive = false;

        public void Bind(BodyPartBase bodypart)
        {
            _controller = bodypart as HandController;
            _gestureController = GetComponent<HandGestureController>();
            _isActive = true;
        }

        public void Initialize()
        {
            
        }

        public void OnConfirm()
        {
            Debug.Log("conf");
            Confirmed?.Invoke();
        }

        public void OnInject()
        {
            throw new NotImplementedException();
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
            
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void OnOne()
        {

            if (!_isActive) return;

            Debug.Log($"_controller: {_controller}");
            Debug.Log($"_gestureController: {_gestureController}");
            Debug.Log($"GestureDatabase.Instance: {GestureDatabase.Instance}");

            _controller.ShowGesture(GestureDatabase.Instance.GetGesture(EHandGestures.OneFinger));
            _gestureController.PlayerDidGesture(EHandGestures.OneFinger);
        }

        public void OnDefault()
        {
            if (!_isActive) return;

            _controller.ShowGesture(GestureDatabase.Instance.GetGesture(EHandGestures.Open));
        }

        public void OnJump()
        {
            throw new NotImplementedException();
        }


        public void Deactivate()
        {
            _isActive = false;
        }

        public void OnTwo()
        {
            _controller.ShowGesture(GestureDatabase.Instance.GetGesture(EHandGestures.TwoFinger));
            _gestureController.PlayerDidGesture(EHandGestures.TwoFinger);
        }

        public void OnT()
        {
            _controller.ShowGesture(GestureDatabase.Instance.GetGesture(EHandGestures.MiddleFinger));
            _gestureController.PlayerDidGesture(EHandGestures.MiddleFinger);
        }
    }
}