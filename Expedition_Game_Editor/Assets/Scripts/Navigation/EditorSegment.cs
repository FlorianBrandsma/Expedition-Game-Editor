using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EditorSegment : MonoBehaviour, IEditorData
{
    public string table;
    public int id;

    public GameObject content;
    bool collapsed;

    private RowManager rowManager;

    public void InitializeSegment()
    {
        if (GetComponent<RowManager>() != null)
            GetComponent<RowManager>().GetRows();

        OpenSegment();

        CollapseSegment(false); 
    }

    public void FilterRows(List<int> list)
    {
        if (GetComponent<RowManager>() != null)
        {
            GetComponent<RowManager>().CloseList();
            GetComponent<RowManager>().id_list = new List<int>(list);
        }

        OpenSegment();
    }

    public void OpenSegment()
    {
        GetComponent<IEditor>().OpenEditor();

        if (GetComponent<ListProperties>() != null)
            GetComponent<ListProperties>().SetList();
    }

    public void CollapseSegment()
    {
        CollapseSegment(collapsed);
    }

    void CollapseSegment(bool collapse)
    {
        content.SetActive(!collapse);
        collapsed = collapse;
    }

    public void CloseSegment()
    {
        if (GetComponent<RowManager>() != null)
            GetComponent<RowManager>().CloseList();
    }

    #region ISubEditor

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
