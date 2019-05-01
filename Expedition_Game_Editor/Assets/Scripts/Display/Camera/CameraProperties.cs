using UnityEngine;
using System.Collections;

public class CameraProperties : MonoBehaviour, IDisplay
{
    public SegmentController SegmentController { get { return GetComponent<SegmentController>(); } }

    public DisplayManager.Type displayType { get; set; }

    public Route route { get; set; }

    public CameraManager cameraManager;

    public void InitializeProperties()
    {
        if (SegmentController.DataController == null) return;

        displayType = GetComponent<IProperties>().Type();

        cameraManager.InitializeCamera(this);
    }

    public void SetDisplay()
    {
        if (SegmentController.DataController == null) return;

        cameraManager.SetProperties();
        cameraManager.SetCamera();
    }

    public void ClearDisplay()
    {
        if (SegmentController.DataController == null) return;

        cameraManager.ClearCamera();
    }

    public void CloseDisplay()
    {
        if (SegmentController.DataController == null) return;

        cameraManager.CloseCamera();
    }
}
