using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ChapterController : MonoBehaviour, IDataController
{
    public IDisplay display { get { return GetComponent<IDisplay>(); } }

    public SegmentController segmentController  { get { return GetComponent<SegmentController>(); } }
    public Enums.DataType data_type             { get { return Enums.DataType.Chapter; } }

    public ICollection dataList { get; set; }

    public bool search_by_id;
    public int temp_id_count;

    ChapterManager chapterManager = new ChapterManager();

    public void InitializeController()
    {
        GetData(new List<int>());
    }

    public void GetData(List<int> id_list)
    {
        dataList = chapterManager.GetChapterDataElements(this);

        var chapterDataElements = dataList.Cast<ChapterDataElement>();

        //chapterDataElements.Where(x => x.changed).ToList().ForEach(x => x.Update());
        //chapterDataElements[0].Update();
    }
}
