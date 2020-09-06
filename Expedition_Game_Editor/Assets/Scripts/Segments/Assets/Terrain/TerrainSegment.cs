using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class TerrainSegment : MonoBehaviour, ISegment
{
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

        var regionData = (RegionElementData)SegmentController.Path.FindLastRoute(Enums.DataType.Region).ElementData;

        var searchProperties = new SearchProperties(Enums.DataType.Terrain);

        var searchParameters = searchProperties.searchParameters.Cast<Search.Terrain>().First();
        searchParameters.regionId = new List<int>() { regionData.Id };

        SegmentController.DataController.GetData(searchProperties);
    }

    public void OpenSegment()
    {
        if (GetComponent<IDisplay>() != null)
            GetComponent<IDisplay>().DataController = SegmentController.DataController;
    }

    public void CloseSegment() { }

    public void SetSearchResult(DataElement dataElement) { }
}
