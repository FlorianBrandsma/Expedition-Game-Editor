using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public interface IDataController
{
    IDataManager DataManager { get; set; }
    SegmentController SegmentController { get; }

    List<IElementData> DataList         { get; set; }
    Enums.DataType DataType             { get; }
    Enums.DataCategory DataCategory     { get; }
    SearchProperties SearchProperties   { get; set; }

    void InitializeController();
    void SetData(DataElement searchDataElement, IElementData resultElementData);
    void ToggleElement(EditorElement editorElement);
}
