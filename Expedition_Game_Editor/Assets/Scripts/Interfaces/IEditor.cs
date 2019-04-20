using UnityEngine;
using System.Collections;

public interface IEditor
{
    Data data { get; set; }
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
