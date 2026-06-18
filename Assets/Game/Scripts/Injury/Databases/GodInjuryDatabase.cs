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
}
