using UnityEngine;

public class SpeechOverlay : MonoBehaviour, IOverlay
{
    private OverlayManager OverlayManager { get { return GetComponent<OverlayManager>(); } }

    public void InitializeOverlay(IDisplayManager displayManager) { }

    public void ActivateOverlay(IOrganizer organizer) { }

    public void UpdateOverlay() { }

    public void SetOverlay() { }
    
    public void CloseOverlay()
    {
        DestroyImmediate(this);
    }
}
