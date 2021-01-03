using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public static class QuestDataManager
{
    private static List<QuestBaseData> questDataList;

    public static List<IElementData> GetData(Search.Quest searchParameters)
    {
        GetQuestData(searchParameters);

        if (searchParameters.includeAddElement)
            questDataList.Add(DefaultData(searchParameters.phaseId.First()));

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

        if (searchParameters.includeAddElement)
            SetDefaultAddValues(list);

        list.ForEach(x => x.SetOriginalValues());

        return list.Cast<IElementData>().ToList();
    }

    public static QuestElementData DefaultData(int phaseId)
    {
        return new QuestElementData()
        {
            PhaseId = phaseId
        };
    }

    public static void SetDefaultAddValues(List<QuestElementData> list)
    {
        var addElementData = list.Where(x => x.Id == 0).First();

        addElementData.ExecuteType = Enums.ExecuteType.Add;

        addElementData.Index = list.Count - 1;
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

    public static void AddData(QuestElementData elementData, DataRequest dataRequest)
    {
        if (dataRequest.requestType == Enums.RequestType.Execute)
        {
            elementData.Id = Fixtures.questList.Count > 0 ? (Fixtures.questList[Fixtures.questList.Count - 1].Id + 1) : 1;
            Fixtures.questList.Add(((QuestData)elementData).Clone());

            elementData.SetOriginalValues();

        } else { }
    }

    public static void UpdateData(QuestElementData elementData, DataRequest dataRequest)
    {
        if (!elementData.Changed) return;

        var data = Fixtures.questList.Where(x => x.Id == elementData.Id).FirstOrDefault();

        if (dataRequest.requestType == Enums.RequestType.Execute)
        {
            if (elementData.ChangedName)
            {
                data.Name = elementData.Name;
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

    static public void UpdateIndex(QuestElementData elementData)
    {
        if (!elementData.ChangedIndex) return;

        var data = Fixtures.questList.Where(x => x.Id == elementData.Id).FirstOrDefault();

        data.Index = elementData.Index;

        elementData.OriginalData.Index = elementData.Index;
    }
}
