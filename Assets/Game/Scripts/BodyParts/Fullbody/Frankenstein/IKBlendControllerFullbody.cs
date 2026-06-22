using Game.Body;
using System.Collections;
using UnityEngine;

public class IKBlendController : MonoBehaviour
{
    [Header("SETTINGS")]
    private SoWorkoutSettings workoutSettings;

    [Header("REFERENCES")]
    [SerializeField] private Animator animator;
    [SerializeField] private Transform rootReference;

    [SerializeField] private Transform leftFootTarget;
    [SerializeField] private Transform rightFootTarget;

    [SerializeField] private Transform leftFootHint;
    [SerializeField] private Transform rightFootHint;

    [SerializeField] private Transform hipsTarget;
    [SerializeField] private Transform spineTarget;
    [SerializeField] private Transform headTarget;

    [Header("GRAPH")]
    public BodyParameterGraph graph;

    public BodyState injuryState = new BodyState();
    private RuntimeWorkoutSettings runtime;
    private SoWorkoutSettings baseSettings;
    private float spineMod = 1f;
    private float pelvisMod = 1f;
    private float instabilityMod = 1f;
    private float legModL = 1f;
    private float legModR = 1f;
    private float painMod = 0f;
    private float neckMod = 1f;

    private float _snakeTime;
    private Coroutine _jumpRoutine;
    // INPUT SYSTEM (FIXED + DECOUPLED)
    private float spineInputTarget;
    private float _spineInputSmooth;

    private Vector3 _rootStartLocalPos;

    private float _leftWeightSmooth;
    private float _rightWeightSmooth;

    private Vector3 leftPlantPos;
    private Vector3 rightPlantPos;

    private bool leftPlanted;
    private bool rightPlanted;

    private bool _isActive = false;

    private float groundY;
    private float proceduralPhase;

    private float stepLength;
    private float stepHeight;
    private float rightStrideMultiplier;
    private float rightHeightMultiplier;
    private float hipSideShift;
    private float instability;

    private Vector3 forward;
    private Vector3 side;

    private Vector3 leftPos;
    private Vector3 rightPos;

    private float leftCycle;
    private float rightCycle;

    private float leftLift;
    private float rightLift;

    private float stanceWidthOffset;

    private Vector3 spineBaseWorld;

    // ─────────────────────────────
    // INIT
    // ─────────────────────────────


    internal void Initialize()
    {

    }

    internal void SetStartData(SoWorkoutSettings settings)
    {
        workoutSettings = settings;
        CopyToRuntime(settings);
        groundY = rootReference.position.y;

        spineBaseWorld = spineTarget.position;
        _rootStartLocalPos = rootReference.localPosition;

        UpdateWorldSpace();

        Vector3 rootFlat = rootReference.position;
        rootFlat.y = groundY;

        Vector3 hipLead = rootReference.forward * runtime.hipLeadAmount;
        Vector3 hipCenter = rootFlat + hipLead;

        leftPos = hipCenter - side * runtime.stanceWidth;
        rightPos = hipCenter + side * runtime.stanceWidth;

        leftPos.y = groundY;
        rightPos.y = groundY;

        leftPlantPos = leftPos;
        rightPlantPos = rightPos;

        leftPlanted = true;
        rightPlanted = true;

        _isActive = true;
    }

    // ─────────────────────────────
    // UPDATE
    // ─────────────────────────────

    void LateUpdate()
    {
        if (!_isActive || workoutSettings == null || graph == null) return;

        ApplyInjuryState();
        ApplyInjuryToSettings();

        UpdateWorldSpace();
        UpdateGraphValues();
        UpdateProceduralCycle();

        SolveSnakeRootMotion();
        SolveFeet();
        ApplyFootTargets();
        SolveHints();

        SolveHips();
        SolvePelvisTilt();
        SolveSpine();
    }

    // ─────────────────────────────
    // ACTIVATION
    // ─────────────────────────────

    internal void ActivateIK() => _isActive = true;
    internal void DeactivateIK() => _isActive = false;

    // ─────────────────────────────
    // WORLD SPACE
    // ─────────────────────────────

    void UpdateWorldSpace()
    {
        forward = Vector3.ProjectOnPlane(rootReference.forward, Vector3.up).normalized;
        side = Vector3.Cross(Vector3.up, forward);
    }

