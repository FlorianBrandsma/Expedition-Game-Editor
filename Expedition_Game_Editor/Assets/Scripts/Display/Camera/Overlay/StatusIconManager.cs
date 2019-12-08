using UnityEngine;
using System.Collections.Generic;

public class StatusIconManager : MonoBehaviour, IOverlay
{
    public enum StatusIconType
    {
        Selection,
        Lock
    }

    public Texture selectTexture;
    public Texture lockTexture;
    
    static public List<EditorStatusIcon> statusIconList = new List<EditorStatusIcon>();

    public void InitializeOverlay(IDisplayManager displayManager)
    {
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

    public GameObject StatusIcon(CameraManager cameraManager, SelectionElement target, StatusIconType statusIconType)
    {
        var statusIcon = SpawnStatusIcon(cameraManager.content);
        
        statusIcon.cam = cameraManager.cam;
        statusIcon.targetDataElement = target.data.dataElement;
        statusIcon.target = target.transform;
        statusIcon.parentRect = cameraManager.displayRect;

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
