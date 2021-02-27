using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameManager : MonoBehaviour
{
    static public GameManager instance;

    public static GameElementData gameElementData;

    public GameSaveElementData activeGameSaveElementData;
    public GameWorldElementData gameWorldData;
    public GameRegionElementData regionData;
    public GameWorldInteractableElementData worldInteractableControllableData;

    private List<TerrainTileData> activeTileList;
    private List<GameWorldInteractableElementData> activeWorldInteractableList;

    public GameSaveDataController gameSaveController;
    public GameWorldDataController gameWorldController;

    public LocalNavMeshBuilder localNavMeshBuilder;

    public GamePauseAction gamePauseAction;
    public GameTimeAction gameTimeAction;
    public GameSpeedAction gameSpeedAction;

    private int activePhaseId;
    private int activeRegionId;
    private int activeWorldInteractableControllableId;

    public GameDataController GameDataController { get { return GetComponent<GameDataController>(); } }

    public DataController WorldObjectDataController                     { get; set; } = new DataController(Enums.DataType.WorldObject);
    public DataController WorldInteractableAgentDataController          { get; set; } = new DataController(Enums.DataType.WorldInteractable);
    public DataController WorldInteractableObjectDataController         { get; set; } = new DataController(Enums.DataType.WorldInteractable);
    public DataController WorldInteractableControllableDataController   { get; set; } = new DataController(Enums.DataType.WorldInteractable);
    public DataController ScenePropDataController                       { get; set; } = new DataController(Enums.DataType.SceneProp);

    public GameWorldOrganizer Organizer { get { return (GameWorldOrganizer)gameWorldController.Display.DisplayManager.Organizer; } }
    
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

                activeGameSaveElementData.SaveData.RegionId = value;

                ChangeRegion();
            }
        }
    }

    public int ActiveWorldInteractableControllableId
    {

        get { return activeWorldInteractableControllableId; }
        set
        {
            if (activeWorldInteractableControllableId != value)
            {
                activeWorldInteractableControllableId = value;

                activeGameSaveElementData.SaveData.WorldInteractableId = value;
                
                ChangeControllable();
            }
        }
    }
    
    public List<TaskSaveElementData> activeTaskSaveList = new List<TaskSaveElementData>();

    private float tempActiveRange = 222.25f;
    public float TempActiveRange { get { return tempActiveRange; } }

    private void Awake()
    {
        instance = this;
        
        if (!GlobalManager.loaded)
        {
            Fixtures.LoadFixtures();

            GlobalManager.programType = GlobalManager.Scenes.Game;

            LoadDefaultGame();

            GlobalManager.OpenScene(GlobalManager.Scenes.Global);
            
            return;
        }
    }

    private void LoadDefaultGame()
    {
        var searchProperties = new SearchProperties(Enums.DataType.Game);

        GameDataController.GetData(searchProperties);

        gameElementData = (GameElementData)GameDataController.Data.dataList.FirstOrDefault();
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
        
        activeGameSaveElementData = GetGameSaveData(saveElementData);
    }

    public GameSaveElementData GetGameSaveData(SaveElementData saveElementData)
    {
        var searchProperties = new SearchProperties(Enums.DataType.GameSave);

        var searchParameters = searchProperties.searchParameters.Cast<Search.GameSave>().First();
        searchParameters.saveId = new List<int>() { saveElementData.Id };

        gameSaveController.GetData(searchProperties);

        var gameSaveData = (GameSaveElementData)gameSaveController.Data.dataList.FirstOrDefault();

        return gameSaveData;
    }

    public void OpenGame()
    {
        //Game is technically opened twice when returning from the data editor
        //The closing of other forms triggers the reaction
        gameWorldController.Display.DataController = gameWorldController;
        
        InitializePhase();

        InitializeLocalNavMesh();

        localNavMeshBuilder.UpdateNavMesh();
        
        TimeManager.active = true;

        TimeManager.instance.SetLighting();
    }

    private void InitializePhase()
    {
        Debug.Log("Determine active phase");

        //1. Find the first chapter that has not been completed
        //2. Find the first phase of that chapter that has not been completed

        var activeChapterSave = activeGameSaveElementData.ChapterSaveDataList.Where(x => !x.Complete).OrderBy(x => x.Index).First();
        var activePhaseSave = activeGameSaveElementData.PhaseSaveDataList.Where(x => x.ChapterId == activeChapterSave.ChapterId && !x.Complete).OrderBy(x => x.Index).First();
        
        ActivePhaseId = activePhaseSave.PhaseId;

        PlayerControlManager.AllowInput = true;
        PlayerControlManager.DisablePlayerMovement = false;
        PlayerControlManager.DisableCameraMovement = false;
    }
    
    private void ChangePhase()
    {
        //Load all game data related to the active phase divided into regions and terrains
        LoadGameData();

        //Set the time speed as a real time multiplier
        SetChapterTimeSpeed();

        //Set the active region based on the save data or else from the phase defaults
        InitializeRegion();

        //Set the active controllable agent based on the save data or else from the phase defaults
        InitializeControllable();

        //Set the time based on the save data or else from the phase defaults, which is already set
        TimeManager.instance.InitializeGameTime(activeGameSaveElementData.SaveData.GameTime);

        //Compare game data with save data to determine which task is active
        CheckProgress();
    }
    
    private void LoadGameData()
    {
        var searchProperties = new SearchProperties(Enums.DataType.GameWorld);

        var searchParameters = searchProperties.searchParameters.Cast<Search.GameWorld>().First();
        searchParameters.phaseId = new List<int>() { ActivePhaseId };

        gameWorldController.GetData(searchProperties);
        
        gameWorldData = gameWorldController.Data.dataList.Cast<GameWorldElementData>().FirstOrDefault();

        SetDataControllers();
    }

    private void SetDataControllers()
    {
        //Objects
        WorldObjectDataController.Data.dataList = gameWorldData.RegionDataList.SelectMany(x => x.GameTerrainDataList.SelectMany(y => y.GameWorldObjectDataList)).Cast<IElementData>().ToList();

        //Interactable agents
        WorldInteractableAgentDataController.Data.dataList = gameWorldData.WorldInteractableDataList.Where(x => x.Type == Enums.InteractableType.Agent).Cast<IElementData>().ToList();

        //Interactable objects
        WorldInteractableObjectDataController.Data.dataList = gameWorldData.WorldInteractableDataList.Where(x => x.Type == Enums.InteractableType.Object).Cast<IElementData>().ToList();

        //Interactable controllables
        WorldInteractableControllableDataController.Data.dataList = gameWorldData.WorldInteractableDataList.Where(x => x.Type == Enums.InteractableType.Controllable).Cast<IElementData>().ToList();

        //Scene props
        ScenePropDataController.Data.dataList = new List<IElementData>();
    }

    public void SetChapterTimeSpeed()
    {
        TimeManager.gameTimeSpeed = gameWorldData.ChapterData.TimeSpeed;
    }

    private void InitializeRegion()
    {
        //Must be a new phase if the saved region is not listed
        if (!gameWorldData.RegionDataList.Select(x => x.Id).Contains(activeGameSaveElementData.SaveData.RegionId))
        {
            activeGameSaveElementData.SaveData.PositionX = gameWorldData.PhaseData.DefaultPositionX;
            activeGameSaveElementData.SaveData.PositionY = gameWorldData.PhaseData.DefaultPositionY;
            activeGameSaveElementData.SaveData.PositionZ = gameWorldData.PhaseData.DefaultPositionZ;

            activeGameSaveElementData.SaveData.GameTime = gameWorldData.PhaseData.DefaultTime;

            ActiveRegionId = gameWorldData.PhaseData.DefaultRegionId;

        } else {

            ActiveRegionId = activeGameSaveElementData.SaveData.RegionId;
        }
    }

    private void InitializeLocalNavMesh()
    {
        localNavMeshBuilder.m_Size = new Vector3(TempActiveRange + (regionData.TileSize * 5), 50, TempActiveRange + (regionData.TileSize * 5));
    }

    private void InitializeControllable()
    {
        //The first controllable agent of a chapter is the default

        //This line doesn't make any sense. Disabled logic for now
        //if (!gameWorldData.RegionDataList.Select(x => x.Id).Contains(gameSaveData.PlayerSaveData.WorldInteractableId))

        //ActiveWorldInteractableControllableId = WorldInteractableControllableDataController.Data.dataList.First().Id;
        ActiveWorldInteractableControllableId = activeGameSaveElementData.SaveData.WorldInteractableId;

        //else
        //ActiveWorldInteractableControllableId = gameSaveData.PlayerSaveData.WorldInteractableId;
    }

    public void ChangeControllable()
    {
        worldInteractableControllableData = gameWorldData.WorldInteractableDataList.Where(x => x.Id == activeGameSaveElementData.SaveData.WorldInteractableId).First();

        PlayerControlManager.instance.SetPlayerCharacter();
    }

    public void CheckProgress()
    {
        //Happens when phase is changed and after every interaction (can hopefully be limited)
        Debug.Log("Check what game data has been completed");

        //Finds the phase save of the active phase
        var activePhaseSave = activeGameSaveElementData.PhaseSaveDataList.Where(x => x.PhaseId == ActivePhaseId).First();

        //Finds the quest saves of the quests belonging to the active phase
        var activeQuestSaves = activeGameSaveElementData.QuestSaveDataList.Where(x => x.PhaseId == activePhaseSave.PhaseId).ToList();

        //Finds the objectives belonging to the quests of the quest saves
        //Groups the objectives per quest and check if there are any uncompleted objectives left
        //Select the first uncompleted objective if there are any, else pick the last objective
        var activeObjectiveSaves = activeGameSaveElementData.ObjectiveSaveDataList.Where(x => activeQuestSaves.Select(y => y.QuestId).Contains(x.QuestId)).OrderBy(x => x.Index)
                                                                     .GroupBy(x => x.QuestId)
                                                                     .Select(x => x.Any(y => !y.Complete) ? x.Where(y => !y.Complete).First() : x.Last()).ToList();

        //Finds the region interactions: interactions that do not belong to an objective
        var regionInteractions = gameWorldData.WorldInteractableDataList.SelectMany(x => x.InteractionDataList.Where(y => y.ObjectiveId == 0)).ToList();

        //Finds the tasks of the region interactions
        var regionTasks = activeGameSaveElementData.TaskSaveDataList.Where(x => regionInteractions.Select(y => y.TaskId).Contains(x.TaskId)).Distinct().ToList();

        //Finds the task saves of the tasks belonging to the objective saves, combined with the region tasks
        //Groups the tasks by world interactable and by objective save, as some world interactables "belong" to multiple objectives and some to none
        //Selects the first uncompleted task if there are any, else pick the last task if it's repeatable
        activeTaskSaveList = activeGameSaveElementData.TaskSaveDataList.Where(x => activeObjectiveSaves.Select(y => y.ObjectiveId).Contains(x.ObjectiveId))
                                                          .Concat(regionTasks)
                                                          .GroupBy(x => new { x.WorldInteractableId, x.ObjectiveId })
                                                          .Select(x => x.ToList().OrderBy(y => y.Index))
                                                          .Select(x => x.Any(y => !y.Complete) ? x.Where(y => !y.Complete).First() : 
                                                                                                 x.Last().Repeatable ? x.Last() : 
                                                                                                                       null).ToList();
        
        activeWorldInteractableList = gameWorldData.WorldInteractableDataList.Where(x => x.InteractionDataList.Any(y => activeTaskSaveList.Select(z => z.TaskId).Contains(y.TaskId))).ToList();
        
        GetInteractionTimeEvents();

        gameWorldData.WorldInteractableDataList.ForEach(x => ResetInteractable(x));

        //Only interactacbles which have multiple destinations or those which are bound to the active region are being moved
        MovementManager.movableWorldInteractableList = activeWorldInteractableList.Where(x => x.ActiveInteraction.InteractionDestinationDataList.Count > 1 || 
                                                                                             (x.ActiveInteraction.ActiveDestination.RegionId == ActiveRegionId && x.ActiveInteraction.ActiveDestination.PositionVariance > 0)).ToList();
    }

    private void ChangeRegion()
    {
        Debug.Log("The region was changed, so the organizer should update itself");
        
        //PlayerControlManager.Enabled = false;

        regionData = gameWorldData.RegionDataList.Where(x => x.Id == ActiveRegionId).First();

        //Something should happen here to reset the world when the region is changed manually
        //CheckTime();
    }
    
    private void GetInteractionTimeEvents()
    {
        TimeManager.timeEventList = new List<TimeManager.TimeEvent>();     
        activeWorldInteractableList.ForEach(x => TimeManager.AddTimeEvent(x));
    }

    public void ResetInteractable(GameWorldInteractableElementData worldInteractableElementData)
    {
        //Deactivate all interactions of the world interactable
        DeactivateInteraction(worldInteractableElementData);

        //Check which of their interaction contains the active time
        ValidateInteractionTime(worldInteractableElementData);

        //Update the world interactables based on the new active time
        UpdateWorldInteractable(worldInteractableElementData);
    }
    
    private void DeactivateInteraction(GameWorldInteractableElementData worldInteractableElementData)
    {
        if (worldInteractableElementData.InteractionDataList.Count == 0) return;

        worldInteractableElementData.InteractionDataList.ForEach(x =>
        {
            x.ContainsActiveTime = false;
        });
    }

    private void ValidateInteractionTime(GameWorldInteractableElementData worldInteractableElementData)
    {
        if (worldInteractableElementData.InteractionDataList.Count == 0) return;

        //Validate times of interactions which belong to the active task saves
        worldInteractableElementData.InteractionDataList.GroupBy(y => y.TaskId)
                                                        .Select(y => y.Where(z => TimeManager.TimeInFrame(TimeManager.instance.ActiveTime, z.StartTime, z.EndTime) || z.Default)
                                                        .OrderBy(z => z.Default).First()).ToList()
                                                        .ForEach(x =>
                                                        {
                                                            x.ContainsActiveTime = true;
                                                        });

        worldInteractableElementData.ActiveInteraction = worldInteractableElementData.InteractionDataList.Where(y => y.ContainsActiveTime).First();
    }

    public void UpdateSceneProp(GameScenePropElementData gameScenePropElementData)
    {
        Organizer.UpdateSceneProp(gameScenePropElementData);
    }

    public void CloseSceneProp(GameScenePropElementData gameScenePropElementData)
    {
        Organizer.CloseSceneProp(gameScenePropElementData);
    }

    public void UpdateWorldInteractable(GameWorldInteractableElementData worldInteractableElementData)
    {
        Organizer.UpdateWorldInteractable(worldInteractableElementData);
    }
    
    public void SaveData(SaveElementData saveElementData)
    {
        activeGameSaveElementData.SaveData.SaveTime = System.DateTime.Now;
        
        //Create new save data if execute type is add
        if (saveElementData.ExecuteType == Enums.ExecuteType.Add)
        {
            AddSaveData(saveElementData);
        }

        UpdateSaveData(saveElementData);
    }

    private void AddSaveData(SaveElementData saveElementData)
    {
        var dataRequest = new DataRequest() { requestType = Enums.RequestType.Execute };
        
        saveElementData.Add(dataRequest);

        saveElementData.SetOriginalValues();
    }

    private void UpdateSaveData(SaveElementData saveElementData)
    {
        var dataRequest = new DataRequest() { requestType = Enums.RequestType.Execute };

        //Apply changes from the active game save to the selected
        var gameSaveElementData = GetGameSaveData(saveElementData);
        
        //Save
        var saveDataSource = activeGameSaveElementData.SaveData;

        gameSaveElementData.SaveData.RegionId = saveDataSource.RegionId;
        gameSaveElementData.SaveData.WorldInteractableId = saveDataSource.WorldInteractableId;

        gameSaveElementData.SaveData.PositionX = saveDataSource.PositionX;
        gameSaveElementData.SaveData.PositionY = saveDataSource.PositionY;
        gameSaveElementData.SaveData.PositionZ = saveDataSource.PositionZ;

        gameSaveElementData.SaveData.GameTime = saveDataSource.GameTime;

        gameSaveElementData.SaveData.PlayTime = saveDataSource.PlayTime;
        gameSaveElementData.SaveData.SaveTime = saveDataSource.SaveTime;

        //Chapter
        gameSaveElementData.ChapterSaveDataList.ForEach(chapterSave =>
        {
            var chapterSaveDataSource = activeGameSaveElementData.ChapterSaveDataList.Where(chapterSaveSource => chapterSaveSource.ChapterId == chapterSave.ChapterId).First();

            chapterSave.Complete = chapterSaveDataSource.Complete;
        });

        //Phase
        gameSaveElementData.PhaseSaveDataList.ForEach(phaseSave =>
        {
            var phaseSaveDataSource = activeGameSaveElementData.PhaseSaveDataList.Where(phaseSaveSource => phaseSaveSource.PhaseId == phaseSave.PhaseId).First();

            phaseSave.Complete = phaseSaveDataSource.Complete;
        });

        //Quest
        gameSaveElementData.QuestSaveDataList.ForEach(questSave =>
        {
            var questSaveDataSource = activeGameSaveElementData.QuestSaveDataList.Where(questSaveSource => questSaveSource.QuestId == questSave.QuestId).First();

            questSave.Complete = questSaveDataSource.Complete;
        });

        //Objective
        gameSaveElementData.ObjectiveSaveDataList.ForEach(objectiveSave =>
        {
            var objectiveSaveDataSource = activeGameSaveElementData.ObjectiveSaveDataList.Where(objectiveSaveSource => objectiveSaveSource.ObjectiveId == objectiveSave.ObjectiveId).First();

            objectiveSave.Complete = objectiveSaveDataSource.Complete;
        });

        //Task
        gameSaveElementData.TaskSaveDataList.ForEach(taskSave =>
        {
            var taskSaveDataSource = activeGameSaveElementData.TaskSaveDataList.Where(taskSaveSource => taskSaveSource.TaskId == taskSave.TaskId).First();

            taskSave.Complete = taskSaveDataSource.Complete;
        });

        //Interaction
        gameSaveElementData.InteractionSaveDataList.ForEach(interactionSave =>
        {
            var interactionSaveDataSource = activeGameSaveElementData.InteractionSaveDataList.Where(interactionSaveSource => interactionSaveSource.InteractionId == interactionSave.InteractionId).First();

            interactionSave.Complete = interactionSaveDataSource.Complete;
        });

        gameSaveElementData.Update(dataRequest);
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
        activeWorldInteractableControllableId = 0;

        gameWorldData = null;
        regionData = null;
        worldInteractableControllableData = null;
        
        gameWorldController.Data = null;
        gameSaveController.Data = null;

        TimeManager.active = false;
        TimeManager.instance.TimeScale = 1;

        InteractionManager.interactionTarget = null;
        InteractionManager.activeOutcome = null;

        ScenarioManager.allowContinue = false;
        ScenarioManager.instance.StopAllCoroutines();
        
        PlayerControlManager.AllowInput = false;
    }
}