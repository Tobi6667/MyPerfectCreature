using System.Collections.Generic;
using UnityEngine;

public interface IDatabaseBase
{
     IInjuryData GetRandomInjury();
    List<IInjuryData> GetAllInjuries();

    List<IInjuryData> GetShownInjuries();

}
