using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;
using System;

namespace Game.Minigames
{
    public class WorkoutPhase : IMinigamePhase
    {
        private VisualTreeAsset uiPrefab;
        private List<FrankensteinWorkoutData> workouts;
        private UIDocument root;

        private bool hasSelected;

        public WorkoutPhase(
            VisualTreeAsset buttonTemplate,
            UIDocument container,
            List<FrankensteinWorkoutData> workouts)
        {
            this.uiPrefab = buttonTemplate;
            this.workouts = workouts;
            this.root = container;
        }

        public IEnumerator Execute(MinigameContext context)
        {
            hasSelected = false;

            var container = root.rootVisualElement.Q<VisualElement>("workout-select-panel");
            container.Clear();

            foreach (var workout in workouts)
            {
                var local = workout;

                var element = uiPrefab.CloneTree();

                var button = element.Q<Button>("workout-btn");
                var label = element.Q<Label>("workout-name");

                if (label != null)
                    label.text = workout.workoutName;

                button.clicked += () =>
                {
                    context._workoutData = local;   // 🔥 IMPORTANT FIX
                    hasSelected = true;
                };

                container.Add(element);
            }

            yield return new WaitUntil(() => hasSelected);
        }
    }
}