using DG.Tweening;
using System;
using UnityEngine;

public class TorsoMovementModule : MonoBehaviour
{
    [Header("Animators")]
    [SerializeField] private Animator _animatorOne;
    [SerializeField] private Animator _animatorTwo;
    [SerializeField] private Transform cameraReference;

    [Header("Spine")]
    [SerializeField] private Transform[] spineBones;

    private TorsoInjuryData _currentInjury;
    private Action onBang; 
    [Header("Bang")]
    [SerializeField] private float _bangAmount = 45f;
    [SerializeField] private float _bangCooldownTime = 0.3f;
    [SerializeField] private float _bangRecoverSpeed = 0.5f;

    [SerializeField] private Transform _headOrigin;
    [SerializeField] private float _bangRadius = 5.5f;


    private float _bangCooldown;
    private float _bangCurrent;

    private Quaternion[] _rest;
    private Quaternion[] _frozenPose;

    private float _hopHeight = 1f;
    private float _hopDuration = 1f;

    private bool _idle;
    private bool _isImpactLocked;

    private float _impactRecover;

    [SerializeField] private float _impactRecoverSpeed = 6f;

    internal void Initialize(Action onBangWall)
    {
        _rest = new Quaternion[spineBones.Length];
        _frozenPose = new Quaternion[spineBones.Length];
        onBang = onBangWall;
        for (int i = 0; i < spineBones.Length; i++)
            _rest[i] = spineBones[i].localRotation;
    }

    internal void Idle()
    {
        _idle = true;
        _animatorOne.enabled = false;
        _animatorTwo.enabled = false;
    }

    internal void InjectInjury(TorsoInjuryData injury)
    {
        _currentInjury = injury;
    }

    internal void RemoveInjury()
    {
        _currentInjury = null;
    }

    internal void Move(Vector2 input)
    {
        Vector3 right = cameraReference.right;
        right.y = 0f;
        right.Normalize();

        transform.position += right * input.x * 6f * Time.deltaTime;
    }

    internal void HopInput(Vector3 target)
    {
        Vector3 start = transform.position;
        target *= 10f;

        DOTween.To(
            () => 0f,
            t =>
            {
                float x = Mathf.Lerp(start.x, target.x, t);
                float z = Mathf.Lerp(start.z, target.z, t);
                float baseY = Mathf.Lerp(start.y, target.y, t);
                float arc = Mathf.Sin(t * Mathf.PI) * _hopHeight;

                transform.position = new Vector3(x, baseY + arc, z);
            },
            1f,
            _hopDuration
        )
        .SetEase(Ease.Linear);
    }
    internal void BangHead(bool pressed)
    {
        if (_bangCooldown > 0f) return;
        if (_isImpactLocked) return;
        Collider[] hits = Physics.OverlapSphere(
            _headOrigin.position,
            _bangRadius
        );

        bool hitTorso = false;

        for (int i = 0; i < hits.Length; i++)
        {

            Debug.Log($"[Bang] Hit {i}: {hits[i].name} | tag: {hits[i].tag} | layer: {LayerMask.LayerToName(hits[i].gameObject.layer)}");

            if (hits[i].CompareTag("image"))
            {
                hitTorso = true;
                var img = hits[i].GetComponent<MoveImagesObject>();
                img.ApplyHit();
                break;
            }
        }

        if (!hitTorso)
        {
            onBang?.Invoke();
            Debug.Log("hit nothing");
            
        }


        _bangCurrent = 1f;
        _bangCooldown = _bangCooldownTime;
    }

    internal void ReleaseBang()
    {
        // no longer needed
    }

    internal void OnHeadImpact()
    {
        if (_isImpactLocked)
            return;

        
        _isImpactLocked = true;
        _impactRecover = 0f;

        for (int i = 0; i < spineBones.Length; i++)
            _frozenPose[i] = spineBones[i].localRotation;
    }

    private void LateUpdate()
    {
        if (_bangCooldown > 0f)
            _bangCooldown -= Time.deltaTime;

        if (_isImpactLocked)
        {
            _impactRecover += Time.deltaTime * _impactRecoverSpeed;

            for (int i = 0; i < spineBones.Length; i++)
            {
                spineBones[i].localRotation = Quaternion.Slerp(
                    _frozenPose[i],
                    _rest[i],
                    _impactRecover
                );
            }

            if (_impactRecover >= 1f)
                _isImpactLocked = false;

            return;
        }

        // automatic bang recovery
        _bangCurrent = Mathf.MoveTowards(
            _bangCurrent,
            0f,
            Time.deltaTime * _bangRecoverSpeed
        );

        int last = Mathf.Max(spineBones.Length - 1, 1);

        for (int i = 0; i < spineBones.Length; i++)
        {
            float t = (float)i / last;

            float sway = Mathf.Sin(Time.time * 1.0f + i) * 6f * t;
            float twist = Mathf.Sin(Time.time * 0.7f + i) * 10f * t;
            float breathe = Mathf.Sin(Time.time * 1.2f) * 3f * t;

            Quaternion idleRot =
                Quaternion.Euler(breathe, twist, sway);

            Quaternion injuryRot = Quaternion.identity;
            float bangMask = 1f;

            if (_currentInjury != null)
            {
                injuryRot = Quaternion.Euler(
                    _currentInjury.spineTilt,
                    _currentInjury.twistOffset,
                    UnityEngine.Random.Range(
                        -_currentInjury.idleShake,
                        _currentInjury.idleShake)
                );

                bangMask = 0f;
            }

            float rootWeight = (1f - t) * (1f - t);

            float bang =
                _bangCurrent *
                _bangAmount *
                rootWeight *
                bangMask;

            Quaternion bangRot =
                Quaternion.AngleAxis(
                    bang,
                    Vector3.right);

            spineBones[i].localRotation =
                _rest[i] *
                idleRot *
                injuryRot *
                bangRot;
        }
    }
}