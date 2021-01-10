using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public static class QuestSaveDataManager
{
    private static List<QuestBaseData> questDataList;

    private static List<QuestSaveBaseData> questSaveDataList;
    
    public static List<IElementData> GetData(Search.QuestSave searchParameters)
    {
        GetQuestData(searchParameters);
        
        if (questDataList.Count == 0) return new List<IElementData>();

        GetQuestSaveData(searchParameters);

        var list = (from questData      in questDataList
                    join questSaveData  in questSaveDataList on questData.Id equals questSaveData.QuestId
                    select new QuestSaveElementData()
                    {
                        Id = questSaveData.Id,
                        
                        QuestId = questSaveData.QuestId,

                        Complete = questSaveData.Complete,

                        Name = questData.Name,

                        Index = questData.Index,

                        PublicNotes = questData.PublicNotes,
                        PrivateNotes = questData.PrivateNotes,

                        PhaseId = questData.PhaseId

                    }).OrderBy(x => x.Index).ToList();

        list.ForEach(x => x.SetOriginalValues());

        return list.Cast<IElementData>().ToList();
    }

    public static QuestSaveElementData DefaultData(int saveId, int questId)
    {
        return new QuestSaveElementData()
        {
            SaveId = saveId,
            QuestId = questId
        };
    }

    private static void GetQuestData(Search.QuestSave searchParameters)
    {
        questDataList = new List<QuestBaseData>();

        foreach (QuestBaseData quest in Fixtures.questList)
        {
            if (searchParameters.phaseId.Count > 0 && !searchParameters.phaseId.Contains(quest.PhaseId)) continue;

            questDataList.Add(quest);
        }
    }

    private static void GetQuestSaveData(Search.QuestSave searchParameters)
    {
        searchParameters.questId = questDataList.Select(x => x.Id).Distinct().ToList();

        questSaveDataList = DataManager.GetQuestSaveData(searchParameters);
    }

    public static void AddData(QuestSaveElementData elementData, DataRequest dataRequest)
    {
        if (dataRequest.requestType == Enums.RequestType.Execute)
        {
            elementData.Id = Fixtures.questSaveList.Count > 0 ? (Fixtures.questSaveList[Fixtures.questSaveList.Count - 1].Id + 1) : 1;
            Fixtures.questSaveList.Add(((QuestSaveData)elementData).Clone());

            elementData.SetOriginalValues();

        } else { }
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

    public static void RemoveData(QuestSaveElementData elementData, DataRequest dataRequest)
    {
        if (dataRequest.requestType == Enums.RequestType.Execute)
        {
            Fixtures.questSaveList.RemoveAll(x => x.Id == elementData.Id);

        } else { }
    }
}
