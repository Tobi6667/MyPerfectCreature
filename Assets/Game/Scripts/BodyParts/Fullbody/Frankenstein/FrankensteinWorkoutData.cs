using Game.Minigames;
using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class FrankensteinWorkoutData
{
    [Header("Workout")]
    public string workoutName;
    public AnimationClip workoutClip;
    public Transform workoutPosition;
    public int vasCosts;
    public int fatigueCosts;

    [Header("IK")]
    public List<WorkoutIKTarget> ikTargets;
    public SoWorkoutSettings workoutSettings;
    public WorkoutType workoutType;
    public MinigameBase _minigame;


}


public enum WorkoutType
{
    Default,
    Game
}


public class ActiveWorkoutRuntime
{
    public FrankensteinWorkoutData workoutData;

    public float elapsedTime;

    public float fatigue;

    public float pain;

    public float stabilityScore = 1f;

    public bool failed;

    public Dictionary<string, float> bodyPartInstability =
        new Dictionary<string, float>();
}