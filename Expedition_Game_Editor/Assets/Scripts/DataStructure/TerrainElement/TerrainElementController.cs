using UnityEngine;
using System.Collections;
using System.Linq;

public class TerrainElementController : MonoBehaviour, IDataController
{
    public IDisplay display { get { return GetComponent<IDisplay>(); } }

    public SegmentController segmentController { get { return GetComponent<SegmentController>(); } }
    public Enums.DataType data_type { get { return Enums.DataType.TerrainElement; } }

    public ICollection data_list { get; set; }

    public bool search_by_id;
    public int temp_id_count;

    TerrainElementManager chapterManager = new TerrainElementManager();

    public void InitializeController()
    {
        GetData();
    }

    public void GetData()
    {
        data_list = chapterManager.GetTerrainElementDataElements(this);

        var chapterDataElements = data_list.Cast<TerrainElementDataElement>();

        //chapterDataElements.Where(x => x.changed).ToList().ForEach(x => x.Update());
        //chapterDataElements[0].Update();
    }
}
