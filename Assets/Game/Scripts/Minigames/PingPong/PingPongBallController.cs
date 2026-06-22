using Game.Minigames;
using UnityEngine;

public class PingPongBallController : MonoBehaviour
{
    [Header("Ball")]
    [SerializeField] private BulletController _bulletPrefab;
    [SerializeField] private Transform _spawnPosition;
    [SerializeField] private Transform _targetPosition;
    [SerializeField] private float _initSpeedBall;

    [Header("Refs")]
    [SerializeField] private EnemyPingPongHand _enemyHand;
    [SerializeField] private HandGameCollider _handGameCollider;
    private BulletController _activeBall;
    private PingPongGameplayPhase _phase;
    private bool _isPlaying = false;

    public void Initialize(
        PingPongGameplayPhase phase)
    {
        _phase = phase;
        _handGameCollider.Initialize(this);
   
    }

    public void StopBall()
    {
        _isPlaying = false;
    }

    public void StartBall()
    {
        _isPlaying=true;
        SpawnBall();
    }


    public void SpawnBall()
    {
        if (_activeBall != null || !_isPlaying)
            return;

        _activeBall = Instantiate(
            _bulletPrefab,
            _spawnPosition.position,
            Quaternion.identity);

        _activeBall.Initialize(
            _targetPosition.position,
            _initSpeedBall);

        _enemyHand.SetBallTarget(_activeBall.transform);
    }

    public void PlayerHitBall()
    {
        _phase.RegisterHit();
    }

    public void EnemyHitBall()
    {
    }

    public void BallOut()
    {
        if (_activeBall != null)
        {
            Destroy(_activeBall.gameObject);
            _activeBall = null;
        }

        SpawnBall();
    }

    public bool HasBall => _activeBall != null;
}