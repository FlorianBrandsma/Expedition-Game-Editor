using UnityEngine;

[RequireComponent(typeof(ListProperties))]

public class PanelProperties : MonoBehaviour, IProperties
{
    public bool icon;
    public bool zigzag;
    public bool edit;

    public ElementData edit_data;

    public string temp_description = "This is a pretty regular sentence. The structure is something you'd expect. Nothing too long though!";

    public RectTransform reference_area;

    public void Copy(PanelProperties new_properties)
    {
        icon = new_properties.icon;
        zigzag = new_properties.zigzag;
        edit = new_properties.edit;
        edit_data = new_properties.edit_data;
    }

    #region IProperties
    public ListProperties.Type Type()
    {
        return ListProperties.Type.Panel;
    }
    #endregion
}
