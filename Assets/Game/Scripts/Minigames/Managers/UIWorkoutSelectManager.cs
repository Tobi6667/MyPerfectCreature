using System;
using UnityEngine;

public class UIWorkoutSelectManager : MonoBehaviour
{
    public static UIWorkoutSelectManager Instance;

    public event Action<FrankensteinWorkoutData> OnWorkoutSelected;

    private void Awake()
    {
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
        OnWorkoutSelected = null;
    }

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }
}