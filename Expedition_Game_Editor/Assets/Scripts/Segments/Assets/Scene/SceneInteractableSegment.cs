﻿using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class SceneInteractableSegment : MonoBehaviour, ISegment
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

        var searchParameters = new Search.SceneInteractable();

        searchParameters.requestType = Search.SceneInteractable.RequestType.GetInteractablesFromInteractionRegion;

        searchParameters.regionId = new List<int>() { EditorManager.editorManager.forms.First().activePath.FindLastRoute(Enums.DataType.Region).GeneralData.Id };
        searchParameters.objectiveId = new List<int>() { 0 };

        SegmentController.DataController.DataList = EditorManager.GetData(SegmentController.DataController, new[] { searchParameters });
    }

    public void OpenSegment()
    {
        if (GetComponent<IDisplay>() != null)
            GetComponent<IDisplay>().DataController = SegmentController.DataController;
    }

    public void CloseSegment() { }

    public void SetSearchResult(SelectionElement selectionElement) { }
}
