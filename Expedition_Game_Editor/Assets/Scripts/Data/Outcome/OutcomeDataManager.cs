using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public static class OutcomeDataManager
{
    private static List<OutcomeBaseData> outcomeDataList;

    public static List<IElementData> GetData(SearchProperties searchProperties)
    {
        var searchParameters = searchProperties.searchParameters.Cast<Search.Outcome>().First();

        GetOutcomeData(searchParameters);

        if (outcomeDataList.Count == 0) return new List<IElementData>();

        var list = (from outcomeData in outcomeDataList
                    select new OutcomeElementData()
                    {
                        Id = outcomeData.Id,

                        Type = outcomeData.Type,

                        InteractionId = outcomeData.InteractionId
                        
                    }).OrderBy(x => x.Type).ToList();

        list.ForEach(x => x.SetOriginalValues());

        return list.Cast<IElementData>().ToList();
    }

    private static void GetOutcomeData(Search.Outcome searchParameters)
    {
        outcomeDataList = new List<OutcomeBaseData>();

        foreach (OutcomeBaseData outcome in Fixtures.outcomeList)
        {
            if (searchParameters.id.Count               > 0 && !searchParameters.id.Contains(outcome.Id)) continue;
            if (searchParameters.interactionId.Count    > 0 && !searchParameters.interactionId.Contains(outcome.InteractionId)) continue;
            
            var outcomeData = new OutcomeBaseData();

            outcomeData.Id = outcome.Id;

            outcomeData.Type = outcome.Type;

            outcomeData.InteractionId = outcome.InteractionId;
            
            outcomeDataList.Add(outcomeData);
        }
    }

    public static void UpdateData(OutcomeElementData elementData)
    {
        var data = Fixtures.outcomeList.Where(x => x.Id == elementData.Id).FirstOrDefault();
    }
}
