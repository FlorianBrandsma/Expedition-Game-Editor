using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class TileController : MonoBehaviour, IDataController
{
    public bool searchById;
    public int temp_id_count;

    private TileManager tileManager             = new TileManager();

    public IDisplay Display                     { get { return GetComponent<IDisplay>(); } }

    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }
    public Enums.DataType DataType              { get { return Enums.DataType.Tile; } }
    public ICollection DataList                 { get; set; }

    public void InitializeController()
    {
        GetData(new List<int>());
    }

    public void GetData(List<int> id_list)
    {
        DataList = tileManager.GetTileDataElements(this);

        var tileDataElements = DataList.Cast<TileDataElement>();

        //tileDataElements.Where(x => x.changed).ToList().ForEach(x => x.Update());
        //tileDataElements[0].Update();
    }

    public void GetData(SearchData searchData)
    {

    }
}