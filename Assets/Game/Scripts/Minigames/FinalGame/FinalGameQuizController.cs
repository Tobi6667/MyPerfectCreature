using DG.Tweening;
using Game.Body;
using Game.Main;
using Game.Minigames;
using System.Collections.Generic;
using UnityEngine;

public class FinalGameQuizController: MonoBehaviour
{
    [SerializeField] private List<BodyPartBase> _bodyParts;
    [SerializeField] private Transform _bodyTarget;

    private BodyPartBase _activeBodypart;

    private IInjuryData _activeInjury;

    public void Initialize()
    {
        _bodyParts = new List<BodyPartBase>();
        _bodyParts = GameManager.Instance.BodypartsList;
    }

    public void SetupBodyPart()
    {
        _activeBodypart = _bodyParts[Random.Range(0, _bodyParts.Count)];
        var database = GodInjuryDatabase.Get(_activeBodypart.Region);

        _activeInjury = database.GetRandomInjury();
        UIMinigameManager.Instance.ShowInjuryPanel(_activeInjury);

        _activeBodypart.transform.DOMove(_bodyTarget.position,3f);

    }
}
