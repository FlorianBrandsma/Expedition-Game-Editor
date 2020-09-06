using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameManager : MonoBehaviour
{
    static public GameManager instance;
    
    public GameSaveElementData gameSaveData;
    public GameWorldElementData gameWorldData;
    public GameRegionElementData regionData;
    public GamePartyMemberElementData partyMemberData;

    public GameSaveDataController gameSaveController;
    public GameWorldDataController gameWorldController;
    
    public LocalNavMeshBuilder localNavMeshBuilder;

    public GamePauseAction gamePauseAction;
    public GameTimeAction gameTimeAction;
    public GameSpeedAction gameSpeedAction;

    private int activePhaseId;
    private int activeRegionId;
    private int activePartyMemberId;

    public int ActivePhaseId
    {
        get { return activePhaseId; }
        set
        {
            if (activePhaseId != value)
            {
                activePhaseId = value;

                ChangePhase();
            }
        }
    }

    public int ActiveRegionId
    {
        get { return activeRegionId; }
        set
        {
            if (activeRegionId != value)
            {
                activeRegionId = value;

                gameSaveData.PlayerSaveData.RegionId = value;

                ChangeRegion();
            }
        }
    }

    public int ActivePartyMemberId
    {

        get { return activePartyMemberId; }
        set
        {
            if (activePartyMemberId != value)
            {
                activePartyMemberId = value;

                gameSaveData.PlayerSaveData.PartyMemberId = value;
                
                ChangePartyMember();
            }
        }
    }

    public GameWorldOrganizer Organizer { get { return (GameWorldOrganizer)gameWorldController.Display.DisplayManager.Organizer; } }

    public List<TaskSaveElementData> activeTaskSaveList = new List<TaskSaveElementData>();

    private float tempActiveRange = 222.25f;
    public float TempActiveRange { get { return tempActiveRange; } }

    private void Awake()
    {
        instance = this;
        
        if (!GlobalManager.loaded)
        {
            GlobalManager.programType = GlobalManager.Scenes.Game;

            GlobalManager.OpenScene(GlobalManager.Scenes.Global);
            
            return;
        }
    }

    private void Start()
    {
        RenderManager.Render(new PathManager.Game().Initialize());

        PlayerControlManager.instance.ControlType = Enums.ControlType.Touch;
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.P))
            ActivePhaseId++;

        if (Input.GetKeyUp(KeyCode.I))
            InteractionTest();
    }

    private void InteractionTest()
    {
        Debug.Log("This is an interaction test");

        //After the interaction, relevant save data is updated

        CheckProgress();

        //Completion of an interaction may cause a phase to change
        ActivePhaseId++;

        //The following function should only be performed if the phase hasn't changed
        //In this test, it always changes
        //CheckTime();
    }

    public void LoadGameSaveData(SaveElementData saveElementData)
    {
        Debug.Log("Get save data");
        
        //Get save data
        var searchProperties = new SearchProperties(Enums.DataType.GameSave);

        var searchParameters = searchProperties.searchParameters.Cast<Search.GameSave>().First();
        searchParameters.saveId = new List<int>() { saveElementData.Id };

        gameSaveController.GetData(searchProperties);
        
        gameSaveData = gameSaveController.DataList.Cast<GameSaveElementData>().FirstOrDefault();
    }

    public void OpenGame()
    {
        //Game is technically opened twice when returning from the data editor
        //The closing of other forms triggers the reaction
        gameWorldController.Display.DataController = gameWorldController;

        InitializeLocalNavMesh();

        InitializePhase();

        localNavMeshBuilder.UpdateNavMesh();

        TimeManager.active = true;

        TimeManager.instance.SetLighting();
    }

    private void InitializePhase()
    {
        Debug.Log("Determine active phase");

        //1. Find the first chapter that has not been completed
        //2. Find the first phase of that chapter that has not been completed

        var activeChapterSave = gameSaveData.ChapterSaveDataList.Where(x => !x.Complete).OrderBy(x => x.Index).First();
        var activePhaseSave = gameSaveData.PhaseSaveDataList.Where(x => x.ChapterSaveId == activeChapterSave.Id && !x.Complete).OrderBy(x => x.Index).First();

        ActivePhaseId = activePhaseSave.PhaseId;

        PlayerControlManager.Enabled = true;
    }
    
    private void ChangePhase()
    {
        //Load all game data related to the active phase divided into regions and terrains
        LoadGameData();

        //Set the time speed as a real time multiplier
        SetChapterTimeSpeed();

        //Set the active region based on the save data or else from the phase defaults
        InitializeRegion();

        //Set the active party member based on the save data or else from the phase defaults
        InitializePartyMember();

        //Set the time based on the save data or else from the phase defaults, which is already set
        TimeManager.instance.InitializeGameTime(gameSaveData.PlayerSaveData.GameTime);

        //Compare game data with save data to determine which task is active
        CheckProgress();
    }
    
    private void LoadGameData()
    {
        var searchProperties = new SearchProperties(Enums.DataType.GameWorld);

        var searchParameters = searchProperties.searchParameters.Cast<Search.GameWorld>().First();
        searchParameters.phaseId = new List<int>() { ActivePhaseId };

        gameWorldController.GetData(searchProperties);
        
        gameWorldData = gameWorldController.DataList.Cast<GameWorldElementData>().FirstOrDefault();
    }

    private void SetChapterTimeSpeed()
    {
        TimeManager.gameTimeSpeed = gameWorldData.ChapterData.TimeSpeed;
    }

    private void InitializeRegion()
    {
        //Must be a new phase if the saved region is not listed
        if (!gameWorldData.RegionDataList.Select(x => x.Id).Contains(gameSaveData.PlayerSaveData.RegionId))
        {
            gameSaveData.PlayerSaveData.PositionX = gameWorldData.PhaseData.DefaultPositionX;
            gameSaveData.PlayerSaveData.PositionY = gameWorldData.PhaseData.DefaultPositionY;
            gameSaveData.PlayerSaveData.PositionZ = gameWorldData.PhaseData.DefaultPositionZ;

            gameSaveData.PlayerSaveData.GameTime = gameWorldData.PhaseData.DefaultTime;

            ActiveRegionId = gameWorldData.PhaseData.DefaultRegionId;
            
        } else {

            ActiveRegionId = gameSaveData.PlayerSaveData.RegionId;
        }
    }

    private void InitializePartyMember()
    {
        //The first party member of a chapter is the default
        if (!gameWorldData.RegionDataList.Select(x => x.Id).Contains(gameSaveData.PlayerSaveData.PartyMemberId))
            ActivePartyMemberId = gameWorldData.PartyMemberList.First().Id;
        else
            ActivePartyMemberId = gameSaveData.PlayerSaveData.PartyMemberId;
    }

    public void ChangePartyMember()
    {
        Debug.Log("Change party member");
        partyMemberData = gameWorldData.PartyMemberList.Where(x => x.Id == gameSaveData.PlayerSaveData.PartyMemberId).First();

        PlayerControlManager.instance.SetPlayerCharacter();
    }

    private void CheckProgress()
    {
        //Happens when phase is changed and after every interaction (can hopefully be limited)
        Debug.Log("Check what game data has been completed");

        //Finds the phase save of the active phase
        var activePhaseSave = gameSaveData.PhaseSaveDataList.Where(x => x.PhaseId == ActivePhaseId).First();

        //Finds the quest saves of the quests belonging to the active phase
        var activeQuestSaves = gameSaveData.QuestSaveDataList.Where(x => x.PhaseSaveId == activePhaseSave.Id).ToList();

        //Finds the objectives belonging to the quests of the quest saves
        //Groups the objectives per quest and check if there are any uncompleted objectives left
        //Select the first uncompleted objective if there are any, else pick the last objective
        var activeObjectiveSaves = gameSaveData.ObjectiveSaveDataList.Where(x => activeQuestSaves.Select(y => y.Id).Contains(x.QuestSaveId)).OrderBy(x => x.Index)
                                                                     .GroupBy(x => x.QuestSaveId)
                                                                     .Select(x => x.Any(y => !y.Complete) ? x.Where(y => !y.Complete).First() : x.Last()).ToList();

        //Finds the region interactions: interactions that do not belong to an objective
        var regionInteractions = gameWorldData.WorldInteractableDataList.SelectMany(x => x.InteractionDataList.Where(y => y.ObjectiveId == 0)).ToList();

        //Finds the tasks of the region interactions
        var regionTasks = gameSaveData.TaskSaveDataList.Where(x => regionInteractions.Select(y => y.TaskId).Contains(x.TaskId)).Distinct().ToList();

        //Finds the task saves of the tasks belonging to the objective saves, combined with the region tasks
        //Groups the tasks by world interactable and by objective save, as some world interactables "belong" to multiple objectives and some to none
        //Selects the first uncompleted task if there are any, else pick the last task if it's repeatable
        activeTaskSaveList = gameSaveData.TaskSaveDataList.Where(x => activeObjectiveSaves.Select(y => y.Id).Contains(x.ObjectiveSaveId))
                                                          .Concat(regionTasks)
                                                          .GroupBy(x => new { x.WorldInteractableId, x.ObjectiveSaveId })
                                                          .Select(x => x.ToList().OrderBy(y => y.Index))
                                                          .Select(x => x.Any(y => !y.Complete) ? x.Where(y => !y.Complete).First() : 
                                                                                                 x.Last().Repeatable ? x.Last() : 
                                                                                                                       null).ToList();
        

        TimeManager.activeWorldInteractableList = gameWorldData.WorldInteractableDataList.Where(x => x.InteractionDataList.Any(y => activeTaskSaveList.Select(z => z.TaskId).Contains(y.TaskId))).ToList();
        
        TimeManager.activeInteractionList = gameWorldData.WorldInteractableDataList.SelectMany(x => x.InteractionDataList.Where(y => activeTaskSaveList.Select(z => z.TaskId).Contains(y.TaskId))).ToList();
        
        GetInteractionTimes();

        UpdateWorld();
    }

    private void ChangeRegion()
    {
        Debug.Log("The region was changed, so the organizer should update itself");
        
        //PlayerControlManager.Enabled = false;

        regionData = gameWorldData.RegionDataList.Where(x => x.Id == ActiveRegionId).First();

        //Something should happen here to reset the world when the region is changed manually
        //CheckTime();
    }
    
    private void GetInteractionTimes()
    {
        TimeManager.interactionTimeList = new List<int>();

        TimeManager.activeWorldInteractableList.SelectMany(x => x.InteractionDataList.Where(y => !y.Default)).ToList().ForEach(x => TimeManager.AddInteractionTimeEvent(x));
    }

    public void UpdateWorld()
    {
        Debug.Log("Check time and update world");

        //Check time to see which interactions and atmospheres are active

        DeactivateInteractionTime();

        //For every interactable, check which of their interactions contains the active time
        ValidateInteractionTime();
        
        //Update all world interactables based on the new active times
        UpdateWorldInteractables();

        //Here's the deal: only update the world if any interactable came from a tile that isn't active or when it's going to a tile that isn't active
        //Collect the active terrains when building the world

        //(Re)build the world according to the active data
        SetWorldData();

        localNavMeshBuilder.UpdateNavMesh(true);
    }

    private void InitializeLocalNavMesh()
    {
        localNavMeshBuilder.m_Size = new Vector3(TempActiveRange + (31.75f * 5), 50, TempActiveRange + (31.75f * 5));
    }

    private void SetWorldData()
    {
        Organizer.SetData();
    }

    private void DeactivateInteractionTime()
    {
        gameWorldData.WorldInteractableDataList.SelectMany(x => x.InteractionDataList).ToList().ForEach(y => 
        {
            y.ContainsActiveTime = false;
        });
    }

    private void ValidateInteractionTime()
    {
        //Validate times of interactions which belong to the active task saves
        TimeManager.activeWorldInteractableList.SelectMany(x => x.InteractionDataList).GroupBy(y => y.TaskId)
                                               .Select(y => y.Where(z => TimeManager.TimeInFrame(TimeManager.instance.ActiveTime, z.StartTime, z.EndTime) || z.Default)
                                               .OrderBy(z => z.Default).First()).ToList()
                                               .ForEach(x => 
                                               {
                                                   x.ContainsActiveTime = true;
                                               });

        TimeManager.activeWorldInteractableList.ForEach(x => x.ActiveInteraction = x.InteractionDataList.Where(y => y.ContainsActiveTime).First());
    }

    private void UpdateWorldInteractables()
    {
        gameWorldData.WorldInteractableDataList.ForEach(x => UpdateWorldInteractable(x));
    }

    public void UpdateWorldInteractable(GameWorldInteractableElementData worldInteractableData)
    {
        var interactionData = worldInteractableData.ActiveInteraction;
        
        if(worldInteractableData.DataElement != null)
        {
            //If the active world interactable contains no active time, deactivate it
            if (interactionData == null)
            {
                worldInteractableData.TerrainTileId = 0;

            } else {
                
                switch((Enums.InteractableType)worldInteractableData.Type)
                {
                    case Enums.InteractableType.Agent:

                        //If the active world interactable (agent) contains active time, update the transform
                        worldInteractableData.DataElement.Element.UpdateElement();
                        break;

                    case Enums.InteractableType.Object:

                        //World interactable objects can relocate instantly by changing the terrain tile id
                        worldInteractableData.TerrainTileId = interactionData.InteractionDestinationDataList.First().TerrainTileId;
                        break;
                }
            }

        } else if (interactionData != null) {

            //If the inactive world interactable contains an active time, activate it
            worldInteractableData.TerrainTileId = interactionData.InteractionDestinationDataList.First().TerrainTileId;
        }
        
        //If terrain tile id is not within the active region, make the interactable (agent) walk away in a random direction as they fade out
        //Possible better solution: all interactables should have an intro and outro animation based on their state which plays when they (de)activate
    }

    public void SaveData()
    {
        gameSaveData.Update();
    }

    public void PreviousPath()
    {
        RenderManager.PreviousPath();
    }

    public void CloseGame()
    {
        Debug.Log("Close game");
        
        activePhaseId = 0;
        activeRegionId = 0;
        activePartyMemberId = 0;

        gameWorldData = null;
        regionData = null;
        partyMemberData = null;

        gameWorldController.DataList = null;
        gameSaveController.DataList = null;

        TimeManager.active = false;
        TimeManager.instance.TimeScale = 1;

        PlayerControlManager.Enabled = false;
    }
}