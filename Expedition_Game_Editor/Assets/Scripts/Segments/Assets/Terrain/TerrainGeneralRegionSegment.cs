using UnityEngine;
using UnityEngine.UI;

public class TerrainGeneralRegionSegment : MonoBehaviour, ISegment
{
    public DataElement editButton;
    public Text buttonText;

    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }
    public IEditor DataEditor                   { get; set; }
    
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

    private void InitializeEditButton()
    {
        //Take the data from the last selected region
        var regionRoute = SegmentController.Path.FindLastRoute(Enums.DataType.Region);

        editButton.Id = regionRoute.id;
        editButton.Data = regionRoute.data;

        editButton.Path = SegmentController.Path;

        editButton.InitializeElement();

        var regionData = (RegionElementData)regionRoute.ElementData;
        buttonText.text = "Tiles - " + regionData.Name;
    }

    public void InitializeData() { }

    public void OpenSegment() { }

    public void SetSearchResult(IElementData mergedElementData, IElementData resultElementData) { }

    public void UpdateSegment() { }

    public void CloseSegment() { }
}
