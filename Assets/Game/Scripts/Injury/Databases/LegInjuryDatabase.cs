using Game.Body;
using System.Collections.Generic;
using UnityEngine;

public enum ELegInjury
{
    PulledHamstring,
    TornACL,
    CalfCramp,
    BrokenKnee,
    TwistedAnkle,
    QuadTear,
    SciaticPain,
    DeadLeg
}

public static class LegInjuryDatabase
{
    private static List<LegInjuryInstance> _cache;

    // =========================================================
    // MAIN ENTRY
    // =========================================================
    public static List<LegInjuryInstance> GetAll()
    {
        if (_cache == null)
            _cache = BuildDatabase();

        return _cache;
    }

    // =========================================================
    // RANDOM INJURY
    // =========================================================
    public static LegInjuryInstance GetRandomInjury(
        bool useSeed = false,
        int seed = 0)
    {
        var list = GetAll();

        if (list.Count == 0)
            return null;

        if (useSeed)
        {
            Random.InitState(seed);
        }

        return list[Random.Range(0, list.Count)];
    }

    // =========================================================
    // GET BY ENUM
    // =========================================================
    public static LegInjuryInstance GetInjury(ELegInjury injury)
    {
        return GetAll().Find(x => x.injuryType == injury);
    }

    // =========================================================
    // GET BY STRING
    // =========================================================
    public static LegInjuryInstance GetInjury(string injuryName)
    {
        var list = GetAll();

        for (int i = 0; i < list.Count; i++)
        {
            if (list[i].injuryName == injuryName ||
                list[i].injuryRealName == injuryName)
            {
                return list[i];
            }
        }

        return null;
    }