    // ─────────────────────────────
    // GRAPH
    // ─────────────────────────────

    void UpdateGraphValues()
    {
        stepLength = graph.Evaluate("stepLength");
        stepHeight = graph.Evaluate("stepHeight");
        rightStrideMultiplier = graph.Evaluate("rightStride");
        rightHeightMultiplier = graph.Evaluate("rightHeight");
        hipSideShift = graph.Evaluate("hipSideShift");
        instability = graph.Evaluate("instability");
    }

    // ─────────────────────────────
    // WALK CYCLE
    // ─────────────────────────────

    void UpdateProceduralCycle()
    {
        proceduralPhase += Time.deltaTime * runtime.walkSpeed;

        float cycle = Mathf.Repeat(proceduralPhase, 1f);

        cycle += Mathf.Sin(Time.time * 7f)
                 * instability
                 * instabilityMod
                 * 0.05f;

        cycle = Mathf.Repeat(cycle, 1f);

        leftCycle = cycle;
        rightCycle = Mathf.Repeat(cycle + 0.5f, 1f);

        leftLift = Mathf.Sin(leftCycle * Mathf.PI);
        rightLift = Mathf.Sin(rightCycle * Mathf.PI);
    }

    // ─────────────────────────────
    // INPUT API (FIXED)
    // ─────────────────────────────

    internal void SetSpineInput(float input)
    {
        // normalized: 0–1920 → -1..1
        float n = Mathf.InverseLerp(0f, 1920f, input) * 2f - 1f;
        spineInputTarget = Mathf.Clamp(n, -1f, 1f) * 10f;
        Debug.Log("inp: " + spineInputTarget);
    }

    // ─────────────────────────────
    // FEET
    // ─────────────────────────────

    void SolveFeet()
    {
        Vector3 rootFlat = rootReference.position;
        rootFlat.y = groundY;

        float leftSwing01 = Mathf.Clamp01(leftLift) * legModL;
        float rightSwing01 = Mathf.Clamp01(rightLift) * legModR;

        Vector3 hipLead = forward * runtime.hipLeadAmount;
        Vector3 hipCenter = rootFlat + hipLead;

        Vector3 leftTarget =
            Vector3.Lerp(
                hipCenter - side * (runtime.stanceWidth + stanceWidthOffset) - forward * runtime.strideLength * 0.5f,
                hipCenter - side * (runtime.stanceWidth + stanceWidthOffset) + forward * runtime.strideLength * 0.5f,
                leftCycle
            );

        Vector3 rightTarget =
            Vector3.Lerp(
                hipCenter + side * (runtime.stanceWidth + stanceWidthOffset) - forward * runtime.strideLength * 0.5f,
                hipCenter + side * (runtime.stanceWidth + stanceWidthOffset) + forward * runtime.strideLength * 0.5f,
                rightCycle
            );

        leftTarget.y = groundY + leftSwing01 * runtime.swingArcHeight;
        rightTarget.y = groundY + rightSwing01 * runtime.swingArcHeight;

        HandleFoot(ref leftPos, ref leftPlantPos, ref leftPlanted, leftTarget, leftSwing01);
        HandleFoot(ref rightPos, ref rightPlantPos, ref rightPlanted, rightTarget, rightSwing01);
    }

    void HandleFoot(ref Vector3 footPos, ref Vector3 plantPos, ref bool planted, Vector3 target, float swing01)
    {
        if (planted && swing01 > runtime.plantThreshold)
            planted = false;

        if (!planted && swing01 < runtime.plantThreshold)
        {
            planted = true;
            plantPos = target;
        }

        footPos = planted
            ? plantPos
            : Vector3.Lerp(footPos, target, Time.deltaTime * runtime.plantSmooth);
    }

    void ApplyFootTargets()
    {
        leftFootTarget.position = leftPos;
        rightFootTarget.position = rightPos;
    }

    void SolveHints()
    {
        leftFootHint.position = leftPos + forward * 0.2f;
        rightFootHint.position = rightPos + forward * 0.2f;
    }

    // ─────────────────────────────
    // HIPS
    // ─────────────────────────────

