using UnityEngine;
using System.Collections;
using System.Linq;

public class ChapterController : MonoBehaviour, IDataController
{
    public IDisplay display { get { return GetComponent<IDisplay>(); } }

    public SegmentController segmentController  { get { return GetComponent<SegmentController>(); } }
    public DataManager.Type data_type           { get { return DataManager.Type.Chapter; } }

    public ICollection data_list { get; set; }

    public bool search_by_id;
    public int temp_id_count;

    ChapterManager chapterManager = new ChapterManager();

    public void InitializeController()
    {
        GetData();
    }

    public void GetData()
    {
        data_list = chapterManager.GetChapterDataElements(this);

        var chapterDataElements = data_list.Cast<ChapterDataElement>();

        //chapterDataElements.Where(x => x.changed).ToList().ForEach(x => x.Update());
        //chapterDataElements[0].Update();
    }
}
