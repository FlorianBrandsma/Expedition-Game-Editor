using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EditorSegment : MonoBehaviour, IController
{
    private SubEditor subEditor { get; set; }

    public string   table { get; set; }
    public int      id    { get; set; }

    private RowManager rowManager;

    public void InitializeSegment(SubEditor new_subEditor)
    {
        subEditor = new_subEditor;

        if (GetComponent<RowManager>() != null)
            GetComponent<RowManager>().InitializeRows();

        OpenSegment();
    }

    public void FilterRows(List<int> list)
    {
        if (GetComponent<RowManager>() != null)
        {
            GetComponent<RowManager>().CloseRows();
            GetComponent<RowManager>().id_list = new List<int>(list);
        }

        OpenSegment();
    }

    public void OpenSegment()
    {
        GetComponent<IEditor>().OpenEditor();

        if (GetComponent<RowManager>() != null)
            GetComponent<RowManager>().SetRows();
    }

    public void CloseSegment()
    {
        if (GetComponent<RowManager>() != null)
            GetComponent<RowManager>().CloseRows();
    }

    #region IController

    public EditorField GetField()
    {
        return subEditor.controller.editorField;
    }

    public Path GetPath()
    {
        return null;
    }

    public string GetTable()
    {
        return table;
    }

    public int GetID()
    {
        return id;
    }

    #endregion
}
