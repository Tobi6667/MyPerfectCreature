using Game.Body;
using Game.Input;
using Game.Minigames;
using System;
using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class MinigameContext
{
    public EMinigameState State;
    public IInputReceiver Receiver;
    public BodyPartBase BodyPart;

    public int CountdownTime;
    public float ReadyTime;
    public Transform StartTransform;
    public UIMinigameManager UI;
    public AudioMinigameManager Audio;
    public MinigameBase Minigame;
    [Header("State Clips")]
    public FrankensteinWorkoutData _workoutData;
    public event Action OnInjuryInjected;
    public bool Cancelled;
    public IEnumerator RunPhase(IMinigamePhase phase)
    {
        yield return phase.Execute(this);
    }

    public void RequestInjury()
    {
        OnInjuryInjected?.Invoke();
    }

}