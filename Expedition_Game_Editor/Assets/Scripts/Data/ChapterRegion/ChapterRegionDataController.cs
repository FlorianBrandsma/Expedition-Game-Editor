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
            dataList = ChapterRegionDataManager.GetData(searchProperties)
        };

        DataManager.ReplaceRouteData(this);
    }

    public void SetData(DataElement searchElement, IElementData resultData)
    {
        var chapterRegionData = (ChapterRegionElementData)searchElement.ElementData;

        switch (resultData.DataType)
        {
            case Enums.DataType.Region:

                var resultElementData = (RegionElementData)resultData;

                chapterRegionData.RegionId = resultElementData.Id;
                chapterRegionData.Name = resultElementData.Name;
                chapterRegionData.TileIconPath = resultElementData.TileIconPath;

                break;
        }
    }

    public void ToggleElement(EditorElement editorElement) { }
}