﻿using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Linq;

public class TerrainGeneralRegionSegment : MonoBehaviour, ISegment
{
    public SegmentController SegmentController { get { return GetComponent<SegmentController>(); } }

    public IEditor DataEditor { get; set; }

    #region UI
    public DataElement editButton;
    #endregion

    #region Data Variables
    #endregion

    #region Methods
    private void InitializeEditButton()
    {
        editButton.Path = SegmentController.Path;

        var regionRoute = SegmentController.Path.FindLastRoute(Enums.DataType.Region);
#warning Fix this
        //editButton.data = new Data(regionRoute.ElementData);

        var regionData = (RegionElementData)regionRoute.ElementData;
        editButton.GetComponentInChildren<Text>().text = "Tiles, " + regionData.Name;
    }
    #endregion

    #region Segment
    public void InitializeDependencies()
    {
        DataEditor = SegmentController.EditorController.PathController.DataEditor;

        if (!DataEditor.EditorSegments.Contains(SegmentController))
            DataEditor.EditorSegments.Add(SegmentController);
    }

    public void InitializeSegment()
    {
        InitializeEditButton();
    }

    public void InitializeData() { }

    public void OpenSegment() { }

    public void CloseSegment() { }

    public void SetSearchResult(DataElement dataElement) { }
    #endregion
}
