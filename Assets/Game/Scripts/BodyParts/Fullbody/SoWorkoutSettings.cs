using UnityEngine;
using UnityEngine.Animations.Rigging;

[CreateAssetMenu(
    fileName = "IKWorkoutSettings",
    menuName = "Workout/IK Workout Settings"
)]
public class SoWorkoutSettings : ScriptableObject
{
    [Header("Snake Motion")]
    public float snakeHeadAmount = 0.22f;
    public float snakePelvisAmount = 0.12f;
    public float snakeFeetAmount = 0.08f;

    public float snakeWaveSpeed = 6f;
    public float snakeWaveOffset = 0.45f;

    [Header("Spine Input")]
    public float spineSideAmount = 0.25f;
    public float spineInputSmooth = 8f;

    [Header("Movement")]
    public float walkSpeed = 1.5f;
    public float hipHeightOffset = 0.9f;

    [Header("Smoothing")]
    public float hipSmooth = 8f;
    public float spineSmooth = 10f;

    [Header("Spine")]
    public float spineForwardLean = 0.2f;

    [Header("Pelvis")]
    public float pelvisTilt = 8f;
    public float pelvisReturnSpeed = 6f;
    public float stanceWidth = 0.18f;

    public float hipLeadAmount = 0.25f;
    public float swingArcHeight = 0.35f;
    public float strideLength = 0.6f;

    [Header("Foot Planting")]
    public float plantThreshold = 0.2f;
    public float plantSmooth = 15f;

    [Header("Snake Chain")]
    public float snakeAmplitude = 0.25f;
    public float snakeFrequency = 2f;

    public float spineDelay = 0.15f;
    public float pelvisDelay = 0.3f;
    public float feetDelay = 0.45f;

    [Header("Snake Root Motion")]
    public float snakeRootAmount = 0.35f;
    public float snakeRootSpeed = 5f;
    public float snakeRootLag = 0.15f;


}