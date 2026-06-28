using UnityEngine;
using Game.Input;
using Game.Body;
using Game.Minigames;
using System.Collections.Generic;
using UnityEngine.Playables;
using DG.Tweening;
using System.Collections;

namespace Game.Main
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private VictorController _victorController;
        [SerializeField] private FrankensteinController _frank;
        [SerializeField] private PlayerInputModule _inputModule;
        [SerializeField] private VictorInputReceiver _inputReceiver;
        private BodyPartBase _activeBodyPart;
        [SerializeField] private PlayableDirector _awakeTimeline;

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

        public void FrankReady()
        {
            _awakeTimeline.Play();

            StartCoroutine(CoWait());
        }

        private IEnumerator CoWait()
        {
            yield return new WaitForSeconds(10f);
            _frank.gameObject.SetActive(true);
            _frank.Initialize();
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
