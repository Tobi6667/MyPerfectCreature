using Game.Body;
using System.Collections.Generic;
using UnityEngine;

public enum ETorsoInjury
{
    HerniatedDisc,
    MuscleSpasm,
    FracturedRib,
    Scoliosis,
    LockedSpine,
    Whiplash,
    CrushedVertebrae
}

public class TorsoInjuryDatabase : IDatabaseBase
{
    private static List<TorsoInjuryData> _cache;

    private List<TorsoInjuryData> GetAll()
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

        return list[Random.Range(0, list.Count)];
    }

    public List<IInjuryData> GetAllInjuries()
    {
        return GetAll()
            .ConvertAll(x => (IInjuryData)x);
    }

    public List<IInjuryData> GetShownInjuries()
    {
        return GetAll()
            .ConvertAll(x => (IInjuryData)x);
    }

    // =========================================================
    // DATABASE
    // =========================================================
    private static List<TorsoInjuryData> BuildDatabase()
    {
        return new List<TorsoInjuryData>()
        {
            // =====================================================
            // HERNIATED DISC
            // =====================================================
            new TorsoInjuryData()
            {
                torsoInjury = ETorsoInjury.HerniatedDisc,

              //  BodyPartType = BodyPartType.Torso,
              //  _bodyinjuryType = EBodyInjuryType.Torso,

                injuryName = "Herniated Disc",
                injuryRealName = "Intervertebral Disc Herniation",

                symptoms =
                    "Sharp lower back pain, radiating nerve pain, reduced mobility.",

                description =
                    "A spinal disc slips out of place and presses on nearby nerves.",

                funFact =
                    "Most herniated discs occur in the lower back region.",

                movementMultiplier = 0.5f,
                breathingMultiplier = 0.8f,
                painAmount = 0.9f,

                spineTilt = 0.6f,
                twistOffset = 0.4f,
                idleShake = 0.3f
            },

            // =====================================================
            // MUSCLE SPASM
            // =====================================================
            new TorsoInjuryData()
            {
                //injuryType = TorsoInjuryTypes.MuscleSpasm,
                torsoInjury = ETorsoInjury.MuscleSpasm,

             //   bodyPartType = BodyPartType.Torso,
              //  _bodyinjuryType = EBodyInjuryType.Torso,

                injuryName = "Muscle Spasm",
                injuryRealName = "Paraspinal Spasm",

                symptoms =
                    "Sudden tightness, restricted movement, painful contractions.",

                description =
                    "Involuntary contraction of back muscles due to overload or strain.",

                funFact =
                    "Spasms often occur after long periods of poor posture.",

                movementMultiplier = 0.6f,
                breathingMultiplier = 0.9f,
                painAmount = 0.6f,

                spineTilt = 0.2f,
                twistOffset = 0.3f,
                idleShake = 0.5f
            },

            // =====================================================
            // FRACTURED RIB
            // =====================================================
            new TorsoInjuryData()
            {
                                torsoInjury = ETorsoInjury.FracturedRib,


            

                injuryName = "Fractured Rib",
                injuryRealName = "Rib Fracture",

                symptoms =
                    "Sharp pain during breathing, reduced torso movement.",

                description =
                    "A break in one of the rib bones, usually from impact or trauma.",

                funFact =
                    "Ribs naturally move slightly with every breath.",

                movementMultiplier = 0.4f,
                breathingMultiplier = 0.5f,
                painAmount = 1.0f,

                spineTilt = 0.1f,
                twistOffset = 0.1f,
                idleShake = 0.7f
            },

            // =====================================================
            // SCOLIOSIS
            // =====================================================
            new TorsoInjuryData()
            {
               torsoInjury = ETorsoInjury.Scoliosis,


                injuryName = "Scoliosis",
                injuryRealName = "Spinal Curvature",

                symptoms =
                    "Uneven posture, tilted spine, long-term imbalance.",

                description =
                    "Abnormal lateral curvature of the spine.",

                funFact =
                    "Mild scoliosis often goes unnoticed for years.",

                movementMultiplier = 0.8f,
                breathingMultiplier = 1f,
                painAmount = 0.3f,

                spineTilt = 0.8f,
                twistOffset = 0.6f,
                idleShake = 0.2f
            },

            // =====================================================
            // LOCKED SPINE
            // =====================================================
            new TorsoInjuryData()
            {
               torsoInjury = ETorsoInjury.LockedSpine,


                injuryName = "Locked Spine",
                injuryRealName = "Facet Joint Lock",

                symptoms =
                    "Severe stiffness, near-zero rotation, sharp pain on movement.",

                description =
                    "Joints in the spine lock due to inflammation or misalignment.",

                funFact =
                    "Even small movements can trigger severe pain.",

                movementMultiplier = 0.2f,
                breathingMultiplier = 0.9f,
                painAmount = 0.95f,

                spineTilt = 0.05f,
                twistOffset = 0.0f,
                idleShake = 0.1f
            },

            // =====================================================
            // WHIPLASH
            // =====================================================
            new TorsoInjuryData()
            {
               torsoInjury = ETorsoInjury.Whiplash,


                injuryName = "Whiplash",
                injuryRealName = "Cervical Acceleration Injury",

                symptoms =
                    "Neck stiffness, headache, delayed pain response.",

                description =
                    "Rapid forward-backward movement of the neck.",

                funFact =
                    "Often caused by car accidents.",

                movementMultiplier = 0.7f,
                breathingMultiplier = 1f,
                painAmount = 0.7f,

                spineTilt = 0.4f,
                twistOffset = 0.2f,
                idleShake = 0.4f
            },

            // =====================================================
            // CRUSHED VERTEBRAE
            // =====================================================
            new TorsoInjuryData()
            {
                torsoInjury = ETorsoInjury.CrushedVertebrae,


                injuryName = "Crushed Vertebrae",
                injuryRealName = "Compression Fracture",

                symptoms =
                    "Extreme pain, loss of stability, severe movement restriction.",

                description =
                    "Severe spinal compression injury causing vertebra collapse.",

                funFact =
                    "Often seen in high-impact trauma cases.",

                movementMultiplier = 0.1f,
                breathingMultiplier = 0.6f,
                painAmount = 1.0f,

                spineTilt = 0.9f,
                twistOffset = 0.8f,
                idleShake = 0.8f
            },
        };
    }


}