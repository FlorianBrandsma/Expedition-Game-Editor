using UnityEngine;
using System.Collections.Generic;

public class ListProperties : MonoBehaviour, IDisplay
{
    [HideInInspector]
    public DisplayManager.Type displayType;

    public ListManager listManager;

    public SelectionManager.Type selectionType;
    public SelectionManager.Property selectionProperty;
    
    public Vector2 elementSize;

    public bool horizontal, vertical;

    public bool enableAdding;
    public bool enableAutoSave;
    public bool enableSliders;
    public bool enableNumbers;
    public bool enablePaging;

    public bool enablePositionCorrection;

    private IDataController dataController;
    public IDataController DataController
    {
        get { return dataController; }
        set
        {
            dataController = value;

            if (DataController != null)
                OpenList();
        }
    }

    public SegmentController SegmentController { get { return GetComponent<SegmentController>(); } }

    private void OpenList()
    { 
        InitializeProperties();
        SetDisplay();
    }

    private void InitializeProperties()
    {
        if (GetComponent<IProperties>() != null)
            displayType = GetComponent<IProperties>().Type();

        listManager.InitializeList(this);
    }

    public void SetDisplay()
    {
        listManager.SetProperties();
        listManager.SetList();  
    }

    public void ClearDisplay() { }

    public void CloseDisplay()
    {
        if (DataController == null) return;

        listManager.CloseList();

        DataController = null;
    }

    public void AutoSelectElement()
    {
        listManager.AutoSelectElement();
    }

    public void ResetList()
    {
        listManager.ResetData();
    }   
}
