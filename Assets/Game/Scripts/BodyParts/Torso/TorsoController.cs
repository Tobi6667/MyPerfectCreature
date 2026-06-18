using Game.Body;
using Game.Main;
using System;
using Unity.Cinemachine;
using UnityEngine;

public class TorsoController : BodyPartBase, IInteractable
{

    [SerializeField] private TorsoMovementModule _movementModule;
    [SerializeField] private CinemachineCamera _followCam;
    private float _bangAmount;
    private bool _movetoLabObject = false;
    private bool _expanded = false;


    public override void Initialize()
    {

       _movementModule.Initialize();
       _hopComponent.Initialize();

    }



    internal void RemoveInjury()
    {

       // _movementModule.RemoveInjury();
    }


    internal void Move(Vector2 input)
    {
        _movementModule.Move(input);
    }


    internal void ReleaseBang()
    {
      //  _movementModule.ReleaseBang();
    }

    
 

    internal void BangHead(bool banging)
    {
        _movementModule.BangHead(banging);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created







    public override void MoveToObject(Transform target, Action onReached, float speed = 4, float arriveDistance = 0)
    {
        throw new NotImplementedException();
    }

    public override CinemachineCamera GetTransitionCam()
    {
        return _followCam;
    }

    public void OnInteract()
    {
        _hopComponent.StopHopping();
        _movementModule.Idle();
    }

    public override void OnInject(IInjuryData injury)
    {
        _movementModule.InjectInjury(injury as TorsoInjuryData);

    }
}
