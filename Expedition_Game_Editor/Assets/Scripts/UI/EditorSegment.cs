using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class EditorSegment : MonoBehaviour, IController
{
    private SubController subController { get; set; }

    public bool loaded { get; set; }

    public bool disable_toggle;
    public Toggle toggle;

    public string segment_name;
    public Text header;
    public GameObject content;

    public EditorSegment[] sibling_segments;

    public ElementData data;

    public Path path { get; set; }

    private void Awake()
    {
        if (header == null) return;

        header.text = segment_name;

        if(disable_toggle)
        {
            toggle.interactable = false;
            toggle.isOn = true;
            toggle.targetGraphic.color = Color.gray;
        }
    }

    public void ActivateSegment()
    {
        //Debug.Log(toggle.isOn);

        foreach(EditorSegment segment in sibling_segments)
        {
            if (segment.toggle.isOn != toggle.isOn)
                segment.toggle.isOn = toggle.isOn;
        }

        content.SetActive(toggle.isOn);
    }

    public void InitializeSegment(SubController new_subController)
    {
        subController = new_subController;

        if (GetComponent<ListData>() != null)
            GetComponent<ListData>().GetData(subController.controller.route);

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

    EditorSection IController.section
    {
        get { return subController.controller.editorSection; }
        set { }
    }
    #endregion
}
