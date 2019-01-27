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
    public ListProperties.Type listType;

    public SelectionElement child;

    public GameObject glow;

    public ListManager listManager { get; set; }
    public IController controller { get; set; }

    //Active Property
    public bool selected;

    public void InitializeSelection(ListManager new_listManager, ElementData new_data, SelectionManager.Property new_property)
    {
        if (selected)
            CancelSelection();

        listManager = new_listManager;

        controller = listManager.listProperties.controller;

        data = new_data;

        selectionType = listManager.selectionType;
        selectionProperty = new_property;

        GetComponent<IElement>().SetElement();

        if (selectionType != SelectionManager.Type.None)
            GetComponent<Button>().onClick.AddListener(delegate { SelectElement(); });  
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
            editorPath = new EditorPath(new Route(this), selectionProperty);

            switch (selectionProperty)
            {
                case SelectionManager.Property.Get:
                    EditorManager.editorManager.OpenPath(editorPath.path);
                    ActivateSelection();
                    break;

                case SelectionManager.Property.Set:
                    SelectionManager.SelectSet(this);       
                    break;

                case SelectionManager.Property.Enter:
                    EditorManager.editorManager.OpenPath(editorPath.path);
                    break;

                case SelectionManager.Property.Edit:
                    EditorManager.editorManager.OpenPath(editorPath.path);
                    break;

                case SelectionManager.Property.Open:
                    EditorManager.editorManager.OpenPath(editorPath.path);
                    break;

                default:
                    break;
            }
        }          
    }
}
