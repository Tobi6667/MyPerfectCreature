using Game.Input;
using Game.Minigames;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Input
{

    public class PingPongInputReceiver : MonoBehaviour, IInputReceiver
    {
        [SerializeField] private PingPongHandComponent _hand;
        [SerializeField] private PingPongManager _pingManager;
        public event Action Confirmed;
        public event Action Injected;

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
            _hand.Slide(Mouse.current.position.ReadValue());
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
    }

}