    // =========================================================
    // DATABASE
    // =========================================================
    private static List<LegInjuryInstance> BuildDatabase()
    {
        return new List<LegInjuryInstance>()
        {
            // =====================================================
            // PULLED HAMSTRING
            // =====================================================
            new LegInjuryInstance()
            {
                injuryType = ELegInjury.PulledHamstring,

                //bodyPartType = BodyPartType.LeftLeg,

                _bodyRegion = EBodyRegion.Leg,

                injuryName = "Pulled Hamstring",
                injuryRealName = "Hamstring Strain",

                symptoms =
                    "Sharp pain in the back of the thigh, weakness while walking, unstable leg control.",

                description =
                    "A strain or tear of the hamstring muscles caused by overstretching or sudden movement.",

                funFact =
                    "Hamstring injuries are one of the most common injuries in sprint athletes.",

                stiffness = 0.7f,
                range = 0.5f,
                jitter = 0.3f,
                delay = 0.4f,

                affectedMuscles = new List<ELegMuscles>()
                {
                    ELegMuscles.BicepsFemoris_LongHead,
                    ELegMuscles.Semitendinosus,
                    ELegMuscles.Semimembranosus
                }
            },

            // =====================================================
            // TORN ACL
            // =====================================================
            new LegInjuryInstance()
            {
                injuryType = ELegInjury.TornACL,

                //bodyPartType = BodyPartType.LeftLeg,

                _bodyRegion = EBodyRegion.Leg,
                injuryName = "Torn ACL",
                injuryRealName = "Anterior Cruciate Ligament Tear",

                symptoms =
                    "Knee instability, collapsing while turning, delayed reactions.",

                description =
                    "A severe knee ligament injury often caused by sudden twisting or impact.",

                funFact =
                    "ACL tears frequently happen without physical contact during sports.",

                stiffness = 0.3f,
                range = 0.4f,
                jitter = 0.8f,
                delay = 0.6f,

                affectedMuscles = new List<ELegMuscles>()
                {
                    ELegMuscles.RectusFemoris,
                    ELegMuscles.VastusLateralis,
                    ELegMuscles.VastusMedialis,
                    ELegMuscles.BicepsFemoris_LongHead,
                    ELegMuscles.Gastrocnemius_MedialHead
                }
            },

            // =====================================================
            // CALF CRAMP
            // =====================================================
            new LegInjuryInstance()
            {
                injuryType = ELegInjury.CalfCramp,

                //bodyPartType = BodyPartType.LeftLeg,

                _bodyRegion = EBodyRegion.Leg,
                injuryName = "Calf Cramp",
                injuryRealName = "Gastrocnemius Spasm",

                symptoms =
                    "Sudden tightening, twitching, difficulty extending the foot.",

                description =
                    "An involuntary muscle contraction in the calf caused by fatigue or dehydration.",

                funFact =
                    "Calf cramps commonly occur during sleep or intense exercise.",

                stiffness = 0.9f,
                range = 0.6f,
                jitter = 0.6f,
                delay = 0.2f,

                affectedMuscles = new List<ELegMuscles>()
                {
                    ELegMuscles.Gastrocnemius_MedialHead,
                    ELegMuscles.Gastrocnemius_LateralHead,
                    ELegMuscles.Soleus
                }
            },

            // =====================================================
            // BROKEN KNEE
            // =====================================================
            new LegInjuryInstance()
            {
                injuryType = ELegInjury.BrokenKnee,

                //bodyPartType = BodyPartType.LeftLeg,

                _bodyRegion = EBodyRegion.Leg,
                injuryName = "Broken Knee",
                injuryRealName = "Patellar Fracture",

                symptoms =
                    "Severe instability, inability to properly extend the leg, shaking movement.",

                description =
                    "A fracture of the kneecap usually caused by direct trauma or falling.",

                funFact =
                    "The patella improves leverage for the quadriceps muscle.",

                stiffness = 0.2f,
                range = 0.2f,
                jitter = 0.9f,
                delay = 0.8f,

                affectedMuscles = new List<ELegMuscles>()
                {
                    ELegMuscles.RectusFemoris,
                    ELegMuscles.VastusLateralis,
                    ELegMuscles.VastusMedialis,
                    ELegMuscles.TibialisAnterior
                }
            },

            // =====================================================
            // TWISTED ANKLE
            // =====================================================
            new LegInjuryInstance()
            {
                injuryType = ELegInjury.TwistedAnkle,

                //bodyPartType = BodyPartType.LeftLeg,

                _bodyRegion = EBodyRegion.Leg,
                injuryName = "Twisted Ankle",
                injuryRealName = "Ankle Sprain",

                symptoms =
                    "Unstable footing, shaking foot movements, delayed balance correction.",

                description =
                    "Stretching or tearing of ankle ligaments caused by rolling the foot.",

                funFact =
                    "Ankle sprains are among the most frequent sports injuries worldwide.",

                stiffness = 0.5f,
                range = 0.5f,
                jitter = 0.7f,
                delay = 0.5f,

                affectedMuscles = new List<ELegMuscles>()
                {
                    ELegMuscles.TibialisAnterior,
                    ELegMuscles.PeroneusLongus,
                    ELegMuscles.Gastrocnemius_LateralHead
                }
            },

            // =====================================================
            // QUAD TEAR
            // =====================================================
            new LegInjuryInstance()
            {
                injuryType = ELegInjury.QuadTear,

                //bodyPartType = BodyPartType.LeftLeg,

                _bodyRegion = EBodyRegion.Leg,
                injuryName = "Quad Tear",
                injuryRealName = "Quadriceps Strain",

                symptoms =
                    "Weak kicking power, trembling during extension, movement hesitation.",

                description =
                    "A strain or tear in the front thigh muscles caused by overload.",

                funFact =
                    "The quadriceps group contains four separate muscles.",

                stiffness = 0.6f,
                range = 0.4f,
                jitter = 0.5f,
                delay = 0.4f,

                affectedMuscles = new List<ELegMuscles>()
                {
                    ELegMuscles.RectusFemoris,
                    ELegMuscles.VastusLateralis,
                    ELegMuscles.VastusMedialis,
                    ELegMuscles.VastusIntermedius
                }
            },

            // =====================================================
            // SCIATIC PAIN
            // =====================================================
            new LegInjuryInstance()
            {
                injuryType = ELegInjury.SciaticPain,

                //bodyPartType = BodyPartType.LeftLeg,

                _bodyRegion = EBodyRegion.Leg,
                injuryName = "Sciatic Pain",
                injuryRealName = "Sciatica",

                symptoms =
                    "Delayed movement, twitching, sudden weakness radiating down the leg.",

                description =
                    "Compression or irritation of the sciatic nerve causing radiating pain.",

                funFact =
                    "The sciatic nerve is the longest nerve in the human body.",

                stiffness = 0.4f,
                range = 0.6f,
                jitter = 0.8f,
                delay = 0.9f,

                affectedMuscles = new List<ELegMuscles>()
                {
                    ELegMuscles.BicepsFemoris_LongHead,
                    ELegMuscles.Gastrocnemius_MedialHead,
                    ELegMuscles.TibialisAnterior
                }
            },

            // =====================================================
            // DEAD LEG
            // =====================================================
            new LegInjuryInstance()
            {
                injuryType = ELegInjury.DeadLeg,

                //bodyPartType = BodyPartType.LeftLeg,

                _bodyRegion = EBodyRegion.Leg,
                injuryName = "Dead Leg",
                injuryRealName = "Quadriceps Contusion",

                symptoms =
                    "Heavy sluggish movement, delayed response, partial numbness.",

                description =
                    "A deep bruise in the thigh muscle caused by blunt impact.",

                funFact =
                    "A strong contusion can temporarily limit knee movement completely.",

                stiffness = 0.8f,
                range = 0.5f,
                jitter = 0.2f,
                delay = 0.7f,

                affectedMuscles = new List<ELegMuscles>()
                {
                    ELegMuscles.RectusFemoris,
                    ELegMuscles.VastusIntermedius,
                    ELegMuscles.VastusLateralis
                }
            },
        };
    }
}