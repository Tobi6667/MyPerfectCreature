using UnityEngine;

public class MoveImagesObject : MovingObjectBase
{
    [Header("REFERENCES")]
    [SerializeField] private Animator _animator;
    [SerializeField] private GameObject _mesh;
    [SerializeField] private ParticleSystem _particleSystem;



    private bool _hasHit;


    internal void ApplyHit()
    {
        _hasHit = true;
        _onHitFrank?.Invoke(this);

        Stop();

        if (_mesh != null)
        {
            Vector3 scale = _mesh.transform.localScale;
            scale.y = 0.1f;
            _mesh.transform.localScale = scale;
        }

        /*    if (_animator != null)
                _animator.SetTrigger(_hitTrigger);
        */
        if (_particleSystem != null)
            _particleSystem.Play();
    }

    protected override void ReachEnd()
    {
        if (_hasHit) return;

        Stop();

        base.ReachEnd();
    }

}
