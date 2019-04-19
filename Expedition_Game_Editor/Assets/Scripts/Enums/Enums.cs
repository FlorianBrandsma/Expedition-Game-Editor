using UnityEngine;
using System.Collections;

public class Enums
{
    public enum DataType
    {
        None,
        Chapter,
        Phase,
        Quest,
        Step,
        StepElement,
        Task,
        Object,
        Item,
        Element,
        Region,
        Terrain,
        Tile,
        TerrainElement,
        TerrainObject
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
