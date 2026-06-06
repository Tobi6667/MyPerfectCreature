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


        [SerializeField] private List<BodyPartBase> _bodypartsList;

        public static GameManager Instance;


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


        private void OnInteract(IInteractable interactor)
        {
            Debug.Log(interactor);
            MinigameManager.Instance.StartMinigame(interactor as BodyPartBase);
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
