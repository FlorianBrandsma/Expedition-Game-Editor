using UnityEngine;

[RequireComponent(typeof(ListProperties))]

public class PanelTileProperties : MonoBehaviour, IProperties
{
    public bool icon;
    public bool edit;
    public GeneralData edit_data;

    public void Copy(PanelTileProperties new_properties)
    {
        edit = new_properties.edit;
        edit_data = new_properties.edit_data;
    }

    #region IProperties
    public DisplayManager.Type Type()
    {
        return DisplayManager.Type.PanelTile;
    }
    #endregion
}
