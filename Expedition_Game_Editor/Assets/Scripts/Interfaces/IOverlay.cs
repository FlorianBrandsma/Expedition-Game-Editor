using UnityEngine;
using System.Collections;

public interface IOverlay
{
    void InitializeOverlay(ListManager listManager);
    void ActivateOverlay(IOrganizer organizer, IList list);
    void SetOverlay();
    void UpdateOverlay();
    void CloseOverlay();
}
