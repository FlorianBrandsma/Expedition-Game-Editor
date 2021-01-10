using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public static class ObjectiveSaveDataManager
{
    private static List<ObjectiveBaseData> objectiveDataList;

    private static List<ObjectiveSaveBaseData> objectiveSaveDataList;
    
    public static List<IElementData> GetData(Search.ObjectiveSave searchParameters)
    {
        GetObjectiveData(searchParameters);

        if (objectiveDataList.Count == 0) return new List<IElementData>();

        GetObjectiveSaveData(searchParameters);

        var list = (from objectiveData      in objectiveDataList
                    join objectiveSaveData  in objectiveSaveDataList on objectiveData.Id equals objectiveSaveData.ObjectiveId
                    select new ObjectiveSaveElementData()
                    {
                        Id = objectiveSaveData.Id,

                        ObjectiveId = objectiveSaveData.ObjectiveId,

                        Complete = objectiveSaveData.Complete,

                        Index = objectiveData.Index,

                        Name = objectiveData.Name,

                        PublicNotes = objectiveData.PublicNotes,
                        PrivateNotes = objectiveData.PrivateNotes,

                        QuestId = objectiveData.QuestId

                    }).OrderBy(x => x.Index).ToList();

        list.ForEach(x => x.SetOriginalValues());

        return list.Cast<IElementData>().ToList();
    }

    public static ObjectiveSaveElementData DefaultData(int saveId, int objectiveId)
    {
        return new ObjectiveSaveElementData()
        {
            SaveId = saveId,
            ObjectiveId = objectiveId
        };
    }

    private static void GetObjectiveData(Search.ObjectiveSave searchParameters)
    {
        objectiveDataList = new List<ObjectiveBaseData>();

        foreach (ObjectiveBaseData objective in Fixtures.objectiveList)
        {
            if (searchParameters.questId.Count > 0 && !searchParameters.questId.Contains(objective.QuestId)) continue;

            objectiveDataList.Add(objective);
        }
    }

    private static void GetObjectiveSaveData(Search.ObjectiveSave searchParameters)
    {
        searchParameters.objectiveId = objectiveDataList.Select(x => x.Id).Distinct().ToList();

        objectiveSaveDataList = DataManager.GetObjectiveSaveData(searchParameters);
    }

    public static void AddData(ObjectiveSaveElementData elementData, DataRequest dataRequest)
    {
        if (dataRequest.requestType == Enums.RequestType.Execute)
        {
            elementData.Id = Fixtures.objectiveSaveList.Count > 0 ? (Fixtures.objectiveSaveList[Fixtures.objectiveSaveList.Count - 1].Id + 1) : 1;
            Fixtures.objectiveSaveList.Add(((ObjectiveSaveData)elementData).Clone());

            elementData.SetOriginalValues();

        } else { }
    }

    public static void UpdateData(ObjectiveSaveElementData elementData, DataRequest dataRequest)
    {
        if (!elementData.Changed) return;

        var data = Fixtures.objectiveSaveList.Where(x => x.Id == elementData.Id).FirstOrDefault();

        if (dataRequest.requestType == Enums.RequestType.Execute)
        {
            if (elementData.ChangedComplete)
            {
                data.Complete = elementData.Complete;
            }

            elementData.SetOriginalValues();

        } else { }
    }

    public static void RemoveData(ObjectiveSaveElementData elementData, DataRequest dataRequest)
    {
        if (dataRequest.requestType == Enums.RequestType.Execute)
        {
            Fixtures.objectiveSaveList.RemoveAll(x => x.Id == elementData.Id);

        } else { }
    }
}
