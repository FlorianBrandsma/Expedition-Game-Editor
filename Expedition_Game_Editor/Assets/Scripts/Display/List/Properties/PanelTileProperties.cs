using UnityEngine;

[RequireComponent(typeof(ListProperties))]

public class PanelTileProperties : MonoBehaviour, IProperties
{
    public bool icon;
    public SelectionManager.Property childProperty;

    #region IProperties
    public DisplayManager.OrganizerType OrganizerType()
    {
        return DisplayManager.OrganizerType.PanelTile;
    }
    #endregion
}
