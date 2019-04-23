using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraManager : MonoBehaviour
{
    private IOrganizer organizer;

    public Camera cam;
    public RectTransform content;
    public BackgroundManager backgroundManager;
    public CameraProperties cameraProperties { get; set; }

    public RectTransform graphic_parent;

    public List<ObjectGraphic> graphic_list = new List<ObjectGraphic>();

    public void InitializeCamera(CameraProperties new_properties)
    {
        transform.parent.gameObject.SetActive(true);

        cameraProperties = new_properties;

        switch (cameraProperties.displayType)
        {
            case DisplayManager.Type.None:      organizer = null; break;
            case DisplayManager.Type.Object:    organizer = gameObject.AddComponent<ObjectOrganizer>(); break;
            case DisplayManager.Type.Region:    organizer = gameObject.AddComponent<RegionOrganizer>(); break;
            default: break;
        }

        if (organizer == null) return;

        organizer.InitializeOrganizer();

        //organizer.GetData();

        if(backgroundManager != null)
            backgroundManager.InitializeBackground(this);

        //overlayManager.InitializeOverlay(this);

        //SelectionManager.lists.Add(this);

        //SetProperties();
    }

    public void SetProperties()
    {
        if (organizer == null) return;

        organizer.SetProperties();

        transform.parent.gameObject.SetActive(true);
    }

    public void SetCamera()
    {
        if (organizer == null) return;

        cam.rect = new Rect(new Vector2((30 / EditorManager.UI.rect.width), cam.rect.y), 
                            new Vector2((cameraProperties.section_rect.rect.width / EditorManager.UI.rect.width) - cam.rect.x, cam.rect.height));

        SetData();

        transform.parent.gameObject.SetActive(true);
    }

    private void SetData()
    {
        //ItemController test = (ItemController)properties.segmentController.dataController;

        //organizer.SetData(/*new List<ItemController>() { test }*/);

        organizer.SetData();
    }

    public void CloseCamera()
    {
        if (organizer == null) return;

        //overlayManager.CloseOverlay();
        if (backgroundManager != null)
            backgroundManager.CloseBackground();

        organizer.CloseOrganizer();

        transform.parent.gameObject.SetActive(false);
    }

    public ObjectGraphic SpawnGraphic(List<ObjectGraphic> list, ObjectGraphic graphic_prefab)
    {
        foreach (ObjectGraphic graphic in list)
        {
            if (graphic.object_id == graphic_prefab.object_id && !graphic.gameObject.activeInHierarchy)
            {
                InitializeGraphic(graphic);
                return graphic;
            }
        }

        ObjectGraphic new_graphic = Instantiate(graphic_prefab);

        InitializeGraphic(new_graphic);

        list.Add(new_graphic);

        return new_graphic;
    }

    public void InitializeGraphic(ObjectGraphic graphic)
    {
        graphic.InitializeGraphic(this);

        graphic.transform.SetParent(graphic_parent, false);
    }

    public void ResetGraphics()
    {
        foreach (ObjectGraphic graphic in graphic_list)
        {
            //element.GetComponent<IElement>().CloseElement();

            graphic.gameObject.SetActive(false);
        }

        graphic_list.Clear();
    }
}
