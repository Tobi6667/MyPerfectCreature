using Game.Body;

public interface IInjuryData
{
    string InjuryName { get; }
    string InjuryRealName { get; }
    string Symptoms { get; }
    string Description { get; }

    string FunFact { get; }
    EBodyPartType BodyPartType { get; }

    //EBodyInjuryType EBodyInjuryType { get; }
}