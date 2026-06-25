using Game.Body;
using System;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;


public enum FrankensteinState
{
    Idle,
    MoveToTarget,
    ExecuteExercise,
}

public class FrankensteinController : BodyPartBase, IInteractable
{

    [SerializeField] private FrankensteinMovementModule _movementModule;
    [SerializeField] private Transform _testTarget;
    [SerializeField] private FrankensteinWorkoutModule _frankensteinAbilityTestsModule;
    [SerializeField] private IKBlendController _ikController;
    public Transform Transform => transform;
    public IKBlendController IkBlendController => _ikController;
    public FrankensteinMovementModule FrankensteinMovementModule => _movementModule;

    [SerializeField] private CinemachineCamera _followCam;

    /*
    internal void SetWorkoutSettings(SOWorkoutSettings settings)
    {
       
        _ikController.SetStartData(settings);
    }

    */


    public override void Initialize()
    {
        //_movementModule.InitializeModule();
        //_movementModule.MoveToTarget(_testTarget, null);
       // _frankensteinAbilityTestsModule.InitializeModule();
        _ikController.Initialize();
    }


    public override void MoveToObject(Transform target, Action onReached, float speed = 4, float arriveDistance = 0)
    {
        throw new NotImplementedException();
    }

    public override void OnInject(IInjuryData injury)
    {
        Debug.Log("BLAAAAAAAAAAA");
        if (injury is FullbodyInjuryEntry fullbody)
        {
            _ikController.InjectInjury(fullbody);
            return;
        }
    }

    public override CinemachineCamera GetTransitionCam()
    {
        Debug.Log(_followCam);

        return _followCam;
    }

    public void OnInteract()
    {
        _gameIndex = 0;
        Debug.Log("fdfdffd");
    }
}
