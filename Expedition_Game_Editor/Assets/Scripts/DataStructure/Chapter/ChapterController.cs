using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ChapterController : MonoBehaviour, IDataController
{
    public Search.Chapter searchParameters;

    private ChapterManager chapterManager       = new ChapterManager();

    public IDisplay Display                     { get { return GetComponent<IDisplay>(); } }
    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }

    public Enums.DataType DataType              { get { return Enums.DataType.Chapter; } }
    public ICollection DataList                 { get; set; }

    public IEnumerable SearchParameters
    {
        get { return new[] { searchParameters }; }
        set { searchParameters = value.Cast<Search.Chapter>().FirstOrDefault(); }
    }

    public void InitializeController()
    {
        chapterManager.InitializeManager(this);
    }

    public void GetData(IEnumerable searchParameters)
    {
        DataList = chapterManager.GetChapterDataElements(searchParameters);

        var chapterDataElements = DataList.Cast<ChapterDataElement>();

        //chapterDataElements.Where(x => x.changed).ToList().ForEach(x => x.Update());
        //chapterDataElements[0].Update();
    }

    public void SetData(SelectionElement searchElement, Data resultData)
    {

    }
}
