using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public static class ObjectiveSaveDataManager
{
    private static List<ObjectiveSaveBaseData> objectiveSaveDataList;

    private static List<ObjectiveBaseData> objectiveDataList;

    public static List<IElementData> GetData(SearchProperties searchProperties)
    {
        var searchParameters = searchProperties.searchParameters.Cast<Search.ObjectiveSave>().First();

        GetObjectiveSaveData(searchParameters);

        if (objectiveSaveDataList.Count == 0) return new List<IElementData>();

        GetObjectiveData();

        var list = (from objectiveSaveData  in objectiveSaveDataList
                    join objectiveData      in objectiveDataList on objectiveSaveData.ObjectiveId equals objectiveData.Id
                    select new ObjectiveSaveElementData()
                    {
                        Id = objectiveSaveData.Id,
                        
                        QuestSaveId = objectiveSaveData.QuestSaveId,
                        ObjectiveId = objectiveSaveData.ObjectiveId,

                        Complete = objectiveSaveData.Complete,

                        Index = objectiveData.Index,

                        Name = objectiveData.Name,

                        PublicNotes = objectiveData.PublicNotes,
                        PrivateNotes = objectiveData.PrivateNotes

                    }).OrderBy(x => x.Index).ToList();

        list.ForEach(x => x.SetOriginalValues());

        return list.Cast<IElementData>().ToList();
    }

    private static void GetObjectiveSaveData(Search.ObjectiveSave searchParameters)
    {
        objectiveSaveDataList = new List<ObjectiveSaveBaseData>();

        foreach (ObjectiveSaveBaseData objectiveSave in Fixtures.objectiveSaveList)
        {
            if (searchParameters.id.Count           > 0 && !searchParameters.id.Contains(objectiveSave.Id))                     continue;
            if (searchParameters.questSaveId.Count  > 0 && !searchParameters.questSaveId.Contains(objectiveSave.QuestSaveId))   continue;

            objectiveSaveDataList.Add(objectiveSave);
        }
    }

    private static void GetObjectiveData()
    {
        var searchParameters = new Search.Objective();
        searchParameters.id = objectiveSaveDataList.Select(x => x.ObjectiveId).Distinct().ToList();

        objectiveDataList = DataManager.GetObjectiveData(searchParameters);
    }

    public static void UpdateData(ObjectiveSaveElementData elementData, DataRequest dataRequest)
    {
        var data = Fixtures.objectiveSaveList.Where(x => x.Id == elementData.Id).FirstOrDefault();
        
        if (elementData.ChangedComplete)
        {
            if (dataRequest.requestType == Enums.RequestType.Execute)
                data.Complete = elementData.Complete;
            else { }
        }
    }
}
