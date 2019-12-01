using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class TileController : MonoBehaviour, IDataController
{
    public Search.Tile searchParameters;

    public IDataManager DataManager { get; set; }
    
    public IDisplay Display                     { get { return GetComponent<IDisplay>(); } }
    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }

    public Enums.DataType DataType              { get { return Enums.DataType.Tile; } }
    public Enums.DataCategory DataCategory      { get { return Enums.DataCategory.None; } }
    public List<IDataElement> DataList          { get; set; }

    public IEnumerable SearchParameters
    {
        get { return new[] { searchParameters }; }
        set { searchParameters = value.Cast<Search.Tile>().FirstOrDefault(); }
    }

    public TileController()
    {
        DataManager = new TileDataManager(this);
    }

    public void SetData(SelectionElement searchElement, IDataElement resultData) { }

    public void ToggleElement(IDataElement dataElement) { }
}