using Game.Input;
using Unity.Cinemachine;
using UnityEngine;

public class PlayerCameraModule : MonoBehaviour 
{
    public enum CameraMode { Player, Muscle, None }

    [SerializeField] private Transform cameraTarget;
    [SerializeField] private CinemachineOrbitalFollow orbital;
    [SerializeField] private float lookSpeed = 120f;   // now in degrees/sec
    [SerializeField] private float examinSpeed = 2f;
    [SerializeField] private float smoothTime = 0.08f; // lower = snappier

    [SerializeField] private float pitchMin = -10f;
    [SerializeField] private float pitchMax = 10f;

    private PlayerInputModule input;
    private CameraMode mode;

    private float pitch;
    private float yaw;

    private float pitchVelocity;
    private float yawVelocity;

    private float smoothPitch;
    private float smoothYaw;

    public void Initialize(PlayerInputModule inputModule)
    {
        input = inputModule;
    }

    public void SetMode(CameraMode newMode)
    {
        mode = newMode;
    }

    public void Tick()
    {
        switch(mode)
        {
            case CameraMode.None:
                break;
            case CameraMode.Player:
                //ManualLook();
                break;
            case CameraMode.Muscle:
                CinemachineLook();
                break;
        }


    }

    internal void ManualLook(Vector2 inpt)
    {
        Vector2 look = inpt;

        // Accumulate raw targets (degrees/sec * deltaTime)
        yaw += look.x * lookSpeed * Time.deltaTime;
        pitch -= look.y * lookSpeed * Time.deltaTime;
        pitch = Mathf.Clamp(pitch, pitchMin, pitchMax);

        // Smooth toward targets
        smoothYaw = Mathf.SmoothDampAngle(smoothYaw, yaw, ref yawVelocity, smoothTime);
        smoothPitch = Mathf.SmoothDampAngle(smoothPitch, pitch, ref pitchVelocity, smoothTime);

        // Apply — body yaw on the player transform, pitch only on the camera target
        transform.rotation = Quaternion.Euler(0f, smoothYaw, 0f);
        cameraTarget.localRotation = Quaternion.Euler(smoothPitch, 0f, 0f);
    }

    private void CinemachineLook()
    {
        Vector2 look = input._mousePosition;

        orbital.HorizontalAxis.Value += look.x * examinSpeed;
        orbital.VerticalAxis.Value -= look.y * examinSpeed;
    }
}   