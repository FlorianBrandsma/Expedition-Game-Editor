﻿using UnityEngine;
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
        WorldObject,
        Chapter,
        PartyMember,
        ChapterInteractable,
        ChapterRegion,
        Phase,
        PhaseInteractable,
        Quest,
        Objective,
        WorldInteractable,
        Task,
        Interaction,
        Outcome,
        World,
        Game,
        GameSave,
        ChapterSave,
        PhaseSave,
        QuestSave,
        ObjectiveSave,
        TaskSave,
        InteractionSave
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

    public enum InteractableType
    {
        Character,
        Object
    }

    public enum OutcomeType
    {
        Positive,
        Negative
    }

    public enum RegionType
    {
        Base,
        Phase,
        Interaction,
        Game
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
        WorldElement,
        WorldInteractable,
        ObjectGraphic,
        Text,
        Dropdown,
        StatusIcon,
        SliderHorizontal,
        SliderVertical,
        TabHorizontal,
        TabVertical,
        Header,
        PagingButton,
        FormButton
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