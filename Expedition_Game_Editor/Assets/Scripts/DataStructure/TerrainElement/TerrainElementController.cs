using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class TerrainElementController : MonoBehaviour, IDataController
{
    public IDisplay display { get { return GetComponent<IDisplay>(); } }

    public SegmentController segmentController { get { return GetComponent<SegmentController>(); } }
    public Enums.DataType data_type { get { return Enums.DataType.TerrainElement; } }

    public ICollection dataList { get; set; }

    public bool search_by_id;
    public int temp_id_count;

    TerrainElementManager terrainElementManager = new TerrainElementManager();

    public void InitializeController()
    {
        GetData(new List<int>());
    }

    public void GetData(List<int> id_list)
    {
        dataList = terrainElementManager.GetTerrainElementDataElements(this);

        var terrainElementDataElements = dataList.Cast<TerrainElementDataElement>();

        //terrainElementDataElements.Where(x => x.changed).ToList().ForEach(x => x.Update());
        //terrainElementDataElements[0].Update();
    }
}