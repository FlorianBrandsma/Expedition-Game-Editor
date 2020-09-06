using UnityEngine;
using System.Collections.Generic;

public interface IEditor
{
    bool Loaded { get; set; }
    Data Data { get; }
    IElementData ElementData { get; }
    List<IElementData> DataList { get; }
    List<IElementData> ElementDataList { get; }
    List<SegmentController> EditorSegments { get; }
    bool Changed();

    void InitializeEditor();
    void OpenEditor();
    void UpdateEditor();
    void ApplyChanges();
    void CancelEdit();
    void CloseEditor();
}
