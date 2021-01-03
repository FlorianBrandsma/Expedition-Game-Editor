using UnityEngine;
using System.Linq;

public class RegionDataController : MonoBehaviour, IDataController
{
    public SearchProperties searchProperties;

    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }
    
    public Data Data                            { get; set; }
    public Enums.DataType DataType              { get { return Enums.DataType.Region; } }
    public Enums.DataCategory DataCategory      { get { return Enums.DataCategory.Navigation; } }
    
    public SearchProperties SearchProperties
    {
        get { return searchProperties; }
        set { searchProperties = value; }
    }
    
    public void InitializeController()
    {
        SearchProperties.Initialize();
    }

    public void GetData()
    {
        GetData(searchProperties);
    }

    public void GetData(SearchProperties searchProperties)
    {
        Data = new Data()
        {
            dataController = this,
            dataList = RegionDataManager.GetData(searchProperties.searchParameters.Cast<Search.Region>().First()),
            searchProperties = this.searchProperties
        };

        DataManager.ReplaceRouteData(this);
    }

    public void SetData(IElementData searchElementData, IElementData resultElementData)
    {
        var searchRegionElementData = (RegionElementData)searchElementData;

        searchRegionElementData.DataElement.Id = resultElementData.Id;

        switch (resultElementData.DataType)
        {
            case Enums.DataType.Region:

                var resultRegionElementData = (RegionElementData)resultElementData;

                searchRegionElementData.Id              = resultRegionElementData.Id;

                searchRegionElementData.Name            = resultRegionElementData.Name;

                searchRegionElementData.TileIconPath    = resultRegionElementData.TileIconPath;

                searchRegionElementData.RegionSize      = resultRegionElementData.RegionSize;
                searchRegionElementData.TerrainSize     = resultRegionElementData.TerrainSize;
                searchRegionElementData.TileSize        = resultRegionElementData.TileSize;

                break;
        }
    }

    public void ToggleElement(EditorElement editorElement) { }
}