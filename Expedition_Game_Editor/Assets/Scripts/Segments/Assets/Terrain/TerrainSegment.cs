using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class TerrainSegment : MonoBehaviour, ISegment
{
    private RegionDataElement regionData;

    public SegmentController SegmentController { get { return GetComponent<SegmentController>(); } }

    public IEditor DataEditor { get; set; }
    
    public void InitializeDependencies() { }

    public void InitializeSegment()
    {
        InitializeData();
    }

    public void InitializeData()
    {
        if (SegmentController.Loaded) return;

        regionData = (RegionDataElement)SegmentController.Path.FindLastRoute(Enums.DataType.Region).data.dataElement;

        var searchProperties = new SearchProperties(Enums.DataType.Terrain);

        var searchParameters = searchProperties.searchParameters.Cast<Search.Terrain>().First();
        searchParameters.regionId = new List<int>() { regionData.Id };

        SegmentController.DataController.DataList = RenderManager.GetData(SegmentController.DataController, searchProperties);
    }

    public void OpenSegment()
    {
        if (GetComponent<IDisplay>() != null)
            GetComponent<IDisplay>().DataController = SegmentController.DataController;
    }

    public void CloseSegment() { }

    public void SetSearchResult(DataElement selectionElement) { }
}
