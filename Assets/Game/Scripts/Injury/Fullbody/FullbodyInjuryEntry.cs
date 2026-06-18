using Game.Body;
using UnityEngine;

public class FullbodyInjuryEntry : IInjuryData
{
    public EFullbodyInjuryType type;
    public FullbodyModifiers modifiers;

    public string injuryName;
    public string injuryRealName;
    public string symptoms;
    [TextArea] public string description;
    [TextArea] public string funFact;
    public EBodyPartType bodyPartType;



    public string InjuryName => injuryName;

    public string InjuryRealName => injuryRealName;

    public string Symptoms => symptoms;

    public string Description => description;

  //  public BodyPartType BodyPartType => bodyPartType;

    public string FunFact => funFact;

  //  public EBodyInjuryType EBodyInjuryType => _bodyinjuryType;

    EBodyPartType IInjuryData.BodyPartType => throw new System.NotImplementedException();
}
