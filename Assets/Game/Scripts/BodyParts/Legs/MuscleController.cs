using System.Drawing;
using TMPro;
using UnityEngine;


namespace Game.Body
{
    public class MuscleController : MonoBehaviour
    {

        [SerializeField] private ELegMuscles _muscleName;

        private GameObject _labelInstance;
        private Transform _camera;
        private Renderer _renderer;

        public ELegMuscles MuscleName => _muscleName;

        private void Start()
        {
            _renderer = GetComponent<Renderer>();
        }
/*
        public void SpawnLabel(MuscleLableController labelPrefab)
        {


            if (_muscleData == null)
            {
                Debug.LogError("MuscleData is NULL on " + gameObject.name);
                return;
            }

            if (_muscleData.labelTarget == null)
            {
                Debug.LogError("labelTarget is NULL on " + gameObject.name);
                return;
            }

            _camera = Camera.main.transform;

            _labelInstance = Instantiate(labelPrefab.gameObject, _muscleData.labelTarget.position, Quaternion.identity);
            // Set text (use GameObject name or custom field)
            var text = _labelInstance.GetComponentInChildren<TextMeshProUGUI>(true);
            text.text = "tt";

            // Parent it (optional, depends if you want it to follow exactly)
            _labelInstance.transform.SetParent(transform);
        }
*/
        void LateUpdate()
        {
            // Make label face camera (billboard)
            if (_labelInstance != null)
            {
                _labelInstance.transform.rotation = Quaternion.LookRotation(
                    _labelInstance.transform.position - _camera.position
                );
            }
        }

        public void ChangeVisual()
        {
            Renderer r = _renderer;

            if (!r)
            {
                Debug.LogError($"Renderer invalid on {name}");
                return;
            }

            try
            {
                Material mat = r.material;

                if (mat == null)
                {
                    Debug.LogError($"Material missing on {name}");
                    return;
                }

                mat.SetColor("_GlowColor", new UnityEngine.Color(0f, 1f, 0f, 0.5f));
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Renderer access failed on {name}: {e}");
            }
        }
    }
}