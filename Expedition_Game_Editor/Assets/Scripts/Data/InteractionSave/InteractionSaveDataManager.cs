﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class InteractionSaveDataManager : IDataManager
{
    public IDataController DataController { get; set; }

    private List<InteractionSaveData> interactionSaveDataList;

    private DataManager dataManager = new DataManager();

    private List<DataManager.InteractionData> interactionDataList;

    public InteractionSaveDataManager(InteractionSaveController interactionController)
    {
        DataController = interactionController;
    }

    public List<IDataElement> GetDataElements(SearchProperties searchProperties)
    {
        var searchParameters = searchProperties.searchParameters.Cast<Search.InteractionSave>().First();

        GetInteractionSaveData(searchParameters);

        if (interactionSaveDataList.Count == 0) return new List<IDataElement>();

        GetInteractionData();

        var list = (from interactionSaveData    in interactionSaveDataList
                    join interactionData        in interactionDataList on interactionSaveData.interactionId equals interactionData.Id
                    select new InteractionSaveDataElement()
                    {
                        Id = interactionSaveData.Id,

                        TaskSaveId = interactionSaveData.taskSaveId,
                        InteractionId = interactionSaveData.interactionId,

                        Complete = interactionSaveData.complete,

                        isDefault = interactionData.isDefault,

                        startTime = interactionData.startTime,
                        endTime = interactionData.endTime,

                        publicNotes = interactionData.publicNotes

                    }).OrderBy(x => x.Index).ToList();

        list.ForEach(x => x.SetOriginalValues());

        return list.Cast<IDataElement>().ToList();
    }

    public void GetInteractionSaveData(Search.InteractionSave searchParameters)
    {
        interactionSaveDataList = new List<InteractionSaveData>();

        foreach (Fixtures.InteractionSave interactionSave in Fixtures.interactionSaveList)
        {
            if (searchParameters.id.Count           > 0 && !searchParameters.id.Contains(interactionSave.Id))                   continue;
            if (searchParameters.taskSaveId.Count   > 0 && !searchParameters.taskSaveId.Contains(interactionSave.taskSaveId))   continue;
            
            var interactionSaveData = new InteractionSaveData();

            interactionSaveData.Id = interactionSave.Id;

            interactionSaveData.taskSaveId = interactionSave.taskSaveId;
            interactionSaveData.interactionId = interactionSave.interactionId;

            interactionSaveData.complete = interactionSave.complete;

            interactionSaveDataList.Add(interactionSaveData);
        }
    }

    internal void GetInteractionData()
    {
        var interactionSearchParameters = new Search.Interaction();
        interactionSearchParameters.id = interactionSaveDataList.Select(x => x.interactionId).Distinct().ToList();

        interactionDataList = dataManager.GetInteractionData(interactionSearchParameters);
    }

    internal class InteractionSaveData : GeneralData
    {
        public int taskSaveId;
        public int interactionId;

        public bool complete;
    }
}
