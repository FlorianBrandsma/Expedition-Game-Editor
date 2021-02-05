using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public static class InteractionSaveDataManager
{
    private static List<InteractionBaseData> interactionDataList;

    private static List<InteractionSaveBaseData> interactionSaveDataList;
    
    public static List<IElementData> GetData(Search.InteractionSave searchParameters)
    {
        GetInteractionData(searchParameters);

        if (interactionDataList.Count == 0) return new List<IElementData>();

        GetInteractionSaveData(searchParameters);
        
        var list = (from interactionData        in interactionDataList
                    join interactionSaveData    in interactionSaveDataList on interactionData.Id equals interactionSaveData.InteractionId
                    select new InteractionSaveElementData()
                    {
                        Id = interactionSaveData.Id,

                        InteractionId = interactionSaveData.InteractionId,

                        Complete = interactionSaveData.Complete,

                        Default = interactionData.Default,

                        StartTime = interactionData.StartTime,
                        EndTime = interactionData.EndTime,

                        EditorNotes = interactionData.EditorNotes,
                        GameNotes = interactionData.GameNotes,

                        TaskId = interactionData.TaskId

                    }).OrderByDescending(x => x.Default).ThenBy(x => x.StartTime).ToList();

        list.ForEach(x => x.SetOriginalValues());

        return list.Cast<IElementData>().ToList();
    }

    public static InteractionSaveElementData DefaultData(int saveId, int interactionId)
    {
        return new InteractionSaveElementData()
        {
            Id = -1,

            SaveId = saveId,
            InteractionId = interactionId
        };
    }

    private static void GetInteractionData(Search.InteractionSave searchParameters)
    {
        interactionDataList = new List<InteractionBaseData>();

        foreach (InteractionBaseData interaction in Fixtures.interactionList)
        {
            if (searchParameters.taskId.Count > 0 && !searchParameters.taskId.Contains(interaction.TaskId)) continue;

            interactionDataList.Add(interaction);
        }
    }

    private static void GetInteractionSaveData(Search.InteractionSave searchParameters)
    {
        searchParameters.interactionId = interactionDataList.Select(x => x.Id).Distinct().ToList();

        interactionSaveDataList = DataManager.GetInteractionSaveData(searchParameters);
    }

    public static void AddData(InteractionSaveElementData elementData, DataRequest dataRequest)
    {
        if (dataRequest.requestType == Enums.RequestType.Execute)
        {
            elementData.Id = Fixtures.interactionSaveList.Count > 0 ? (Fixtures.interactionSaveList[Fixtures.interactionSaveList.Count - 1].Id + 1) : 1;
            Fixtures.interactionSaveList.Add(((InteractionSaveData)elementData).Clone());

            elementData.SetOriginalValues();

        } else { }
    }

    public static void UpdateData(InteractionSaveElementData elementData, DataRequest dataRequest)
    {
        if (!elementData.Changed) return;

        var data = Fixtures.interactionSaveList.Where(x => x.Id == elementData.Id).FirstOrDefault();

        if (dataRequest.requestType == Enums.RequestType.Execute)
        {
            if (elementData.ChangedComplete)
            {
                data.Complete = elementData.Complete;
            }

            elementData.SetOriginalValues();

        } else { }       
    }

    public static void RemoveData(InteractionSaveElementData elementData, DataRequest dataRequest)
    {
        if (dataRequest.requestType == Enums.RequestType.Execute)
        {
            Fixtures.interactionSaveList.RemoveAll(x => x.Id == elementData.Id);

        } else { }
    }
}
