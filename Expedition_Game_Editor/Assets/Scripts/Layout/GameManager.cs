using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameManager : MonoBehaviour
{
    static public GameManager instance;
    
    public GameSaveDataElement gameSaveData;
    public GameWorldDataElement gameWorldData;

    public GameSaveController gameSaveController;
    public GameWorldController gameWorldController;

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

        CheckTime();
    }

    public void LoadGameSaveData(SaveDataElement saveDataElement)
    {
        Debug.Log("Get save data");

        activePhaseId = 0;
        activeRegionId = 0;

        //Get save data
        var searchProperties = new SearchProperties(Enums.DataType.GameSave);

        var searchParameters = searchProperties.searchParameters.Cast<Search.GameSave>().First();
        searchParameters.saveId = new List<int>() { saveDataElement.Id };

        gameSaveController.DataList = RenderManager.GetData(gameSaveController, searchProperties);

        gameSaveData = gameSaveController.DataList.Cast<GameSaveDataElement>().FirstOrDefault();
    }

    public void OpenGame()
    {
        //Game is technically opened twice when returning from the data editor
        //The closing of other forms triggers the reaction
        gameWorldController.Display.DataController = gameWorldController;

        DetermineActivePhase();
    }

    private void DetermineActivePhase()
    {
        Debug.Log("Determine active phase");

        //Find the first unfinished phase of the first unfinished chapter
        //Might have to take chapter and phase index into consideration
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

        gameWorldData = gameWorldController.DataList.Cast<GameWorldDataElement>().FirstOrDefault();
    }

    private void CheckCompletion()
    {
        //Happens when phase is changed and after every interaction (can hopefully be limited)
        Debug.Log("Check what game data has been completed");
    }

    private void ChangeRegion()
    {
        Debug.Log("The region was changed, so the organizer should update itself");

        var regionData = gameWorldData.regionDataList.Where(x => x.PhaseId == ActivePhaseId).First();

        var tempWorldSize = regionData.RegionSize * regionData.TerrainSize * regionData.tileSize;
        gameWorldData.tempPlayerPosition = new Vector3(238.125f, -238.125f, 0);

        gameWorldController.Display.DisplayManager.Organizer.UpdateData();

        CheckTime();
    }

    public void CheckTime()
    {
        Debug.Log("Check time to see which interactions and atmospheres are active");
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