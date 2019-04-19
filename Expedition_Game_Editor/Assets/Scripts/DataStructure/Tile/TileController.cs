using UnityEngine;
using System.Collections;
using System.Linq;

public class TileController : MonoBehaviour, IDataController
{
    public SegmentController segmentController { get { return GetComponent<SegmentController>(); } }
    public Enums.DataType data_type { get { return Enums.DataType.Tile; } }

    public ICollection data_list { get; set; }

    public bool search_by_id;
    public int temp_id_count;

    TileManager tileManager = new TileManager();

    public void InitializeController()
    {
        GetData();
    }

    public void GetData()
    {
        data_list = tileManager.GetTileDataElements(this);

        var tileDataElements = data_list.Cast<TileDataElement>();

        //TileDataElements.Where(x => x.changed).ToList().ForEach(x => x.Update());
        //TileDataElements[0].Update();
    }
}
