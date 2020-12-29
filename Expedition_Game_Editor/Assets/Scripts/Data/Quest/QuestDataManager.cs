using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public static class QuestDataManager
{
    private static List<QuestBaseData> questDataList;

    public static List<IElementData> GetData(SearchProperties searchProperties)
    {
        var searchParameters = searchProperties.searchParameters.Cast<Search.Quest>().First();

        GetQuestData(searchParameters);

        if (questDataList.Count == 0) return new List<IElementData>();

        var list = (from questData in questDataList
                    select new QuestElementData()
                    {
                        Id = questData.Id,
                        Index = questData.Index,

                        PhaseId = questData.PhaseId,

                        Name = questData.Name,

                        PublicNotes = questData.PublicNotes,
                        PrivateNotes = questData.PrivateNotes

                    }).OrderBy(x => x.Id > 0).ThenBy(x => x.Index).ToList();

        list.ForEach(x => x.SetOriginalValues());

        return list.Cast<IElementData>().ToList();
    }

    private static void GetQuestData(Search.Quest searchParameters)
    {
        questDataList = new List<QuestBaseData>();

        foreach(QuestBaseData quest in Fixtures.questList)
        {
            if (searchParameters.id.Count       > 0 && !searchParameters.id.Contains(quest.Id)) continue;
            if (searchParameters.phaseId.Count  > 0 && !searchParameters.phaseId.Contains(quest.PhaseId)) continue;

            questDataList.Add(quest);
        }
    }

    public static void UpdateData(QuestElementData elementData, DataRequest dataRequest)
    {
        var data = Fixtures.questList.Where(x => x.Id == elementData.Id).FirstOrDefault();
        
        if (elementData.ChangedName)
        {
            if (dataRequest.requestType == Enums.RequestType.Execute)
                data.Name = elementData.Name;
            else { }
        }

        if (elementData.ChangedPublicNotes)
        {
            if (dataRequest.requestType == Enums.RequestType.Execute)
                data.PublicNotes = elementData.PublicNotes;
            else { }
        }

        if (elementData.ChangedPrivateNotes)
        {
            if (dataRequest.requestType == Enums.RequestType.Execute)
                data.PrivateNotes = elementData.PrivateNotes;
            else { }
        }
    }

    static public void UpdateIndex(QuestElementData elementData)
    {
        var data = Fixtures.questList.Where(x => x.Id == elementData.Id).FirstOrDefault();

        data.Index = elementData.Index;
    }
}
