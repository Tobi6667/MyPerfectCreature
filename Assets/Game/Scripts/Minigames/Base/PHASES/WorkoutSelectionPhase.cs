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
        container.style.display = DisplayStyle.Flex;
        if (container == null)
        {
            Debug.LogError("workout-select-panel not found!");
            return;
        }

        container.Clear();

        foreach (var workout in workouts)
        {
            var local = workout; // capture fix

            TemplateContainer instance = uiPrefab.Instantiate();

            var btn = instance.Q<Button>(); // or instance.Q<Button>("button-name") if your prefab has multiple elements
            if (btn == null)
            {
                Debug.LogError("No Button found in workout button prefab!");
                continue;
            }

            var name = instance.Q<Label>("workout-name");
            name.text = local.workoutName;

            var vasc = instance.Q<Label>("vas-cost");
            vasc.text = "50";
            var fatc = instance.Q<Label>("fat-cost");
            fatc.text = local.fatigueCosts.ToString();


            btn.text = local.workoutName;
            btn.clicked += () =>
            {
                if (hasSelected) return;

                hasSelected = true;
                selectedWorkout = local;

                Debug.Log("SELECTED: " + local.workoutName);
                Exit();
                onSelected?.Invoke(local);
            };

            container.Add(instance);
        }
    }

    public IEnumerator Execute(MinigameContext context)
    {
        Enter();

        yield return new WaitUntil(() => hasSelected);
        Exit();

        context._workoutData = selectedWorkout;
    }

    public void Exit()
    {
        var root = uiDocument.rootVisualElement;
        var container = root.Q<VisualElement>("workout-select-panel");

        container?.Clear();
        container.style.display = DisplayStyle.None;
    }
}