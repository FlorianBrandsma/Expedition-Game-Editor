using UnityEngine;
using System.Collections;
using System.Linq;

public class TerrainController : MonoBehaviour, IDataController
{
    public IDisplay display { get { return GetComponent<IDisplay>(); } }

    public SegmentController segmentController { get { return GetComponent<SegmentController>(); } }
    public Enums.DataType data_type { get { return Enums.DataType.Terrain; } }

    public ICollection data_list { get; set; }

    public bool search_by_id;
    public int temp_id_count;

    TerrainManager terrainManager = new TerrainManager();

    public void InitializeController()
    {
        GetData();
    }

    public void GetData()
    {
        data_list = terrainManager.GetTerrainDataElements(this);

        var terrainDataElements = data_list.Cast<TerrainDataElement>();

        //terrainDataElements.Where(x => x.changed).ToList().ForEach(x => x.Update());
        //terrainDataElements[0].Update();
    }
}
