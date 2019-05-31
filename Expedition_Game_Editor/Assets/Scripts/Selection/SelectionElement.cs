﻿using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class SelectionElement : MonoBehaviour
{
    public SegmentController segmentController;
    public GameObject displayParent;
    public SelectionManager.Type selectionType;
    public SelectionManager.Property selectionProperty;
    public Enums.ElementType elementType;

    public SelectionElement child;
    public GameObject glow;
    public bool selected;

    public Route route = new Route();

    public IElement Element                 { get; set; }
    public IEditor DataEditor               { get; set; }
    public ListManager ListManager          { get; set; }

    public IDataController DataController
    {
        get
        {
            if (GetComponent<IDataController>() != null)
                return GetComponent<IDataController>();
            else
                return ListManager.listProperties.DataController;
        }
    }

    public void InitializeElement()
    {
        DataEditor = segmentController.editorController.pathController.dataEditor;

        route.data = new Data(GetComponent<IDataController>());

        CancelSelection();

        route.property = selectionProperty;

        GetComponent<IElement>().InitializeElement();
    }

    public void InitializeElement(ListManager listManager, SelectionManager.Property selectionProperty)
    {
        CancelSelection();

        ListManager = listManager;
        
        route.property = selectionProperty;

        segmentController = listManager.listProperties.DataController.SegmentController;

        selectionType = listManager.selectionType;

        GetComponent<IElement>().InitializeElement();

        if (selectionType != SelectionManager.Type.None)
            GetComponent<Button>().onClick.AddListener(delegate { SelectElement(); });  
    }

    public void SetElement()
    {
        GetComponent<IElement>().SetElement();

        if (displayParent != null)
            displayParent.GetComponent<IDisplay>().DataController = DataController;
    }

    public void ActivateSelection()
    {
        if (ListManager != null)
            ListManager.selectedElement = this;

        selected = true;

        glow.SetActive(true);
    }

    public void CancelSelection()
    {
        if (!selected) return;

        selected = false;

        if (ListManager != null)
            ListManager.selectedElement = null;

        glow.SetActive(false);
    }

    public void SelectElement()
    {
        if (selectionType == SelectionManager.Type.None) return;

        if (selected) return;

        EditorPath editorPath = new EditorPath(this);

        switch (route.property)
        {
            case SelectionManager.Property.Get:

                var dataElement = route.data.DataElement;

                EditorManager.editorManager.InitializePath(editorPath.path);

                SelectionManager.SelectSearch(dataElement);

                if (ListManager == null)
                    ActivateSelection();

                break;

            case SelectionManager.Property.Set:
                SelectionManager.SelectSet(route.data.DataElement);
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

    public void SetResult(Data elementData)
    {
        if (displayParent != null)
            displayParent.GetComponent<IDisplay>().ClearDisplay();

        DataController.SetData(this, elementData);
        segmentController.GetComponent<ISegment>().SetSearchResult(this);

        SetElement();
    }

    public GeneralData GeneralData()
    {
        return (GeneralData)route.data.DataElement;      
    }
}
