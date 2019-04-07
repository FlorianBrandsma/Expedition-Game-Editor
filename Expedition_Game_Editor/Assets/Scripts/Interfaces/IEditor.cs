using UnityEngine;
using System.Collections;

public interface IEditor
{
    DataManager.Type data_type { get; }
    IEnumerable data { get; set; }
    ICollection data_list { get; set; }
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
