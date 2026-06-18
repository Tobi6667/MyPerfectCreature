using Game.Body;
using Game.Input;
using System;
using UnityEngine;

public class WorkoutReceiver : MonoBehaviour, IInputReceiver
{
    public event Action Confirmed;
    public event Action Injected;

    [SerializeField] private FrankensteinController _frank;

    private bool _isActive = false;
    public void Initialize(FrankensteinController frank)
    {
        _frank = frank;
        Debug.Log("JOO ACTIVATE REC");
    }

    public void OnInteract()
    {
        Debug.Log("interaaaa");
        _frank?.OnInteract();
    }

    public void OnMove(Vector2 input)
    {
        if (!_isActive) return;
        //  _frank.IkBlendController.SetSpineInput(input.x);
        //_frank?.OnMove(input);
    }

    public void OnLook(Vector2 input)
    {

        if (!_isActive) return;
        //  _frank.IkBlendController.SetSpineInput(input.x);

        //_frank?.OnLook(input);
        //_frank.IkBlendController.SetSpineInput(input.x);

    }

    public void OnConfirm()
    {
        Confirmed?.Invoke();
    }

    public void OnInject()
    {
        var god = GodInjuryDatabase.Get(EBodyRegion.Fullbody);
        _frank.IkBlendController.InjectInjury(god.GetRandomInjury());
        Injected?.Invoke();
    }

    public void Initialize()
    {

    }

    public void Bind(BodyPartBase bodypart)
    {
        _frank = bodypart as FrankensteinController;
        _isActive = true;
        Debug.Log("fffffffffff");
    }

    public void OnMousePosition(Vector2 mouse)
    {
        if (!_isActive) return;
        _frank.IkBlendController.SetSpineInput(mouse.x);
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
