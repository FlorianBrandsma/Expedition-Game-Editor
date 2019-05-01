using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class TerrainController : MonoBehaviour, IDataController
{
    public bool searchById;
    public int temp_id_count;

    private TerrainManager terrainManager       = new TerrainManager();

    public IDisplay Display                     { get { return GetComponent<IDisplay>(); } }

    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }
    public Enums.DataType DataType              { get { return Enums.DataType.Terrain; } }
    public ICollection DataList                 { get; set; }

    public void InitializeController()
    {
        GetData(new List<int>());
    }

    public void GetData(List<int> id_list)
    {
        DataList = terrainManager.GetTerrainDataElements(this);

        var terrainDataElements = DataList.Cast<TerrainDataElement>();

        //terrainDataElements.Where(x => x.changed).ToList().ForEach(x => x.Update());
        //terrainDataElements[0].Update();
    }

    public void GetData(SearchData searchData)
    {

    }
}