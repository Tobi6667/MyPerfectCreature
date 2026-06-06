using Game.Body;
using Game.Minigames;
using System;
using UnityEngine;

namespace Game.Body
{

    public class LegBalanceController : MonoBehaviour
    {
        public event Action FellOver;

        [SerializeField] private LegController _legController;

        private bool _active;

        public void StartBalance(LegGameRoundData round)
        {

            _active = true;

            _legController.SetRoundData(round);
            _legController.ResetLeg(OnResetComplete);
        }

        public void StopBalance()
        {
            _active = false;
        }

        public void MoveLeg(Vector2 screenPos)
        {
            if (!_active || _legController == null)
                return;

            Camera cam = Camera.main;
            if (cam == null)
                return;

            Ray ray = cam.ScreenPointToRay(screenPos);

            Vector3 targetPoint = _legController.transform.position;

            if (Physics.Raycast(ray, out RaycastHit hit, 100f))
            {
                targetPoint = hit.point;
            }

            _legController.MoveLegRoot(targetPoint);
        }

        private void OnResetComplete()
        {
            Debug.Log("[LegBalanceController] Ready");
        }

        private void OnBalanceLost()
        {
            FellOver?.Invoke();
        }
    }
}