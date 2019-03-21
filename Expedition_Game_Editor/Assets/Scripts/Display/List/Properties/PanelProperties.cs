using UnityEngine;

[RequireComponent(typeof(ListProperties))]

public class PanelProperties : MonoBehaviour, IProperties
{
    public bool icon;
    public bool zigzag;
    public bool edit;

    //public GeneralData edit_data;

    public RectTransform reference_area;

    public void Copy(PanelProperties new_properties)
    {
        icon = new_properties.icon;
        zigzag = new_properties.zigzag;
        edit = new_properties.edit;
        //edit_data = new_properties.edit_data;
    }

    #region IProperties
    public DisplayManager.Type Type()
    {
        return DisplayManager.Type.Panel;
    }
    #endregion
}
