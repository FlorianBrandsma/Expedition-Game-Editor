using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class TerrainTileController : MonoBehaviour, IDataController
{
    public bool search_by_id;
    public int temp_id_count;

    private TerrainTileManager terrainTileManager = new TerrainTileManager();

    public IDisplay Display                     { get { return GetComponent<IDisplay>(); } }

    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }
    public Enums.DataType DataType              { get { return Enums.DataType.TerrainTile; } }
    public ICollection DataList                 { get; set; }

    public void InitializeController()
    {
        GetData(new List<int>());
    }

    public void GetData(List<int> id_list)
    {
        DataList = terrainTileManager.GetTerrainTileDataElements(this);

        var terrainTileDataElements = DataList.Cast<TerrainTileDataElement>();

        //terrainTileDataElements.Where(x => x.changed).ToList().ForEach(x => x.Update());
        //terrainTileDataElements[0].Update();
    }

    public void GetData(SearchData searchData)
    {

    }
}