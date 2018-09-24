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

    public SelectionManager.Type selectionType;
    public SelectionManager.Property selectionProperty;

    public Text id_text, header, content;

    //PanelElement exclusive
    public SelectionElement parent;
    public SelectionElement child;
    public RawImage icon;

    public GameObject glow;

    public ListManager listManager { get; set; }

    //Active Property
    public bool selected;

    public void InitializeSelection(ListManager new_listManager, ElementData new_data, SelectionManager.Property new_property)
    {
        listManager = new_listManager;

        data = new_data;

        selectionType = listManager.selectionType;
        selectionProperty = new_property;

        if(selected)
            CancelSelection();

        if(selectionType != SelectionManager.Type.None)
        {
            if (icon != null)
                icon.texture = Resources.Load<Texture2D>("Textures/Icons/" + selectionProperty.ToString());

            GetComponent<Button>().onClick.AddListener(delegate { OpenPath(); });
        }
    }

    public void SelectElement()
    {
        selected = true;

        glow.SetActive(true);
    }

    public void CancelSelection()
    {
        selected = false;

        glow.SetActive(false);
    }

    public void OpenPath()
    {
        editorPath = new EditorPath(data, listManager.listData.controller.path, new Selection(this));

        if (!selected)
        {
            switch (selectionProperty)
            {
                case SelectionManager.Property.Open:
                    EditorManager.editorManager.OpenPath(editorPath.open);
                    break;

                case SelectionManager.Property.Edit:
                    EditorManager.editorManager.OpenPath(editorPath.edit);
                    break;

                default:
                    break;
            }
        }          
    }

    public void SetElement(SelectionElement new_element)
    {
        data.id = new_element.data.id;
    }
}
