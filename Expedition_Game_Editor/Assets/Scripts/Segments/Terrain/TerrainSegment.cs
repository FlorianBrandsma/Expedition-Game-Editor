using UnityEngine;
using System.Collections.Generic;

public class TerrainSegment : MonoBehaviour, ISegment
{
    private RegionDataElement regionData;

    private SegmentController SegmentController { get { return GetComponent<SegmentController>(); } }
    public IEditor DataEditor { get; set; }

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

        regionData = (RegionDataElement)SegmentController.Path.FindLastRoute(Enums.DataType.Region).data.dataElement;

        var searchParameters = new Search.Terrain();

        searchParameters.regionId = new List<int>() { regionData.id };

        SegmentController.DataController.DataList = SegmentController.DataController.GetData(new[] { searchParameters });
    }

    public void OpenSegment()
    {
        if (GetComponent<IDisplay>() != null)
            GetComponent<IDisplay>().DataController = SegmentController.DataController;
    }

    public void ApplySegment() { }
    public void CloseSegment() { }

    public void SetSearchResult(SelectionElement selectionElement) { }
}
