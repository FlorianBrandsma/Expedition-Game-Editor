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

    public SegmentController SegmentController { get { return GetComponent<SegmentController>(); } }

    public void InitializeProperties()
    {
        if (SegmentController.DataController == null) return;

        if (GetComponent<IProperties>() != null)
            displayType = GetComponent<IProperties>().Type();

        listManager.InitializeList(this);
    }

    public void SetDisplay()
    {
        if (SegmentController.DataController == null) return;

        listManager.SetProperties();
        listManager.SetList();  
    }

    public void ClearDisplay() { }

    public void CloseDisplay()
    {
        if (SegmentController.DataController == null) return;

        listManager.CloseList();
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
