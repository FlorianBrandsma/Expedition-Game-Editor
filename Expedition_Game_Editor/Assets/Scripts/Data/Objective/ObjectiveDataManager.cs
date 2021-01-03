using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public static class ObjectiveDataManager
{
    private static List<ObjectiveBaseData> objectiveDataList;

    public static List<IElementData> GetData(Search.Objective searchParameters)
    {
        GetObjectiveData(searchParameters);

        if (searchParameters.includeAddElement)
            objectiveDataList.Add(DefaultData(searchParameters.questId.First()));

        if (objectiveDataList.Count == 0) return new List<IElementData>();

        var list = (from objectiveData in objectiveDataList
                    select new ObjectiveElementData()
                    {
                        Id = objectiveData.Id,
                        
                        QuestId = objectiveData.QuestId,

                        Index = objectiveData.Index,

                        Name = objectiveData.Name,

                        Journal = objectiveData.Journal,

                        PublicNotes = objectiveData.PublicNotes,
                        PrivateNotes = objectiveData.PrivateNotes

                    }).OrderBy(x => x.Id > 0).ThenBy(x => x.Index).ToList();

        if (searchParameters.includeAddElement)
            SetDefaultAddValues(list);

        list.ForEach(x => x.SetOriginalValues());

        return list.Cast<IElementData>().ToList();
    }

    public static ObjectiveElementData DefaultData(int questId)
    {
        return new ObjectiveElementData()
        {
            QuestId = questId
        };
    }

    public static void SetDefaultAddValues(List<ObjectiveElementData> list)
    {
        var addElementData = list.Where(x => x.Id == 0).First();

        addElementData.ExecuteType = Enums.ExecuteType.Add;

        addElementData.Index = list.Count - 1;
    }

    private static void GetObjectiveData(Search.Objective searchParameters)
    {
        objectiveDataList = new List<ObjectiveBaseData>();

        foreach(ObjectiveBaseData objective in Fixtures.objectiveList)
        {
            if (searchParameters.id.Count       > 0 && !searchParameters.id.Contains(objective.Id))             continue;
            if (searchParameters.questId.Count  > 0 && !searchParameters.questId.Contains(objective.QuestId))   continue;

            objectiveDataList.Add(objective);
        }
    }

    public static void AddData(ObjectiveElementData elementData, DataRequest dataRequest)
    {
        if (dataRequest.requestType == Enums.RequestType.Execute)
        {
            elementData.Id = Fixtures.objectiveList.Count > 0 ? (Fixtures.objectiveList[Fixtures.objectiveList.Count - 1].Id + 1) : 1;
            Fixtures.objectiveList.Add(((ObjectiveData)elementData).Clone());

            elementData.SetOriginalValues();

        } else { }
    }

    public static void UpdateData(ObjectiveElementData elementData, DataRequest dataRequest)
    {
        if (!elementData.Changed) return;

        var data = Fixtures.objectiveList.Where(x => x.Id == elementData.Id).FirstOrDefault();

        if (dataRequest.requestType == Enums.RequestType.Execute)
        {
            if (elementData.ChangedName)
            {
                data.Name = elementData.Name;
            }

            if (elementData.ChangedJournal)
            {
                data.Journal = elementData.Journal;
            }

            if (elementData.ChangedPublicNotes)
            {
                data.PublicNotes = elementData.PublicNotes;
            }

            if (elementData.ChangedPrivateNotes)
            {
                data.PrivateNotes = elementData.PrivateNotes;
            }

            elementData.SetOriginalValues();

        } else { }   
    }

    static public void UpdateIndex(ObjectiveElementData elementData)
    {
        if (!elementData.ChangedIndex) return;

        var data = Fixtures.objectiveList.Where(x => x.Id == elementData.Id).FirstOrDefault();

        data.Index = elementData.Index;

        elementData.OriginalData.Index = elementData.Index;
    }
}
