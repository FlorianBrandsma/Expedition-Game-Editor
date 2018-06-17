using UnityEngine;
using System.Collections;

public class EditorSegment : MonoBehaviour, IEditorData
{
    public string table;
    public int id;

    public GameObject content;
    bool collapsed;

    public void OpenSegment()
    {
        if (GetComponent<RowManager>() != null)
            GetComponent<RowManager>().GetRows();

        GetComponent<IEditor>().OpenEditor();

        if (GetComponent<ListProperties>() != null)
            GetComponent<ListProperties>().SetList();

        CollapseSegment(false);
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
