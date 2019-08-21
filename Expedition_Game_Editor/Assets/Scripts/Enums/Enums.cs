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
        Terrain,
        Tile,
        TerrainTile,
        SceneInteractable,
        SceneObject,
        Chapter,
        ChapterRegion,
        Phase,
        PhaseInteractable,
        Quest,
        Objective,
        Interaction,
        PartyMember
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
