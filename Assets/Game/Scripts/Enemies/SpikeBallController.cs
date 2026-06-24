using UnityEngine;

public class SpikeBallController : MovingObjectBase
{


    private bool _hasHit;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("hit " + other);


        if (_hasHit) return;

        if (other.CompareTag("Frankenstein"))
        {
            Debug.Log("frank hit ball");
            _onHitFrank?.Invoke(this);
            ApplyHit();
        }
    }

    private void ApplyHit()
    {
        _hasHit = true;

        Stop();
        /*
        if (_mesh != null)
        {
            Vector3 scale = _mesh.transform.localScale;
            scale.y = 0.1f;
            _mesh.transform.localScale = scale;
        }

            if (_animator != null)
                _animator.SetTrigger(_hitTrigger);
        
        if (_particleSystem != null)
            _particleSystem.Play();*/
    }

    protected override void ReachEnd()
    {
        if (_hasHit) return;

        Stop();

        base.ReachEnd();
    }
}
