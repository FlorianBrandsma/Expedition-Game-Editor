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
        PartyMember,
        Scene
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
        SceneElement
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