using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class TerrainTileController : MonoBehaviour, IDataController
{
    public IDisplay display { get { return GetComponent<IDisplay>(); } }

    public SegmentController segmentController { get { return GetComponent<SegmentController>(); } }
    public Enums.DataType data_type { get { return Enums.DataType.TerrainTile; } }

    public ICollection dataList { get; set; }

    public bool search_by_id;
    public int temp_id_count;

    TerrainTileManager terrainTileManager = new TerrainTileManager();

    public void InitializeController()
    {
        GetData(new List<int>());
    }

    public void GetData(List<int> id_list)
    {
        dataList = terrainTileManager.GetTerrainTileDataElements(this);

        var terrainTileDataElements = dataList.Cast<TerrainTileDataElement>();

        //terrainTileDataElements.Where(x => x.changed).ToList().ForEach(x => x.Update());
        //terrainTileDataElements[0].Update();
    }
}