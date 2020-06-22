using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public interface IDataController
{
    IDataManager DataManager { get; set; }
    SegmentController SegmentController { get; }

    List<IDataElement> DataList         { get; set; }
    Enums.DataType DataType             { get; }
    Enums.DataCategory DataCategory     { get; }
    SearchProperties SearchProperties   { get; set; }

    void InitializeController();
    void SetData(DataElement searchElement, IDataElement resultDataElement);
    void ToggleElement(EditorElement editorElement);
}
