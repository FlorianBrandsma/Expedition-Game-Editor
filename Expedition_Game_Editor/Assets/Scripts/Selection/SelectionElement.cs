using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class SelectionElement : MonoBehaviour
{
    public class Data
    {
        public IDataController dataController;
        public IDataElement dataElement;
        public IEnumerable searchParameters;

        public Data() { }

        public Data(IDataController dataController)
        {
            this.dataController = dataController;
            dataElement = new GeneralDataElement();
        }

        public Data(IDataController dataController, IDataElement dataElement)
        {
            this.dataController = dataController;
            this.dataElement = dataElement;
        }

        public Data(IDataController dataController, IDataElement dataElement, IEnumerable searchParameters)
        {
            this.dataController = dataController;
            this.dataElement = dataElement;
            this.searchParameters = searchParameters;
        }
    }

    public Data data = new Data();
    public Path path;

    public SegmentController segmentController;
    public GameObject displayParent;
    public Enums.SelectionGroup selectionGroup;
    public SelectionManager.Type selectionType;
    public SelectionManager.Property selectionProperty;
    public Enums.ElementType elementType;

    public Enums.ElementStatus elementStatus;
    public Color enabledColor;
    public Color disabledColor;
    
    public bool disableSpawn;
    public SelectionElement parent;
    public SelectionElement child;
    public GameObject glow;
    public GameObject lockIcon;

    public bool selected;
    
    public RectTransform RectTransform { get { return GetComponent<RectTransform>(); } }

    public GeneralData GeneralData { get { return (GeneralData)data.dataElement; } }
    
    public IElement Element                 { get { return GetComponent<IElement>(); } }
    public IEditor DataEditor               { get; set; }
    public IDisplayManager DisplayManager   { get; set; }

    public IDataController dataController;

    public void InitializeElement(IDataController dataController)
    {
        DataEditor = segmentController.editorController.PathController.DataEditor;

        this.dataController = dataController;

        data = new Data(GetComponent<IDataController>());

        CancelSelection();

        GetComponent<IElement>().InitializeElement();
    }

    public void InitializeElement(IDisplayManager displayManager, SelectionManager.Type selectionType, SelectionManager.Property selectionProperty)
    {
        CancelSelection();

        DisplayManager = displayManager;

        segmentController = DisplayManager.Display.DataController.SegmentController;

        //Can be overwritten
        dataController = DisplayManager.Display.DataController;

        this.selectionType = selectionType;
        this.selectionProperty = selectionProperty;

        GetComponent<IElement>().InitializeElement();

        if (selectionType != SelectionManager.Type.None)
            GetComponent<Button>().onClick.AddListener(delegate { SelectElement(); });  
    }

    public void SetElement()
    {
        GetComponent<IElement>().SetElement();

        SetOverlay();

        if (displayParent != null)
            displayParent.GetComponent<IDisplay>().DataController = dataController;
    }

    public void UpdateElement()
    {
        GetComponent<IElement>().SetElement();
    }

    public void SetOverlay()
    {
        if (selectionGroup == Enums.SelectionGroup.Child) return;

        switch (elementStatus)
        {
            case Enums.ElementStatus.Enabled:

                Element.ElementColor = enabledColor;

                if(lockIcon != null)
                    lockIcon.SetActive(false);

                break;

            case Enums.ElementStatus.Disabled:

                Element.ElementColor = disabledColor;

                break;

            case Enums.ElementStatus.Locked:

                Element.ElementColor = disabledColor;

                if (lockIcon != null)
                    lockIcon.SetActive(true);

                break;

            case Enums.ElementStatus.Hidden: break;
            case Enums.ElementStatus.Related: break;
            case Enums.ElementStatus.Unrelated: break;
        }
    }

    public void ActivateSelection()
    {
        if (DisplayManager != null)
            DisplayManager.SelectedElement = this;

        selected = true;

        glow.SetActive(true);
    }

    public void CancelSelection()
    {
        if (!selected) return;

        selected = false;

        if (DisplayManager != null)
            DisplayManager.SelectedElement = null;

        glow.SetActive(false);
    }

    public void SelectElement()
    {
        if (selectionType == SelectionManager.Type.None) return;

        if (selected) return;

        EditorPath editorPath = new EditorPath(this, new Route(this));
        
        switch (selectionProperty)
        {
            case SelectionManager.Property.None: break;

            case SelectionManager.Property.Get:
                
                var dataElement = data.dataElement;

                EditorManager.editorManager.InitializePath(editorPath.path);

                SelectionManager.SelectSearch(dataElement);

                break;

            case SelectionManager.Property.Set:
                SelectionManager.SelectSet(data.dataElement);
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

                dataController.ToggleElement(data.dataElement);

                break;

            default: Debug.Log("CASE MISSING"); break;
        }
    }

    public void SetResult(IDataElement resultData)
    {
        if (displayParent != null)
            displayParent.GetComponent<IDisplay>().ClearDisplay();

        dataController.SetData(this, resultData);

        if (dataController.SearchParameters.Cast<SearchParameters>().FirstOrDefault().autoUpdate)
            data.dataElement.UpdateSearch();

        segmentController.GetComponent<ISegment>().SetSearchResult(this);

        SetElement();
    }
}
