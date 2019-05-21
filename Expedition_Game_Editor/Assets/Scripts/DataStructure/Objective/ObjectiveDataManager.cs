using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ObjectiveDataManager
{
    private ObjectiveController objectiveController;
    private List<ObjectiveData> objectiveDataList;

    public void InitializeManager(ObjectiveController objectiveController)
    {
        this.objectiveController = objectiveController;
    }

    public List<ObjectiveDataElement> GetObjectiveDataElements(IEnumerable searchParameters)
    {
        var objectiveSearchData = searchParameters.Cast<Search.Objective>().FirstOrDefault();

        GetObjectiveData(objectiveSearchData);

        var list = (from objectiveData in objectiveDataList
                    select new ObjectiveDataElement()
                    {
                        id = objectiveData.id,
                        table = objectiveData.table,

                        Index = objectiveData.index,
                        Name = objectiveData.name,
                        Journal = objectiveData.journal,
                        Notes = objectiveData.notes

                    }).OrderBy(x => x.Index).ToList();

        list.ForEach(x => x.SetOriginalValues());

        return list;
    }

    public void GetObjectiveData(Search.Objective searchParameters)
    {
        objectiveDataList = new List<ObjectiveData>();
        
        foreach(Fixtures.Objective objective in Fixtures.objectiveList)
        {
            var objectiveData = new ObjectiveData();
            
            objectiveData.id = objective.id;
            objectiveData.table = "Objective";
            objectiveData.index = objective.index;

            if (searchParameters.questId.Count > 0 && !searchParameters.questId.Contains(objective.questId)) continue;

            objectiveData.questId = objective.questId;
            objectiveData.name = objective.name;
            objectiveData.journal = objective.journal;
            objectiveData.notes = objective.notes;

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
