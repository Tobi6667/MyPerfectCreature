using System;
using UnityEngine;

namespace Game.Input
{
    public interface IInputReceiver
    {
        void Initialize();

       public event Action Confirmed;
       public event Action Injected;

        public void OnConfirm();
        void OnLook(UnityEngine.Vector2 input);
        void OnMove(UnityEngine.Vector2 input);
        void OnInteract();

        void OnInject();

    }
}