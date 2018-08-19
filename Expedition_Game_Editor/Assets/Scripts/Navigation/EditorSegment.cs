using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EditorSegment : MonoBehaviour, IController
{
    private SubController subController { get; set; }

    public ElementData data;

    private ListData listData;

    public void InitializeSegment(SubController new_subController)
    {
        subController = new_subController;

        if (GetComponent<ListData>() != null)
            GetComponent<ListData>().InitializeRows();

        OpenSegment();
    }

    public void FilterRows(List<ElementData> list)
    {
        if (GetComponent<ListData>() != null)
        {
            GetComponent<ListData>().CloseRows();
            GetComponent<ListData>().list = new List<ElementData>(list);
        }

        OpenSegment();
    }

    public void OpenSegment()
    {
        GetComponent<IEditor>().OpenEditor();

        if (GetComponent<ListData>() != null)
            GetComponent<ListData>().SetRows();
    }

    public void CloseSegment()
    {
        if (GetComponent<ListData>() != null)
            GetComponent<ListData>().CloseRows();
    }

    #region IController

    ElementData IController.data
    {
        get { return data; }
        set { }
    }

    EditorField IController.field
    {
        get { return subController.controller.editorField; }
        set { }
    }
    #endregion
}
