using UnityEngine;
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
    public Enums.ElementType elementType;

    public SelectionElement child;
    public GameObject glow;
    //Active Property
    public bool selected;

    public SelectionElement ParentElement { get; set; }
    public ListManager ListManager { get; set; }
    public IElement Element { get; set; }

    public void InitializeElement(ListManager listManager, SelectionManager.Property property)
    {
        if (selected)
            CancelSelection();

        ListManager = listManager;

        //segmentController = listManager.listProperties.dataController.segmentController;

        selectionType = listManager.selectionType;
        route.property = property;

        GetComponent<IElement>().InitializeElement();

        if (selectionType != SelectionManager.Type.None)
            GetComponent<Button>().onClick.AddListener(delegate { SelectElement(); });  
    }

    public void UpdateElement()
    {
        if (ParentElement != null)
            ParentElement.SetElement();
        else
            SetElement();
    }

    public void SetElement()
    {
        GetComponent<IElement>().SetElement();
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
        selected = false;

        if (ListManager != null)
            ListManager.selectedElement = null;

        glow.SetActive(false);
    }

    public void SelectElement()
    {
        if(SelectionManager.getElement != null)
        {
            SelectionManager.SelectSet(this);
            return;
        }

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
