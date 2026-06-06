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

        public void Bind(BodyPartBase bodypart)
        {
            throw new NotImplementedException();
        }

        public void Initialize()
        {
            throw new NotImplementedException();
        }

        public void OnConfirm()
        {
            throw new NotImplementedException();
        }

        public void OnInject()
        {
            throw new NotImplementedException();
        }

        public void OnInteract()
        {
            throw new NotImplementedException();
        }

        public void OnLook(Vector2 input)
        {
            throw new NotImplementedException();
        }

        public void OnMousePosition(Vector2 mouse)
        {
            throw new NotImplementedException();
        }

        public void OnMove(Vector2 input)
        {
            throw new NotImplementedException();
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }

}