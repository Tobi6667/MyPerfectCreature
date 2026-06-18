using System;
using UnityEngine;

[Serializable]
public struct FullbodyModifiers
{
    [Header("Core Movement")]
    public float spineStiffness;        // affects spine wave + bending
    public float pelvisTiltLimit;       // affects SolvePelvisTilt
    public float bendRestriction;       // affects hip/spine coupling

    [Header("Locomotion")]
    public float leftLegWeakness;       // affects foot swing + planting
    public float rightLegWeakness;
    public float instability;           // affects cycle noise + noise injection

    [Header("Upper Body")]
    public float neckProtection;        // reduces head motion
    public float coordinationLoss;      // adds desync/noise to cycles

    [Header("Physiological")]
    public float pain;                  // global damping (optional)
}