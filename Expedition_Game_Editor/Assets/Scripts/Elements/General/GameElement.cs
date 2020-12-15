using UnityEngine;
using UnityEngine.Events;

public class GameElement : MonoBehaviour, ISelectionElement
{
    public DataElement DataElement  { get { return GetComponent<DataElement>(); } }
    public IElement Element         { get { return GetComponent<IElement>(); } }

    public Model Model              { get; set; }

    public readonly float fieldOfView = 90f;

    public GameObject SelectionIcon { get; set; }
    public ExSpeechBubble SpeechBubble  { get; set; }

    public UnityEvent OnSelection = new UnityEvent();

    public void InitializeElement()
    {
        InitializeElement(SelectionManager.Type.Select, SelectionManager.Property.None, false);
    }

    public void InitializeElement(SelectionManager.Type selectionType, SelectionManager.Property selectionProperty, bool uniqueSelection)
    {
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
        SetElement();
    }

    public void SelectElement()
    {
        Debug.Log("Select " + this);
    }

    public void CancelSelection()
    {
        DataElement.ElementData.SelectionStatus = Enums.SelectionStatus.None;

        if (SelectionIcon != null)
        {
            SelectionIcon.SetActive(false);
        }    
    }

    public void CloseSpeechBubble()
    {
        if (SpeechBubble == null) return;

        PoolManager.ClosePoolObject(SpeechBubble);
        SpeechBubble = null;
    }

    public void CloseElement()
    {
        CancelSelection();
        CloseSpeechBubble();

        DataElement.ElementData.DataElement = null;

        if (Model == null) return;

        PoolManager.ClosePoolObject(Model);
        Model = null;
    }
}
