﻿using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class PhaseSaveRegionsRegionSegment : MonoBehaviour, ISegment
{
    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }
    public IEditor DataEditor                   { get; set; }

    private PhaseSaveEditor PhaseSaveEditor     { get { return (PhaseSaveEditor)DataEditor; } }

    public void InitializeDependencies()
    {
        DataEditor = SegmentController.EditorController.PathController.DataEditor;

        if (!DataEditor.EditorSegments.Contains(SegmentController))
            DataEditor.EditorSegments.Add(SegmentController);
    }

    public void InitializeData()
    {
        if (DataEditor.Loaded) return;

        var searchProperties = new SearchProperties(Enums.DataType.Region);

        var searchParameters = searchProperties.searchParameters.Cast<Search.Region>().First();

        searchParameters.phaseId = new List<int>() { PhaseSaveEditor.PhaseId };
        searchParameters.type = Enums.RegionType.Game;

        SegmentController.DataController.GetData(searchProperties);
    }

    public void InitializeSegment() { }
    
    public void OpenSegment()
    {
        if (GetComponent<IDisplay>() != null)
            GetComponent<IDisplay>().DataController = SegmentController.DataController;
    }

    public void SetSearchResult(IElementData mergedElementData, IElementData resultElementData) { }

    public void UpdateSegment() { }

    public void CloseSegment() { }
}