    void SolveHips()
    {
        Vector3 mid = (leftPos + rightPos) * 0.5f;

        Vector3 worldTarget =
            new Vector3(
                mid.x,
                groundY + runtime.hipHeightOffset,
                mid.z
            );

        Vector3 localTarget = hipsTarget.parent.InverseTransformPoint(worldTarget);

        hipsTarget.localPosition =
            Vector3.Lerp(hipsTarget.localPosition, localTarget,
            Time.deltaTime * runtime.hipSmooth * pelvisMod);
    }

    void SolvePelvisTilt()
    {
        float leftWeight = leftPlanted ? 1f : 0f;
        float rightWeight = rightPlanted ? 1f : 0f;

        _leftWeightSmooth = Mathf.Lerp(_leftWeightSmooth, leftWeight, Time.deltaTime * 8f);
        _rightWeightSmooth = Mathf.Lerp(_rightWeightSmooth, rightWeight, Time.deltaTime * 8f);

        float bias = _rightWeightSmooth - _leftWeightSmooth;

        Quaternion targetTilt =
            Quaternion.Euler(0f, 0f, bias * runtime.pelvisTilt * pelvisMod);

        hipsTarget.localRotation =
            Quaternion.Slerp(hipsTarget.localRotation, targetTilt,
            Time.deltaTime * runtime.pelvisReturnSpeed);
    }

    // ─────────────────────────────
    // SPINE (FIXED INPUT DECOUPLING)
    // ─────────────────────────────

    void SolveSpine()
    {
        // SMOOTH ONLY (response layer)
        _spineInputSmooth =
            Mathf.Lerp(_spineInputSmooth, spineInputTarget,
            Time.deltaTime * runtime.spineInputSmooth);

        float inputResponse = _spineInputSmooth * spineMod;
        float baseInput = inputResponse * runtime.spineSideAmount;

        float time = Time.time * runtime.snakeFrequency;

        float headWave = Mathf.Sin(time) * runtime.snakeAmplitude;
        float spineWave = Mathf.Sin(time - runtime.spineDelay) * runtime.snakeAmplitude;

        Vector3 headTargetPos =
            hipsTarget.position +
            Vector3.up * 1.15f +
            forward * runtime.spineForwardLean +
            side * (headWave * neckMod + baseInput);

        headTarget.position =
            Vector3.Lerp(headTarget.position, headTargetPos,
            Time.deltaTime * runtime.spineSmooth);

        Vector3 spinePos =
            hipsTarget.position +
            Vector3.up * 0.8f +
            forward * runtime.spineForwardLean +
            side * (spineWave * spineMod + baseInput * 0.5f);

        spineTarget.position =
            Vector3.Lerp(spineTarget.position, spinePos,
            Time.deltaTime * runtime.spineSmooth);
    }

    // ─────────────────────────────
    // ROOT MOTION
    // ─────────────────────────────

    void SolveSnakeRootMotion()
    {
        _snakeTime += Time.deltaTime * runtime.snakeRootSpeed;
        float headWave = Mathf.Sin(_snakeTime) * runtime.snakeRootAmount;

        float inputOffset = _spineInputSmooth * runtime.sideRootAmount; // new

        Vector3 rootOffset = side * (headWave + inputOffset);

        rootReference.localPosition = Vector3.Lerp(
            rootReference.localPosition,
            _rootStartLocalPos + rootOffset,
            Time.deltaTime * 8f);
    }

    // ─────────────────────────────
    // INJURY
    // ─────────────────────────────

    void ApplyInjuryState()
    {
        instabilityMod = 1f - injuryState.instability;
        spineMod = 1f - injuryState.spineInjury;
        pelvisMod = 1f - injuryState.fullBodyInjury;
        legModL = 1f - injuryState.leftLegInjury;
        legModR = 1f - injuryState.rightLegInjury;
        neckMod = 1f - injuryState.headInjury;
        painMod = injuryState.pain;
    }


    internal void InjectInjury(FullbodyInjuryEntry injury)
    {



        var mod = injury.modifiers;
        injuryState.spineInjury = Mathf.Clamp01(mod.spineStiffness);
        injuryState.fullBodyInjury = Mathf.Clamp01(mod.bendRestriction);
        injuryState.leftLegInjury = Mathf.Clamp01(mod.leftLegWeakness);
        injuryState.rightLegInjury = Mathf.Clamp01(mod.rightLegWeakness);
        injuryState.instability = Mathf.Clamp01(mod.instability);
        injuryState.headInjury = Mathf.Clamp01(1f - mod.neckProtection);
        injuryState.pain = Mathf.Clamp01(mod.pain);

        injuryState.coordinationLoss = Mathf.Clamp01(mod.coordinationLoss);
        injuryState.bendRestriction = Mathf.Clamp01(mod.bendRestriction);
    }

