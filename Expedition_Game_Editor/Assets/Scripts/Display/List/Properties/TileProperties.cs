using UnityEngine;

[RequireComponent(typeof(ListProperties))]

public class TileProperties : MonoBehaviour, IProperties
{
    public Enums.ElementType elementType;

    public Vector2 GridSize { get; set; }

    #region IProperties
    public DisplayManager.OrganizerType OrganizerType()
    {
        return DisplayManager.OrganizerType.Tile;
    }
    #endregion
}
