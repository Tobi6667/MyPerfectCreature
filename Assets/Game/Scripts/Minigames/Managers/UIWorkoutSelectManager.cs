using System;
using UnityEngine;

public class UIWorkoutSelectManager : MonoBehaviour
{
    public static UIWorkoutSelectManager Instance;

    public event Action<FrankensteinWorkoutData> OnWorkoutSelected;

    private void Awake()
    {
        // 🔥 prevents duplicate singletons when reloading scenes
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void SelectWorkout(FrankensteinWorkoutData data)
    {
        if (data == null)
        {
            Debug.LogWarning("SelectWorkout called with NULL data");
            return;
        }

        OnWorkoutSelected?.Invoke(data);
    }

    public void ClearAllListeners()
    {
        // 🔥 useful when exiting minigames or re-entering selection
        OnWorkoutSelected = null;
    }

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }
}