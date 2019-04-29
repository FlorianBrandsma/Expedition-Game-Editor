﻿using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class SelectionElement : MonoBehaviour
{
    public Route route = new Route();

    public SelectionManager.Type selectionType;
    public DisplayManager.Type displayType;

    public SelectionElement parent_element { get; set; }
    public SelectionElement child;

    public GameObject glow;

    public ListManager listManager { get; set; }
    //public SegmentController segmentController { get; set; }

    public IElement element { get; set; }

    //Active Property
    public bool selected;

    public void InitializeElement(ListManager listManager, SelectionManager.Property property)
    {
        if (selected)
            CancelSelection();

        this.listManager = listManager;

        //segmentController = listManager.listProperties.dataController.segmentController;

        selectionType = listManager.selectionType;
        route.property = property;

        GetComponent<IElement>().InitializeElement();

        if (selectionType != SelectionManager.Type.None)
            GetComponent<Button>().onClick.AddListener(delegate { SelectElement(); });  
    }

    public void UpdateElement()
    {
        if (parent_element != null)
            parent_element.SetElement();
        else
            SetElement();
    }

    public void SetElement()
    {
        GetComponent<IElement>().SetElement();
    }

    public void ActivateSelection()
    {
        if (route.property == SelectionManager.Property.Get)
            SelectionManager.SelectGet(this);

        if(listManager != null)
            listManager.selectedElement = this;

        selected = true;

        glow.SetActive(true);
    }

    public void CancelSelection()
    {
        selected = false;

        if(listManager != null)
            listManager.selectedElement = null;

        glow.SetActive(false);
    }

    public void SelectElement()
    {
        if (!selected)
        {
            EditorPath editorPath = new EditorPath(this);

            switch (route.property)
            {
                case SelectionManager.Property.Get:
                    EditorManager.editorManager.InitializePath(editorPath.path);
                    ActivateSelection();
                    break;

                case SelectionManager.Property.Set:
                    SelectionManager.SelectSet(this);
                    break;

                case SelectionManager.Property.Enter:
                    EditorManager.editorManager.InitializePath(editorPath.path);
                    break;

                case SelectionManager.Property.Edit:
                    EditorManager.editorManager.InitializePath(editorPath.path);
                    break;

                case SelectionManager.Property.Open:
                    EditorManager.editorManager.InitializePath(editorPath.path);
                    break;

                default:
                    break;
            }
        }          
    }

    public GeneralData GeneralData()
    {
        return route.data.element.Cast<GeneralData>().FirstOrDefault();
    }
}
