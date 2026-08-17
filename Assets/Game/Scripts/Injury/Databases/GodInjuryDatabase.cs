using Game.Body;
using System.Collections.Generic;
using UnityEngine;

public static class GodInjuryDatabase
{
    private static readonly Dictionary<EBodyRegion, IDatabaseBase> Databases =
        new()
        {
            { EBodyRegion.Torso, new TorsoInjuryDatabase() },
            { EBodyRegion.Leg, new LegInjuryDatabase() },
            { EBodyRegion.Arm, new HandInjuryDatabase() },
            {EBodyRegion.Fullbody, new FullbodyInjuryDatabase()  },
           // { EBodyRegion.Head, new HeadInjuryDatabase() }
        };

    public static IDatabaseBase Get(EBodyRegion region)
    {
        return Databases[region];
    }

    public static List<IInjuryData> GetAll()
    {
        List<IInjuryData> all = new List<IInjuryData>();
        all.AddRange(Databases[EBodyRegion.Torso].GetAllInjuries());
        all.AddRange(Databases[EBodyRegion.Leg].GetAllInjuries());
        all.AddRange(Databases[EBodyRegion.Arm].GetAllInjuries());
        all.AddRange(Databases[EBodyRegion.Fullbody].GetAllInjuries());

        return all;
    }

    public static List<IInjuryData> GetAllShown()
    {

        List<IInjuryData> all = new List<IInjuryData>();
        all.AddRange(Databases[EBodyRegion.Torso].GetShownInjuries());
        all.AddRange(Databases[EBodyRegion.Leg].GetShownInjuries());
        all.AddRange(Databases[EBodyRegion.Arm].GetShownInjuries());
        all.AddRange(Databases[EBodyRegion.Fullbody].GetShownInjuries());

        return all;
    }
}
