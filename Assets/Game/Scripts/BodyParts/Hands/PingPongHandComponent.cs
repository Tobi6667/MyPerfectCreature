using UnityEngine;

public class PingPongHandComponent : MonoBehaviour
{
    [Header("Hit")]
    [SerializeField] private float _hitSpeed = 12f;
    [SerializeField] private float _maxBounceAngle = 60f; // degrees off straight-back

    private Camera _cam;
    [SerializeField] Transform _minBounds;
    [SerializeField] Transform _maxBounds;
    [SerializeField] float _smoothSpeed = 5f;
    [SerializeField] Vector3 _smoothedPos;

    internal void Initialize()
    {
        _smoothedPos = transform.position;
        _cam = Camera.main;
    }

    private PingPongBallController _ballController;

    internal void SetBallController(PingPongBallController controller) => _ballController = controller;


    internal void Slide(Vector2 screenPos)
    {

        Vector2 viewport = _cam.ScreenToViewportPoint(screenPos);

      //  viewport.x = 1f - viewport.x;


        Vector3 min = _minBounds.position;
        Vector3 max = _maxBounds.position;

        float x = Mathf.Lerp(_minBounds.position.x, _maxBounds.position.x, viewport.x);
        float z = Mathf.Lerp(_minBounds.position.z, _maxBounds.position.z, viewport.y);

        Vector3 target = new Vector3(x, 0, z);

        float t = 1f - Mathf.Exp(-_smoothSpeed * Time.deltaTime);

        _smoothedPos = Vector3.Lerp(_smoothedPos, target, t);

        transform.position = new Vector3(
            _smoothedPos.x,
            transform.position.y,
            _smoothedPos.z
        );
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.collider.CompareTag("Ball")) return;
        if (!collision.collider.TryGetComponent<BulletController>(out var ball)) return;
        _ballController?.PlayerHitBall();
        float width = _maxBounds.position.x - _minBounds.position.x;
        float xOffset = Mathf.Clamp((transform.position.x - _minBounds.position.x) / width * 2f - 1f, -1f, 1f);

        float angle = xOffset * _maxBounceAngle * Mathf.Deg2Rad;
        Vector3 dir = new Vector3(Mathf.Sin(angle), 0f, -1f).normalized; // -Z = back toward enemy

        ball.SetVelocity(dir * _hitSpeed);

    }
}
