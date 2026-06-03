using UnityEngine;

public class PingPongHandComponent : MonoBehaviour
{

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

}
