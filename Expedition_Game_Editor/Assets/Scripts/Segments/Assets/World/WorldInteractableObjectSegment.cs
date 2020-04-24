﻿using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class WorldInteractableObjectSegment : MonoBehaviour, ISegment
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

        var searchProperties = new SearchProperties(Enums.DataType.WorldInteractable);

        var searchParameters = searchProperties.searchParameters.Cast<Search.WorldInteractable>().First();

        searchParameters.requestType = Search.WorldInteractable.RequestType.GetRegionWorldInteractables;

        searchParameters.type = new List<int>() { (int)Enums.InteractableType.Objects };

        searchParameters.regionId = new List<int>() { EditorManager.editorManager.forms.First().activePath.FindLastRoute(Enums.DataType.Region).GeneralData.Id };
        searchParameters.objectiveId = new List<int>() { 0 };
        
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