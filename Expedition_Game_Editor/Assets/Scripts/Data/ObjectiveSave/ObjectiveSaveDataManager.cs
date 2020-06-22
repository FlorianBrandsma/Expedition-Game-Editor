using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ObjectiveSaveDataManager : IDataManager
{
    public IDataController DataController { get; set; }

    private List<ObjectiveSaveData> objectiveSaveDataList;

    private DataManager dataManager = new DataManager();

    private List<DataManager.ObjectiveData> objectiveDataList;

    public ObjectiveSaveDataManager(ObjectiveSaveController objectiveController)
    {
        DataController = objectiveController;
    }

    public List<IElementData> GetData(SearchProperties searchProperties)
    {
        var searchParameters = searchProperties.searchParameters.Cast<Search.ObjectiveSave>().First();

        GetObjectiveSaveData(searchParameters);

        if (objectiveSaveDataList.Count == 0) return new List<IElementData>();

        GetObjectiveData();

        var list = (from objectiveSaveData  in objectiveSaveDataList
                    join objectiveData      in objectiveDataList on objectiveSaveData.objectiveId equals objectiveData.Id
                    select new ObjectiveSaveElementData()
                    {
                        Id = objectiveSaveData.Id,

                        QuestSaveId = objectiveSaveData.questSaveId,
                        ObjectiveId = objectiveSaveData.objectiveId,

                        Complete = objectiveSaveData.complete,

                        name = objectiveData.name,

                        publicNotes = objectiveData.publicNotes

                    }).OrderBy(x => x.Index).ToList();

        list.ForEach(x => x.SetOriginalValues());

        return list.Cast<IElementData>().ToList();
    }

    public void GetObjectiveSaveData(Search.ObjectiveSave searchParameters)
    {
        objectiveSaveDataList = new List<ObjectiveSaveData>();

        foreach (Fixtures.ObjectiveSave objectiveSave in Fixtures.objectiveSaveList)
        {
            if (searchParameters.id.Count           > 0 && !searchParameters.id.Contains(objectiveSave.Id))                     continue;
            if (searchParameters.questSaveId.Count  > 0 && !searchParameters.questSaveId.Contains(objectiveSave.questSaveId))   continue;

            var objectiveSaveData = new ObjectiveSaveData();

            objectiveSaveData.Id = objectiveSave.Id;

            objectiveSaveData.questSaveId = objectiveSave.questSaveId;
            objectiveSaveData.objectiveId = objectiveSave.objectiveId;

            objectiveSaveData.complete = objectiveSave.complete;

            objectiveSaveDataList.Add(objectiveSaveData);
        }
    }

    internal void GetObjectiveData()
    {
        var objectiveSearchParameters = new Search.Objective();
        objectiveSearchParameters.id = objectiveSaveDataList.Select(x => x.objectiveId).Distinct().ToList();

        objectiveDataList = dataManager.GetObjectiveData(objectiveSearchParameters);
    }

    internal class ObjectiveSaveData : GeneralData
    {
        public int questSaveId;
        public int objectiveId;

        public bool complete;
    }
}
