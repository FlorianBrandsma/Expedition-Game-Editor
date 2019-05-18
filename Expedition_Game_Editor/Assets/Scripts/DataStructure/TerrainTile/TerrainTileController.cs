using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class TerrainTileController : MonoBehaviour//, IDataController
{
    public int temp_id_count;

    public SearchParameters searchParameters;

    private TerrainTileDataManager terrainTileDataManager = new TerrainTileDataManager();

    public IDisplay Display                     { get { return GetComponent<IDisplay>(); } }
    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }

    public Enums.DataType DataType              { get { return Enums.DataType.TerrainTile; } }
    public ICollection DataList                 { get; set; }

    public SearchParameters SearchParameters
    {
        get { return searchParameters; }
        set { searchParameters = value; }
    }

    public void InitializeController()
    {
        //GetData(new List<int>());
    }

    public void GetData(SearchParameters searchParameters)
    {
        DataList = terrainTileDataManager.GetTerrainTileDataElements(this);

        var terrainTileDataElements = DataList.Cast<TerrainTileDataElement>();

        //terrainTileDataElements.Where(x => x.changed).ToList().ForEach(x => x.Update());
        //terrainTileDataElements[0].Update();
    }

    public void ReplaceData(IEnumerable dataElement)
    {

    }
}