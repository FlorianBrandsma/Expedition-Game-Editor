﻿using UnityEngine;
using System.Collections.Generic;

public class RegionSegment : MonoBehaviour, ISegment
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

        InitializeData();
    }

    private void InitializeData()
    {
        if (SegmentController.editorController.pathController.loaded) return;

        if (!SegmentController.loaded && !SegmentController.editorController.pathController.loaded)
        {
            var searchParameters = new Search.Region();

            SegmentController.DataController.GetData(new[] { searchParameters });
        }
    }

    public void OpenSegment()
    {
        if (GetComponent<IDisplay>() != null)
            GetComponent<IDisplay>().DataController = SegmentController.DataController;
    }

    public void SetSearchResult(SelectionElement selectionElement)
    {

    }
}
