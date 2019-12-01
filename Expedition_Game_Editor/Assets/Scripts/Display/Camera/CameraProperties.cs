using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CameraProperties : MonoBehaviour, IDisplay
{
    [HideInInspector]
    public DisplayManager.Type displayType;

    public CameraManager cameraManager;

    public SelectionManager.Type selectionType;
    public SelectionManager.Property selectionProperty;

    public bool enableScroll;

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

    public IProperties Properties { get { return GetComponent<IProperties>(); } }

    public SelectionManager.Property SelectionProperty { get { return selectionProperty; } }
    public SelectionManager.Type SelectionType { get { return selectionType; } }

    public SegmentController SegmentController { get { return GetComponent<SegmentController>(); } }

    private void OpenCamera()
    {
        InitializeProperties();
        SetDisplay();
    }

    private void InitializeProperties()
    {
        displayType = GetComponent<IProperties>().Type();

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
