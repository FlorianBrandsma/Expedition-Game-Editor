using UnityEngine;

[RequireComponent(typeof(ListProperties))]

public class PanelProperties : MonoBehaviour, IProperties
{
    public Enums.ElementType elementType;
    public RectTransform referenceArea;
    public bool icon;
    public bool constantHeight;
    public bool zigzag;

    public SelectionManager.Property childProperty;

    public void Copy(PanelProperties panelProperties)
    {
        icon = panelProperties.icon;
        constantHeight = panelProperties.constantHeight;
        zigzag = panelProperties.zigzag;

        childProperty = panelProperties.childProperty;
    }

    #region IProperties
    public DisplayManager.Type Type()
    {
        return DisplayManager.Type.Panel;
    }
    #endregion
}
