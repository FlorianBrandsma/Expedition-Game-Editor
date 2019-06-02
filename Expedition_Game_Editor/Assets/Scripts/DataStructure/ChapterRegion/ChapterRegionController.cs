using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ChapterRegionController : MonoBehaviour, IDataController
{
    public Search.ChapterRegion searchParameters;

    private ChapterRegionDataManager chapterRegionDataManager = new ChapterRegionDataManager();

    public IDisplay Display { get { return GetComponent<IDisplay>(); } }
    public SegmentController SegmentController { get { return GetComponent<SegmentController>(); } }

    public Enums.DataType DataType { get { return Enums.DataType.ChapterRegion; } }
    public List<IDataElement> DataList { get; set; }

    public IEnumerable SearchParameters
    {
        get { return new[] { searchParameters }; }
        set { searchParameters = value.Cast<Search.ChapterRegion>().FirstOrDefault(); }
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

    }

    public void ToggleElement(IDataElement dataElement)
    {

    }
}