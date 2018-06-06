using UnityEngine;
using System.Collections;

public interface IEditor
{
    void OpenEditor();
    void SaveEdit();
    void ApplyEdit();
    void CancelEdit();
    void CloseEditor();
}
