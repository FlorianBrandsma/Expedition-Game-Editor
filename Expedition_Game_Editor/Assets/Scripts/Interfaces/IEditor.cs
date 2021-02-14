using UnityEngine;
using System.Collections.Generic;

public interface IEditor
{
    bool Loaded                             { get; set; }

    Data Data                               { get; }
    IElementData ElementData                { get; }
    IElementData EditData                   { get; }
    List<IElementData> DataList             { get; }
    List<IElementData> ElementDataList      { get; }
    List<SegmentController> EditorSegments  { get; }

    bool Addable();
    bool Applicable();
    bool Removable();

    void InitializeEditor();
    void ResetEditor();
    void UpdateEditor();
    void ApplyChanges(DataRequest dataRequest);
    void FinalizeChanges();
    void CancelEdit();
    void CloseEditor();
}
