using UnityEngine;
using System.Collections.Generic;

public interface IEditor
{
    bool Loaded { get; }
    Route.Data Data { get; set; }
    List<IDataElement> DataElements { get; }
    bool Changed();
    void InitializeEditor();
    void UpdateEditor();
    void UpdateIndex(int index);
    void SetEditor();
    void OpenEditor(); 
    void ApplyChanges();
    void CancelEdit();
    void CloseEditor();
}
