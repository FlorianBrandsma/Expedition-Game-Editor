using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public interface IDataController
{
    IDataManager DataManager { get; set; }
    SegmentController SegmentController { get; }

    List<IDataElement> DataList     { get; set; }
    Enums.DataType DataType         { get; }
    Enums.DataCategory DataCategory { get; }
    IEnumerable SearchParameters    { get; set; }

    void SetData(SelectionElement searchElement, IDataElement resultDataElement);
    void ToggleElement(IDataElement dataElement);
}
