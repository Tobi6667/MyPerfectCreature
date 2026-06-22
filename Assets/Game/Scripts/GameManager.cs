using UnityEngine;
using Game.Input;
using Game.Body;
using Game.Minigames;
using System.Collections.Generic;

namespace Game.Main
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private VictorController _victorController;

        [SerializeField] private PlayerInputModule _inputModule;
        [SerializeField] private VictorInputReceiver _inputReceiver;
        private BodyPartBase _activeBodyPart;

        [SerializeField] private List<BodyPartBase> _bodypartsList;

        public static GameManager Instance;
        public BodyPartBase ActiveBodyPart => _activeBodyPart;

        private void Awake()
        {
            Instance = this;
        }

        void Start()
        {

            foreach (var part in _bodypartsList)
            {
                part.Initialize();
            }

            _victorController.Initialize((interactor) =>
            {
                Debug.Log(interactor);
                OnInteract(interactor);
            });
            _inputModule.SetReceiver(_inputReceiver);

        }

        public void ChangeReceiver(IInputReceiver inputReceiver)
        {
            Debug.Log(inputReceiver);
            _inputModule.SetReceiver(inputReceiver);
        }

        public void ChangeToDefaultReceiver()
        {
            _inputModule.SetReceiver(_inputReceiver);
        }


        private void OnInteract(IInteractable interactor)
        {
            _activeBodyPart = interactor as BodyPartBase;
            if (interactor as FrankensteinController)
            {
                MinigameManager.Instance.StartMinigame(interactor as BodyPartBase);
                // FrankWorkoutManager.Instance.OpenWorkoutSelection();
                Debug.Log("dgfdfdfdfdfdAAAAA");
                return;
            }
            Debug.Log(interactor);
            MinigameManager.Instance.StartMinigame(interactor as BodyPartBase);
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
