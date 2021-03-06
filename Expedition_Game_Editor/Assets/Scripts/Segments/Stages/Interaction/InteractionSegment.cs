﻿using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class InteractionSegment : MonoBehaviour, ISegment
{
    public ListProperties ListProperties        { get { return GetComponent<ListProperties>(); } }

    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }
    public IEditor DataEditor                   { get; set; }
    
    public void InitializeDependencies() { }

    public void InitializeData()
    {
        if (SegmentController.Loaded) return;

        var searchProperties = new SearchProperties(Enums.DataType.Interaction);

        InitializeSearchParameters(searchProperties);

        SegmentController.DataController.GetData(searchProperties);
    }

    private void InitializeSearchParameters(SearchProperties searchProperties)
    {
        var searchParameters = searchProperties.searchParameters.Cast<Search.Interaction>().First();

        searchParameters.includeAddElement = ListProperties.AddProperty != SelectionManager.Property.None;

        searchParameters.taskId = new List<int>() { SegmentController.Path.FindLastRoute(Enums.DataType.Task).ElementData.Id };
    }

    public void InitializeSegment()
    {
        InitializeData();

        CheckTimeSlots();
    }

    private void CheckTimeSlots()
    {
        if (SegmentController.Loaded) return;

        //Only allow adding new rows if there are time slots available
        //ListProperties.enableAdding = TimeManager.TimeFramesAvailable(SegmentController.DataController);
    }
    
    public void OpenSegment()
    {
        if (GetComponent<IDisplay>() != null)
            GetComponent<IDisplay>().DataController = SegmentController.DataController;
    }

    public void SetSearchResult(IElementData mergedElementData, IElementData resultElementData) { }

    public void UpdateSegment() { }

    public void CloseSegment() { }
}
