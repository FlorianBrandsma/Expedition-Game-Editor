using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class EditorElement : MonoBehaviour, ISelectionElement
{
    public DataElement DataElement  { get { return GetComponent<DataElement>(); } }
    public IElement Element         { get { return GetComponent<IElement>(); } }

    public Enums.SelectionStatus selectionStatus;
    public SelectionManager.Type selectionType;
    public SelectionManager.Property selectionProperty;

    public bool uniqueSelection;

    public Enums.ElementStatus elementStatus;
    public Color enabledColor;
    public Color disabledColor;
    public Color relatedColor;
    public Color unrelatedColor;
    public Color hiddenColor;

    public bool disableSpawn;
    public EditorElement parent;
    public EditorElement child;
    public GameObject glow;
    public GameObject lockIcon;

    public RectTransform RectTransform  { get { return GetComponent<RectTransform>(); } }
    public Button Button                { get { return GetComponent<Button>(); } }

    public UnityEvent OnSelection = new UnityEvent();

    public void InitializeElement()
    {
        GetComponent<IElement>().InitializeElement();
    }

    public void InitializeElement(SelectionManager.Type selectionType, SelectionManager.Property selectionProperty, bool uniqueSelection)
    {
        this.selectionType = selectionType;
        this.selectionProperty = selectionProperty;
        this.uniqueSelection = uniqueSelection;

        if(DataElement != null && DataElement.Data != null && DataElement.ElementData != null)
            DataElement.ElementData.UniqueSelection = uniqueSelection;

        GetComponent<IElement>().InitializeElement();

        OnSelection.RemoveAllListeners();

        if (selectionType != SelectionManager.Type.None)
        {
            OnSelection.AddListener(delegate { SelectElement(); });
        }
    }

    public void SetElement()
    {
        GetComponent<IElement>().SetElement();
    }

    public void UpdateElement()
    {
        GetComponent<IElement>().SetElement();

        UpdateTrackingElement();
    }

    private void UpdateTrackingElement()
    {
        if (glow == null) return;

        if (glow.GetComponent<TrackingElement>() != null)
            glow.GetComponent<TrackingElement>().UpdateTrackingElement();
    }

    public void SetOverlay()
    {
        if (DataElement.ElementData == null) return;

        if (child != null && child.isActiveAndEnabled)
            child.SetOverlay();

        SetSelection();
        SetStatus();
    }

    private void SetSelection()
    {
        if (selectionStatus == Enums.SelectionStatus.None || glow == null) return;

        //Kind of a band-aid fix, but it works (for now)
        //Cancelling selection after changing index cancels all selections
        if (selectionStatus == DataElement.ElementData.SelectionStatus || DataElement.ElementData.SelectionStatus == Enums.SelectionStatus.Both)
        {
            glow.SetActive(true);

        } else {

            glow.SetActive(false);
        }
    }

    public void SetStatus()
    {
        if (selectionStatus == Enums.SelectionStatus.Child) return;

        if (lockIcon != null)
            lockIcon.SetActive(false);

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
                RenderManager.Render(editorPath.path);
                SelectionManager.SelectSearch(DataElement.ElementData);

                break;

            case SelectionManager.Property.Set:
                SelectionManager.SelectSet(DataElement.ElementData);
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

                DataElement.Data.dataController.ToggleElement(this);

                break;

            case SelectionManager.Property.OpenPhaseSaveRegion:
                RenderManager.Render(editorPath.path);
                break;

            case SelectionManager.Property.OpenPhaseSaveRegionWorldInteractable:
                RenderManager.Render(editorPath.path);
                break;

            case SelectionManager.Property.OpenOutcomeScenes:
                RenderManager.Render(editorPath.path);
                break;

            case SelectionManager.Property.OpenSceneRegion:
                RenderManager.Render(editorPath.path);
                break;

            default: Debug.Log("CASE MISSING: " + selectionProperty); break;
        }
    }
    
    public void CloseElement()
    {
        if (DataElement.ElementData == null) return;

        ResetStatus();

        Element.CloseElement();

        gameObject.SetActive(false);

        if (child != null && child.isActiveAndEnabled)
            child.CloseElement();

        DataElement.ElementData.DataElement = null;
    }

    public void ResetStatus()
    {
        if (elementStatus == Enums.ElementStatus.Enabled) return;

        elementStatus = Enums.ElementStatus.Enabled;

        SetStatus();
    }

    public void CancelSelection()
    {
        if (DataElement.ElementData == null) return;
        
        DataElement.ElementData.SelectionStatus = Enums.SelectionStatus.None;

        if (glow != null)
        {
            glow.SetActive(false);
        }

        if (child != null && child.isActiveAndEnabled)
            child.CancelSelection();
    }
}
