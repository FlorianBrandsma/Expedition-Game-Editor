using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public interface IEditorData
{
    Path GetPath();
    string GetTable();
    int GetID();
    void FilterRows(List<int> list);
}
