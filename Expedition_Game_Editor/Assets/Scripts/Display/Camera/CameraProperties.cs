using UnityEngine;
using System.Collections;

public class CameraProperties : MonoBehaviour, IDisplay
{
    public DisplayManager.Type displayType { get; set; }

    public Route route { get; set; }

    public CameraManager cameraManager;

    public RectTransform section_rect { get; set; }

    public IDataController dataController { get {return GetComponent<IDataController>(); } }

    public void InitializeProperties()
    {
        section_rect = GetComponent<PathController>().editorSection.GetComponent<RectTransform>();

        //route = new_route;

        displayType = GetComponent<IProperties>().Type();
    }

    public void SetDisplay()
    {
        cameraManager.InitializeCamera(this);

        cameraManager.SetCamera();
    }

    public void CloseDisplay()
    {
        cameraManager.CloseCamera();
    }
}
