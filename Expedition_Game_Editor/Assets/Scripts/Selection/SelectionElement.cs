﻿using UnityEngine;
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
    public ListProperties.Type listType;

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

            GetComponent<Button>().onClick.AddListener(delegate { SelectElement(); });
        }
    }

    public void ActivateSelection()
    {
        if (selectionProperty == SelectionManager.Property.Get)
            SelectionManager.SelectGet(this);

        if(listManager != null)
            listManager.selected_element = this;

        selected = true;

        glow.SetActive(true);
    }

    public void CancelSelection()
    {
        selected = false;

        if(listManager != null)
            listManager.selected_element = null;

        glow.SetActive(false);
    }

    public void SelectElement()
    {
        if (!selected)
        {
            editorPath = new EditorPath(new Route(this));

            switch (selectionProperty)
            {
                case SelectionManager.Property.Open:
                    EditorManager.editorManager.OpenPath(editorPath.open);
                    break;

                case SelectionManager.Property.Edit:
                    EditorManager.editorManager.OpenPath(editorPath.edit);
                    break;

                case SelectionManager.Property.Get:
                    EditorManager.editorManager.OpenPath(editorPath.get);
                    ActivateSelection();
                    break;

                case SelectionManager.Property.Set:
                    SelectionManager.SelectSet(this);
                    
                    break;

                default:
                    break;
            }
        }          
    }
}
