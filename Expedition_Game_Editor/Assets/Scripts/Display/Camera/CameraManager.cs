using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraManager : MonoBehaviour
{
    private IOrganizer organizer;

    public Camera cam;
    public RectTransform content;
    public BackgroundManager backgroundManager;
    public CameraProperties properties { get; set; }

    public void InitializeCamera(CameraProperties new_properties)
    {
        transform.parent.gameObject.SetActive(true);

        properties = new_properties;

        switch (properties.displayType)
        {
            case DisplayManager.Type.None:      organizer = null; break;
            case DisplayManager.Type.Object:    organizer = gameObject.AddComponent<ObjectOrganizer>(); break;
            case DisplayManager.Type.Region:    organizer = gameObject.AddComponent<RegionOrganizer>(); break;
            default: break;
        }

        if (organizer == null) return;

        organizer.InitializeOrganizer();

        //organizer.GetData();

        //overlayManager.InitializeOverlay(this);

        //SelectionManager.lists.Add(this);

        //SetProperties();
    }

    public void SetCamera()
    {
        if (organizer == null) return;

        cam.rect = new Rect(new Vector2(cam.rect.x, cam.rect.y), 
                            new Vector2((properties.section_rect.rect.width / EditorManager.UI.rect.width) - cam.rect.x, cam.rect.height));

        SetData();

        transform.parent.gameObject.SetActive(true);
    }

    private void SetData()
    {
        ItemController test = (ItemController)properties.dataController;

        organizer.SetData(/*new List<ItemController>() { test }*/);

        //organizer.SetData(properties.dataList.list);
    }

    public void CloseCamera()
    {
        if (organizer == null) return;

        //overlayManager.CloseOverlay();

        organizer.CloseOrganizer();

        transform.parent.gameObject.SetActive(false);
    }
}
