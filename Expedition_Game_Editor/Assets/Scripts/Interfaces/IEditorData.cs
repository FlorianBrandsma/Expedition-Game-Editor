using UnityEngine;
using System.Collections;

public interface IEditorData
{
    Path GetPath();
    string GetTable();
    int GetID();
}
