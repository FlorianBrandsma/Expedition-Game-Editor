using UnityEngine;
using System.Collections.Generic;

public class StatusIconOverlay : MonoBehaviour, IOverlay
{
    public enum StatusIconType
    {
        Selection,
        Lock
    }

    private List<ExStatusIcon> statusIconList = new List<ExStatusIcon>();

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

        Multiplier  = (((cameraManager.displayRect.rect.width / Screen.width)) * 2f) / ((cameraManager.displayRect.rect.width / RenderManager.UI.rect.width) * 2);
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
        var prefab = Resources.Load<ExStatusIcon>("Elements/UI/StatusIcon");

        var statusIcon = (ExStatusIcon)PoolManager.SpawnObject(prefab);
        statusIconList.Add(statusIcon);

        statusIcon.transform.SetParent(cameraManager.overlayParent);
        statusIcon.transform.localEulerAngles = Vector3.zero;
        
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

        statusIcon.gameObject.SetActive(true);

        return statusIcon.gameObject;
    }

    public void CloseOverlay()
    {
        CloseStatusIcons();

        DestroyImmediate(this);
    }

    public void CloseStatusIcons()
    {
        statusIconList.ForEach(x => PoolManager.ClosePoolObject(x));
        
        statusIconList.Clear();
    }
}
