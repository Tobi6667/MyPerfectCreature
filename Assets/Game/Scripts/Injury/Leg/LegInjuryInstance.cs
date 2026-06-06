using Game.Body;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LegInjuryInstance : IInjuryData
{
    //public LegInjuryTypes injuryType;
    public EBodyPartType bodyPartType;
    [Header("UI")]
    public string injuryName;
    public string injuryRealName;
    public string symptoms;
    [TextArea] public string description;
    [TextArea] public string funFact;

    [Range(0f, 1f)] [SerializeField] internal float stiffness = 1f;
    [Range(0f, 1f)] [SerializeField] internal float range = 1f;
    [Range(0f, 1f)] [SerializeField] internal float jitter = 1f;
    [Range(0f, 1f)] [SerializeField] internal float delay = 1f;
    public ELegInjury injuryType;

    [SerializeField] internal List<ELegMuscles> affectedMuscles;
    [SerializeField] internal float startTime;
    public EBodyRegion _bodyRegion;
    public string InjuryName => injuryName;

    public string InjuryRealName => injuryRealName;

    public string Symptoms => symptoms;

    public string Description => description;

    public EBodyRegion BodyRegion => _bodyRegion;

    string IInjuryData.FunFact => funFact;


    EBodyPartType IInjuryData.BodyPartType => throw new System.NotImplementedException();
}