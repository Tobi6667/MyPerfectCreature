using Game.Body;
using Game.Input;
using Game.Main;
using Game.Minigames;
using UnityEngine;


namespace Game.Minigames
{
    public class MinigameManager : MonoBehaviour
    {
        public static MinigameManager Instance;

        private MinigameBase _activeMinigame;

        private void Awake()
        {
            Instance = this;
        }
        public void StartMinigame(BodyPartBase bodyPart)
        {
            Debug.Log($"[Minigame] Start request from BodyPart: {bodyPart.name}");

            var prefab = bodyPart.GetMinigame();
            Debug.Log($"[Minigame] Prefab resolved: {prefab}");

            if (prefab == null)
            {
                Debug.LogError("[Minigame] Prefab is NULL");
                return;
            }

            var instance = Instantiate(prefab);
            Debug.Log($"[Minigame] Instance created: {instance.name}");

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
            _activeMinigame.Completed -= OnMinigameCompleted;

            _activeMinigame = null;

        }
    }
}