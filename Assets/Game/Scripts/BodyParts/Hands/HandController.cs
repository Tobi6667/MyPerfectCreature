using UnityEngine;

namespace Game.Body
{
    public class HandController : MonoBehaviour
    {
        [SerializeField] private HopComponent _hopComponent;
        [SerializeField] private PingPongHandComponent _pingPongHandComponent;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            //_hopComponent.Initialize();
            _pingPongHandComponent.Initialize();
            
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
