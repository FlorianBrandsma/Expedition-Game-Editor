using UnityEngine;
using System.Collections;

public class CameraProperties : MonoBehaviour, IDisplay
{
    public SegmentController segmentController { get { return GetComponent<SegmentController>(); } }

    public DisplayManager.Type displayType { get; set; }

    public Route route { get; set; }

    public CameraManager cameraManager;

    public RectTransform section_rect { get; set; }

    public void InitializeProperties()
    {
        if (segmentController.dataController == null) return;

        section_rect = segmentController.editorController.pathController.editorSection.GetComponent<RectTransform>();

        displayType = GetComponent<IProperties>().Type();
    }

    public void SetDisplay()
    {
        if (segmentController.dataController == null) return;

        cameraManager.InitializeCamera(this);

        cameraManager.SetCamera();
    }

    public void CloseDisplay()
    {
        if (segmentController.dataController == null) return;

        cameraManager.CloseCamera();
    }
}
