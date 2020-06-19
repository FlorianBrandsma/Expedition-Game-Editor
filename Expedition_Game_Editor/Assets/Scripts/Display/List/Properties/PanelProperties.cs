using UnityEngine;

[RequireComponent(typeof(ListProperties))]

public class PanelProperties : MonoBehaviour, IProperties
{
    public Enums.ElementType elementType;
    public RectTransform referenceArea;
    
    public bool constantHeight;
    public bool zigzag;

    public Enums.IconType iconType;
    public SelectionManager.Property childProperty;

    #region IProperties
    public DisplayManager.OrganizerType OrganizerType()
    {
        return DisplayManager.OrganizerType.Panel;
    }
    #endregion
}
