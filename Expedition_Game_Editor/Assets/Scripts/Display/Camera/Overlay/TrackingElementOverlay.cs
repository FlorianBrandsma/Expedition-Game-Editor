using UnityEngine;
using System.Collections.Generic;

public class TrackingElementOverlay : MonoBehaviour, IOverlay
{
    private List<TrackingElement> trackingElementList = new List<TrackingElement>();

    private Texture selectTexture;
    private Texture lockTexture;

    private Vector2 iconSize = new Vector2(35, 35);

    public CameraManager CameraManager { get; set; }
    
    public float Multiplier { get; set; }
    
    public void InitializeOverlay(IDisplayManager displayManager)
    {
        CameraManager = (CameraManager)displayManager;

        Multiplier  = (((CameraManager.displayRect.rect.width / Screen.width)) * 2f) / ((CameraManager.displayRect.rect.width / RenderManager.UI.rect.width) * 2);
        
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
        trackingElementList.ForEach(x => x.UpdatePosition());
    }

    public ExSelectionIcon SpawnSelectionIcon(ExSelectionIcon prefab, Enums.SelectionIconType selectionIconType, Enums.TrackingElementType trackingElementType)
    {
        var selectionIcon = (ExSelectionIcon)PoolManager.SpawnObject(prefab);
        selectionIcon.SelectionIconType = selectionIconType;

        InitializeTrackingElement(selectionIcon.TrackingElement, trackingElementType);

        selectionIcon.SetElement();

        selectionIcon.gameObject.SetActive(true);

        return selectionIcon;
    }

    public ExSpeechBubble SpawnSpeechBubble(ExSpeechBubble prefab)
    {
        var speechBubble = (ExSpeechBubble)PoolManager.SpawnObject(prefab);

        InitializeTrackingElement(speechBubble.TrackingElement, Enums.TrackingElementType.Limited);

        speechBubble.gameObject.SetActive(true);

        speechBubble.InitializeElement();

        return speechBubble;
    }

    private void InitializeTrackingElement(TrackingElement trackingElement, Enums.TrackingElementType trackingElementType)
    {
        trackingElement.TrackingElementType = trackingElementType;
        trackingElement.TrackingElementOverlay = this;

        trackingElement.InitializeElement();

        if (!trackingElementList.Contains(trackingElement)) 
            trackingElementList.Add(trackingElement);
    }

    public void CloseOverlay()
    {
        CloseTrackingElements();

        DestroyImmediate(this);
    }

    public void CloseTrackingElements()
    {
        trackingElementList.ForEach(x => PoolManager.ClosePoolObject(x.GetComponent<IPoolable>()));
        
        trackingElementList.Clear();
    }
}
