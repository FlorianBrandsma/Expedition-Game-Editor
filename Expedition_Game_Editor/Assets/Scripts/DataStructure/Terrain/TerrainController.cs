using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class TerrainController : MonoBehaviour, IDataController
{
    public Search.Terrain searchParameters;

    private TerrainDataManager terrainDataManager;

    public IDisplay Display                     { get { return GetComponent<IDisplay>(); } }
    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }

    public Enums.DataType DataType              { get { return Enums.DataType.Terrain; } }
    public Enums.DataCategory DataCategory      { get { return Enums.DataCategory.None; } }
    public List<IDataElement> DataList          { get; set; }

    public IEnumerable SearchParameters
    {
        get { return new[] { searchParameters }; }
        set { searchParameters = value.Cast<Search.Terrain>().FirstOrDefault(); }
    }

    public TerrainController()
    {
        terrainDataManager = new TerrainDataManager(this);
    }

    public List<IDataElement> GetData(IEnumerable searchParameters)
    {
        return terrainDataManager.GetTerrainDataElements(searchParameters);
    }

    public void SetData(SelectionElement searchElement, IDataElement resultData) { }

    public void ToggleElement(IDataElement dataElement) { }
}