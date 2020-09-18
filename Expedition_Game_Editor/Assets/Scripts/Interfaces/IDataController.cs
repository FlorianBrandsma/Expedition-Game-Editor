using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public interface IDataController
{
    SegmentController SegmentController { get; }

    Data Data                           { get; set; }
    
    Enums.DataType DataType             { get; }
    Enums.DataCategory DataCategory     { get; }
    SearchProperties SearchProperties   { get; set; }

    void InitializeController();
    void GetData(SearchProperties searchProperties);
    void SetData(IElementData searchElementData, IElementData resultElementData);
    void ToggleElement(EditorElement editorElement);
}
