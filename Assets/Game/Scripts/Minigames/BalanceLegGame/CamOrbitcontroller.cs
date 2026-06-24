using Unity.Cinemachine;
using UnityEngine;

public class CamOrbitcontroller : MonoBehaviour
{
    [SerializeField] private CinemachineCamera cam;
    [SerializeField] private float orbitSpeed = 100f;

    private CinemachineOrbitalFollow orbitalFollow;

    private void Awake()
    {
        orbitalFollow = cam.GetComponent<CinemachineOrbitalFollow>();
    }

    internal void Update()
    {


        //orbitalFollow.HorizontalAxis.Value += input * orbitSpeed * Time.deltaTime;
    }
}