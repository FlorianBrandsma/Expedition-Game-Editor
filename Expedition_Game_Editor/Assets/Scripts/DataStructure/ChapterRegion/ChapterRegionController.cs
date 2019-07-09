using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ChapterRegionController : MonoBehaviour, IDataController
{
    public Search.Region searchParameters;

    private ChapterRegionDataManager chapterRegionDataManager = new ChapterRegionDataManager();

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

    public void InitializeController()
    {
        chapterRegionDataManager.InitializeManager(this);
    }

    public void GetData(IEnumerable searchParameters)
    {
        DataList = chapterRegionDataManager.GetChapterRegionDataElements(searchParameters);
    }

    public void SetData(SelectionElement searchElement, Data resultData)
    {
        var searchElementData = (ChapterRegionDataElement)searchElement.route.data.DataElement;

        var chapterRegionDataElement = DataList.Cast<ChapterRegionDataElement>().Where(x => x.id == searchElementData.id).FirstOrDefault();

        switch (resultData.DataController.DataType)
        {
            case Enums.DataType.Region:

                var resultElementData = (RegionDataElement)resultData.DataElement;

                chapterRegionDataElement.RegionId = resultElementData.id;
                chapterRegionDataElement.name = resultElementData.Name;

                break;
        }

        searchElement.route.data.DataElement = chapterRegionDataElement;
    }

    public void ToggleElement(IDataElement dataElement)
    {

    }
}