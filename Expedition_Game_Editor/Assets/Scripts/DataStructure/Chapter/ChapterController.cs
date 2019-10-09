using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ChapterController : MonoBehaviour, IDataController
{
    public Search.Chapter searchParameters;

    private ChapterDataManager chapterDataManager;

    public IDisplay Display                     { get { return GetComponent<IDisplay>(); } }
    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }

    public Enums.DataType DataType              { get { return Enums.DataType.Chapter; } }
    public Enums.DataCategory DataCategory      { get { return Enums.DataCategory.Navigation; } }
    public List<IDataElement> DataList          { get; set; }

    public IEnumerable SearchParameters
    {
        get { return new[] { searchParameters }; }
        set { searchParameters = value.Cast<Search.Chapter>().FirstOrDefault(); }
    }

    public ChapterController()
    {
        chapterDataManager = new ChapterDataManager(this);
    }

    public List<IDataElement> GetData(IEnumerable searchParameters)
    {
        return chapterDataManager.GetChapterDataElements(searchParameters);
    }

    public void SetData(SelectionElement searchElement, IDataElement resultDataElement) { }

    public void ToggleElement(IDataElement dataElement) { }
}
