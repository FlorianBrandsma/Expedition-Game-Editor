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
        Interactable,
        Region,
        Atmosphere,
        Terrain,
        Tile,
        TerrainTile,
        WorldInteractable,
        WorldObject,
        Chapter,
        ChapterRegion,
        Phase,
        PhaseInteractable,
        Quest,
        Objective,
        Task,
        Interaction,
        PartyMember,
        World
    }

    public enum DataCategory
    {
        None,
        Navigation
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

    public enum IconType
    {
        None,
        Base,
        Icon
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
        Interaction
    }

    public enum DisplayType
    {
        List,
        Camera
    }

    public enum SelectionStatus
    {
        None,
        Main,
        Child,
        Both
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
        Button,
        WorldElement
    }

    public enum ElementStatus
    {
        Enabled,
        Disabled,
        Locked,
        Hidden,
        Related, 
        Unrelated
    }

    public enum LoadType
    {
        Normal,
        Reload,
        Return
    }
}