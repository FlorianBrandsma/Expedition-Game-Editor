using UnityEngine;
using System.Collections.Generic;

public class StatusIconOverlay : MonoBehaviour, IOverlay
{
    public enum StatusIconType
    {
        Selection,
        Lock
    }

    static public List<EditorStatusIcon> statusIconList = new List<EditorStatusIcon>();

    public Texture selectTexture;
    public Texture lockTexture;

    private Vector2 iconSize = new Vector2(35, 35);

    private CameraManager cameraManager;
    
    public float Multiplier { get; set; }
    public float WidthCap   { get; set; }
    public float HeightCap  { get; set; }
    
    public void InitializeOverlay(IDisplayManager displayManager)
    {
        cameraManager = (CameraManager)displayManager;

        Multiplier  = (((cameraManager.displayRect.rect.width / Screen.width)) * 2f) / ((cameraManager.displayRect.rect.width / EditorManager.UI.rect.width) * 2);
        WidthCap    = (cameraManager.displayRect.rect.width / 2 - iconSize.x);
        HeightCap   = (cameraManager.displayRect.rect.height / 2 - iconSize.y);
        
        selectTexture   = Resources.Load<Texture>("Textures/Icons/Status/SelectIcon");
        lockTexture     = Resources.Load<Texture>("Textures/Icons/Status/LockIcon");
    }

    public void ActivateOverlay(IOrganizer organizer) { }

    public void SetOverlay()
    {
        UpdateOverlay();
    }

    public void UpdateOverlay()
    {
        statusIconList.ForEach(x => x.UpdatePosition());
    }

    public GameObject StatusIcon(SelectionElement target, StatusIconType statusIconType)
    {
        var statusIcon = SpawnStatusIcon(cameraManager.overlayParent);
        
        statusIcon.cam = cameraManager.cam;
        statusIcon.targetDataElement = target.data.dataElement;
        statusIcon.target = target.transform;
        statusIcon.parentRect = cameraManager.displayRect;

        statusIcon.RectTransform.sizeDelta = new Vector2(iconSize.x, iconSize.y);

        statusIcon.statusIconManager = this;

        statusIcon.statusIconType = statusIconType;

        switch (statusIconType)
        {
            case StatusIconType.Selection:
                statusIcon.RawImage.texture = selectTexture;
                statusIcon.RawImage.color = Color.green;
                break;

            case StatusIconType.Lock:
                statusIcon.RawImage.texture = lockTexture;
                statusIcon.RawImage.color = Color.white;
                break;
        }

        return statusIcon.gameObject;
    }

    public void CloseOverlay()
    {
        ResetStatusIcons();

        DestroyImmediate(this);
    }
    
    private EditorStatusIcon SpawnStatusIcon(RectTransform parentRect)
    {
        foreach (EditorStatusIcon statusIcon in statusIconList)
        {
            if (!statusIcon.gameObject.activeInHierarchy)
            {
                statusIcon.gameObject.SetActive(true);
                return statusIcon;
            }
        }

        var newStatusIcon = Instantiate(Resources.Load<EditorStatusIcon>("UI/StatusIcon"));
        statusIconList.Add(newStatusIcon);

        newStatusIcon.transform.SetParent(parentRect);
        newStatusIcon.transform.localEulerAngles = Vector3.zero;
        
        return newStatusIcon;
    }

    public void ResetStatusIcons()
    {
        foreach (EditorStatusIcon statusIcon in statusIconList)
            statusIcon.gameObject.SetActive(false);
    }
}
