using System;
using UnityEngine;

[Serializable]
public class BodyState
{


    [Range(0f, 1f)] public float fullBodyInjury;
    [Range(0f, 1f)] public float leftLegInjury;
    [Range(0f, 1f)] public float rightLegInjury;
    [Range(0f, 1f)] public float spineInjury;
    [Range(0f, 1f)] public float headInjury;

    [Range(0f, 1f)] public float exhaustion;
    [Range(0f, 1f)] public float pain;
    [Range(0f, 1f)] public float instability;

    // NEW (IMPORTANT FOR YOUR SYSTEM)
    [Range(0f, 1f)] public float coordinationLoss;
    [Range(0f, 1f)] public float bendRestriction;

    // Optional runtime helper (not serialized)
    [NonSerialized] public FullbodyModifiers modifiers;




}