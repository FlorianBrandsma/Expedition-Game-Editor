using UnityEngine;
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

    public Enums.ElementStatus elementStatus;
    public Color enabledColor;
    public Color disabledColor;
    public Image background;

    public SelectionElement child;
    public GameObject glow;
    public GameObject lockIcon;

    public bool selected;

    public Route route = new Route();

    public RectTransform RectTransform { get { return GetComponent<RectTransform>(); } }

    public Color BackgroundColor
    {
        get
        {
            if (elementType == Enums.ElementType.Tile)
                return GetComponent<EditorTile>().icon.color;
            else
                return background.color;
        }
        set
        {
            if (elementType == Enums.ElementType.Tile)
                GetComponent<EditorTile>().icon.color = value;
            else
                background.color = value;
        }
    }

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

        SetOverlay(elementStatus);

        if (displayParent != null)
            displayParent.GetComponent<IDisplay>().DataController = DataController;
    }

    public void SetOverlay(Enums.ElementStatus elementStatus)
    {
        switch (elementStatus)
        {
            case Enums.ElementStatus.Enabled:

                BackgroundColor = enabledColor;
                lockIcon.SetActive(false);

                break;

            case Enums.ElementStatus.Disabled:

                BackgroundColor = disabledColor;

                break;

            case Enums.ElementStatus.Locked:

                BackgroundColor = disabledColor;
                lockIcon.SetActive(true);

                break;
        }
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
            case SelectionManager.Property.None: break;

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

            case SelectionManager.Property.Toggle:

                DataController.ToggleElement(route.data.DataElement);

                break;

            default: Debug.Log("CASE MISSING"); break;
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
