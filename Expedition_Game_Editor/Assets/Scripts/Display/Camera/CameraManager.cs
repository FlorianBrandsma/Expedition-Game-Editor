using UnityEngine;
using UnityEngine.UI;
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

    public ExScrollRect ScrollRect          { get { return GetComponent<ExScrollRect>(); } }
    public RectTransform RectTransform      { get { return GetComponent<RectTransform>(); } }
    public RectTransform ScrollRectContent  { get { return GetComponent<ExScrollRect>().content; } }

    public RectTransform cameraParent;

    public RectTransform displayRect;
    public RectTransform leftBorder;

    public Light directionalLight;

    public bool enableFog;

    public void InitializeCamera(CameraProperties cameraProperties)
    {
        TimeManager.activeLight = directionalLight;

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

        InitializeViewportRect();
        
        if (Organizer == null) return;

        if (cameraProperties.enableScroll && !Display.DataController.SegmentController.Loaded && RenderManager.loadType == Enums.LoadType.Normal)
            ResetListPosition();

        Organizer.InitializeOrganizer();

        if(overlayManager != null)
            overlayManager.InitializeOverlay(this);
        
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

        if (overlayManager != null)
            overlayManager.SetOverlayProperties(cameraProperties);

        transform.parent.gameObject.SetActive(true);
    }

    public void CorrectPosition()
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
            TimeManager.instance.SetLighting(TimeManager.instance.ActiveTime);
        else
            TimeManager.instance.ResetLighting();
    }

    private void ResetListPosition()
    {
        ScrollRectContent.transform.localPosition = new Vector3(0, ScrollRectContent.transform.localPosition.z, 0);
    }

    public void UpdateData()
    {
        Organizer.UpdateData();
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

    public void CorrectPosition(IElementData elementData, List<IElementData> dataList)
    {
        var planes = GeometryUtility.CalculateFrustumPlanes(cam);

        var elementBoundSize = new Vector3(1, 1, 1);

        var elementPosition = Vector3.zero;
        
        switch(elementData.DataType)
        {
            case Enums.DataType.InteractionDestination:

                var interactionDestinationData = (InteractionDestinationElementData)elementData;
                elementPosition = new Vector3(interactionDestinationData.PositionX, interactionDestinationData.PositionY, -interactionDestinationData.PositionZ);

                break;

            case Enums.DataType.WorldInteractable:

                var worldInteractableData = (WorldInteractableElementData)elementData;
                elementPosition = new Vector3(worldInteractableData.PositionX, worldInteractableData.PositionY, -worldInteractableData.PositionZ);
                
                break;

            case Enums.DataType.WorldObject:

                var worldObjectData = (WorldObjectElementData)elementData;
                elementPosition = new Vector3(worldObjectData.PositionX, worldObjectData.PositionY, -worldObjectData.PositionZ);

                break;

            case Enums.DataType.Phase:

                var phaseData = (PhaseElementData)elementData;
                elementPosition = new Vector3(phaseData.DefaultPositionX, phaseData.DefaultPositionY, -phaseData.DefaultPositionZ);

                break;

            case Enums.DataType.SceneActor:

                var sceneActorData = (SceneActorElementData)elementData;
                elementPosition = new Vector3(sceneActorData.PositionX, sceneActorData.PositionY, -sceneActorData.PositionZ);

                break;

            case Enums.DataType.SceneProp:

                var scenePropData = (ScenePropElementData)elementData;
                elementPosition = new Vector3(scenePropData.PositionX, scenePropData.PositionY, -scenePropData.PositionZ);

                break;

            default: Debug.Log("CASE MISSING: " + elementData.DataType); return;
        }

        var regionData = (RegionElementData)Display.DataController.SegmentController.Path.FindLastRoute(Enums.DataType.Region).ElementData;
        var worldSize = regionData.RegionSize * regionData.TerrainSize * regionData.TileSize;

        var localPosition = new Vector3(-(worldSize / 2) + elementPosition.x, -elementPosition.y, (worldSize / 2) + elementPosition.z);

        if (!GeometryUtility.TestPlanesAABB(planes, new Bounds(content.TransformPoint(elementPosition), elementBoundSize)))
        {
            ScrollRectContent.transform.localPosition = new Vector3(localPosition.x, localPosition.z, ScrollRectContent.transform.localPosition.z);
            Organizer.UpdateData();
        }
    }

    public void EnableScrolling(bool enable)
    {
        ScrollRect.horizontal = enable;
        ScrollRect.vertical = enable;
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
