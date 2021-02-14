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
        Scene,
        SceneShot,
        CameraFilter,
        SceneActor,
        SceneProp,
        EditorWorld,
        Game,
        GameWorld,
        GameRegion,
        GameTerrain,
        GameTerrainTile,
        GameAtmosphere,
        GameInteraction,
        GameInteractionDestination,
        GameOutcome,
        GameScene,
        GameSceneShot,
        GameSceneActor,
        GameSceneProp,
        GameWorldInteractable,
        GameWorldObject,
        Save,
        GameSave,
        PlayerSave,
        InteractableSave,
        ChapterSave,
        PhaseSave,
        QuestSave,
        ObjectiveSave,
        TaskSave,
        InteractionSave,
        Notification
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
        Object,
        Controllable
    }

    public enum NoteType
    {
        Editor,
        Game
    }

    public enum OutcomeType
    {
        Positive,
        Negative
    }

    public enum CancelScenarioType
    {
        Cancel,
        Skip
    }

    public enum RegionType
    {
        Base,
        Phase,
        Controllable,
        InteractionDestination,
        Scene,
        Game
    }

    public enum WorldSelectionType
    {
        InteractionDestination,
        Object,
        Controllable,
        Shot,
        Actor,
        Prop
    }

    public enum SaveType
    {
        Save,
        Load
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
        SelectionIcon,
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
        InputNumber,
        LoadingBar,
        TouchButton,
        SpeechBubble,
        SpeechTextBox,
        CameraFilter,
        SavePanel,
        NotificationPanel
    }

    public enum SelectionIconType
    {
        Select,
        Lock
    }

    public enum TrackingElementType
    {
        Limited,
        Free
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

    public enum GameButtonType
    {
        Primary,
        Secondary
    }

    public enum ButtonEventType
    {
        None,
        Interact,
        Cancel,
        Attack,
        Defend
    }

    public enum ArrivalType
    {
        Stay,
        Backtrace,
        Repeat
    }

    public enum DelayMethod
    {
        Instant,
        Waiting,
        Gathering
    }

    public enum SpeechMethod
    {
        None,
        Think,
        Whisper,
        Speak,
        Shout
    }

    public enum SceneShotType
    {
        Base,
        Start,
        End
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
        MenuSelection,
        Scene,
        SceneSelection,
        Dialog
    }

    public enum RequestType
    {
        Validate,
        Execute
    }

    public enum ExecuteType
    {
        Update,
        Add,
        Remove
    }

    public enum DialogType
    {
        Main,
        Error,
        Warning,
        Notification
    }

    public enum NotificationType
    {
        Error,
        Warning
    }

    public enum ButtonActionType
    {
        Apply,
        Confirm,
        Close,
        Cancel
    }
}