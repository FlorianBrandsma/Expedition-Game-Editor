using UnityEngine;
using System.Collections;

public class CameraProperties : MonoBehaviour, IDisplay
{
    public Enums.DisplayType DisplayType { get { return Enums.DisplayType.Camera; } }
    
    public DisplayManager.OrganizerType OrganizerType { get; set; }

    public CameraManager cameraManager;

    public SelectionManager.Type selectionType;
    public SelectionManager.Property selectionProperty;
    public bool uniqueSelection;

    public bool timeBasedLighting;
    public bool enableScroll;
    public bool enableTrackingElements;
    public bool enableTerrainInfo;
    public bool enableGameUI;
    public bool enableCameraFilters;

    private IDataController dataController;
    public IDataController DataController
    {
        get { return dataController; }
        set
        {
            dataController = value;
            OpenCamera();
        }
    }

    public IDisplayManager DisplayManager               { get { return cameraManager; } }

    public IProperties Properties                       { get { return GetComponent<IProperties>(); } }

    public SelectionManager.Property SelectionProperty  { get { return selectionProperty; } }
    public SelectionManager.Property AddProperty        { get { return SelectionManager.Property.None; } }
    public SelectionManager.Type SelectionType          { get { return selectionType; } }
    
    public bool UniqueSelection                         { get { return uniqueSelection; } }

    public SegmentController SegmentController          { get { return GetComponent<SegmentController>(); } }

    private void OpenCamera()
    {
        InitializeProperties();
        SetDisplay();
    }

    private void InitializeProperties()
    {
        OrganizerType = Properties.OrganizerType();

        cameraManager.InitializeCamera(this);
    }

    public void SetDisplay()
    {
        cameraManager.SetProperties();
        cameraManager.SelectData();
        cameraManager.SetCamera();
    }

    public void ClearDisplay()
    {
        if (DataController == null) return;

        cameraManager.ClearCamera();
    }

    public void CloseDisplay()
    {
        if (DataController == null) return;

        cameraManager.CloseCamera();
    }
}
