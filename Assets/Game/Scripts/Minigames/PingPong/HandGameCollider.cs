using UnityEngine;

public class HandGameCollider : MonoBehaviour
{

    private PingPongBallController _controller;


    internal void Initialize(PingPongBallController gameModule)
    {
        _controller = gameModule;
    }
    private void OnTriggerEnter(Collider other)
    {
        // Debug.Log("collided with " + other.name);
        if (other.CompareTag("Ball"))
        {
            _controller.BallOut();
        }
    }
}
