using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public interface IDataController
{
    SegmentController SegmentController { get; }

    List<IDataElement> DataList     { get; set; }
    Enums.DataType DataType         { get; }
    Enums.DataCategory DataCategory { get; }
    IEnumerable SearchParameters    { get; set; }

    List<IDataElement> GetData(IEnumerable searchParameters);
    void SetData(SelectionElement searchElement, IDataElement resultDataElement);
    void ToggleElement(IDataElement dataElement);
}
