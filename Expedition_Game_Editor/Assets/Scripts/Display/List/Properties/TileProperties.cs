﻿using UnityEngine;

[RequireComponent(typeof(ListProperties))]

public class TileProperties : MonoBehaviour, IProperties
{
    public Vector2 grid_size;

    public void Copy(TileProperties new_properties)
    {
        grid_size = new_properties.grid_size;
    }

    #region IProperties
    public DisplayManager.Type Type()
    {
        return DisplayManager.Type.Tile;
    }
    #endregion
}