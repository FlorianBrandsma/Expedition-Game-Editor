using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameManager : MonoBehaviour
{
    static public GameManager instance;
    
    public GameSaveElementData gameSaveData;
    public GameWorldElementData gameWorldData;

    public GameSaveController gameSaveController;
    public GameWorldController gameWorldController;

    public LocalNavMeshBuilder localNavMeshBuilder;

    public Light gameLight;

    private int activePhaseId;
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

    private int activeRegionId;
    public int ActiveRegionId
    {
        get { return activeRegionId; }
        set
        {
            if (activeRegionId != value)
            {
                activeRegionId = value;

                ChangeRegion();
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
            GlobalManager.programType = GlobalManager.Scenes.Game;

            GlobalManager.OpenScene(GlobalManager.Scenes.Global);

            return;
        }
    }

    private void Start()
    {
        RenderManager.Render(new PathManager.Game().Initialize());
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.P))
            ActivePhaseId++;

        if (Input.GetKeyUp(KeyCode.R))
            ActiveRegionId++;

        if (Input.GetKeyUp(KeyCode.I))
            InteractionTest();

        if (Input.GetKeyUp(KeyCode.T))
            TimeTest();
    }

    private void InteractionTest()
    {
        Debug.Log("This is an interaction test");

        //After the interaction, relevant save data is updated

        CheckCompletion();

        //Completion of an interaction may cause a phase to change
        ActivePhaseId++;

        //The following function should only be performed if the phase hasn't changed
        //In this test, it always changes
        //CheckTime();
    }

    private void TimeTest()
    {
        if (TimeManager.activeTime < TimeManager.hoursInDay - 1)
            TimeManager.activeTime++;
        else
            TimeManager.activeTime = 0;

        TimeManager.instance.SetTime(TimeManager.activeTime);
        TimeManager.instance.SetCameraLight(gameLight);

        Debug.Log("The time is " + TimeManager.activeTime + ":00");

        CheckTime();
    }

    public void LoadGameSaveData(SaveElementData saveElementData)
    {
        Debug.Log("Get save data");

        Debug.Log("GET DATA FROM SAVE!");
        Debug.Log("USE PHASE DEFAULTS IF PLAYER SAVE DATA DOES NOT 'WORK'");

        activePhaseId = 0;
        activeRegionId = 0;

        //Get save data
        var searchProperties = new SearchProperties(Enums.DataType.GameSave);

        var searchParameters = searchProperties.searchParameters.Cast<Search.GameSave>().First();
        searchParameters.saveId = new List<int>() { saveElementData.Id };

        gameSaveController.DataList = RenderManager.GetData(gameSaveController, searchProperties);

        gameSaveData = gameSaveController.DataList.Cast<GameSaveElementData>().FirstOrDefault();
    }

    public void OpenGame()
    {
        //Game is technically opened twice when returning from the data editor
        //The closing of other forms triggers the reaction
        gameWorldController.Display.DataController = gameWorldController;

        DetermineActivePhase();

        localNavMeshBuilder.UpdateNavMesh(true);

        TimeManager.instance.SetLighting();
    }

    private void DetermineActivePhase()
    {
        Debug.Log("Determine active phase");

        //1. Find the first chapter that has not been completed
        //2. Find the first phase of that chapter that has not been completed

        var activeChapterSave = gameSaveData.chapterSaveDataList.Where(x => !x.Complete).OrderBy(x => x.Index).First();
        var activePhaseSave = gameSaveData.phaseSaveDataList.Where(x => x.ChapterSaveId == activeChapterSave.Id && !x.Complete).OrderBy(x => x.Index).First();

        ActivePhaseId = activePhaseSave.PhaseId;
    }

    private void ChangePhase()
    {
        //Load all game data related to the active phase divided into regions and terrains
        LoadGameData();

        //Compare game data with save data to determine which task is active
        CheckCompletion();

        //By now you should know what region is active, either from player save or from the phase
        //The spawn position for each phase should be given in the editor
        
        var regionData = gameWorldData.regionDataList.Where(x => x.PhaseId == ActivePhaseId).First();

        ActiveRegionId = regionData.Id;
    }

    private void LoadGameData()
    {
        var searchProperties = new SearchProperties(Enums.DataType.GameWorld);

        var searchParameters = searchProperties.searchParameters.Cast<Search.GameWorld>().First();
        searchParameters.phaseId = new List<int>() { ActivePhaseId };

        gameWorldController.DataList = RenderManager.GetData(gameWorldController, searchProperties);

        gameWorldData = gameWorldController.DataList.Cast<GameWorldElementData>().FirstOrDefault();
    }

    private void CheckCompletion()
    {
        //Happens when phase is changed and after every interaction (can hopefully be limited)
        Debug.Log("Check what game data has been completed");

        //Finds the phase save of the active phase
        var activePhaseSave = gameSaveData.phaseSaveDataList.Where(x => x.PhaseId == ActivePhaseId).First();

        //Finds the quest saves of the quests belonging to the active phase
        var activeQuestSaves = gameSaveData.questSaveDataList.Where(x => x.PhaseSaveId == activePhaseSave.Id).ToList();

        //Finds the objectives belonging to the quests of the quest saves
        //Groups the objectives per quest and check if there are any uncompleted objectives left
        //Select the first uncompleted objective if there are any, else pick the last objective
        var activeObjectiveSaves = gameSaveData.objectiveSaveDataList.Where(x => activeQuestSaves.Select(y => y.Id).Contains(x.QuestSaveId)).OrderBy(x => x.Index)
                                                                     .GroupBy(x => x.QuestSaveId)
                                                                     .Select(x => x.Any(y => !y.Complete) ? x.Where(y => !y.Complete).First() : x.Last()).ToList();

        //Finds the region interactions: interactions that do not belong to an objective
        var regionInteractions = gameWorldData.worldInteractableDataList.SelectMany(x => x.interactionDataList.Where(y => y.objectiveId == 0 && gameWorldData.regionDataList.Select(z => z.Id).Contains(y.RegionId))).ToList();

        //Finds the tasks of the region interactions
        var regionTasks = gameSaveData.taskSaveDataList.Where(x => regionInteractions.Select(y => y.TaskId).Contains(x.TaskId)).Distinct().ToList();

        //Finds the task saves of the tasks belonging to the objective saves, combined with the region tasks
        //Groups the tasks by world interactable and by objective save, as some world interactables "belong" to multiple objectives and some to none
        //Selects the first uncompleted task if there are any, else pick the last task if it's repeatable
        activeTaskSaveList = gameSaveData.taskSaveDataList.Where(x => activeObjectiveSaves.Select(y => y.Id).Contains(x.ObjectiveSaveId))
                                                          .Concat(regionTasks)
                                                          .GroupBy(x => new { x.WorldInteractableId, x.ObjectiveSaveId })
                                                          .Select(x => x.ToList().OrderBy(y => y.Index))
                                                          .Select(x => x.Any(y => !y.Complete) ? x.Where(y => !y.Complete).First() : 
                                                                                                 x.Last().repeatable ? x.Last() : 
                                                                                                                       null).ToList();

        CheckTime();
    }

    private void ChangeRegion()
    {
        Debug.Log("The region was changed, so the organizer should update itself");

        var regionData = gameWorldData.regionDataList.Where(x => x.PhaseId == ActivePhaseId).First();

        

        //var tempWorldSize = regionData.RegionSize * regionData.TerrainSize * regionData.tileSize;
        //gameWorldData.tempPlayerPosition = new Vector3(238.125f, -241.9375f, 0);
        //-238.125f -13.8125
        //UpdateWorld();
        
        CheckTime();
    }
    
    public void CheckTime()
    {
        Debug.Log("Check time to see which interactions and atmospheres are active");
        
        DeactivateInteractionTime();

        //For every interactable, check which of their interactions contains the active time
        ValidateInteractionTime();
        
        //Update all world interactables based on the new active times
        UpdateWorldInteractables();

        UpdateWorld();
    }

    private void UpdateWorld()
    {
        gameWorldController.Display.DisplayManager.Organizer.UpdateData();
    }

    private void DeactivateInteractionTime()
    {
        gameWorldData.worldInteractableDataList.SelectMany(x => x.interactionDataList).ToList().ForEach(y => 
        {
            y.containsActiveTime = false;
        });
    }

    private void ValidateInteractionTime()
    {
        //Validate times of interactions which belong to the active task saves
        gameWorldData.worldInteractableDataList.SelectMany(x => x.interactionDataList.Where(y => activeTaskSaveList.Select(z => z.TaskId).Contains(y.TaskId)).GroupBy(y => y.TaskId)
                                                                 .Select(y => y.Where(z => TimeManager.TimeInFrame(TimeManager.activeTime, z.StartTime, z.EndTime) || z.Default)
                                                                 .OrderBy(z => z.Default).First())).ToList()
                                                                 .ForEach(x => 
                                                                 {
                                                                     x.containsActiveTime = true;
                                                                 });

        //Debug.Log(gameWorldData.worldInteractableDataList.Sum(x => x.interactionDataList.Where(y => y.containsActiveTime).ToList().Count));
    }

    private void UpdateWorldInteractables()
    {
        gameWorldData.worldInteractableDataList.ForEach(x => UpdateWorldInteractable(x));
    }

    private void UpdateWorldInteractable(WorldInteractableElementData worldInteractableData)
    {
        var interactionData = worldInteractableData.interactionDataList.Where(x => x.containsActiveTime).FirstOrDefault();
        
        if(worldInteractableData.DataElement != null)
        {
            //If the active world interactable contains no active time, deactivate it
            if (interactionData == null)
            {
                worldInteractableData.terrainTileId = 0;

            } else {

                switch((Enums.InteractableType)worldInteractableData.Type)
                {
                    case Enums.InteractableType.Agent:

                        //If the active world interactable (agent) contains active time, update the transform
                        worldInteractableData.DataElement.Element.UpdateElement();
                        break;

                    case Enums.InteractableType.Object:

                        //World interactable objects can relocate instantly by changing the terrain tile id
                        worldInteractableData.terrainTileId = interactionData.TerrainTileId;
                        break;
                }
            }

        } else if (interactionData != null) {

            //If the inactive world interactable contains an active time, activate it
            worldInteractableData.terrainTileId = interactionData.TerrainTileId;
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
}