using UnityEngine;
using System.Collections.Generic;

public interface IEditor
{
    bool Loaded { get; set; }
    Route.Data Data { get; }
    List<IElementData> DataList { get; }
    List<IElementData> ElementDataList { get; }
    List<SegmentController> EditorSegments { get; }
    bool Changed();

    void UpdateEditor();
    void ApplyChanges();
    void CancelEdit();
    void CloseEditor();
}
