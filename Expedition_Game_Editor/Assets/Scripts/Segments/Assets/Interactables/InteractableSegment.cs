﻿using UnityEngine;
using System.Collections.Generic;

public class InteractableSegment : MonoBehaviour, ISegment
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
        DataEditor = SegmentController.editorController.PathController.dataEditor;
    }

    public void InitializeSegment()
    {
        InitializeData();
    }

    public void InitializeData()
    {
        if (SegmentController.editorController.PathController.loaded) return;

        if (!SegmentController.loaded && !SegmentController.editorController.PathController.loaded)
        {
            var searchParameters = new Search.Interactable();

            SegmentController.DataController.DataList = SegmentController.DataController.GetData(new[] { searchParameters });
        }
    }

    public void OpenSegment()
    {
        if (GetComponent<IDisplay>() != null)
            GetComponent<IDisplay>().DataController = SegmentController.DataController;
    }

    public void SetSearchResult(SelectionElement selectionElement) { }
}
