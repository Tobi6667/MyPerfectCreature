using Game.Body;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class HandInjuryDatabase : IDatabaseBase
{
    private readonly List<IInjuryData> _shownInjuries = new();

    List<IInjuryData> IDatabaseBase.GetAllInjuries()
    {
        return injuries.Cast<IInjuryData>().ToList();
    }

    IInjuryData IDatabaseBase.GetRandomInjury()
    {
        if (injuries == null || injuries.Count == 0)
            return null;

        var injury = injuries[Random.Range(0, injuries.Count)];
        var data = (IInjuryData)injury;

        _shownInjuries.Add(data);
        return data;
    }

    List<IInjuryData> IDatabaseBase.GetShownInjuries()
    {
        return _shownInjuries;
    }

    public List<HandInjuryTypes> injuries = new()
    {
        // =====================================================
        // GAMER THUMB
        // =====================================================
        new HandInjuryTypes()
        {
            type = EHandInjuryTypes.GameThumb,
            injuryName = "Gamer Thumb",
            injuryRealName = "De Quervain's Tenosynovitis",
            symptoms = "Thumb pain, weak grip, pain during movement",
            description = "Overuse injury caused by repetitive thumb movement from gaming, texting, or controllers.",
            funfact = "This became extremely common with smartphones and handheld consoles.",

            thumb = new SingleFingerInjuryData
            {
                finger = EFingerTypes.Thumb,
                strengthLoss = 0.8f,
                rangeLoss = 0.5f,
                noise = 0.1f,
                tremor = 0.2f,
                delay = 0.15f,
                stiffness = 0.8f
            },

            index = new SingleFingerInjuryData { finger = EFingerTypes.Index },
            middle = new SingleFingerInjuryData { finger = EFingerTypes.Middle },
            ring = new SingleFingerInjuryData { finger = EFingerTypes.Ring },
            pinky = new SingleFingerInjuryData { finger = EFingerTypes.Pinky },
        },

        // =====================================================
        // MALLET FINGER
        // =====================================================
        new HandInjuryTypes()
        {
            type = EHandInjuryTypes.MalletFinger,
            injuryName = "Dropped Finger",
            injuryRealName = "Mallet Finger",
            symptoms = "Finger tip droops and cannot fully straighten",
            description = "Tendon damage at the fingertip usually caused by impact injuries.",
            funfact = "Often called Baseball Finger.",
            //bodyPartType = BodyPartType.RightArm,
            //_bodyinjuryType = EBodyInjuryType.Arm,

            middle = new SingleFingerInjuryData
            {
                finger = EFingerTypes.Middle,
                strengthLoss = 0.7f,
                rangeLoss = 0.85f,
                noise = 0.05f,
                tremor = 0.05f,
                delay = 0.2f,
                stiffness = 0.9f
            },

            thumb = new SingleFingerInjuryData { finger = EFingerTypes.Thumb },
            index = new SingleFingerInjuryData { finger = EFingerTypes.Index },
            ring = new SingleFingerInjuryData { finger = EFingerTypes.Ring },
            pinky = new SingleFingerInjuryData { finger = EFingerTypes.Pinky },
        },

        // =====================================================
        // BOUTONNIERE
        // =====================================================
        new HandInjuryTypes()
        {
            type = EHandInjuryTypes.Boutonniere,
            injuryName = "Bent Finger",
            injuryRealName = "Boutonniere Deformity",
            symptoms = "Middle joint bends inward abnormally",
            description = "Extensor tendon injury causing abnormal finger posture.",
            funfact = "The name comes from the French word for buttonhole.",
          //  bodyPartType = BodyPartType.RightArm,
          //  _bodyinjuryType = EBodyInjuryType.Arm,

            ring = new SingleFingerInjuryData
            {
                finger = EFingerTypes.Ring,
                strengthLoss = 0.6f,
                rangeLoss = 0.75f,
                noise = 0.1f,
                tremor = 0.1f,
                delay = 0.25f,
                stiffness = 0.85f
            },

            thumb = new SingleFingerInjuryData { finger = EFingerTypes.Thumb },
            index = new SingleFingerInjuryData { finger = EFingerTypes.Index },
            middle = new SingleFingerInjuryData { finger = EFingerTypes.Middle },
            pinky = new SingleFingerInjuryData { finger = EFingerTypes.Pinky },
        },

        // =====================================================
        // CARPAL TUNNEL
        // =====================================================
        new HandInjuryTypes()
        {
            type = EHandInjuryTypes.Carpal,
            injuryName = "Numb Hand",
            injuryRealName = "Carpal Tunnel Syndrome",
            symptoms = "Numbness, tingling, weakness",
            description = "Compression of the median nerve in the wrist.",
            funfact = "Very common among office workers and gamers.",
            //bodyPartType = BodyPartType.RightArm,
            //_bodyinjuryType = EBodyInjuryType.Arm,
            thumb = new SingleFingerInjuryData
            {
                finger = EFingerTypes.Thumb,
                strengthLoss = 0.5f,
                rangeLoss = 0.2f,
                noise = 0.15f,
                tremor = 0.25f,
                delay = 0.2f,
                stiffness = 0.3f
            },

            index = new SingleFingerInjuryData
            {
                finger = EFingerTypes.Index,
                strengthLoss = 0.6f,
                rangeLoss = 0.3f,
                noise = 0.2f,
                tremor = 0.3f,
                delay = 0.25f,
                stiffness = 0.35f
            },

            middle = new SingleFingerInjuryData
            {
                finger = EFingerTypes.Middle,
                strengthLoss = 0.55f,
                rangeLoss = 0.25f,
                noise = 0.2f,
                tremor = 0.3f,
                delay = 0.2f,
                stiffness = 0.3f
            },

            ring = new SingleFingerInjuryData
            {
                finger = EFingerTypes.Ring,
                strengthLoss = 0.2f,
                rangeLoss = 0.1f,
                noise = 0.05f,
                tremor = 0.1f,
                delay = 0.05f,
                stiffness = 0.1f
            },

            pinky = new SingleFingerInjuryData
            {
                finger = EFingerTypes.Pinky
            },
        },

        // =====================================================
        // METACARPAL FRACTURE
        // =====================================================
        new HandInjuryTypes()
        {
            type = EHandInjuryTypes.Metacarpal,
            injuryName = "Broken Hand",
            injuryRealName = "Metacarpal Fracture",
            symptoms = "Swelling, severe pain, weak grip",
            description = "Fracture in the long bones of the hand.",
            funfact = "Boxer's fractures are common after punching hard objects.",
          //  bodyPartType = BodyPartType.RightArm,
          //  _bodyinjuryType = EBodyInjuryType.Arm,

            ring = new SingleFingerInjuryData
            {
                finger = EFingerTypes.Ring,
                strengthLoss = 0.95f,
                rangeLoss = 0.8f,
                noise = 0.15f,
                tremor = 0.4f,
                delay = 0.4f,
                stiffness = 1f
            },

            pinky = new SingleFingerInjuryData
            {
                finger = EFingerTypes.Pinky,
                strengthLoss = 0.9f,
                rangeLoss = 0.75f,
                noise = 0.15f,
                tremor = 0.35f,
                delay = 0.35f,
                stiffness = 0.95f
            },

            thumb = new SingleFingerInjuryData { finger = EFingerTypes.Thumb },
            index = new SingleFingerInjuryData { finger = EFingerTypes.Index },
            middle = new SingleFingerInjuryData { finger = EFingerTypes.Middle },
        },

        // =====================================================
        // TRIGGER FINGER
        // =====================================================
        new HandInjuryTypes()
        {
            type = EHandInjuryTypes.TriggerFinger,
            injuryName = "Click Finger",
            injuryRealName = "Trigger Finger",
            symptoms = "Finger locks and suddenly releases",
            description = "Inflamed tendon causing snapping movement.",
            funfact = "Movement resembles pulling a trigger.",
            //bodyPartType = BodyPartType.RightArm,
            //_bodyinjuryType = EBodyInjuryType.Arm,

            index = new SingleFingerInjuryData
            {
                finger = EFingerTypes.Index,
                strengthLoss = 0.3f,
                rangeLoss = 0.5f,
                noise = 0.45f,
                tremor = 0.15f,
                delay = 0.4f,
                stiffness = 0.8f
            },

            thumb = new SingleFingerInjuryData { finger = EFingerTypes.Thumb },
            middle = new SingleFingerInjuryData { finger = EFingerTypes.Middle },
            ring = new SingleFingerInjuryData { finger = EFingerTypes.Ring },
            pinky = new SingleFingerInjuryData { finger = EFingerTypes.Pinky },
        },

        // =====================================================
        // ULNAR NERVE
        // =====================================================
        new HandInjuryTypes()
        {
            type = EHandInjuryTypes.Ulnar,
            injuryName = "Claw Hand",
            injuryRealName = "Ulnar Nerve Entrapment",
            symptoms = "Weak grip and curled fingers",
            description = "Compression or damage of the ulnar nerve.",
            funfact = "The funny bone sensation comes from the ulnar nerve.",
           // bodyPartType = BodyPartType.RightArm,
           // _bodyinjuryType = EBodyInjuryType.Arm,

            ring = new SingleFingerInjuryData
            {
                finger = EFingerTypes.Ring,
                strengthLoss = 0.7f,
                rangeLoss = 0.6f,
                noise = 0.2f,
                tremor = 0.3f,
                delay = 0.3f,
                stiffness = 0.8f
            },

            pinky = new SingleFingerInjuryData
            {
                finger = EFingerTypes.Pinky,
                strengthLoss = 0.85f,
                rangeLoss = 0.7f,
                noise = 0.25f,
                tremor = 0.35f,
                delay = 0.35f,
                stiffness = 0.9f
            },

            thumb = new SingleFingerInjuryData { finger = EFingerTypes.Thumb },
            index = new SingleFingerInjuryData { finger = EFingerTypes.Index },
            middle = new SingleFingerInjuryData { finger = EFingerTypes.Middle },
        },

        // =====================================================
        // DUPUYTREN
        // =====================================================
        new HandInjuryTypes()
        {
            type = EHandInjuryTypes.Dupuytren,
            injuryName = "Curled Fingers",
            injuryRealName = "Dupuytren's Contracture",
            symptoms = "Fingers curl permanently toward the palm",
            description = "Connective tissue thickens and contracts over time.",
            funfact = "Usually progresses slowly over years.",
            //bodyPartType = BodyPartType.RightArm,
           // _bodyinjuryType = EBodyInjuryType.Arm,

            ring = new SingleFingerInjuryData
            {
                finger = EFingerTypes.Ring,
                strengthLoss = 0.6f,
                rangeLoss = 0.95f,
                noise = 0.05f,
                tremor = 0.05f,
                delay = 0.2f,
                stiffness = 1f
            },

            pinky = new SingleFingerInjuryData
            {
                finger = EFingerTypes.Pinky,
                strengthLoss = 0.65f,
                rangeLoss = 1f,
                noise = 0.05f,
                tremor = 0.05f,
                delay = 0.2f,
                stiffness = 1f
            },

            thumb = new SingleFingerInjuryData { finger = EFingerTypes.Thumb },
            index = new SingleFingerInjuryData { finger = EFingerTypes.Index },
            middle = new SingleFingerInjuryData { finger = EFingerTypes.Middle },
        },

        // =====================================================
        // FLEXOR TENDON
        // =====================================================
        new HandInjuryTypes()
        {
            type = EHandInjuryTypes.FlexorTendon,
            injuryName = "Cut Tendon",
            injuryRealName = "Flexor Tendon Injury",
            symptoms = "Cannot properly bend finger",
            description = "Damage to the tendon responsible for gripping.",
            funfact = "Even tiny cuts can completely sever tendons.",

            index = new SingleFingerInjuryData
            {
                finger = EFingerTypes.Index,
                strengthLoss = 1f,
                rangeLoss = 0.85f,
                noise = 0.2f,
                tremor = 0.2f,
                delay = 0.5f,
                stiffness = 0.7f
            },

            middle = new SingleFingerInjuryData
            {
                finger = EFingerTypes.Middle,
                strengthLoss = 0.9f,
                rangeLoss = 0.8f,
                noise = 0.2f,
                tremor = 0.2f,
                delay = 0.45f,
                stiffness = 0.7f
            },

            thumb = new SingleFingerInjuryData { finger = EFingerTypes.Thumb },
            ring = new SingleFingerInjuryData { finger = EFingerTypes.Ring },
            pinky = new SingleFingerInjuryData { finger = EFingerTypes.Pinky },
        },
    };


}