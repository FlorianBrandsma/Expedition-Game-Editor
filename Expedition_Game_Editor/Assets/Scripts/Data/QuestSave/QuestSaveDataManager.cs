using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public static class QuestSaveDataManager
{
    private static List<QuestSaveBaseData> questSaveDataList;

    private static List<QuestBaseData> questDataList;

    public static List<IElementData> GetData(SearchProperties searchProperties)
    {
        var searchParameters = searchProperties.searchParameters.Cast<Search.QuestSave>().First();

        GetQuestSaveData(searchParameters);

        if (questSaveDataList.Count == 0) return new List<IElementData>();

        GetQuestData();

        var list = (from questSaveData  in questSaveDataList
                    join questData      in questDataList on questSaveData.QuestId equals questData.Id
                    select new QuestSaveElementData()
                    {
                        Id = questSaveData.Id,
                        Index = questSaveData.Index,

                        PhaseSaveId = questSaveData.PhaseSaveId,
                        QuestId = questSaveData.QuestId,

                        Complete = questSaveData.Complete,

                        Name = questData.Name,

                        PublicNotes = questData.PublicNotes

                    }).OrderBy(x => x.Index).ToList();

        list.ForEach(x => x.SetOriginalValues());

        return list.Cast<IElementData>().ToList();
    }

    private static void GetQuestSaveData(Search.QuestSave searchParameters)
    {
        questSaveDataList = new List<QuestSaveBaseData>();

        foreach (QuestSaveBaseData questSave in Fixtures.questSaveList)
        {
            if (searchParameters.id.Count           > 0 && !searchParameters.id.Contains(questSave.Id))                     continue;
            if (searchParameters.phaseSaveId.Count  > 0 && !searchParameters.phaseSaveId.Contains(questSave.PhaseSaveId))   continue;

            var questSaveData = new QuestSaveBaseData();

            questSaveData.Id = questSave.Id;
            questSaveData.Index = questSave.Index;

            questSaveData.PhaseSaveId = questSave.PhaseSaveId;
            questSaveData.QuestId = questSave.QuestId;

            questSaveData.Complete = questSave.Complete;

            questSaveDataList.Add(questSaveData);
        }
    }

    private static void GetQuestData()
    {
        var questSearchParameters = new Search.Quest();
        questSearchParameters.id = questSaveDataList.Select(x => x.QuestId).Distinct().ToList();

        questDataList = DataManager.GetQuestData(questSearchParameters);
    }

    public static void UpdateData(QuestSaveElementData elementData)
    {
        var data = Fixtures.questSaveList.Where(x => x.Id == elementData.Id).FirstOrDefault();
        
        if (elementData.ChangedComplete)
            data.Complete = elementData.Complete;
    }
}
