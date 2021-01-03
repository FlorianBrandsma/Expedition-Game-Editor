using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public static class QuestSaveDataManager
{
    private static List<QuestSaveBaseData> questSaveDataList;

    private static List<QuestBaseData> questDataList;

    public static List<IElementData> GetData(Search.QuestSave searchParameters)
    {
        GetQuestSaveData(searchParameters);

        if (questSaveDataList.Count == 0) return new List<IElementData>();

        GetQuestData();

        var list = (from questSaveData  in questSaveDataList
                    join questData      in questDataList on questSaveData.QuestId equals questData.Id
                    select new QuestSaveElementData()
                    {
                        Id = questSaveData.Id,
                        
                        PhaseSaveId = questSaveData.PhaseSaveId,
                        QuestId = questSaveData.QuestId,

                        Complete = questSaveData.Complete,

                        Name = questData.Name,

                        Index = questData.Index,

                        PublicNotes = questData.PublicNotes,
                        PrivateNotes = questData.PrivateNotes

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

            questSaveDataList.Add(questSave);
        }
    }

    private static void GetQuestData()
    {
        var searchParameters = new Search.Quest();
        searchParameters.id = questSaveDataList.Select(x => x.QuestId).Distinct().ToList();

        questDataList = DataManager.GetQuestData(searchParameters);
    }

    public static void UpdateData(QuestSaveElementData elementData, DataRequest dataRequest)
    {
        if (!elementData.Changed) return;

        var data = Fixtures.questSaveList.Where(x => x.Id == elementData.Id).FirstOrDefault();

        if (dataRequest.requestType == Enums.RequestType.Execute)
        {
            if (elementData.ChangedComplete)
            {
                data.Complete = elementData.Complete;
            }

            elementData.SetOriginalValues();

        } else { }    
    }
}
