using UnityEngine;

[RequireComponent(typeof(ListProperties))]

public class PanelTileProperties : MonoBehaviour, IProperties
{
    public bool icon;
    public SelectionManager.Property childProperty;

    #region IProperties
    public DisplayManager.Type Type()
    {
        return DisplayManager.Type.PanelTile;
    }
    #endregion
}
