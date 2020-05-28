﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class CameraManager : MonoBehaviour, IDisplayManager
{
    public IOrganizer Organizer { get; set; }

    public SelectionManager.Property    SelectionProperty   { get; set; }
    public SelectionManager.Type        SelectionType       { get; set; }

    public Camera cam;
    public RectTransform content;
    public RectTransform overlayParent;

    public OverlayManager overlayManager;

    public IDisplay Display { get; set; }
    private CameraProperties cameraProperties;

    public ExScrollRect ScrollRect { get { return GetComponent<ExScrollRect>(); } }
    public RectTransform RectTransform { get { return GetComponent<RectTransform>(); } }
    public RectTransform cameraParent;

    public RectTransform displayRect;
    public RectTransform leftBorder;

    public Light directionalLight;

    public bool enableFog;

    public void InitializeCamera(CameraProperties cameraProperties)
    {
        transform.parent.gameObject.SetActive(true);

        Display = cameraProperties;
        this.cameraProperties = cameraProperties;

        switch (cameraProperties.OrganizerType)
        {
            case DisplayManager.OrganizerType.None:         Organizer = null; break;
            case DisplayManager.OrganizerType.Object:       Organizer = gameObject.AddComponent<ObjectOrganizer>();         break;
            case DisplayManager.OrganizerType.EditorWorld:  Organizer = gameObject.AddComponent<EditorWorldOrganizer>();    break;
            case DisplayManager.OrganizerType.GameWorld:    Organizer = gameObject.AddComponent<GameWorldOrganizer>();      break;

            default: Debug.Log("CASE MISSING: " + cameraProperties.OrganizerType); break;
        }

        //Don't do this in games. Actually base it off border
        InitializeViewportRect();

        if (Organizer == null) return;

        Organizer.InitializeOrganizer();

        if(overlayManager != null)
            overlayManager.InitializeOverlay(this);

        if (cameraProperties.enableScroll && !Display.DataController.SegmentController.Loaded && RenderManager.loadType == Enums.LoadType.Normal)
            ResetListPosition();

        RenderSettings.fog = enableFog;
    }

    private void InitializeViewportRect()
    {
        float leftBorderOffset = 0;

        if(leftBorder != null)
            leftBorderOffset = leftBorder.rect.width / RenderManager.UI.rect.width;

        cam.rect = new Rect(new Vector2(leftBorderOffset, cam.rect.y),
                            new Vector2((displayRect.rect.width / RenderManager.UI.rect.width) - leftBorderOffset, cam.rect.height));
    }

    public void SetProperties()
    {
        if (Organizer == null) return;

        if(overlayManager != null)
            overlayManager.SetOverlayProperties(cameraProperties);

        transform.parent.gameObject.SetActive(true);
    }

    public void SelectData()
    {
        Organizer.SelectData();
    }

    public void SetCamera()
    {
        if (Organizer == null) return;

        if(overlayManager != null)
            overlayManager.ActivateOverlay(Organizer);
        
        SetData();

        if (overlayManager != null)
            overlayManager.SetOverlay();

        if (cameraProperties.timeBasedLighting)
            TimeManager.instance.SetCameraLight(directionalLight);
        else
            TimeManager.instance.ResetLighting(directionalLight);
    }

    private void ResetListPosition()
    {
        cameraParent.transform.localPosition = new Vector3(0, 0, cameraParent.transform.localPosition.z);
    }

    public void UpdateData()
    {
        Organizer.UpdateData();

        UpdateOverlay();
    }

    private void SetData()
    {
        Organizer.SetData();
    }

    public void UpdateOverlay()
    {
        if (Organizer == null) return;

        if (overlayManager != null)
            overlayManager.UpdateOverlay();
    }

    public void CorrectPosition(IDataElement dataElement)
    {
        var planes = GeometryUtility.CalculateFrustumPlanes(cam);

        var elementBoundSize = new Vector3(1, 1, 1);

        var elementPosition = Vector3.zero;
        
        switch(dataElement.DataType)
        {
            case Enums.DataType.Interaction:

                var interactionData = (InteractionDataElement)dataElement;

                elementPosition = new Vector3(interactionData.PositionX, interactionData.PositionY, interactionData.PositionZ);

                break;

            case Enums.DataType.WorldInteractable:

                var worldInteractableData = (WorldInteractableDataElement)dataElement;

                elementPosition = new Vector3(worldInteractableData.positionX, worldInteractableData.positionY, worldInteractableData.positionZ);
                
                break;

            case Enums.DataType.WorldObject:

                var worldObjectData = (WorldObjectDataElement)dataElement;

                elementPosition = new Vector3(worldObjectData.PositionX, worldObjectData.PositionY, worldObjectData.PositionZ);

                break;

            default: return;
        }

        var startPos = new Vector3(-content.rect.width / 2, content.rect.height / 2, 0);
        var localPosition = new Vector3(startPos.x + elementPosition.x , startPos.y - elementPosition.y, -elementPosition.z);
        
        if (!GeometryUtility.TestPlanesAABB(planes, new Bounds(content.TransformPoint(localPosition), elementBoundSize)))
            cameraParent.transform.localPosition = new Vector3(localPosition.x, localPosition.y, cameraParent.transform.localPosition.z);     
    }

    public void ClearCamera()
    {
        if (Organizer == null) return;

        Organizer.ClearOrganizer();
    }

    public void CloseCamera()
    {
        if (Organizer == null) return;

        ClearCamera();

        if (overlayManager != null)
            overlayManager.CloseOverlay();

        Organizer.CloseOrganizer();

        transform.parent.gameObject.SetActive(false);
    }
}
