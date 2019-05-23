using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Source;

public class CameraManager : MonoBehaviour
{
    private IOrganizer organizer;

    public Camera cam;
    public RectTransform content;
    public BackgroundManager backgroundManager;
    public CameraProperties cameraProperties { get; set; }

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
            case DisplayManager.Type.Region:    organizer = gameObject.AddComponent<RegionOrganizer>(); break;
            default: break;
        }

        if (organizer == null) return;

        organizer.InitializeOrganizer();

        //organizer.GetData();

        if (backgroundManager != null)
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

        float leftBorder = (30 / EditorManager.UI.rect.width);

        cam.rect = new Rect(new Vector2(leftBorder, cam.rect.y),
                            new Vector2((displayRect.rect.width / EditorManager.UI.rect.width) - leftBorder, cam.rect.height));

        SetData();

        transform.parent.gameObject.SetActive(true);
    }

    private void SetData()
    {
        //ItemController test = (ItemController)properties.segmentController.dataController;

        //organizer.SetData(/*new List<ItemController>() { test }*/);

        organizer.SetData();
    }

    public void ClearCamera()
    {
        if (organizer == null) return;

        if (backgroundManager != null)
            backgroundManager.CloseBackground();

        organizer.ClearOrganizer();
    }

    public void CloseCamera()
    {
        if (organizer == null) return;

        ClearCamera();

        organizer.CloseOrganizer();

        transform.parent.gameObject.SetActive(false);
    }

    public ObjectGraphic SpawnGraphic(List<ObjectGraphic> list, ObjectGraphic graphic_prefab)
    {
        foreach (ObjectGraphic graphic in list)
        {
            if (graphic.id == graphic_prefab.id && !graphic.gameObject.activeInHierarchy)
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

        graphic.transform.SetParent(graphicParent, false);
    }

    public void ClearGraphics()
    {
        foreach (ObjectGraphic graphic in graphicList)
        {
            //element.GetComponent<IElement>().CloseElement();

            graphic.gameObject.SetActive(false);
        }

        graphicList.Clear();
    }
}
