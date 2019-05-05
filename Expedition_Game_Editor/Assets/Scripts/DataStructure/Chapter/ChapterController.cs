using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ChapterController : MonoBehaviour, IDataController
{
    public int temp_id_count;

    public SearchParameters searchParameters;

    private ChapterManager chapterManager       = new ChapterManager();

    public IDisplay Display                     { get { return GetComponent<IDisplay>(); } }
    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }

    public Enums.DataType DataType              { get { return Enums.DataType.Chapter; } }
    public ICollection DataList                 { get; set; }

    public SearchParameters SearchParameters
    {
        get { return searchParameters; }
        set { searchParameters = value; }
    }

    public void InitializeController()
    {
        GetData(new List<int>());
    }

    public void GetData(List<int> idList)
    {
        DataList = chapterManager.GetChapterDataElements(this);

        var chapterDataElements = DataList.Cast<ChapterDataElement>();

        //chapterDataElements.Where(x => x.changed).ToList().ForEach(x => x.Update());
        //chapterDataElements[0].Update();
    }

    public void GetData(SearchData searchData)
    {

    }

    public void ReplaceData(IEnumerable dataElement)
    {

    }
}
