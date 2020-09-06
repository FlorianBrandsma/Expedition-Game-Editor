using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class RegionDimensionsRegionSegment : MonoBehaviour, ISegment
{
    private RegionElementData RegionElementData { get { return (RegionElementData)DataEditor.ElementData; } }

    #region UI
    public ExInputNumber sizeInputNumber;
    public Text heightText;
    #endregion

    public SegmentController SegmentController { get { return GetComponent<SegmentController>(); } }

    public IEditor DataEditor { get; set; }

    public void UpdateSize()
    {
        var regionDataList = DataEditor.DataList.Cast<RegionElementData>().ToList();
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

    public void OpenSegment()
    {
        SegmentController.EnableSegment(false);

        sizeInputNumber.Value = RegionElementData.RegionSize;
        heightText.text = RegionElementData.RegionSize.ToString();
    }

    public void CloseSegment() { }

    public void SetSearchResult(DataElement dataElement) { }
}
