using UnityEngine;
using System.Collections;
using System.Linq;

public class TerrainTileController : MonoBehaviour, IDataController
{
    public IDisplay display { get { return GetComponent<IDisplay>(); } }

    public SegmentController segmentController { get { return GetComponent<SegmentController>(); } }
    public Enums.DataType data_type { get { return Enums.DataType.TerrainTile; } }

    public ICollection data_list { get; set; }

    public bool search_by_id;
    public int temp_id_count;

    TerrainTileManager terrainTileManager = new TerrainTileManager();

    public void InitializeController()
    {
        GetData();
    }

    public void GetData()
    {
        data_list = terrainTileManager.GetTerrainTileDataElements(this);

        var terrainTileDataElements = data_list.Cast<TerrainTileDataElement>();

        //terrainTileDataElements.Where(x => x.changed).ToList().ForEach(x => x.Update());
        //terrainTileDataElements[0].Update();
    }
}
