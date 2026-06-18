using Game.Body;
using System.Collections.Generic;
using UnityEngine;

public class FullbodyInjuryDatabase : MonoBehaviour, IDatabaseBase
{
    private static List<FullbodyInjuryEntry> _cache;
    private readonly List<IInjuryData> _shown = new();

    // =========================
    // CORE
    // =========================

    private List<FullbodyInjuryEntry> GetAll()
    {
        if (_cache == null)
            _cache = BuildDatabase();

        return _cache;
    }

    public IInjuryData GetRandomInjury()
    {
        var list = GetAll();

        if (list.Count == 0)
            return null;

        var injury = list[Random.Range(0, list.Count)];

        _shown.Add(injury);
        return injury;
    }

    public List<IInjuryData> GetAllInjuries()
    {
        return GetAll().ConvertAll(x => (IInjuryData)x);
    }

    public List<IInjuryData> GetShownInjuries()
    {
        return new List<IInjuryData>(_shown);
    }

    // =========================
    // DATABASE BUILDER
    // =========================

    private static List<FullbodyInjuryEntry> BuildDatabase()
    {
        return new List<FullbodyInjuryEntry>()
        {
            // =====================================================
            // SLIDING DISC
            // =====================================================
            new FullbodyInjuryEntry
            {
                type = EFullbodyInjuryType.SlidingDisc,
                injuryName = "Sliding Disc",
                injuryRealName = "Hernia disci intervertebralis",
                symptoms = "Back pain, numbness, tingling, weakness, radiating pain",
                description = "Spinal disc presses on nearby nerves due to displacement.",
                funFact = "One of your spine cushions just gave up its position.",
                modifiers = new FullbodyModifiers
                {
                    spineStiffness = 0.8f,
                    pelvisTiltLimit = 0.4f,
                    bendRestriction = 0.6f,
                    instability = 0.3f,
                    pain = 0.7f
                },
                
            },

            // =====================================================
            // HIP INJURY
            // =====================================================
            new FullbodyInjuryEntry
            {
                type = EFullbodyInjuryType.HipInjury,
                injuryName = "Hip Injury",
                injuryRealName = "Trauma articulationis coxae",
                symptoms = "Hip pain, limping, instability",
                description = "Damage to hip joint affecting walking stability.",
                funFact = "Your pelvis is negotiating with gravity.",
                modifiers = new FullbodyModifiers
                {
                    pelvisTiltLimit = 0.9f,
                    leftLegWeakness = 0.4f,
                    rightLegWeakness = 0.4f,
                    instability = 0.5f,
                    pain = 0.5f
                },
                
            },

            // =====================================================
            // KNEE INJURY
            // =====================================================
            new FullbodyInjuryEntry
            {
                type = EFullbodyInjuryType.KneeInjury,
                injuryName = "Knee Injury",
                injuryRealName = "Trauma genus",
                symptoms = "Pain while walking, weak support",
                description = "Knee cannot stabilize weight properly anymore.",
                funFact = "Your knees have quit leg day.",
                modifiers = new FullbodyModifiers
                {
                    leftLegWeakness = 0.7f,
                    rightLegWeakness = 0.7f,
                    instability = 0.3f,
                    pain = 0.4f
                },
            },

            // =====================================================
            // NECK INJURY
            // =====================================================
            new FullbodyInjuryEntry
            {
                type = EFullbodyInjuryType.NeckInjury,
                injuryName = "Neck Injury",
                injuryRealName = "Distorsio cervicalis",
                symptoms = "Stiff neck, limited head movement",
                description = "Neck muscles are strained and restricted.",
                funFact = "Your neck is experiencing latency.",
                modifiers = new FullbodyModifiers
                {
                    neckProtection = 0.9f,
                    coordinationLoss = 0.3f,
                    spineStiffness = 0.2f
                },
            },

            // =====================================================
            // CONCUSSION
            // =====================================================
            new FullbodyInjuryEntry
            {
                type = EFullbodyInjuryType.Concussion,
                injuryName = "Concussion",
                injuryRealName = "Commotio cerebri",
                symptoms = "Dizziness, confusion, instability",
                description = "Brain impact disrupts coordination and balance.",
                funFact = "Your brain is running unstable build mode.",
                modifiers = new FullbodyModifiers
                {
                    coordinationLoss = 1.0f,
                    instability = 0.8f,
                    neckProtection = 0.6f,
                    pain = 0.2f
                },
            },

            // =====================================================
            // MUSCLE TEAR
            // =====================================================
            new FullbodyInjuryEntry
            {
                type = EFullbodyInjuryType.MuscleTear,
                injuryName = "Muscle Tear",
                injuryRealName = "Ruptura muscularis",
                symptoms = "Sharp pain, weakness",
                description = "Muscle fibers partially torn.",
                funFact = "A movement cable just snapped.",
                modifiers = new FullbodyModifiers
                {
                    bendRestriction = 0.5f,
                    leftLegWeakness = 0.4f,
                    rightLegWeakness = 0.4f,
                    pain = 0.6f
                },
            },

            // =====================================================
            // NERVE DAMAGE
            // =====================================================
            new FullbodyInjuryEntry
            {
                type = EFullbodyInjuryType.NerveDamage,
                injuryName = "Nerve Damage",
                injuryRealName = "Laesio nervorum",
                symptoms = "Numbness, delayed movement",
                description = "Signal transmission between brain and body is disrupted.",
                funFact = "Packet loss in human hardware.",
                modifiers = new FullbodyModifiers
                {
                    coordinationLoss = 0.6f,
                    instability = 0.6f,
                    spineStiffness = 0.3f,
                    pain = 0.5f
                },
            }
        };
    }
}