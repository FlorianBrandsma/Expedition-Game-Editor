using UnityEngine;

[RequireComponent(typeof(ListProperties))]

public class PanelProperties : MonoBehaviour, IProperties
{
    public bool icon;
    public bool constant_height;
    public bool zigzag;
    public bool edit;

    //public GeneralData edit_data;

    public RectTransform reference_area;

    public void Copy(PanelProperties panelProperties)
    {
        icon = panelProperties.icon;
        constant_height = panelProperties.constant_height;
        zigzag = panelProperties.zigzag;
        edit = panelProperties.edit;
    }

    #region IProperties
    public DisplayManager.Type Type()
    {
        return DisplayManager.Type.Panel;
    }
    #endregion
}
