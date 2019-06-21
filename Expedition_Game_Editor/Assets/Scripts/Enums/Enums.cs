using UnityEngine;
using System.Collections;

public class Enums
{
    public enum DataType
    {
        None,
        Option,
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
        Phase,
        PhaseElement,
        Quest,
        Objective,
        Task,
        PartyElement
    }

    public enum IconCategory
    {
        Nothing,
        Polearm,
        Bow,
        Crossbow,
        Staff,
        Humanoid,
        Dragonkin,
        Goblin,
        Environment
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

    public enum SelectionGroup
    {
        Main,
        Child
    }

    public enum ElementType
    {
        Panel,
        CompactPanel,
        Tile,
        CompactTile,
        PanelTile,
        MultiGrid,
        CompactMultiGrid,
        Button
    }

    public enum ElementStatus
    {
        Enabled,
        Disabled,
        Locked
    }
}
