 using Game.Body;
using Game.Input;
using Game.Main;
using Game.Minigames;
using Unity.Cinemachine;
using UnityEditor;
using UnityEngine;


namespace Game.Minigames
{
    public class MinigameManager : MonoBehaviour
    {
        public static MinigameManager Instance;

        private MinigameBase _activeMinigame;
        private BodyPartBase _activeBodyPart;

        [SerializeField] private CinemachineCamera _tableCam;

        private void Awake()
        {
            Instance = this;
        }
        public void StartMinigame(BodyPartBase bodyPart)
        {
            Debug.Log($"[Minigame] Start request from BodyPart: {bodyPart.name}");
            _activeBodyPart = bodyPart;
            var prefab = bodyPart.GetMinigame();
            Debug.Log($"[Minigame] Prefab resolved: {prefab}");

            if (prefab == null)
            {
                Debug.LogError("[Minigame] Prefab is NULL");
                return;
            }

            var instance = Instantiate(prefab);
            Debug.Log($"[Minigame] Instance created: {instance.name}");
            _activeMinigame = instance;
            _activeMinigame.Completed += OnMinigameCompleted;
            var receiver = instance.GetComponent<IInputReceiver>();
            Debug.Log($"[Minigame] Receiver found: {receiver}");

            if (receiver == null)
            {
                Debug.LogError("[Minigame] NO IInputReceiver on instance!");
                return;
            }
            Debug.Log("[Minigame] Building context...");

            var context = new MinigameContext
            {
                UI = UIMinigameManager.Instance,
                Audio = AudioMinigameManager.Instance,
                BodyPart = bodyPart,
                Receiver = receiver,
                StartTransform = instance.GetStartTrans(),

            };

            Debug.Log($"[Minigame] Context created:");
            Debug.Log($" - BodyPart: {context.BodyPart}");
            Debug.Log($" - Receiver: {context.Receiver}");
            Debug.Log($" - StartTransform: {context.StartTransform}");

            Debug.Log("[Minigame] Setting active receiver...");
            GameManager.Instance.ChangeReceiver(context.Receiver);

            Debug.Log("[Minigame] Initializing instance...");
            instance.Initialize(context);

            Debug.Log("[Minigame] Starting minigame...");
            instance.StartMinigame();

        }

        public void StartMinigame(MinigameBase prefab, BodyPartBase bodyPart = null)
        {
            Debug.Log($"[Minigame] Starting {prefab.name}");

            var instance = Instantiate(prefab);
            _activeMinigame = instance;
            _activeMinigame.Completed += OnMinigameCompleted;
            var receiver = instance.GetComponent<IInputReceiver>();

            if (receiver == null)
            {
                Debug.LogError("No IInputReceiver found");
                return;
            }

            var context = new MinigameContext
            {
                UI = UIMinigameManager.Instance,
                Audio = AudioMinigameManager.Instance,
                BodyPart = bodyPart,
                Receiver = receiver,
                StartTransform = instance.GetStartTrans(),

            };

            GameManager.Instance.ChangeReceiver(receiver);

            instance.Initialize(context);
            instance.StartMinigame();
        }

        private void OnMinigameCompleted()
        {
            if(_activeBodyPart as FrankensteinController)
            { Debug.Log("test frank");

                var frank = _activeBodyPart as FrankensteinController;
                //frank.FrankensteinMovementModule.MoveToTarget();
                //GameManager.Instance.ChangeToDefaultReceiver();
                //CameraMinigameManager.Instance.VictorCam();
                return;
            }
            Debug.Log("minigame Done");
            CameraMinigameManager.Instance.ChangeTo(_activeBodyPart.GetTransitionCam());

            _activeMinigame.Completed -= OnMinigameCompleted;
            if (_activeMinigame.gameObject != null)
            {

                Destroy(_activeMinigame.gameObject);
            }
            _activeMinigame = null;

            var mini = _activeBodyPart.GetMinigame();
            if (mini != null)
            {
                Debug.Log("next");

                Debug.Log("nn: " + mini);
                _activeMinigame = mini;
                StartMinigame(mini, _activeBodyPart);
            }
            else
            {
                CameraMinigameManager.Instance.ChangeTo(_activeBodyPart.GetTransitionCam());

                Debug.Log("should move");
                MoveToTable();
            }




        }

        private void MoveToTable()
        {
            _activeBodyPart.HopComponent.HopOffObject(() =>
            {
                _activeBodyPart.HopComponent.MoveToTarget(TableFrankenstein.Instance.transform, () =>
                {
                    var tr = TableFrankenstein.Instance.GetTargetOnObject(_activeBodyPart.Type);
                    _activeBodyPart.HopComponent.HopOnObject(tr.position, () =>
                    {
                        CameraMinigameManager.Instance.ChangeTo(_tableCam);

                        _activeBodyPart.HopComponent.AttachToTable(tr, () =>
                        {
                            GameManager.Instance.ChangeToDefaultReceiver();
                            CameraMinigameManager.Instance.VictorCam();
                        });



                    });
                }, 4f);
                //GameManager.Instance.ChangeToDefaultReceiver();
                //CameraMinigameManager.Instance.VictorCam();

            });
        }
    }
}