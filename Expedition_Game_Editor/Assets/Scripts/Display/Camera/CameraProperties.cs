using UnityEngine;
using System.Collections;

public class CameraProperties : MonoBehaviour, IDisplay
{
    public SegmentController segmentController { get { return GetComponent<SegmentController>(); } }

    public DisplayManager.Type displayType { get; set; }

    public Route route { get; set; }

    public CameraManager cameraManager;

    public void InitializeProperties()
    {
        if (segmentController.dataController == null) return;

        displayType = GetComponent<IProperties>().Type();

        cameraManager.InitializeCamera(this);
    }

    public void SetDisplay()
    {
        if (segmentController.dataController == null) return;

        cameraManager.SetProperties();
        cameraManager.SetCamera();
    }

    public void CloseDisplay()
    {
        if (segmentController.dataController == null) return;

        cameraManager.CloseCamera();
    }
}
