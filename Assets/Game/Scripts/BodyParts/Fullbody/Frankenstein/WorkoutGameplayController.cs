using System;
using UnityEngine;

public class WorkoutGameplayController : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float defaultWorkoutDuration = 20f;

    [Header("Debug")]
    [SerializeField] private bool debugLogs = true;

    private ActiveWorkoutRuntime activeWorkout;

    private Action onWorkoutFinished;

    private bool isRunning;

    private float workoutTimer;

    #region Properties

    public bool IsRunning => isRunning;

    #endregion

    #region Public API

    public void StartWorkout(
        ActiveWorkoutRuntime runtime,
        Action onFinished)
    {
        activeWorkout = runtime;

        onWorkoutFinished = onFinished;

        workoutTimer = 0f;

        activeWorkout.failed = false;
        activeWorkout.fatigue = 0f;
        activeWorkout.pain = 0f;
        activeWorkout.stabilityScore = 1f;

        isRunning = true;

        Log("Workout Started");
    }

    public void StopWorkout()
    {
        if (!isRunning)
            return;

        isRunning = false;

        Log("Workout Stopped");
    }

    #endregion

    #region Unity

    private void Update()
    {
        if (!isRunning)
            return;

        TickWorkout();
    }

    #endregion

    #region Runtime

    private void TickWorkout()
    {
        float deltaTime = Time.deltaTime;

        workoutTimer += deltaTime;

        activeWorkout.elapsedTime += deltaTime;

        EvaluateGameplay(deltaTime);

        ApplyFatigue(deltaTime);

        CheckFailureConditions();

        CheckWorkoutCompletion();
    }

    #endregion

    #region Evaluation

    private void EvaluateGameplay(float deltaTime)
    {
        /*
         * TEMP PLACEHOLDER
         * Replace later with:
         *
         * - pose accuracy
         * - IK drift
         * - rhythm accuracy
         * - balance
         * - smile tracking
         * - compensation detection
         */

        float fakeStabilityDrain = 0.02f * deltaTime;

        activeWorkout.stabilityScore -= fakeStabilityDrain;

        activeWorkout.stabilityScore =
            Mathf.Clamp01(activeWorkout.stabilityScore);
    }

    #endregion

    #region Fatigue

    private void ApplyFatigue(float deltaTime)
    {
        float fatigueGain =
            activeWorkout.workoutData.fatigueCosts * 0.01f * deltaTime;

        activeWorkout.fatigue += fatigueGain;
    }

    #endregion

    #region Failure

    private void CheckFailureConditions()
    {
        if (activeWorkout.stabilityScore <= 0f)
        {
            FailWorkout("Stability reached zero");
        }

        if (activeWorkout.fatigue >= 100f)
        {
            FailWorkout("Fatigue exceeded limit");
        }
    }

    private void FailWorkout(string reason)
    {
        if (activeWorkout.failed)
            return;

        activeWorkout.failed = true;

        Log($"Workout Failed: {reason}");

        EndWorkout();
    }

    #endregion

    #region Completion

    private void CheckWorkoutCompletion()
    {
        if (workoutTimer >= defaultWorkoutDuration)
        {
            CompleteWorkout();
        }
    }

    private void CompleteWorkout()
    {
        Log("Workout Completed");

        EndWorkout();
    }

    private void EndWorkout()
    {
        isRunning = false;

        onWorkoutFinished?.Invoke();
    }

    #endregion

    #region Debug

    private void Log(string msg)
    {
        if (!debugLogs)
            return;

        Debug.Log($"[WorkoutGameplay] {msg}");
    }

    #endregion
}