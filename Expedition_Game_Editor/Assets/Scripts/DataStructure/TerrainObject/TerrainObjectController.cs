using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class TerrainObjectController : MonoBehaviour, IDataController
{
    public IDisplay display { get { return GetComponent<IDisplay>(); } }

    public SegmentController segmentController { get { return GetComponent<SegmentController>(); } }
    public Enums.DataType data_type { get { return Enums.DataType.TerrainObject; } }

    public ICollection dataList { get; set; }

    public bool search_by_id;
    public int temp_id_count;

    TerrainObjectManager terrainObjectManager = new TerrainObjectManager();

    public void InitializeController()
    {
        GetData(new List<int>());
    }

    public void GetData(List<int> id_list)
    {
        dataList = terrainObjectManager.GetTerrainObjectDataElements(this);

        var terrainObjectDataElements = dataList.Cast<TerrainObjectDataElement>();

        //terrainObjectDataElements.Where(x => x.changed).ToList().ForEach(x => x.Update());
        //terrainObjectDataElements[0].Update();
    }
}