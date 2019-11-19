using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GenericDataController : IDataController
{
    private List<IDataElement> dataList = new List<IDataElement>();

    public SegmentController SegmentController { get; set; }
    
    public List<IDataElement> DataList
    {
        get { return dataList; }
        set { dataList = value; }
    }

    public Enums.DataType DataType { get; set; }

    public Enums.DataCategory DataCategory { get; set; }

    public IEnumerable SearchParameters { get; set; }

    public List<IDataElement> GetData(IEnumerable searchParameters)
    {
        return null;
    }

    public void SetData(SelectionElement searchElement, IDataElement resultDataElement) { }

    public void ToggleElement(IDataElement dataElement) { }

    public GenericDataController(Enums.DataType dataType)
    {
        DataType = dataType;
    }
}
