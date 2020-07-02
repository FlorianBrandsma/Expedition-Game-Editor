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
        Debug.Log("The clock is ticking...");

        TimeManager.activeTime++;

        CheckTime();
    }

    public void LoadGameSaveData(SaveElementData saveElementData)
    {
        Debug.Log("Get save data");

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
    }

    private void DetermineActivePhase()
    {
        Debug.Log("Determine active phase");

        //1. Find the first chapter that has not been completed
        //2. Find the first phase of that chapter that has not been completed
        //(Might have to take chapter and phase index into consideration)

        var activeChapterSave = gameSaveData.chapterSaveDataList.Where(x => !x.Complete).First();
        var activePhaseSave = gameSaveData.phaseSaveDataList.Where(x => x.ChapterSaveId == activeChapterSave.Id && !x.Complete).First();

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

        var activePhaseSave         = gameSaveData.phaseSaveDataList.Where(x => x.PhaseId == ActivePhaseId).First();

        var activeQuestSaves        = gameSaveData.questSaveDataList.Where(x => x.PhaseSaveId == activePhaseSave.Id).ToList();

        var activeObjectiveSaves    = gameSaveData.objectiveSaveDataList.Where(x => activeQuestSaves.Select(y => y.Id)
                                                                        .Contains(x.QuestSaveId) && !x.Complete)
                                                                        .GroupBy(x => x.QuestSaveId)
                                                                        .Select(x => x.First()).ToList();

        var regionInteractions      = gameWorldData.worldInteractableDataList.SelectMany(x => x.interactionDataList.Where(y => y.objectiveId == 0 && gameWorldData.regionDataList.Select(z => z.Id).Contains(y.RegionId))).ToList();
        var worldTasks              = gameSaveData.taskSaveDataList.Where(x => regionInteractions.Select(y => y.TaskId).Contains(x.TaskId)).ToList();

        var activeTaskSaves         = gameSaveData.taskSaveDataList.Where(x => activeObjectiveSaves.Select(y => y.Id).Contains(x.ObjectiveSaveId))
                                                                   .Concat(worldTasks)
                                                                   .GroupBy(x => new { x.WorldInteractableId, x.ObjectiveSaveId })
                                                                   .Select(x => x.ToList())
                                                                   .Select(x => x.Where(y => !y.Complete).First()).ToList();

        Debug.Log(activeTaskSaves.Count);


        //Spawn interactables: only those belonging to active tasks



        //1. Find all quests that have not been completed which belong to the active phase
        //2. For each quest, find the first objective which has not been completed
        //3. For every interactable, find the first task which has not been completed

        //Result: only show interactions of the first task that has not been completed
    }

    private void ChangeRegion()
    {
        Debug.Log("The region was changed, so the organizer should update itself");

        var regionData = gameWorldData.regionDataList.Where(x => x.PhaseId == ActivePhaseId).First();

        var tempWorldSize = regionData.RegionSize * regionData.TerrainSize * regionData.tileSize;
        gameWorldData.tempPlayerPosition = new Vector3(238.125f, -241.9375f, 0);
        //-238.125f -13.8125
        UpdateWorld();
        
        CheckTime();
    }

    private void UpdateWorld()
    {
        gameWorldController.Display.DisplayManager.Organizer.UpdateData();
    }

    public void CheckTime()
    {
        Debug.Log("Check time to see which interactions and atmospheres are active");

        //For every interactable, check which of their interactions contains the active time
        ValidateInteractionTime();

        UpdateWorld();
    }

    private void ValidateInteractionTime()
    {
        gameWorldData.worldInteractableDataList.SelectMany(x => x.interactionDataList.GroupBy(y => y.TaskId)
                                                                 .Select(y => y.Where(z => TimeManager.TimeInFrame(TimeManager.activeTime, z.StartTime, z.EndTime) || z.Default)
                                                                 .OrderBy(z => z.Default).First())).ToList()
                                                                 .ForEach(x => 
                                                                 {
                                                                     x.containsActiveTime = true;
                                                                     SetActiveInteraction(x);
                                                                 });
    }

    private void SetActiveInteraction(InteractionElementData interactionData)
    {
        var worldInteractableData = gameWorldData.worldInteractableDataList.Where(x => x.Id == interactionData.worldInteractableId).First();
        
        //If the world interactable is not active and the region is the active region, set the terrain tile id and transform
        if(worldInteractableData.DataElement == null)
        {
            worldInteractableData.terrainTileId = interactionData.TerrainTileId;

        } else if(worldInteractableData.Type == (int)Enums.InteractableType.Agent) {

            worldInteractableData.DataElement.Element.UpdateElement();
        }

        //If the world interactable is an active agent and the region is the active region, only set the destination
        
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