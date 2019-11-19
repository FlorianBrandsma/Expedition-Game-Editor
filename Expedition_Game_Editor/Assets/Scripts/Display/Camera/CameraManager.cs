using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class CameraManager : MonoBehaviour, IDisplayManager
{
    private IOrganizer organizer;

    public SelectionManager.Property    SelectionProperty   { get; set; }
    public SelectionManager.Type        SelectionType       { get; set; }

    public Camera cam;
    public RectTransform content;

    public OverlayManager overlayManager;

    public IDisplay Display { get; set; }
    private CameraProperties cameraProperties;

    public CustomScrollRect ScrollRect { get { return GetComponent<CustomScrollRect>(); } }
    public RectTransform RectTransform { get { return GetComponent<RectTransform>(); } }
    public RectTransform cameraParent;

    public RectTransform displayRect;

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

        if (cameraProperties.enableScroll && !Display.DataController.SegmentController.Loaded && EditorManager.loadType == Enums.LoadType.Normal)
            ResetListPosition();
    }

    public void SetProperties()
    {
        if (organizer == null) return;

        transform.parent.gameObject.SetActive(true);
    }

    public void SelectData()
    {
        organizer.SelectData();
    }

    public void SetCamera()
    {
        if (organizer == null) return;

        float leftBorder = (30 / EditorManager.UI.rect.width);

        cam.rect = new Rect(new Vector2(leftBorder, cam.rect.y),
                            new Vector2((displayRect.rect.width / EditorManager.UI.rect.width) - leftBorder, cam.rect.height));

        SetData();
    }

    private void ResetListPosition()
    {
        cameraParent.transform.localPosition = new Vector3(0, 0, cameraParent.transform.localPosition.z);
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

            case Enums.DataType.SceneObject:

                var sceneObjectData = (SceneObjectDataElement)dataElement;

                elementPosition = new Vector3(sceneObjectData.PositionX, sceneObjectData.PositionY, sceneObjectData.PositionZ);

                break;
        }

        var startPos = new Vector3(-content.rect.width / 2, content.rect.height / 2, 0);
        var localPosition = new Vector3(startPos.x + elementPosition.x , startPos.y - elementPosition.y, -elementPosition.z);

        if (!GeometryUtility.TestPlanesAABB(planes, new Bounds(content.TransformPoint(localPosition), elementBoundSize)))
        {
            cameraParent.transform.localPosition = new Vector3(localPosition.x, localPosition.y, cameraParent.transform.localPosition.z);
            organizer.UpdateData();
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
