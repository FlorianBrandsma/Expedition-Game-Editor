using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
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
        public SearchProperties searchProperties;

        public Data() { }

        public Data(Route.Data data)
        {
            dataController = data.dataController;
            dataElement = data.dataElement;
        }

        public Data(IDataController dataController)
        {
            dataElement = new GeneralDataElement();

            if (dataController != null)
            {
                this.dataController = dataController;
                dataElement.DataType = dataController.DataType;
            }
        }
        
        public Data(IDataController dataController, IDataElement dataElement)
        {
            this.dataController = dataController;
            this.dataElement = dataElement;
        }

        public Data(IDataController dataController, IDataElement dataElement, SearchProperties searchProperties)
        {
            this.dataController = dataController;
            this.dataElement = dataElement;
            this.searchProperties = searchProperties;
        }
    }

    public Data data = new Data();
    public Path path;

    public SegmentController segmentController;
    public GameObject displayParent;
    public Enums.SelectionStatus selectionStatus;
    public SelectionManager.Type selectionType;
    public SelectionManager.Property selectionProperty;
    
    public Enums.ElementStatus elementStatus;
    public Color enabledColor;
    public Color disabledColor;
    public Color relatedColor;
    public Color unrelatedColor;
    public Color hiddenColor;
    
    public bool disableSpawn;
    public SelectionElement parent;
    public SelectionElement child;
    public GameObject glow;
    public GameObject lockIcon;

    public RectTransform RectTransform { get { return GetComponent<RectTransform>(); } }
    public Button Button { get { return GetComponent<Button>(); } }

    public GeneralData GeneralData { get { return (GeneralData)data.dataElement; } }
    
    public IPoolable Poolable               { get { return GetComponent<IPoolable>(); } }
    public IElement Element                 { get { return GetComponent<IElement>(); } }
    public IEditor DataEditor               { get; set; }
    public IDisplayManager DisplayManager   { get; set; }

    public UnityEvent OnSelection = new UnityEvent();

    public void InitializeElement(IDataController dataController)
    {
        DataEditor = segmentController.editorController.PathController.DataEditor;

        data = new Data(dataController);

        GetComponent<IElement>().InitializeElement();
    }

    public void InitializeElement(Route.Data data)
    {
        DataEditor = segmentController.editorController.PathController.DataEditor;

        this.data = new Data(data);
        
        GetComponent<IElement>().InitializeElement();
    }

    public void InitializeElement(IDisplayManager displayManager, SelectionManager.Type selectionType, SelectionManager.Property selectionProperty)
    {
        DisplayManager = displayManager;

        segmentController = DisplayManager.Display.DataController.SegmentController;

        //Can be overwritten
        data.dataController = DisplayManager.Display.DataController;
        
        this.selectionType = selectionType;
        this.selectionProperty = selectionProperty;

        GetComponent<IElement>().InitializeElement();

        if (selectionType != SelectionManager.Type.None)
        {
            OnSelection.AddListener(delegate { SelectElement(); });
        }
    }

    public void SetElement()
    {
        GetComponent<IElement>().SetElement();
        
        if (displayParent != null)
            displayParent.GetComponent<IDisplay>().DataController = data.dataController;
    }

    public void UpdateElement()
    {
        GetComponent<IElement>().SetElement();

        UpdateStatusIcon();
    }

    private void UpdateStatusIcon()
    {
        if (glow == null) return;

        if (glow.GetComponent<ExStatusIcon>() != null)
            glow.GetComponent<ExStatusIcon>().UpdatePosition();
    }

    public void SetOverlay()
    {
        if (data.dataElement == null) return;
        
        if (child != null)
            child.SetOverlay();

        SetSelection();
        SetStatus();
    }

    private void SetSelection()
    {
        if (selectionStatus == Enums.SelectionStatus.None) return;
        
        if (selectionStatus == data.dataElement.SelectionStatus || data.dataElement.SelectionStatus == Enums.SelectionStatus.Both)
            glow.SetActive(true);
    }

    public void SetStatus()
    {
        if (selectionStatus == Enums.SelectionStatus.Child) return;

        switch (elementStatus)
        {
            case Enums.ElementStatus.Enabled:

                Element.ElementColor = enabledColor;

                break;

            case Enums.ElementStatus.Disabled:

                Element.ElementColor = disabledColor;

                break;

            case Enums.ElementStatus.Locked:

                Element.ElementColor = disabledColor;

                if (lockIcon != null)
                    lockIcon.SetActive(true);

                break;

            //case Enums.ElementStatus.Hidden:

            //    //Hidden elements are prevented from opening

            //    break;

            case Enums.ElementStatus.Related:

                Element.ElementColor = relatedColor;

                break;
            case Enums.ElementStatus.Unrelated:

                Element.ElementColor = unrelatedColor;

                break;
        }
    }

    public void InvokeSelection()
    {
        OnSelection.Invoke();
    }

    public void SelectElement()
    {
        if (elementStatus == Enums.ElementStatus.Locked) return;

        if (selectionType == SelectionManager.Type.None) return;
        
        EditorPath editorPath = new EditorPath(this, new Route(this));
        
        switch (selectionProperty)
        {
            case SelectionManager.Property.None: break;

            case SelectionManager.Property.Get:
                
                var dataElement = data.dataElement;

                RenderManager.Render(editorPath.path);
                
                SelectionManager.SelectSearch(dataElement);

                break;

            case SelectionManager.Property.Set:
                SelectionManager.SelectSet(data.dataElement);
                break;

            case SelectionManager.Property.Enter:
                RenderManager.Render(editorPath.path);
                break;

            case SelectionManager.Property.Edit:
                RenderManager.Render(editorPath.path);
                break;

            case SelectionManager.Property.Open:
                RenderManager.Render(editorPath.path);
                break;

            case SelectionManager.Property.Toggle:

                data.dataController.ToggleElement(data.dataElement);

                break;

            case SelectionManager.Property.OpenDataCharacters:
                RenderManager.Render(editorPath.path);
                break;

            default: Debug.Log("CASE MISSING: " + selectionProperty); break;
        }
    }

    public void SetResult(IDataElement resultData)
    {
        if (displayParent != null)
            displayParent.GetComponent<IDisplay>().ClearDisplay();

        data.dataController.SetData(this, resultData);
        
        if(data.dataController.SearchProperties != null)
        {
            if (data.dataController.SearchProperties.autoUpdate)
                data.dataElement.UpdateSearch();
        }
        
        segmentController.GetComponent<ISegment>().SetSearchResult(this);
    }

    public void CloseElement()
    {
        if (data.dataElement == null) return;

        ResetStatus();

        Element.CloseElement();
        
        OnSelection.RemoveAllListeners();

        gameObject.SetActive(false);

        if (child != null)
            child.CloseElement();
    }

    public void ResetStatus()
    {
        if (elementStatus == Enums.ElementStatus.Enabled) return;

        if (elementStatus == Enums.ElementStatus.Locked)
            lockIcon.SetActive(false);

        elementStatus = Enums.ElementStatus.Enabled;

        SetStatus();
    }

    public void CancelSelection()
    {
        if (data.dataElement == null) return;

        data.dataElement.SelectionStatus = Enums.SelectionStatus.None;

        if (glow != null)
            glow.SetActive(false);

        if (child != null)
            child.CancelSelection();
    }
}
