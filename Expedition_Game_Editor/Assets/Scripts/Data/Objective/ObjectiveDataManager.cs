using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public static class ObjectiveDataManager
{
    private static List<ObjectiveBaseData> objectiveDataList;

    public static List<IElementData> GetData(SearchProperties searchProperties)
    {
        var searchParameters = searchProperties.searchParameters.Cast<Search.Objective>().First();
        
        GetObjectiveData(searchParameters);

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

                    }).OrderBy(x => x.Index).ToList();

        list.ForEach(x => x.SetOriginalValues());

        return list.Cast<IElementData>().ToList();
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

    public static void UpdateData(ObjectiveElementData elementData)
    {
        var data = Fixtures.objectiveList.Where(x => x.Id == elementData.Id).FirstOrDefault();
        
        if (elementData.ChangedName)
            data.Name = elementData.Name;

        if (elementData.ChangedJournal)
            data.Journal = elementData.Journal;

        if (elementData.ChangedPublicNotes)
            data.PublicNotes = elementData.PublicNotes;

        if (elementData.ChangedPrivateNotes)
            data.PrivateNotes = elementData.PrivateNotes;
    }

    static public void UpdateIndex(ObjectiveElementData elementData)
    {
        var data = Fixtures.objectiveList.Where(x => x.Id == elementData.Id).FirstOrDefault();

        data.Index = elementData.Index;
    }
}
