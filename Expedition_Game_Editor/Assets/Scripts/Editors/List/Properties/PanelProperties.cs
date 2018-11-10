using UnityEngine;

[RequireComponent(typeof(ListProperties))]

public class PanelProperties : MonoBehaviour, IProperties
{
    public bool icon;
    public bool zigzag;
    public bool edit;

    public string temp_description = "This is a pretty regular sentence. The structure is something you'd expect. Nothing too long though!";

    public void Copy(PanelProperties new_properties)
    {
        icon = new_properties.icon;
        zigzag = new_properties.zigzag;
        edit = new_properties.edit;
    }

    #region IProperties
    public ListProperties.Type Type()
    {
        return ListProperties.Type.Panel;
    }
    #endregion
}
