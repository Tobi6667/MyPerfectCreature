using Game.Body;
using UnityEngine;

public class ArmFightModule : MonoBehaviour
{
    [SerializeField] private Transform shootPoint;
    [SerializeField] private float distance = 20f;
    [SerializeField] private LayerMask hitMask;
    [SerializeField] private ParticleSystem particleSystemFX;
    [SerializeField] private float hitCooldown = 0.15f;
    private float _lastHitTime;
    private HandController _armController;

    private Vector3 _velocity;
    private Vector3 _lastPos;

    internal void InitializeModule(HandController controller)
    {
        _armController = controller;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.collider.CompareTag("Ball")) return;


        if (Time.time - _lastHitTime < hitCooldown)
            return;

        _lastHitTime = Time.time;


        Rigidbody ball = collision.rigidbody;

        ContactPoint c = collision.GetContact(0);
        //_armController.PlayerHitBall();

        // =========================
        // 1. DEFINE FORWARD DIRECTION (GAME RULE, NOT PHYSICS)
        // =========================

        Vector3 forward = Vector3.forward; // change to back if needed

        // =========================
        // 2. SIDE CONTROL (ONLY INPUT FROM CONTACT POINT)
        // =========================

        float side = (c.point.x - transform.position.x) * 2f;
        side = Mathf.Clamp(side, -1f, 1f);

        Vector3 right = Vector3.right * side;

        // =========================
        // 3. FINAL DIRECTION (NO REFLECTION!)
        // =========================

        Vector3 dir = forward + right;

        dir.y = 0.05f; // fixed small arc
        dir.Normalize();

        // =========================
        // 4. SPEED (FIXED RANGE = NO CHAOS)
        // =========================

        float speed = 11f;

        ball.linearVelocity = dir * speed;

        // =========================
        // 5. IMPORTANT: RESET PHYSICS NOISE
        // =========================

        ball.angularVelocity = Vector3.zero;
    }

    internal void Shoot()
    {
        particleSystemFX.Play();

        Ray ray = new Ray(
            shootPoint.position,
            shootPoint.forward
        );

        Debug.DrawRay(
            ray.origin,
            ray.direction * distance,
            Color.red,
            1f
        );

        if (Physics.Raycast(ray, out RaycastHit hit, distance, hitMask))
        {
            Debug.Log("Hit: " + hit.collider.name);

            Debug.DrawLine(ray.origin,hit.point,Color.green,1f);
            if(hit.collider.TryGetComponent<EnemyPingPongHand>(out var enemyHand))
            {
                enemyHand.PushBack(4f); // Example force value
            }
        }
    }

    void Update()
    {
        _velocity = (transform.position - _lastPos) / Time.deltaTime;
        _lastPos = transform.position;
    }
}