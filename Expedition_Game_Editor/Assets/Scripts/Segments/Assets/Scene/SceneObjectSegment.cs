﻿using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class SceneObjectSegment : MonoBehaviour, ISegment
{
    private SegmentController SegmentController { get { return GetComponent<SegmentController>(); } }

    public IEditor DataEditor { get; set; }
    
    public void InitializeDependencies() { }

    public void InitializeSegment()
    {
        InitializeData();
    }

    public void InitializeData()
    {
        if (SegmentController.Loaded) return;

        var searchParameters = new Search.SceneObject();

        searchParameters.requestType = Search.SceneObject.RequestType.Custom;

        searchParameters.regionId = new List<int>() { EditorManager.editorManager.forms.First().activePath.FindLastRoute(Enums.DataType.Region).GeneralData.Id };

        SegmentController.DataController.DataList = SegmentController.DataController.GetData(new[] { searchParameters });
    }

    public void OpenSegment()
    {
        if (GetComponent<IDisplay>() != null)
            GetComponent<IDisplay>().DataController = SegmentController.DataController;
    }

    public void CloseSegment() { }

    public void SetSearchResult(SelectionElement selectionElement) { }
}