using UnityEngine;

public class ListProperties : MonoBehaviour, IDisplay
{
    public ListManager listManager;

    public SelectionManager.Type selectionType;
    public SelectionManager.Property selectionProperty;
    public SelectionManager.Property addProperty;
    public bool uniqueSelection;

    public Vector2 elementSize;

    public bool horizontal, vertical;

    public bool enableAdding;
    public bool enableAutoSave;
    public bool enableSliders;
    public bool enableNumbers;
    public bool enablePaging;
    public bool enablePositionCorrection;

    public string headerText;

    public int autoSelectId = 0;

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

    public IDisplayManager DisplayManager               { get { return listManager; } }
    public Enums.DisplayType DisplayType                { get { return Enums.DisplayType.List; } }
    public DisplayManager.OrganizerType OrganizerType   { get; set; }

    public IProperties Properties                       { get { return GetComponent<IProperties>(); } }

    public SelectionManager.Type SelectionType          { get { return selectionType; } }
    public SelectionManager.Property SelectionProperty  { get { return selectionProperty; } }
    public SelectionManager.Property AddProperty        { get { return addProperty; } }
    
    public bool UniqueSelection                         { get { return uniqueSelection; } }

    public SegmentController SegmentController          { get { return GetComponent<SegmentController>(); } }

    private void OpenList()
    {
        InitializeProperties();
        SetDisplay();
    }

    private void InitializeProperties()
    {
        if (Properties != null)
            OrganizerType = Properties.OrganizerType();

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
        listManager.AutoSelectElement(autoSelectId);
    }  
}
