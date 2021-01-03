using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public static class InteractionSaveDataManager
{
    private static List<InteractionSaveBaseData> interactionSaveDataList;

    private static List<InteractionBaseData> interactionDataList;

    public static List<IElementData> GetData(Search.InteractionSave searchParameters)
    {
        GetInteractionSaveData(searchParameters);

        if (interactionSaveDataList.Count == 0) return new List<IElementData>();

        GetInteractionData();

        var list = (from interactionSaveData    in interactionSaveDataList
                    join interactionData        in interactionDataList on interactionSaveData.InteractionId equals interactionData.Id
                    select new InteractionSaveElementData()
                    {
                        Id = interactionSaveData.Id,

                        TaskSaveId = interactionSaveData.TaskSaveId,
                        InteractionId = interactionSaveData.InteractionId,

                        Complete = interactionSaveData.Complete,

                        Default = interactionData.Default,

                        StartTime = interactionData.StartTime,
                        EndTime = interactionData.EndTime,

                        PublicNotes = interactionData.PublicNotes,
                        PrivateNotes = interactionData.PrivateNotes

                    }).OrderByDescending(x => x.Default).ThenBy(x => x.StartTime).ToList();

        list.ForEach(x => x.SetOriginalValues());

        return list.Cast<IElementData>().ToList();
    }

    private static void GetInteractionSaveData(Search.InteractionSave searchParameters)
    {
        interactionSaveDataList = new List<InteractionSaveBaseData>();

        foreach (InteractionSaveBaseData interactionSave in Fixtures.interactionSaveList)
        {
            if (searchParameters.id.Count           > 0 && !searchParameters.id.Contains(interactionSave.Id))                   continue;
            if (searchParameters.taskSaveId.Count   > 0 && !searchParameters.taskSaveId.Contains(interactionSave.TaskSaveId))   continue;

            interactionSaveDataList.Add(interactionSave);
        }
    }

    private static void GetInteractionData()
    {
        var searchParameters = new Search.Interaction();
        searchParameters.id = interactionSaveDataList.Select(x => x.InteractionId).Distinct().ToList();

        interactionDataList = DataManager.GetInteractionData(searchParameters);
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
}
