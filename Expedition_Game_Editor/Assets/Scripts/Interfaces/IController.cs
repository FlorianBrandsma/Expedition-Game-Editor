using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public interface IController
{
    EditorField GetField();
    Path GetPath();
    string GetTable();
    int GetID();
    void FilterRows(List<int> list);
}
