﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraManager : MonoBehaviour, IDisplayManager
{
    private IOrganizer organizer;

    public SelectionManager.Property    SelectionProperty   { get; set; }
    public SelectionManager.Type        SelectionType       { get; set; }

    private Vector3 tileBoundSize;
    private Plane[] planes;

    public Camera cam;
    public RectTransform content;

    public OverlayManager overlayManager;

    public IDisplay Display { get; set; }
    private CameraProperties cameraProperties;

    public RectTransform RectTransform { get { return GetComponent<RectTransform>(); } }
    public RectTransform graphicParent;
    public RectTransform displayRect;

    public SelectionElement SelectedElement { get; set; }

    public void InitializeCamera(CameraProperties cameraProperties)
    {
        transform.parent.gameObject.SetActive(true);

        Display = cameraProperties;
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

    public void UpdateData()
    {
        organizer.UpdateData();
    }

    private void SetData()
    {
        organizer.SetData();
    }

    public void UpdateOverlay()
    {
        if (organizer == null) return;

        overlayManager.UpdateOverlay();
    }

    public void CorrectPosition(SelectionElement element)
    {
        planes = GeometryUtility.CalculateFrustumPlanes(cam);

        tileBoundSize = new Vector3(EditorManager.UI.localScale.x * 1,
                                    0,
                                    EditorManager.UI.localScale.z * 1);

        Debug.Log(content.TransformPoint(element.transform.position));

        if (GeometryUtility.TestPlanesAABB(planes, new Bounds(content.TransformPoint(element.transform.position), tileBoundSize)))
        {
            Debug.Log("Correct camera position " + element);
        }
            
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
