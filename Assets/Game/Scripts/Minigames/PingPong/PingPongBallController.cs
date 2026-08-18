using Game.Body;
using Game.Input;
using Game.Minigames;
using UnityEngine;

public class PingPongBallController : MonoBehaviour
{
    [Header("Ball")]
    [SerializeField] private BulletController _bulletPrefab;
    [SerializeField] private Transform _spawnPosition;
    [SerializeField] private Transform _targetPosition;
    [SerializeField] private float _initSpeedBall;

    [Header("Stuck Ball Recovery")]
    [SerializeField] private float _stuckTimeout = 5f;
    [SerializeField] private float _pushForce = 2f;
    private float _lastHitTime;

    [Header("Refs")]
    [SerializeField] private EnemyPingPongHand _enemyHand;
    [SerializeField] private HandGameCollider _handGameCollider;
    [SerializeField] private HandGameCollider _enemyHandGameCollider;
    private BulletController _activeBall;
    private PingPongGameplayPhase _phase;
    private bool _isPlaying = false;

    private HandController _handController;

    private enum LastHitter { None, Player, Enemy }
    private LastHitter _lastHitter = LastHitter.None;



    public void Initialize(PingPongGameplayPhase phase)
    {
        _phase = phase;
        _handGameCollider.Initialize(this);
        _enemyHandGameCollider.Initialize(this);
        _enemyHand.SetBallController(this);
    }

    private void Update()
    {
        if (!_isPlaying || _activeBall == null)
            return;

        if (Time.time - _lastHitTime >= _stuckTimeout)
        {
            PushStuckBall();
            _lastHitTime = Time.time;
        }
    }

    private void PushStuckBall()
    {
        Vector3 currentVelocity = _activeBall.GetComponent<Rigidbody>().linearVelocity;
        float speed = currentVelocity.magnitude;
        if (speed < 0.01f)
            speed = _initSpeedBall;

        Vector3 sideways = Random.value < 0.5f ? Vector3.left : Vector3.right;
        Vector3 newDir = (currentVelocity.normalized + sideways * 0.5f).normalized;

        _activeBall.SetVelocity(newDir * speed);
    }

    public void StopBall()
    {
        _isPlaying = false;
        if (_activeBall != null)
        {
            Destroy(_activeBall.gameObject);
            _activeBall = null;
        }
    }

    public void StartBall()
    {
        _isPlaying = true;
        SpawnBall();
    }




    public void SpawnBall()
    {
        if (_activeBall != null || !_isPlaying)
            return;

        _activeBall = Instantiate(_bulletPrefab,
            _spawnPosition.position,
            Quaternion.identity);

        _activeBall.Initialize(
            _targetPosition.position,
            _initSpeedBall);

        _enemyHand.SetBallTarget(_activeBall.transform);

        _lastHitTime = Time.time;
    }


    public void PlayerHitBall()
    {
        Debug.Log("player hit");
        _lastHitter = LastHitter.Player;
        _lastHitTime = Time.time;
    }

    public void EnemyHitBall()
    {
        _lastHitter = LastHitter.Enemy;
        _lastHitTime = Time.time;
    }


    public void BallOut(HandGameCollider collider)
    {
        var next = false;
        if (_activeBall != null)
        {
            Destroy(_activeBall.gameObject);
            _activeBall = null;
            
        }

        if (collider == _handGameCollider)
            next = _phase.RegisterPoint(isPlayer: true);
        else if (collider == _enemyHandGameCollider)
            next = _phase.RegisterPoint(isPlayer: false);

        _lastHitter = LastHitter.None;
        if (next)
        {
            SpawnBall();
        }
    }

    public bool HasBall => _activeBall != null;
}