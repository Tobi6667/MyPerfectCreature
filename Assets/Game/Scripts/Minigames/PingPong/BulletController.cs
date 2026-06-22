using UnityEngine;

public class BulletController : MonoBehaviour
{
    private Rigidbody _rigidbody;


    internal void Initialize(Vector3 targetPosition, float initialSpeed)
    {
        _rigidbody = GetComponent<Rigidbody>();

        //Debug.Log(targetPosition);
        Vector3 dir = (targetPosition - transform.position).normalized;



        _rigidbody.linearVelocity = dir * initialSpeed;
    }
    void FixedUpdate()
    {
        if(!_rigidbody) return;
      // _rigidbody.linearVelocity = Vector3.ClampMagnitude(_rigidbody.linearVelocity, 12f);
    }
}
