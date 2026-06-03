using UnityEngine;
using GameInput;
using Game.Input;
using Game.Body;
using Game.Minigames;

namespace Game.Main
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private VictorController _victorController;
        //  [SerializeField] private BodyPartBase _handController;
        [SerializeField] private PingPongManager _pingPongManager;
        [SerializeField] private PlayerInputModule _inputModule;
        [SerializeField] private PingPongInputReceiver _pingPongReceiver;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            //_victorController.Initialize();
          //  _handController.Initialize();
            _inputModule.SetReceiver(_pingPongReceiver);
           // _pingPongManager.StartMinigame();

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
