using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ChapterRegionController : MonoBehaviour, IDataController
{
    public SearchProperties searchProperties;

    public IDataManager DataManager             { get; set; }
    
    public IDisplay Display                     { get { return GetComponent<IDisplay>(); } }
    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }

    public Enums.DataType DataType              { get { return Enums.DataType.ChapterRegion; } }
    public Enums.DataCategory DataCategory      { get { return Enums.DataCategory.None; } }
    public List<IDataElement> DataList          { get; set; }

    public SearchProperties SearchProperties
    {
        get { return searchProperties; }
        set { searchProperties = value; }
    }

    public ChapterRegionController()
    {
        DataManager = new ChapterRegionDataManager(this);
    }

    public void InitializeController()
    {
        SearchProperties.Initialize();
    }

    public void SetData(DataElement searchElement, IDataElement resultData)
    {
        var chapterRegionData = (ChapterRegionDataElement)searchElement.data.dataElement;

        switch (((GeneralData)resultData).DataType)
        {
            case Enums.DataType.Region:

                var resultElementData = (RegionDataElement)resultData;

                chapterRegionData.RegionId = resultElementData.Id;
                chapterRegionData.name = resultElementData.Name;
                chapterRegionData.tileIconPath = resultElementData.tileIconPath;

                break;
        }
    }

    public void ToggleElement(EditorElement editorElement) { }
}