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

    [Header("Refs")]
    [SerializeField] private EnemyPingPongHand _enemyHand;
    [SerializeField] private HandGameCollider _handGameCollider;
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
        _enemyHand.SetBallController(this);
    }

    public void StopBall()
    {
        _isPlaying = false;
        if(_activeBall != null)
        {
            Destroy(_activeBall.gameObject);
            _activeBall = null;
        }
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

        _activeBall = Instantiate(_bulletPrefab,
            _spawnPosition.position,
            Quaternion.identity);

        _activeBall.Initialize(
            _targetPosition.position,
            _initSpeedBall);

        _enemyHand.SetBallTarget(_activeBall.transform);
    }


    public void PlayerHitBall()
    {
        Debug.Log("player hit");
        _lastHitter = LastHitter.Player;
    }

    public void EnemyHitBall()
    {
        _lastHitter = LastHitter.Enemy;
    }


    public void BallOut()
    {
        var next = false;
        if (_activeBall != null)
        {
            Destroy(_activeBall.gameObject);
            _activeBall = null;
        }
      
        if (_lastHitter == LastHitter.Player)
            next = _phase.RegisterPoint(isPlayer: true);
        else if (_lastHitter == LastHitter.Enemy)
           next = _phase.RegisterPoint(isPlayer: false);

        _lastHitter = LastHitter.None;
        if (next)
        {
            SpawnBall();
        }
    }

    public bool HasBall => _activeBall != null;
}