using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ItemController : MonoBehaviour, IDataController
{
    public Search.Item searchParameters;

    public IDataManager DataManager { get; set; }
    
    public IDisplay Display                     { get { return GetComponent<IDisplay>(); } }
    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }

    public Enums.DataType DataType              { get { return Enums.DataType.Item; } }
    public Enums.DataCategory DataCategory      { get { return Enums.DataCategory.None; } }
    public List<IDataElement> DataList          { get; set; }

    public IEnumerable SearchParameters
    {
        get { return new[] { searchParameters }; }
        set { searchParameters = value.Cast<Search.Item>().FirstOrDefault(); }
    }

    public ItemController()
    {
        DataManager = new ItemDataManager(this);
    }

    public void SetData(SelectionElement searchElement, IDataElement resultData) { }

    public void ToggleElement(IDataElement dataElement) { }
}
