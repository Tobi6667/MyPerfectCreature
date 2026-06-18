using Game.Body;
using UnityEngine;

[System.Serializable]
public class TorsoInjuryData : IInjuryData
{

    [Header("UI")]
    public string injuryName;
    public string injuryRealName;
    public string symptoms;
    [TextArea] public string description;
    [TextArea] public string funFact;
    [Header("Gameplay")]
    public float movementMultiplier = 1f;
    public float breathingMultiplier = 1f;
    public float painAmount = 0f;

    [Header("Animation")]
    public float spineTilt;
    public float twistOffset;
    public float idleShake;

    [Header("FX")]
    public AudioClip impactSound;
    public ParticleSystem hitFX;

    public ETorsoInjury torsoInjury;

    public string InjuryName => injuryName;

    public string InjuryRealName => injuryRealName;

    public string Symptoms => symptoms;

    public string Description => description;

    
    public string FunFact => funFact;

    EBodyPartType IInjuryData.BodyPartType => throw new System.NotImplementedException();
}