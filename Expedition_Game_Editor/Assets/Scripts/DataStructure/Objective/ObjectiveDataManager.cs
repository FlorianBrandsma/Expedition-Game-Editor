using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ObjectiveDataManager : IDataManager
{
    public IDataController DataController { get; set; }

    private List<ObjectiveData> objectiveDataList;

    public ObjectiveDataManager(ObjectiveController objectiveController)
    {
        DataController = objectiveController;
    }

    public List<IDataElement> GetDataElements(SearchProperties searchProperties)
    {
        var searchParameters = searchProperties.searchParameters.Cast<Search.Objective>().First();
        
        GetObjectiveData(searchParameters);

        if (objectiveDataList.Count == 0) return new List<IDataElement>();

        var list = (from objectiveData in objectiveDataList
                    select new ObjectiveDataElement()
                    {
                        Id = objectiveData.Id,
                        Index = objectiveData.Index,

                        QuestId = objectiveData.questId,
                        Name = objectiveData.name,
                        Journal = objectiveData.journal,
                        PublicNotes = objectiveData.notes

                    }).OrderBy(x => x.Index).ToList();

        list.ForEach(x => x.SetOriginalValues());

        return list.Cast<IDataElement>().ToList();
    }

    public void GetObjectiveData(Search.Objective searchParameters)
    {
        objectiveDataList = new List<ObjectiveData>();

        foreach(Fixtures.Objective objective in Fixtures.objectiveList)
        {
            if (searchParameters.id.Count > 0 && !searchParameters.id.Contains(objective.Id)) continue;
            if (searchParameters.questId.Count > 0 && !searchParameters.questId.Contains(objective.questId)) continue;

            var objectiveData = new ObjectiveData();
            
            objectiveData.Id = objective.Id;
            objectiveData.Index = objective.Index;

            objectiveData.questId = objective.questId;
            objectiveData.name = objective.name;
            objectiveData.journal = objective.journal;
            objectiveData.notes = objective.publicNotes;

            objectiveDataList.Add(objectiveData);
        }
    }

    internal class ObjectiveData : GeneralData
    {
        public int questId;
        public string name;
        public string journal;
        public string notes;
    }
}
