using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class TileController : MonoBehaviour, IDataController
{
    public Search.Element searchParameters;

    public TileDataManager tileDataManager = new TileDataManager();

    public IDisplay Display { get { return GetComponent<IDisplay>(); } }
    public SegmentController SegmentController { get { return GetComponent<SegmentController>(); } }

    public Enums.DataType DataType { get { return Enums.DataType.Tile; } }
    public List<IDataElement> DataList { get; set; }

    public IEnumerable SearchParameters
    {
        get { return new[] { searchParameters }; }
        set { searchParameters = value.Cast<Search.Element>().FirstOrDefault(); }
    }

    public void InitializeController()
    {
        tileDataManager.InitializeManager(this);
    }

    public void GetData(IEnumerable searchParameters)
    {
        DataList = tileDataManager.GetTileDataElements(searchParameters);
    }

    public void SetData(SelectionElement searchElement, Data resultData) { }

    public void ToggleElement(IDataElement dataElement) { }
}