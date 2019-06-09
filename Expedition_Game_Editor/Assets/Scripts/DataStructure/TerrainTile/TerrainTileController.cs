using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class TerrainTileController : MonoBehaviour, IDataController
{
    public Search.TerrainTile searchParameters;

    private TerrainTileDataManager terrainTileDataManager = new TerrainTileDataManager();

    public IDisplay Display                     { get { return GetComponent<IDisplay>(); } }
    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }

    public Enums.DataType DataType              { get { return Enums.DataType.TerrainTile; } }
    public List<IDataElement> DataList          { get; set; }

    public IEnumerable SearchParameters
    {
        get { return new[] { searchParameters }; }
        set { searchParameters = value.Cast<Search.TerrainTile>().FirstOrDefault(); }
    }

    public void InitializeController()
    {
        terrainTileDataManager.InitializeManager(this);
    }

    public void GetData(IEnumerable searchParameters)
    {
        DataList = terrainTileDataManager.GetTerrainTileDataElements(searchParameters);
    }

    public void SetData(SelectionElement searchElement, Data resultData)
    {

    }

    public void ToggleElement(IDataElement dataElement)
    {

    }
}