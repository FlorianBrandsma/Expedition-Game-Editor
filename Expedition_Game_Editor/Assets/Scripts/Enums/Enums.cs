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
        Model,
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
        Quest,
        Objective,
        WorldInteractable,
        Task,
        Interaction,
        InteractionDestination,
        Outcome,
        EditorWorld,
        Game,
        GameWorld,
        GameRegion,
        GameTerrain,
        GameTerrainTile,
        GameAtmosphere,
        GameInteraction,
        GameInteractionDestination,
        GameWorldInteractable,
        GameWorldObject,
        GamePartyMember,
        Save,
        GameSave,
        PlayerSave,
        InteractableSave,
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
        Agent,
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
        Party,
        InteractionDestination,
        Game
    }

    public enum WorldSelectionType
    {
        InteractionDestination,
        Object,
        Party
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
        Model,
        Text,
        Dropdown,
        StatusIcon,
        SliderHorizontal,
        SliderVertical,
        TabHorizontal,
        TabVertical,
        Header,
        PagingButton,
        FormButton,
        GameWorldElement,
        GameWorldAgent,
        Joystick,
        InputNumber
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

    public enum ControlType
    {
        Controller,
        Keyboard,
        Touch
    }

    public enum InteractionDelayMethod
    {
        Wait
    }

    public enum HistoryGroup
    {
        None,
        Assets,
        Stage,
        Chapter,
        ChapterSelection,
        Phase,
        PhaseSelection,
        Quest,
        QuestSelection,
        Objective,
        ObjectiveSelection,
        Interactable,
        InteractableSelection,
        Task,
        TaskSelection,
        Interaction,
        InteractionSelection,
        Region,
        Terrain,
        TerrainSelection,
        Popup,
        Atmosphere,
        Outcome,
        OutcomeSelection,
        Menu,
        MenuSelection
    }
}