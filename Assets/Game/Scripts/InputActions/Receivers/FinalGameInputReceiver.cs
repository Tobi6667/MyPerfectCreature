using Game.Body;
using Game.Input;
using System;
using UnityEngine;

public class FinalGameInputReceiver : MonoBehaviour, IInputReceiver
{
    public event Action Confirmed;
    public event Action Injected;

    public void Bind(BodyPartBase bodypart)
    {
    }

    public void Deactivate()
    {
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
    }

    public void OnInteract()
    {
    }

    public void OnJump()
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

    public void OnOne()
    {
    }

    public void OnT()
    {
    }

    public void OnTwo()
    {
    }


}
