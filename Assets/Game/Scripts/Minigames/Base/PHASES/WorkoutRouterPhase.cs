using Game.Minigames;
using System.Collections;
using UnityEngine;

public class WorkoutRouterPhase : IMinigamePhase
{
    public IEnumerator Execute(MinigameContext context)
    {
        var work = context._workoutData;

        if (work == null)
        {
            Debug.LogError("No workout selected in context!");
            yield break;
        }

        var frank = context.BodyPart as FrankensteinController;

        if (frank == null)
        {
            Debug.LogError("No FrankensteinController in context!");
            yield break;
        }

        if (work.workoutType == WorkoutType.Game)
        {
            if (work._minigame == null)
            {
                Debug.LogError($"WorkoutType.Game but no minigame assigned on {work.workoutName}!");
                yield break;
            }

            bool finished = false;
            void OnCompleted() => finished = true;

            // Instantiate so we don't reuse/share a single prefab/scene instance across plays
            var minigameInstance = Object.Instantiate(work._minigame);

            minigameInstance.Completed += OnCompleted;
            minigameInstance.Initialize(context);
            minigameInstance.StartMinigame();

            yield return new WaitUntil(() => finished);

            minigameInstance.Completed -= OnCompleted; // harmless safety; object is destroyed in Finish() anyway
        }
        else
        {
            frank.FrankensteinMovementModule.PlayWorkout(work);
            /*
            yield return new WaitUntil(() =>
                frank.FrankensteinMovementModule.IsFinished == true
            );*/
        }
    }
}