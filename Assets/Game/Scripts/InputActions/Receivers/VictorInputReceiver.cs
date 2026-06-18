using Game.Body;
using Game.Input;
using System;
using UnityEngine;

public class VictorInputReceiver : MonoBehaviour, IInputReceiver
{
    public event Action Confirmed;
    public event Action Injected;

   [SerializeField] private VictorController _victor;

    public void Initialize(VictorController victor)
    {
        _victor = victor;
    }

    public void OnInteract()
    {
        _victor?.OnInteract();
    }

    public void OnMove(Vector2 input)
    {
        _victor?.OnMove(input);
    }

    public void OnLook(Vector2 input)
    {
        _victor?.OnLook(input);
       
    }

    public void OnConfirm()
    {
        Confirmed?.Invoke();
    }

    public void OnInject()
    {
        Injected?.Invoke();
    }

    public void Initialize()
    {

    }

    public void Bind(BodyPartBase bodypart)
    {

    }

    public void OnMousePosition(Vector2 mouse)
    {
  
    }

    public void OnOne()
    {
        throw new NotImplementedException();
    }

    public void OnDefault()
    {
        throw new NotImplementedException();
    }

    public void OnJump()
    {
        throw new NotImplementedException();
    }

    public void Deactivate()
    {
        //_isActive = false;
    }
}