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

    public void InitializeElement(SelectionManager.Type selectionType, SelectionManager.Property selectionProperty)
    {
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
        if (DataElement.data.elementData == null) return;

        if (child != null)
            child.SetOverlay();

        SetSelection();
        SetStatus();
    }

    private void SetSelection()
    {
        if (selectionStatus == Enums.SelectionStatus.None) return;
        
        if (selectionStatus == DataElement.data.elementData.SelectionStatus || DataElement.data.elementData.SelectionStatus == Enums.SelectionStatus.Both)
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

                var elementData = DataElement.data.elementData;

                RenderManager.Render(editorPath.path);

                SelectionManager.SelectSearch(elementData);

                break;

            case SelectionManager.Property.Set:
                SelectionManager.SelectSet(DataElement.data.elementData);
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

                DataElement.data.dataController.ToggleElement(this);

                break;

            case SelectionManager.Property.OpenDataCharacters:
                RenderManager.Render(editorPath.path);
                break;

            case SelectionManager.Property.OpenPhaseSaveRegion:
                RenderManager.Render(editorPath.path);
                break;

            case SelectionManager.Property.OpenPhaseSaveRegionWorldInteractable:
                RenderManager.Render(editorPath.path);
                break;

            default: Debug.Log("CASE MISSING: " + selectionProperty); break;
        }
    }
    
    public void CloseElement()
    {
        if (DataElement.data.elementData == null) return;

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
        if (DataElement.data.elementData == null) return;

        DataElement.data.elementData.SelectionStatus = Enums.SelectionStatus.None;

        if (glow != null)
            glow.SetActive(false);

        if (child != null)
            child.CancelSelection();
    }
}
