using UnityEngine;
using UnityEngine.UI;

public class RegionDimensionsRegionSegment : MonoBehaviour, ISegment
{
    public ExInputNumber sizeInputNumber;
    public Text heightText;
    
    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }
    public IEditor DataEditor                   { get; set; }

    public RegionEditor RegionEditor            { get { return (RegionEditor)DataEditor; } }

    public int RegionSize
    {
        get { return RegionEditor.RegionSize; }
        set { RegionEditor.RegionSize = value; }
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
        SegmentController.EnableSegment(false);

        sizeInputNumber.Value = RegionSize;

        SetSizeValues();
    }

    public void SetSizeValues()
    {
        heightText.text = RegionSize.ToString();
    }

    public void UpdateSize()
    {
        RegionSize = (int)sizeInputNumber.Value;

        SetSizeValues();

        DataEditor.UpdateEditor();
    }

    public void SetSearchResult(IElementData mergedElementData, IElementData resultElementData) { }

    public void UpdateSegment() { }

    public void CloseSegment() { }
}
