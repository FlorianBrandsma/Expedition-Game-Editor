using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class TerrainElementController : MonoBehaviour, IDataController
{
    public Search.TerrainElement searchParameters;

    private TerrainElementDataManager terrainElementDataManager = new TerrainElementDataManager();

    public IDisplay Display                     { get { return GetComponent<IDisplay>(); } }
    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }

    public Enums.DataType DataType              { get { return Enums.DataType.TerrainElement; } }
    public ICollection DataList                 { get; set; }

    public IEnumerable SearchParameters
    {
        get { return new[] { searchParameters }; }
        set { searchParameters = value.Cast<Search.TerrainElement>().FirstOrDefault(); }
    }

    public void InitializeController()
    {
        terrainElementDataManager.InitializeManager(this);
    }

    public void GetData(IEnumerable searchParameters)
    {
        DataList = terrainElementDataManager.GetTerrainElementDataElements(searchParameters);
    }

    public void SetData(SelectionElement searchElement, Data resultData)
    {

    }
}