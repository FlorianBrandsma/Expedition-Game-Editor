﻿using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class TaskSegment : MonoBehaviour, ISegment
{
    private SegmentController SegmentController { get { return GetComponent<SegmentController>(); } }
    public IEditor DataEditor { get; set; }

    public void ApplySegment()
    {

    }

    public void CloseSegment()
    {

    }

    public void InitializeSegment()
    {
        DataEditor = SegmentController.editorController.pathController.dataEditor;

        InitializeTaskData();
    }

    private void InitializeTaskData()
    {
        if (SegmentController.editorController.pathController.loaded) return;

        var searchParameters = new Search.Task();

        //If a terrainElement is selected without being directly related to an objective, don't try to get this data
        var objectiveData = SegmentController.path.FindLastRoute("Objective").data.ElementData.Cast<ObjectiveDataElement>().FirstOrDefault();
        searchParameters.objectiveId = new List<int>() { objectiveData.id };

        var terrainElementData = SegmentController.path.FindLastRoute("TerrainElement").data.ElementData.Cast<TerrainElementDataElement>().FirstOrDefault();
        searchParameters.terrainElementId = new List<int>() { terrainElementData.id };

        SegmentController.DataController.GetData(new[] { searchParameters });
    }

    public void OpenSegment()
    {
        if (GetComponent<IDisplay>() != null)
            GetComponent<IDisplay>().DataController = SegmentController.DataController;
    }

    public void SetSearchResult(SelectionElement selectionElement) { }
}
