﻿using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class RegionDimensionsRegionSegment : MonoBehaviour, ISegment
{
    private RegionDataElement RegionDataElement { get { return (RegionDataElement)DataEditor.Data.dataElement; } }

    #region UI
    public ExInputNumber sizeInputNumber;
    public Text heightText;
    #endregion

    public SegmentController SegmentController { get { return GetComponent<SegmentController>(); } }

    public IEditor DataEditor { get; set; }

    public void UpdateSize()
    {
        var regionDataList = DataEditor.DataList.Cast<RegionDataElement>().ToList();
        regionDataList.ForEach(regionData =>
        {
            regionData.RegionSize = (int)sizeInputNumber.Value;
            heightText.text = regionData.RegionSize.ToString();
        });

        DataEditor.UpdateEditor();
    }
    
    public void InitializeDependencies()
    {
        DataEditor = SegmentController.EditorController.PathController.DataEditor;

        if (!DataEditor.EditorSegments.Contains(SegmentController))
            DataEditor.EditorSegments.Add(SegmentController);
    }

    public void InitializeSegment() { }

    public void InitializeData() { }

    private void SetSearchParameters() { }

    public void OpenSegment()
    {
        SegmentController.EnableSegment(false);

        sizeInputNumber.Value = RegionDataElement.RegionSize;
        heightText.text = RegionDataElement.RegionSize.ToString();
    }

    public void CloseSegment() { }

    public void SetSearchResult(DataElement selectionElement) { }
}
