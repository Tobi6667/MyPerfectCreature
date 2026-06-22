using System;
using UnityEngine;

public abstract class MovingObjectBase : MonoBehaviour
{
    protected float _speed = 100f;
    protected bool _active = true;

    protected Vector3 _direction = Vector3.forward;

    protected Action<MovingObjectBase> _onReachedEnd;
    protected Action<MovingObjectBase> _onHitFrank;

    protected Transform _endPoint;

    public virtual void Initialize(
        float speed,
        Vector3 direction,
        Transform endPoint,
        Action<MovingObjectBase> onReachedEnd = null,
        Action<MovingObjectBase> onHitFrank = null)
    {
        _speed = speed;
        _direction = direction.normalized;
        _endPoint = endPoint;

        _active = true;

        _onReachedEnd = onReachedEnd;
        _onHitFrank = onHitFrank;
    }

    protected virtual void Update()
    {
        if (!_active) return;

        Move();
        Tick();
        //CheckReachedEnd();
    }

    protected virtual void Move()
    {

        transform.position += _direction * _speed * Time.deltaTime;
    }

    protected virtual void Tick() { }

    protected virtual void CheckReachedEnd()
    {
        if (_endPoint == null) return;

        Vector3 toTarget = _endPoint.position - transform.position;

        float remaining = Vector3.Dot(toTarget, _direction);

        if (remaining <= 0f)
        {
            ReachEnd();
        }
    }

    protected virtual void ReachEnd()
    {
        _active = false;
        _onReachedEnd?.Invoke(this);
    }

    public virtual void Stop() => _active = false;

    public virtual void Continue()
    {
        if (_speed <= 0f) _speed = 1f; // safety fallback
        _active = true;
    }

    public virtual void DestroyObject()
    {
        _active = false;
        //_onDestroyed?.Invoke(this);
        Destroy(gameObject);
    }
}