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

                    }).OrderBy(x => x.Index).ToList();

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

    public static void UpdateData(QuestElementData elementData)
    {
        var data = Fixtures.questList.Where(x => x.Id == elementData.Id).FirstOrDefault();
        
        if (elementData.ChangedName)
            data.Name = elementData.Name;

        if (elementData.ChangedPublicNotes)
            data.PublicNotes = elementData.PublicNotes;

        if (elementData.ChangedPrivateNotes)
            data.PrivateNotes = elementData.PrivateNotes;
    }

    static public void UpdateIndex(QuestElementData elementData)
    {
        var data = Fixtures.questList.Where(x => x.Id == elementData.Id).FirstOrDefault();

        data.Index = elementData.Index;
    }
}