    private void CopyToRuntime(SoWorkoutSettings settings)
    {

        baseSettings = settings;

        runtime = new RuntimeWorkoutSettings
        {
            snakeHeadAmount = settings.snakeHeadAmount,
            snakePelvisAmount = settings.snakePelvisAmount,
            snakeFeetAmount = settings.snakeFeetAmount,

            snakeWaveSpeed = settings.snakeWaveSpeed,
            snakeWaveOffset = settings.snakeWaveOffset,

            spineSideAmount = settings.spineSideAmount,
            spineInputSmooth = settings.spineInputSmooth,

            walkSpeed = settings.walkSpeed,
            hipHeightOffset = settings.hipHeightOffset,

            hipSmooth = settings.hipSmooth,
            spineSmooth = settings.spineSmooth,

            spineForwardLean = settings.spineForwardLean,

            pelvisTilt = settings.pelvisTilt,
            pelvisReturnSpeed = settings.pelvisReturnSpeed,
            stanceWidth = settings.stanceWidth,

            hipLeadAmount = settings.hipLeadAmount,
            swingArcHeight = settings.swingArcHeight,
            strideLength = settings.strideLength,

            plantThreshold = settings.plantThreshold,
            plantSmooth = settings.plantSmooth,

            snakeAmplitude = settings.snakeAmplitude,
            snakeFrequency = settings.snakeFrequency,

            spineDelay = settings.spineDelay,
            pelvisDelay = settings.pelvisDelay,
            feetDelay = settings.feetDelay,
            sideRootAmount = settings.sideRootAmount,
            snakeRootAmount = settings.snakeRootAmount,
            snakeRootSpeed = settings.snakeRootSpeed,
            snakeRootLag = settings.snakeRootLag
        };

    }

    void ApplyInjuryToSettings()
    {
        // 1. ALWAYS reset from base first
        CopyToRuntime(baseSettings);

        float concussion = injuryState.headInjury;
        float spine = injuryState.spineInjury;
        float legL = injuryState.leftLegInjury;
        float legR = injuryState.rightLegInjury;
        float instability = injuryState.instability;
        float pain = injuryState.pain;

        // 🧠 nervous system disruption
        runtime.spineInputSmooth *= (1f - concussion * 0.6f);

        // 🐍 locomotion instability
        runtime.walkSpeed *= (1f - instability * 0.4f);

        // 🦴 pelvis control loss
        runtime.pelvisTilt *= (1f - spine * 0.5f);

        // 🦵 stride collapse
        runtime.strideLength *= (1f - Mathf.Max(legL, legR) * 0.6f);

        // 🧍 stance narrowing
        runtime.stanceWidth *= (1f - spine * 0.3f);

        // 🫀 global damping
        float damping = 1f - pain * 0.5f;

        runtime.snakeAmplitude *= damping;
        runtime.snakeRootAmount *= damping;
    }




    public void TriggerJump(float jumpWidth = 3f, float jumpDuration = 0.15f)
    {
        Debug.Log("juuuump");
        if (_jumpRoutine != null)
        {
            StopCoroutine(_jumpRoutine);
        }

        _jumpRoutine = StartCoroutine(
            JumpRoutine(jumpWidth, jumpDuration)
        );
    }

    private IEnumerator JumpRoutine(float jumpWidth, float jumpDuration)
    {
        float originalOffset = stanceWidthOffset;

        float timer = 0f;

        // EXPAND
        while (timer < jumpDuration)
        {
            timer += Time.deltaTime;

            float t = timer / jumpDuration;

            stanceWidthOffset = Mathf.Lerp(
                originalOffset,
                jumpWidth,
                t
            );

            yield return null;
        }

        stanceWidthOffset = jumpWidth;

        timer = 0f;

        // RETURN
        while (timer < jumpDuration)
        {
            timer += Time.deltaTime;

            float t = timer / jumpDuration;

            stanceWidthOffset = Mathf.Lerp(
                jumpWidth,
                originalOffset,
                t
            );

            yield return null;
        }

        stanceWidthOffset = originalOffset;

        _jumpRoutine = null;
    }

}