﻿using UnityEngine;
using UnityEngine.UI;

public class RegionDimensionsTerrainSegment : MonoBehaviour, ISegment
{
    public ExInputNumber sizeInputNumber;
    public Text heightText;

    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }
    public IEditor DataEditor                   { get; set; }

    public RegionEditor RegionEditor            { get { return (RegionEditor)DataEditor; } }

    public int TerrainSize
    {
        get { return RegionEditor.TerrainSize; }
        set { RegionEditor.TerrainSize = value; }
    }
    
    public void InitializeDependencies()
    {
        DataEditor = SegmentController.EditorController.PathController.DataEditor;

        if (!DataEditor.EditorSegments.Contains(SegmentController))
            DataEditor.EditorSegments.Add(SegmentController);
    }

    public void InitializeSegment() { }

    public void InitializeData() { }

    public void OpenSegment()
    {
        SegmentController.EnableSegment(DataEditor.EditData.ExecuteType == Enums.ExecuteType.Add);

        sizeInputNumber.Value = TerrainSize;

        SetSizeValues();
    }

    public void SetSizeValues()
    {
        heightText.text = TerrainSize.ToString();
    }

    public void UpdateSize()
    {
        TerrainSize = (int)sizeInputNumber.Value;

        SetSizeValues();

        DataEditor.UpdateEditor();
    }

    public void SetSearchResult(IElementData mergedElementData, IElementData resultElementData) { }

    public void UpdateSegment() { }

    public void CloseSegment() { }
}
