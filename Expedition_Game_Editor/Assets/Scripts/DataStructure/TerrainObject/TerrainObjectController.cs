using UnityEngine;
using System.Collections;
using System.Linq;

public class TerrainObjectController : MonoBehaviour, IDataController
{
    public IDisplay display { get { return GetComponent<IDisplay>(); } }

    public SegmentController segmentController { get { return GetComponent<SegmentController>(); } }
    public Enums.DataType data_type { get { return Enums.DataType.TerrainObject; } }

    public ICollection data_list { get; set; }

    public bool search_by_id;
    public int temp_id_count;

    TerrainObjectManager terrainObjectManager = new TerrainObjectManager();

    public void InitializeController()
    {
        GetData();
    }

    public void GetData()
    {
        data_list = terrainObjectManager.GetTerrainObjectDataElements(this);

        var terrainObjectDataElements = data_list.Cast<TerrainObjectDataElement>();

        //terrainObjectDataElements.Where(x => x.changed).ToList().ForEach(x => x.Update());
        //terrainObjectDataElements[0].Update();
    }
}
