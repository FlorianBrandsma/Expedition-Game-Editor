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

    void InitializeController();
    List<IDataElement> GetData(IEnumerable searchParameters);
    void SetData(SelectionElement searchElement, SelectionElement.Data resultData);
    void ToggleElement(IDataElement dataElement);
}
