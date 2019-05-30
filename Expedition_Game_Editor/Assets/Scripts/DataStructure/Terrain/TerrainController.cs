using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class TerrainController : MonoBehaviour, IDataController
{
    public Search.Terrain searchParameters;

    private TerrainDataManager terrainDataManager = new TerrainDataManager();

    public IDisplay Display                     { get { return GetComponent<IDisplay>(); } }
    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }

    public Enums.DataType DataType              { get { return Enums.DataType.Terrain; } }
    public List<IDataElement> DataList          { get; set; }

    public IEnumerable SearchParameters
    {
        get { return new[] { searchParameters }; }
        set { searchParameters = value.Cast<Search.Terrain>().FirstOrDefault(); }
    }

    public void InitializeController()
    {
        terrainDataManager.InitializeManager(this);
    }

    public void GetData(IEnumerable searchParameters)
    {
        DataList = terrainDataManager.GetTerrainDataElements(searchParameters);
    }

    public void SetData(SelectionElement searchElement, Data resultData)
    {

    }
}