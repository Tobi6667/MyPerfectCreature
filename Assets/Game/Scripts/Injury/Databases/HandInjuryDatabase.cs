using Game.Body;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HandInjuryTypes : IInjuryData
{
    public HandInjuryTypes type;

    public string injuryName;
    public string injuryRealName;
    public string symptoms;
    [TextArea] public string description;
    [TextArea] public string funfact;
    [Header("All Finger Data")]
    public SingleFingerInjuryData thumb;
    public SingleFingerInjuryData index;
    public SingleFingerInjuryData middle;
    public SingleFingerInjuryData ring;
    public SingleFingerInjuryData pinky;
    public EBodyPartType bodyPartType;

    //public EBodyInjuryType _bodyinjuryType;

    public string InjuryName => injuryName;

    public string InjuryRealName => injuryRealName;

    public string Symptoms => symptoms;

    public string Description => description;

    public EBodyPartType BodyPartType => bodyPartType;

    //public EBodyInjuryType EBodyInjuryType => _bodyinjuryType;

    string IInjuryData.FunFact => funfact;

    EBodyPartType IInjuryData.BodyPartType => throw new System.NotImplementedException();
}