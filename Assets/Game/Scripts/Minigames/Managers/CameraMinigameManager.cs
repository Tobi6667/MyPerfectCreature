using Unity.Cinemachine;
using UnityEngine;

namespace Game.Minigames
{
    public class CameraMinigameManager : MonoBehaviour
    {
        public static CameraMinigameManager Instance;
      [SerializeField]  private CinemachineCamera _victorCam;
        private CinemachineCamera _activeCam;

        private const int _prioMax = 20;
        private const int _prioMin = 0;

        private void Awake()
        {
            Instance = this;
        }

        public void ChangeTo(CinemachineCamera cam)
        {
            if (_activeCam != null)
                _activeCam.Priority = _prioMin;

            _activeCam = cam;
            _activeCam.Priority = _prioMax;
        }

        public void VictorCam()
        {
            ChangeTo(_victorCam);

        }
    }
}