using UnityEngine;

[RequireComponent(typeof(ListProperties))]

public class TileProperties : MonoBehaviour, IProperties
{
    public Vector2 grid_size;

    //Spawn tiles in rect without altering size
    public bool fit_axis = true;

    public void Copy(TileProperties new_properties)
    {
        grid_size = new_properties.grid_size;
        fit_axis = new_properties.fit_axis;
    }

    #region IProperties
    public ListProperties.Type Type()
    {
        return ListProperties.Type.Tile;
    }
    #endregion
}
