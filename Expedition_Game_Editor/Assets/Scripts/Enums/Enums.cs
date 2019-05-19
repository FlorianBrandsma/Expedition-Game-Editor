using UnityEngine;
using System.Collections;

public class Enums
{
    public enum DataType
    {
        None,
        Search,
        Icon,
        ObjectGraphic,
        Item,
        Element,
        Region,
        Terrain,
        Tile,
        TerrainTile,
        TerrainElement,
        TerrainObject,
        Chapter,
        ChapterRegion,
        ChapterElement,
        Phase,
        PhaseRegion,
        Quest,
        QuestElement,
        Objective,
        ObjectiveElement,
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

    public enum ElementType
    {
        Panel,
        SearchPanel,
        Tile,
        PanelTile,
        Button
    }
}
