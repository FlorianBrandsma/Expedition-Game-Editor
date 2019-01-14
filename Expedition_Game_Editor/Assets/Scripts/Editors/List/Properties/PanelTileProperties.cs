using UnityEngine;

[RequireComponent(typeof(ListProperties))]

public class PanelTileProperties : MonoBehaviour, IProperties
{
    public bool icon;
    public bool edit;
    public ElementData edit_data;

    public void Copy(PanelTileProperties new_properties)
    {
        edit = new_properties.edit;
        edit_data = new_properties.edit_data;
    }

    #region IProperties
    public ListProperties.Type Type()
    {
        return ListProperties.Type.PanelTile;
    }
    #endregion
}
