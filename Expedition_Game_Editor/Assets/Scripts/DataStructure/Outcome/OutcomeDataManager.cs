using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class OutcomeDataManager : IDataManager
{
    public IDataController DataController { get; set; }

    private List<OutcomeData> outcomeDataList;

    public OutcomeDataManager(IDataController dataController)
    {
        DataController = dataController;
    }

    public List<IDataElement> GetDataElements(SearchProperties searchProperties)
    {
        var searchParameters = searchProperties.searchParameters.Cast<Search.Outcome>().First();

        GetOutcomeData(searchParameters);

        if (outcomeDataList.Count == 0) return new List<IDataElement>();

        var list = (from outcomeData in outcomeDataList
                    select new OutcomeDataElement()
                    {
                        Id = outcomeData.Id,

                        Type = outcomeData.type,

                        InteractionId = outcomeData.interactionId
                        
                    }).OrderBy(x => x.Index).ToList();

        list.ForEach(x => x.SetOriginalValues());

        return list.Cast<IDataElement>().ToList();
    }

    public void GetOutcomeData(Search.Outcome searchParameters)
    {
        outcomeDataList = new List<OutcomeData>();

        foreach (Fixtures.Outcome outcome in Fixtures.outcomeList)
        {
            if (searchParameters.id.Count > 0 && !searchParameters.id.Contains(outcome.Id)) continue;
            if (searchParameters.interactionId.Count > 0 && !searchParameters.interactionId.Contains(outcome.interactionId)) continue;
            
            var outcomeData = new OutcomeData();

            outcomeData.Id = outcome.Id;

            outcomeData.type = outcome.type;

            outcomeData.interactionId = outcome.interactionId;
            
            outcomeDataList.Add(outcomeData);
        }
    }

    internal class OutcomeData : GeneralData
    {
        public int type;

        public int interactionId;
    }
}
