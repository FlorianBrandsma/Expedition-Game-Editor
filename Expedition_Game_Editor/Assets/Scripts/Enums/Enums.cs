using UnityEngine;
using System.Collections;

public class Enums
{
    public enum DataType
    {
        None,
        Object,
        Item,
        Element,
        Region,
        Terrain,
        Tile,
        TerrainTile,
        TerrainElement,
        TerrainObject,
        Chapter,
        Phase,
        Quest,
        Step,
        StepElement,
        Task      
    }

    public enum ItemType
    {
        Supplies,
        Gear,
        Spoils,
    }

    public enum RegionType
    {
        Base,
        Phase,
        Task
    }
}
