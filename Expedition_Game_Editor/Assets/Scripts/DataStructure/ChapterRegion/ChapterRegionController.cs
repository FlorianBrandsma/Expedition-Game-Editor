using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ChapterRegionController : MonoBehaviour, IDataController
{
    public Search.Region searchParameters;

    private ChapterRegionDataManager chapterRegionDataManager;

    public IDisplay Display                     { get { return GetComponent<IDisplay>(); } }
    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }

    public Enums.DataType DataType              { get { return Enums.DataType.ChapterRegion; } }
    public Enums.DataCategory DataCategory      { get { return Enums.DataCategory.None; } }
    public List<IDataElement> DataList          { get; set; }

    public IEnumerable SearchParameters
    {
        get { return new[] { searchParameters }; }
        set { searchParameters = value.Cast<Search.Region>().FirstOrDefault(); }
    }

    public ChapterRegionController()
    {
        chapterRegionDataManager = new ChapterRegionDataManager(this);
    }

    public List<IDataElement> GetData(IEnumerable searchParameters)
    {
        return chapterRegionDataManager.GetChapterRegionDataElements(searchParameters);
    }

    public void SetData(SelectionElement searchElement, IDataElement resultData)
    {
        var chapterRegionData = (ChapterRegionDataElement)searchElement.data.dataElement;

        switch (((GeneralData)resultData).DataType)
        {
            case Enums.DataType.Region:

                var resultElementData = (RegionDataElement)resultData;

                chapterRegionData.RegionId = resultElementData.Id;
                chapterRegionData.name = resultElementData.Name;

                break;
        }
    }

    public void ToggleElement(IDataElement dataElement) { }
}