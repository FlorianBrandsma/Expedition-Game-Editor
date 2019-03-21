using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class SelectionElement : MonoBehaviour
{
    public IEnumerable data;
    public DataManager.Type data_type;

    EditorPath editorPath;

    public SelectionManager.Type selectionType;
    public SelectionManager.Property selectionProperty;
    public DisplayManager.Type displayType;

    public SelectionElement parent_element { get; set; }
    public SelectionElement child;

    public GameObject glow;

    public ListManager listManager { get; set; }
    public SegmentController segmentController { get; set; }

    public IElement element { get; set; }

    //Active Property
    public bool selected;

    public void InitializeElement(ListManager new_listManager, SelectionManager.Property new_property)
    {
        if (selected)
            CancelSelection();

        listManager = new_listManager;

        segmentController = listManager.listProperties.dataController.segmentController;

        selectionType = listManager.selectionType;
        selectionProperty = new_property;

        GetComponent<IElement>().InitializeElement();

        if (selectionType != SelectionManager.Type.None)
            GetComponent<Button>().onClick.AddListener(delegate { SelectElement(); });  
    }

    public void SetElementData(IEnumerable new_data, DataManager.Type new_data_type)
    {
        data = new_data;
        data_type = new_data_type;
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

    public GeneralData GeneralData()
    {
        return data.Cast<GeneralData>().FirstOrDefault();
    }
}
