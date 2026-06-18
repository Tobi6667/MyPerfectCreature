using Game.Minigames;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class WorkoutSelectionPhase : IMinigamePhase
{
    private VisualTreeAsset uiPrefab;
    private List<FrankensteinWorkoutData> workouts;
    private UIDocument uiDocument;

    private Action<FrankensteinWorkoutData> onSelected;

    private bool hasSelected;
    private FrankensteinWorkoutData selectedWorkout;

    public WorkoutSelectionPhase(
        VisualTreeAsset buttonTemplate,
        UIDocument container,
        List<FrankensteinWorkoutData> workouts,
        Action<FrankensteinWorkoutData> onSelected)
    {
        this.uiPrefab = buttonTemplate;
        this.uiDocument = container;
        this.workouts = workouts;
        this.onSelected = onSelected;
    }

    public void Enter()
    {
        Debug.Log("WorkoutSelectionPhase.Enter() called. Stack: " + Environment.StackTrace);

        hasSelected = false;
        selectedWorkout = null;

        var root = uiDocument.rootVisualElement;

        var container = root.Q<VisualElement>("workout-select-panel");

        if (container == null)
        {
            Debug.LogError("workout-select-panel not found!");
            return;
        }

        container.Clear();

        foreach (var workout in workouts)
        {
            var local = workout; // IMPORTANT capture fix

            var btn = new Button(() =>
            {
                if (hasSelected) return;

                hasSelected = true;
                selectedWorkout = local;

                Debug.Log("SELECTED: " + local.workoutName);
                Exit();
                onSelected?.Invoke(local);
            })
            {
                text = workout.workoutName
            };

            container.Add(btn);
        }
    }

    public IEnumerator Execute(MinigameContext context)
    {
        Enter();

        yield return new WaitUntil(() => hasSelected);

        context._workoutData = selectedWorkout;
    }

    public void Exit()
    {
        var root = uiDocument.rootVisualElement;
        var container = root.Q<VisualElement>("workout-select-panel");

        container?.Clear();
    }
}