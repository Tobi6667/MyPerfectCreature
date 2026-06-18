using System;
using UnityEngine;
using UnityEngine.Animations.Rigging;

[System.Serializable]
public class WorkoutIKTarget
{
    public string targetName;

    [Header("Tracked Bone")]
    public Transform trackedBone;

    [Header("Target")]
    public Transform target;

    [Header("Animation")]
    public bool animateTarget = true;
    public Vector3 localOffset;
    public Vector3 localRotationOffset;

    [Header("Motion")]
    public AnimationCurve moveCurve =
        AnimationCurve.Linear(0, 0, 1, 1);

    public float moveAmount = 0.1f;

    public Vector3 moveDirection = Vector3.up;

    [Header("Timing")]
    public float speed = 1f;
    public float delay = 0f;

    [Header("IK")]
    public IKTarget ikConstraint;

    [Header("Runtime")]
    public float currentWeight = 0f;

    [Header("Settings")]
    public bool enabled = true;
}