﻿using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class QuestSegment : MonoBehaviour, ISegment
{
    public SegmentController SegmentController { get { return GetComponent<SegmentController>(); } }

    public IEditor DataEditor { get; set; }
    
    public void InitializeDependencies() { }

    public void InitializeSegment()
    {
        InitializeData();
    }

    public void InitializeData()
    {
        if (SegmentController.Loaded) return;

        var searchProperties = new SearchProperties(Enums.DataType.Quest);

        var searchParameters = searchProperties.searchParameters.Cast<Search.Quest>().First();
        searchParameters.phaseId = new List<int>() { SegmentController.Path.FindLastRoute(Enums.DataType.Phase).GeneralData.Id };

        SegmentController.DataController.DataList = EditorManager.GetData(SegmentController.DataController, searchProperties);
    }

    public void OpenSegment()
    {
        if (GetComponent<IDisplay>() != null)
            GetComponent<IDisplay>().DataController = SegmentController.DataController;
    }

    public void CloseSegment() { }

    public void SetSearchResult(SelectionElement selectionElement) { }
}