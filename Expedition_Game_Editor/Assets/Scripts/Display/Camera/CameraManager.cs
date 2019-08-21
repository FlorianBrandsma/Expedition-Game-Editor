using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraManager : MonoBehaviour
{
    private IOrganizer organizer;

    public Camera cam;
    public RectTransform content;

    public OverlayManager overlayManager;

    [HideInInspector]
    public CameraProperties cameraProperties;

    public RectTransform graphicParent;
    public RectTransform displayRect;

    public List<ObjectGraphic> graphicList = new List<ObjectGraphic>();

    public void InitializeCamera(CameraProperties cameraProperties)
    {
        transform.parent.gameObject.SetActive(true);

        this.cameraProperties = cameraProperties;

        switch (cameraProperties.displayType)
        {
            case DisplayManager.Type.None:      organizer = null; break;
            case DisplayManager.Type.Object:    organizer = gameObject.AddComponent<ObjectOrganizer>(); break;
            case DisplayManager.Type.Scene:     organizer = gameObject.AddComponent<SceneOrganizer>(); break;
            default: break;
        }

        if (organizer == null) return;

        organizer.InitializeOrganizer();

        //organizer.GetData();

        //overlayManager.InitializeOverlay(this);

        //SelectionManager.lists.Add(this);

        //SetProperties();
    }

    public void SetProperties()
    {
        if (organizer == null) return;

        organizer.InitializeProperties();

        transform.parent.gameObject.SetActive(true);
    }

    public void SetCamera()
    {
        if (organizer == null) return;

        float leftBorder = (30 / EditorManager.UI.rect.width);

        cam.rect = new Rect(new Vector2(leftBorder, cam.rect.y),
                            new Vector2((displayRect.rect.width / EditorManager.UI.rect.width) - leftBorder, cam.rect.height));

        SetData();

        transform.parent.gameObject.SetActive(true);
    }

    public void UpdateCamera()
    {
        organizer.UpdateData();
    }

    private void SetData()
    {
        organizer.SetData();
    }

    public void ClearCamera()
    {
        if (organizer == null) return;

        organizer.ClearOrganizer();
    }

    public void CloseCamera()
    {
        if (organizer == null) return;

        ClearCamera();

        organizer.CloseOrganizer();

        transform.parent.gameObject.SetActive(false);
    }
}
