﻿using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class TerrainInteractableSegment : MonoBehaviour, ISegment
{
    private SegmentController SegmentController { get { return GetComponent<SegmentController>(); } }
    public IEditor DataEditor { get; set; }

    public void ApplySegment()
    {

    }

    public void CloseSegment()
    {

    }

    public void InitializeDependencies()
    {
        //DataEditor = SegmentController.editorController.pathController.dataEditor;
    }

    public void InitializeSegment()
    {
        InitializeData();
    }

    public void InitializeData()
    {
        if (SegmentController.editorController.pathController.loaded) return;

        var searchParameters = new Search.TerrainInteractable();

        searchParameters.requestType = Search.TerrainInteractable.RequestType.GetInteractablesFromInteractionRegion;

        searchParameters.regionId = new List<int>() { EditorManager.editorManager.forms.First().activePath.FindLastRoute(Enums.DataType.Region).GeneralData().id };
        searchParameters.objectiveId = new List<int>() { 0 };

        SegmentController.DataController.GetData(new[] { searchParameters });
    }

    public void OpenSegment()
    {
        if (GetComponent<IDisplay>() != null)
            GetComponent<IDisplay>().DataController = SegmentController.DataController;
    }

    public void SetSearchResult(SelectionElement selectionElement) { }
}