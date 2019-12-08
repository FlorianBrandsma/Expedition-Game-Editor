using UnityEngine;
using System.Collections;

public interface IOverlay
{
    void InitializeOverlay(IDisplayManager displayManager);
    void ActivateOverlay(IOrganizer organizer);
    void SetOverlay();
    void UpdateOverlay();
    void CloseOverlay();
}
