using UnityEngine;
using UnityEngine.Events;

public class GameElement : MonoBehaviour, ISelectionElement
{
    public DataElement DataElement  { get { return GetComponent<DataElement>(); } }

    public Model Model              { get; set; }

    public readonly float fieldOfView = 90f;

    public GameObject selectionIcon;

    public UnityEvent OnSelection = new UnityEvent();

    public void InitializeElement()
    {
        InitializeElement(SelectionManager.Type.Select, SelectionManager.Property.None);
    }

    public void InitializeElement(SelectionManager.Type selectionType, SelectionManager.Property selectionProperty)
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

        if (selectionIcon != null)
        {
            selectionIcon.SetActive(false);
        }
    }

    public void CloseElement()
    {
        CancelSelection();

        PlayerControlManager.instance.RemoveTarget(this);
        
        if (Model == null) return;

        PoolManager.ClosePoolObject(Model);
        Model = null;
    }
}
