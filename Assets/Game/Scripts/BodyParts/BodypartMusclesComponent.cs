using Game.Body;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class BodypartMusclesComponent : MonoBehaviour
{
    [SerializeField] private Transform _musclesParent;
    [SerializeField] private MuscleController[] _muscles;

    private Dictionary<ELegMuscles, MuscleController> _muscleMap =
        new Dictionary<ELegMuscles, MuscleController>();

    internal void InitializeModule()
    {

        _muscles = _musclesParent
            .GetComponentsInChildren<MuscleController>()
            .ToArray();

        _muscleMap.Clear();

        foreach (var muscle in _muscles)
        {

            if (!_muscleMap.ContainsKey(muscle.MuscleName))
            {
                _muscleMap.Add(muscle.MuscleName, muscle);
            }
        }
    }
}
