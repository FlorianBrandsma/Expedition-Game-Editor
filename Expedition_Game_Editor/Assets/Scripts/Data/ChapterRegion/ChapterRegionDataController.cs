using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ChapterRegionDataController : MonoBehaviour, IDataController
{
    public SearchProperties searchProperties;

    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }

    public Data Data                            { get; set; }
    public Enums.DataType DataType              { get { return Enums.DataType.ChapterRegion; } }
    public Enums.DataCategory DataCategory      { get { return Enums.DataCategory.None; } }
    
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
            dataList = ChapterRegionDataManager.GetData(searchProperties),
            searchProperties = this.searchProperties
        };

        DataManager.ReplaceRouteData(this);
    }

    public void SetData(IElementData searchElementData, IElementData resultElementData)
    {
        var searchChapterRegionElementData = (ChapterRegionElementData)searchElementData;

        switch (resultElementData.DataType)
        {
            case Enums.DataType.Region:

                var resultRegionElementData = (RegionElementData)resultElementData;

                searchChapterRegionElementData.RegionId     = resultRegionElementData.Id;
                searchChapterRegionElementData.Name         = resultRegionElementData.Name;
                searchChapterRegionElementData.TileIconPath = resultRegionElementData.TileIconPath;

                break;
        }
    }

    public void ToggleElement(EditorElement editorElement) { }
}