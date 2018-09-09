using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class SelectionElement : MonoBehaviour
{
    public ElementData data;
    EditorPath editorPath;

    public Enums.SelectionType selectionType;
    public Enums.SelectionProperty selectionProperty;

    public SelectionGroup selectionGroup;

    public Text id_text, header, content;

    //PanelElement exclusive
    public Button edit_button;
    public RawImage icon;


    public ListManager listManager { get; set; }

    void SetData(ElementData new_data)
    {
        data = new_data;

        editorPath = new EditorPath(data, listManager.listData.controller.path);
    }

    public void InitializeSelection(ListManager new_listManager, ElementData data)
    {
        listManager = new_listManager;

        SetData(data);

        selectionType = listManager.selectionType;
        selectionProperty = listManager.selectionProperty;

        //To do: "Select" element before opening

        if(selectionType != Enums.SelectionType.None)
        {
            if (listManager.listData.sort_type == Enums.SortType.List)
                icon.texture = Resources.Load<Texture2D>("Textures/Icons/" + selectionProperty.ToString());

            switch(selectionProperty)
            {
                case Enums.SelectionProperty.Open:
                    GetComponent<Button>().onClick.AddListener(delegate { OpenPath(editorPath.open); });

                    if(edit_button != null)
                        edit_button.onClick.AddListener(delegate { OpenPath(editorPath.edit); });
                    break;

                case Enums.SelectionProperty.Edit:
                    GetComponent<Button>().onClick.AddListener(delegate { OpenPath(editorPath.edit); });
                    break;

                default:
                    break;
            }
        } 
    }

    public void OpenPath(Path new_path)
    {
        EditorManager.editorManager.OpenPath(new_path);
    }

    public void SelectElement()
    {
        if(selectionType != Enums.SelectionType.None)
            selectionGroup.SelectElement(this);
    }

    public void SetElement(SelectionElement new_element)
    {
        data.id = new_element.data.id;
    }
}
