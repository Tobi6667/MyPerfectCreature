using DG.Tweening;
using Game.Body;
using NUnit.Framework.Internal;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class BodypartMusclesComponent : MonoBehaviour
{
    [SerializeField] private Transform _musclesParent;
    [SerializeField] private MuscleController[] _muscles;
    [SerializeField] private Animation _animator;
    [SerializeField] private GameObject _boneMesh;
    [SerializeField] private GameObject _legMainMesh;

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
        //ShowMuscles();
    }
    //
    internal void ShowMuscles(List<ELegMuscles> muscles)
    {
        foreach (var muscleType in muscles)
        {
            if (_muscleMap.TryGetValue(muscleType, out var muscleController))
            {
                muscleController.ChangeVisual();
            }
        }
        _boneMesh.SetActive(true);
        _animator.gameObject.SetActive(true);
        _legMainMesh.SetActive(false);
        _animator.Play("expand");
    }

    internal void HideMuscles(Action onhide)
    {
        _animator.Play("join");
        DOVirtual.DelayedCall(1f, () =>
        {
            _boneMesh.SetActive(false);
            _animator.gameObject.SetActive(false);

            _legMainMesh.SetActive(true);
            onhide?.Invoke();
        });

    }


}
