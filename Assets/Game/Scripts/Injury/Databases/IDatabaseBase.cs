using System.Collections.Generic;


public interface IDatabaseBase
{
     IInjuryData GetRandomInjury();
    List<IInjuryData> GetAllInjuries();

    List<IInjuryData> GetShownInjuries();

}
