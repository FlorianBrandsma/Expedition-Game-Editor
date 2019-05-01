using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class TerrainObjectController : MonoBehaviour, IDataController
{
    public bool search_by_id;
    public int temp_id_count;

    private TerrainObjectManager terrainObjectManager = new TerrainObjectManager();

    public IDisplay Display                     { get { return GetComponent<IDisplay>(); } }

    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }
    public Enums.DataType DataType              { get { return Enums.DataType.TerrainObject; } }

    public ICollection DataList                 { get; set; }

    public void InitializeController()
    {
        GetData(new List<int>());
    }

    public void GetData(List<int> id_list)
    {
        DataList = terrainObjectManager.GetTerrainObjectDataElements(this);

        var terrainObjectDataElements = DataList.Cast<TerrainObjectDataElement>();

        //terrainObjectDataElements.Where(x => x.changed).ToList().ForEach(x => x.Update());
        //terrainObjectDataElements[0].Update();
    }

    public void GetData(SearchData searchData)
    {

    }
}