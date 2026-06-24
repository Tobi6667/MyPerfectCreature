using UnityEngine;

public class EyeBallController : MonoBehaviour
{
    [Header("Local Eye Movement")]
    [SerializeField] private float yawRange = 60f;
    [SerializeField] private float pitchRange = 35f;

    [Header("Noise Speed")]
    [SerializeField] private float yawNoiseSpeed = 0.8f;
    [SerializeField] private float pitchNoiseSpeed = 0.9f;

    [Header("Micro Saccades")]
    [SerializeField] private float microYawAmount = 3f;
    [SerializeField] private float microPitchAmount = 2f;

    [SerializeField] private float microYawSpeed = 30f;
    [SerializeField] private float microPitchSpeed = 27f;

    [Header("World Drift")]
    [SerializeField] private bool addWorldSpaceDrift = true;

    [SerializeField] private float worldYawRange = 8f;
    [SerializeField] private float worldPitchRange = 5f;
    [SerializeField] private float worldRollRange = 3f;

    [SerializeField] private float worldNoiseSpeed = 0.15f;

    [Header("Smoothing")]
    [SerializeField] private float rotationSmoothness = 12f;

    private Quaternion initialWorldRotation;

    void Start()
    {
        initialWorldRotation = transform.rotation;
    }

    void Update()
    {
        float t = Time.time;

        // -------------------------
        // LOCAL EYEBALL MOVEMENT
        // -------------------------

        float yaw =
            (Mathf.PerlinNoise(t * yawNoiseSpeed, 0f) - 0.5f) * yawRange;

        float pitch =
            (Mathf.PerlinNoise(0f, t * pitchNoiseSpeed) - 0.5f) * pitchRange;

        // micro saccades
        yaw += Mathf.Sin(t * microYawSpeed) * microYawAmount;
        pitch += Mathf.Cos(t * microPitchSpeed) * microPitchAmount;

        Quaternion localEyeRotation = Quaternion.Euler(
            pitch,
            yaw,
            0f
        );

        // -------------------------
        // WORLD DRIFT
        // -------------------------

        Quaternion worldDrift = Quaternion.identity;

        if (addWorldSpaceDrift)
        {
            float worldYaw =
                (Mathf.PerlinNoise(t * worldNoiseSpeed, 10f) - 0.5f)
                * worldYawRange;

            float worldPitch =
                (Mathf.PerlinNoise(20f, t * worldNoiseSpeed) - 0.5f)
                * worldPitchRange;

            float worldRoll =
                (Mathf.PerlinNoise(t * worldNoiseSpeed, 30f) - 0.5f)
                * worldRollRange;

            worldDrift = Quaternion.Euler(
                worldPitch,
                worldYaw,
                worldRoll
            );
        }

        Quaternion finalRotation =
            initialWorldRotation *
            worldDrift *
            localEyeRotation;

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            finalRotation,
            Time.deltaTime * rotationSmoothness
        );
    }
}