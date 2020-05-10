using UnityEngine;
using System.Collections.Generic;
using System.Linq;

static public class Fixtures
{
    static public int timeFrames = 2;
 
    static public int supplies = 4;
    static public int gear = 6;
    static public int spoils = 10;
    static public int interactables = 6;
    static public int objectGraphics = 15;
    static public int icons = 9;
    static public int regions = 2;
    static public int terrainsInRegions = 3;
    static public int terrainTilesInTerrains = 5;
    static public int chapters = 3;
    static public int partyMembersInChapter = 1;
    static public int chapterInteractablesInChapter = 3;
    static public int phasesInChapter = 4;
    static public int questsInPhase = 4;
    static public int objectivesInQuest = 3;
    static public int interactablesInObjective = 2;
    static public int sideWorldInteractables = 1;
    static public int interactionsPerWorldInteractable = 2;
    static public int baseTasks = 2;
    static public int tileSets = 2;
    static public int tilesInTileSet = 10;
    static public int objectsInWorld = 3;

    static public int gameSaves = 1;

    static public List<Icon>                iconList                = new List<Icon>();
    static public List<ObjectGraphic>       objectGraphicList       = new List<ObjectGraphic>();
    static public List<Item>                itemList                = new List<Item>();
    static public List<Interactable>        interactableList        = new List<Interactable>();
    static public List<TileSet>             tileSetList             = new List<TileSet>();
    static public List<Tile>                tileList                = new List<Tile>();
    static public List<Region>              regionList              = new List<Region>();
    static public List<Atmosphere>          atmosphereList          = new List<Atmosphere>();
    static public List<WorldObject>         worldObjectList         = new List<WorldObject>();
    static public List<Terrain>             terrainList             = new List<Terrain>();
    static public List<TerrainTile>         terrainTileList         = new List<TerrainTile>();
    static public List<Chapter>             chapterList             = new List<Chapter>();
    static public List<PartyMember>         partyMemberList         = new List<PartyMember>();
    static public List<ChapterInteractable> chapterInteractableList = new List<ChapterInteractable>();
    static public List<ChapterRegion>       chapterRegionList       = new List<ChapterRegion>();
    static public List<Phase>               phaseList               = new List<Phase>();
    static public List<PhaseInteractable>   phaseInteractableList   = new List<PhaseInteractable>();
    static public List<Quest>               questList               = new List<Quest>();
    static public List<Objective>           objectiveList           = new List<Objective>();
    static public List<WorldInteractable>   worldInteractableList   = new List<WorldInteractable>();
    static public List<Task>                taskList                = new List<Task>();
    static public List<Interaction>         interactionList         = new List<Interaction>();
    static public List<Outcome>             outcomeList             = new List<Outcome>();

    static public List<GameSave>            gameSaveList            = new List<GameSave>();
    static public List<ChapterSave>         chapterSaveList         = new List<ChapterSave>();
    static public List<PhaseSave>           phaseSaveList           = new List<PhaseSave>();
    static public List<QuestSave>           questSaveList           = new List<QuestSave>();
    static public List<ObjectiveSave>       objectiveSaveList       = new List<ObjectiveSave>();
    static public List<TaskSave>            taskSaveList            = new List<TaskSave>();
    static public List<InteractionSave>     interactionSaveList     = new List<InteractionSave>();

    public class Item : GeneralData
    {
        public int type;

        public int objectGraphicId;

        public string name;
    }

    public class Interactable : GeneralData
    {
        public int type;

        public int objectGraphicId;
        
        public string name;
    }
    
    public class WorldObject : GeneralData
    {
        public int objectGraphicId;
        public int regionId;
        public int terrainId;
        public int terrainTileId;

        public float positionX;
        public float positionY;
        public float positionZ;

        public int rotationX;
        public int rotationY;
        public int rotationZ;

        public float scaleMultiplier;

        public int animation;
    }
    
    public class ObjectGraphic : GeneralData
    {
        public int iconId;

        public string name;
        public string path;

        public float height;
        public float width;
        public float depth;
    }

    public class Icon : GeneralData
    {
        public int category;
        public string path;
    }

    public class TileSet : GeneralData
    {
        public string name;
        public float tileSize;
    }

    public class Tile : GeneralData
    {
        public int tileSetId;
        public string iconPath;
    }
    
    public class Region : GeneralData
    {
        public int chapterRegionId;
        public int phaseId;
        public int tileSetId;

        public string name;
        public int regionSize;
        public int terrainSize;
    }

    public class Atmosphere : GeneralData
    {
        public int terrainId;

        public bool isDefault;

        public int startTime;
        public int endTime;

        public string publicNotes;
        public string privateNotes;
    }

    public class Terrain : GeneralData
    {
        public int regionId;
        public int iconId;

        public string name;
    }

    public class TerrainTile : GeneralData
    {
        public int terrainId;
        public int tileId;
    }

    public class Chapter : GeneralData
    {
        public string name;

        public string publicNotes;
        public string privateNotes;
    }

    public class PartyMember : GeneralData
    {
        public int chapterId;
        public int interactableId;
    }

    public class ChapterInteractable : GeneralData
    {
        public int chapterId;
        public int interactableId;
    }

    public class ChapterRegion : GeneralData
    {
        public int chapterId;
        public int regionId;
    }

    public class Phase : GeneralData
    {
        public int chapterId;

        public string name;

        public string publicNotes;
        public string privateNotes;
    }

    public class PhaseInteractable : GeneralData
    {
        public int phaseId;
        public int chapterInteractableId;
        public int questId;      
    }

    public class Quest : GeneralData
    {
        public int phaseId;

        public string name;

        public string publicNotes;
        public string privateNotes;
    }

    public class Objective : GeneralData
    {
        public int questId;

        public string name;
        public string journal;

        public string publicNotes;
        public string privateNotes;
    }

    public class WorldInteractable : GeneralData
    {
        public int type;

        public int objectiveId;
        public int interactableId;

        public bool isDefault;

        public int interactionIndex;
    }

    public class Task : GeneralData
    {
        public int worldInteractableId;
        public int objectiveId;

        public string name;
        public string publicNotes;
        public string privateNotes;
    }

    public class Interaction : GeneralData
    {
        public int taskId;

        public int regionId;
        public int terrainId;
        public int terrainTileId;

        public bool isDefault;

        public int startTime;
        public int endTime;

        public string publicNotes;
        public string privateNotes;

        public float positionX;
        public float positionY;
        public float positionZ;

        public int rotationX;
        public int rotationY;
        public int rotationZ;

        public float scaleMultiplier;

        public int animation;
    }

    public class Outcome : GeneralData
    {
        public int type;

        public int interactionId;
    }


    #region Save data

    public class Game : GeneralData
    {
        public string name;
    }

    public class GameSave : GeneralData
    {
        public int gameId;
    }

    public class ChapterSave : GeneralData
    {
        public int gameSaveId;
        public int chapterId;

        public bool complete;
    }

    public class PhaseSave : GeneralData
    {
        public int chapterSaveId;
        public int phaseId;

        public bool complete;
    }

    public class QuestSave : GeneralData
    {
        public int phaseSaveId;
        public int questId;

        public bool complete;
    }

    public class ObjectiveSave : GeneralData
    {
        public int questSaveId;
        public int objectiveId;

        public bool complete;
    }

    public class TaskSave : GeneralData
    {
        public int worldInteractableId;
        public int objectiveSaveId;
        public int taskId;

        public bool complete;
    }

    public class InteractionSave : GeneralData
    {
        public int taskSaveId;
        public int interactionId;

        public bool complete;
    }

    #endregion


    static public void LoadFixtures()
    {
        LoadIcons();
        LoadObjectGraphics();
        LoadTileSets();
        LoadTiles();
        LoadItems();
        LoadInteractableCharacters();
        LoadInteractableObjects();
        LoadRegions();
        LoadTerrains();
        LoadTerrainTiles();
        LoadWorldObjects();
        LoadChapters();
        LoadChapterPartyMembers();
        LoadChapterInteractables();
        LoadChapterRegions();
        LoadPhases();
        LoadPhaseRegions();
        LoadQuests();
        LoadPhaseInteractables();
        LoadObjectives();
        LoadObjectiveWorldInteractables();
        LoadTasks();

        Query();
    }

    #region Icons
    static public void LoadIcons()
    {
        /*01*/CreateIcon("Textures/Icons/Objects/Nothing",          Enums.IconCategory.Nothing);
        /*02*/CreateIcon("Textures/Icons/Objects/Polearm",          Enums.IconCategory.Polearm);
        /*03*/CreateIcon("Textures/Icons/Objects/MightyPolearm",    Enums.IconCategory.Polearm);
        /*04*/CreateIcon("Textures/Icons/Objects/Shortbow",         Enums.IconCategory.Bow);
        /*05*/CreateIcon("Textures/Icons/Objects/Longbow",          Enums.IconCategory.Bow);
        /*06*/CreateIcon("Textures/Icons/Objects/Crossbow",         Enums.IconCategory.Crossbow);
        /*07*/CreateIcon("Textures/Icons/Objects/StrongCrossbow",   Enums.IconCategory.Crossbow);
        /*08*/CreateIcon("Textures/Icons/Objects/Staff",            Enums.IconCategory.Staff);
        /*09*/CreateIcon("Textures/Icons/Objects/MenacingStaff",    Enums.IconCategory.Staff);
        /*10*/CreateIcon("Textures/Icons/Objects/RedWarrior",       Enums.IconCategory.Humanoid);
        /*11*/CreateIcon("Textures/Icons/Objects/BlueWarrior",      Enums.IconCategory.Humanoid);
        /*12*/CreateIcon("Textures/Icons/Objects/GreenWarrior",     Enums.IconCategory.Humanoid);
        /*13*/CreateIcon("Textures/Icons/Objects/Ranger",           Enums.IconCategory.Humanoid);
        /*14*/CreateIcon("Textures/Icons/Objects/Mage",             Enums.IconCategory.Humanoid);
        /*15*/CreateIcon("Textures/Icons/Objects/Drake",            Enums.IconCategory.Dragonkin);
        /*16*/CreateIcon("Textures/Icons/Objects/Goblin",           Enums.IconCategory.Goblin);
        /*17*/CreateIcon("Textures/Icons/Objects/Skull",            Enums.IconCategory.Environment);
        /*18*/CreateIcon("Textures/Icons/Objects/Rock",             Enums.IconCategory.Environment);
        /*19*/CreateIcon("Textures/Icons/Objects/Cactus",           Enums.IconCategory.Environment);
        /*20*/CreateIcon("Textures/Icons/Objects/Tree",             Enums.IconCategory.Environment);
        /*21*/CreateIcon("Textures/Icons/Objects/Pool",             Enums.IconCategory.Environment);
    }

    static public int CreateIcon(string path, Enums.IconCategory category)
    {
        var icon = new Icon();

        int id = iconList.Count > 0 ? (iconList[iconList.Count - 1].Id + 1) : 1;

        icon.Id = id;
        icon.category = (int)category;
        icon.path = path;

        iconList.Add(icon);

        return id;
    }
    #endregion

    #region ObjectGraphics
    static public void LoadObjectGraphics()
    {
        //Note: size values are not realistic until models have been made with these values in mind

        /*01*/CreateObjectGraphic("Nothing",        1,  new Vector3(1,      1,      1));
        /*02*/CreateObjectGraphic("Polearm",        2,  new Vector3(0.5f,   0.15f,  4));
        /*03*/CreateObjectGraphic("Mighty Polearm", 3,  new Vector3(0.65f,  0.15f,  4));
        /*04*/CreateObjectGraphic("Shortbow",       4,  new Vector3(0.5f,   0.1f,   2.75f));
        /*05*/CreateObjectGraphic("Longbow",        5,  new Vector3(1,      0.25f,  3.5f));
        /*06*/CreateObjectGraphic("Crossbow",       6,  new Vector3(1,      0.35f,  1.25f));
        /*07*/CreateObjectGraphic("Strong Crossbow",7,  new Vector3(1.35f,  0.35f,  1.25f));
        /*08*/CreateObjectGraphic("Staff",          8,  new Vector3(0.55f,  0.25f,  2.5f));
        /*09*/CreateObjectGraphic("Menacing Staff", 9,  new Vector3(1.5f,   0.3f,   2.3f));
        /*10*/CreateObjectGraphic("Red Warrior",    10, new Vector3(1,      1,      3.5f));
        /*11*/CreateObjectGraphic("Blue Warrior",   11, new Vector3(1,      1,      3.5f));
        /*12*/CreateObjectGraphic("Green Warrior",  12, new Vector3(1,      1,      3.5f));
        /*13*/CreateObjectGraphic("Ranger",         13, new Vector3(1,      1,      3.3f));
        /*14*/CreateObjectGraphic("Mage",           14, new Vector3(1,      1,      3.3f));
        /*15*/CreateObjectGraphic("Drake",          15, new Vector3(1.5f,   3,      2));
        /*16*/CreateObjectGraphic("Skull",          17, new Vector3(1.25f,  4.5f,   1.5f));
        /*17*/CreateObjectGraphic("Rock",           18, new Vector3(4,      3,      2));
        /*18*/CreateObjectGraphic("Cactus",         19, new Vector3(1,      1,      4));
        /*19*/CreateObjectGraphic("Tree",           20, new Vector3(1,      1,      4));
        /*20*/CreateObjectGraphic("Pool",           21, new Vector3(2.75f,  2.5f,   0.75f));
    }

    static public void CreateObjectGraphic(string name, int iconId, Vector3 size)
    {
        var objectGraphic = new ObjectGraphic();

        int id = objectGraphicList.Count > 0 ? (objectGraphicList[objectGraphicList.Count - 1].Id + 1) : 1;

        objectGraphic.Id = id;
        objectGraphic.iconId = iconId;
        objectGraphic.name = name;
        objectGraphic.path = "Objects/" + name;
        objectGraphic.height = size.z;
        objectGraphic.width = size.x;
        objectGraphic.depth = size.y;

        objectGraphicList.Add(objectGraphic);
    }
    #endregion

    static public void LoadTileSets()
    {
        CreateTileSet("Sand");
        CreateTileSet("Snow");
    }

    static public void CreateTileSet(string name)
    {
        var tileSet = new TileSet();

        int id = tileSetList.Count > 0 ? (tileSetList[tileSetList.Count - 1].Id + 1) : 1;

        tileSet.Id = id;
        tileSet.name = name;
        tileSet.tileSize = 31.75f;

        tileSetList.Add(tileSet);
    }

    static public void LoadTiles()
    {
        int index = 0;

        foreach(TileSet tileSet in tileSetList)
        {
            for (int i = 0; i < tilesInTileSet; i++)
            {
                var tile = new Tile();

                int id = tileList.Count > 0 ? (tileList[tileList.Count - 1].Id + 1) : 1;

                tile.Id = id;
                tile.tileSetId = tileSet.Id;
                tile.iconPath = "Textures/Tiles/" + tileSet.name + "/" + i;

                tileList.Add(tile);
            }

            index++;
        }
    }

    static public void LoadItems()
    {
        LoadSupplies();
        LoadGear();
        LoadSpoils();
    }

    static public void LoadSupplies()
    {
        int index = 0;

        for (int i = 0; i < supplies; i++)
        {
            var item = new Item();

            int id = itemList.Count > 0 ? (itemList[itemList.Count - 1].Id + 1) : 1;

            item.Id = id;
            item.Index = index;

            item.type = (int)Enums.ItemType.Supplies;
            
            item.objectGraphicId = 1;

            item.name = "Item " + id;

            itemList.Add(item);

            index++;
        }
    }

    static public void LoadGear()
    {
        int index = 0;

        var gearList = new List<int> { 2, 3, 4, 5, 6, 7, 8, 9 };

        for (int i = 0; i < gearList.Count; i++)
        {
            var item = new Item();

            int id = itemList.Count > 0 ? (itemList[itemList.Count - 1].Id + 1) : 1;

            item.Id = id;
            item.Index = index;

            item.type = (int)Enums.ItemType.Gear;
            
            item.objectGraphicId = gearList[i];

            item.name = "Item " + id;

            itemList.Add(item);

            index++;
        }
    }

    static public void LoadSpoils()
    {
        int index = 0;

        for (int i = 0; i < spoils; i++)
        {
            var item = new Item();

            int id = itemList.Count > 0 ? (itemList[itemList.Count - 1].Id + 1) : 1;

            item.Id = id;
            item.Index = index;

            item.type = (int)Enums.ItemType.Spoils;
            
            item.objectGraphicId = 1;

            item.name = "Item " + id;

            itemList.Add(item);

            index++;
        }
    }

    static public void LoadInteractableCharacters()
    {
        var objectList = new List<int> { 10, 11, 12, 13, 14, 15 };

        for (int i = 0; i < objectList.Count; i++)
        {
            var interactable = new Interactable();

            int id = interactableList.Count > 0 ? (interactableList[interactableList.Count - 1].Id + 1) : 1;

            interactable.Id = id;
            interactable.Index = i;

            interactable.type = (int)Enums.InteractableType.Character;

            interactable.objectGraphicId = objectList[i];

            interactable.name = "Interactable " + id;

            interactableList.Add(interactable);
        }
    }

    static public void LoadInteractableObjects()
    {
        var objectList = new List<int> { 20 };

        for (int i = 0; i < objectList.Count; i++)
        {
            var interactable = new Interactable();

            int id = interactableList.Count > 0 ? (interactableList[interactableList.Count - 1].Id + 1) : 1;

            interactable.Id = id;
            interactable.Index = i;

            interactable.type = (int)Enums.InteractableType.Object;

            interactable.objectGraphicId = objectList[i];

            interactable.name = "Interactable " + id;

            interactableList.Add(interactable);
        }
    }

    static public void LoadRegions()
    {
        for (int i = 0; i < regions; i++)
        {
            var region = new Region();

            int id = regionList.Count > 0 ? (regionList[regionList.Count - 1].Id + 1) : 1;

            region.Id = id;
            region.Index = i;

            region.chapterRegionId = 0;
            region.phaseId = 0;
            region.tileSetId = (i % tileSetList.Count) + 1;
            region.name = "Region " + id;
            region.regionSize = terrainsInRegions;
            region.terrainSize = terrainTilesInTerrains;

            regionList.Add(region);
        }
    }

    static public void LoadTerrains()
    {
        foreach(Region region in regionList)
        {
            for(int i = 0; i < (region.regionSize * region.regionSize); i++)
            {
                var terrain = new Terrain();

                int id = terrainList.Count > 0 ? (terrainList[terrainList.Count - 1].Id + 1) : 1;

                terrain.Id = id;
                terrain.Index = i;

                terrain.regionId = region.Id;
                terrain.iconId = 1;
                terrain.name = "Terrain " + (i + 1);

                CreateAtmosphere(terrain, true, 0, 0);
                CreateAtmosphere(terrain, false, 8, 12);
                CreateAtmosphere(terrain, false, 16, 20);

                terrainList.Add(terrain);
            }
        }
    }

    static public void CreateAtmosphere(Terrain terrain, bool isDefault, int startTime, int endTime)
    {
        var atmosphere = new Atmosphere();

        int id = atmosphereList.Count > 0 ? (atmosphereList[atmosphereList.Count - 1].Id + 1) : 1;

        atmosphere.Id = id;

        atmosphere.terrainId = terrain.Id;

        atmosphere.isDefault = isDefault;

        atmosphere.startTime = startTime;
        atmosphere.endTime = endTime;

        atmosphereList.Add(atmosphere);
    }

    static public void LoadTerrainTiles()
    {
        foreach(Terrain terrain in terrainList)
        {
            var regionData = regionList.Where(x => x.Id == terrain.regionId).FirstOrDefault();
            var tileData = tileList.Where(x => x.tileSetId == regionData.tileSetId).ToList();

            for (int i = 0; i < (regionData.terrainSize * regionData.terrainSize); i++)
            {
                var terrainTile = new TerrainTile();

                int id = terrainTileList.Count > 0 ? (terrainTileList[terrainTileList.Count - 1].Id + 1) : 1;

                terrainTile.Id = id;
                terrainTile.Index = i;

                terrainTile.terrainId = terrain.Id;
                terrainTile.tileId = tileData.FirstOrDefault().Id;

                terrainTileList.Add(terrainTile);
            }
        }
    }

    static public void LoadWorldObjects()
    {
        foreach (Region region in regionList)
        {
            var terrains = terrainList.Where(x => x.regionId == region.Id).Distinct().ToList();
            var middleTerrain = terrains[terrains.Count / 2];
            var terrainTiles = terrainTileList.Where(x => x.terrainId == (middleTerrain.Id)).Distinct().ToList();
            var middleTile = terrainTiles[terrainTiles.Count / 2];

            /*Skull*/
            CreateWorldObject(16, 0, region.Id, new Vector3(245f, 236.5f, -0.2f), new Vector3(5, 0, 25));

            /*Rock*/
            CreateWorldObject(17, 1, region.Id, new Vector3(230f, 241.75f, 0f), new Vector3(0, 0, 180f));

            /*Cactus*/
            CreateWorldObject(18, 2, region.Id, new Vector3(246.5f, 236.75f, 0f), new Vector3(0, 0, 0));

            /*Red warrior*/
            CreateWorldInteractable(Enums.InteractableType.Character, 1, region.Id, new Vector3(238.125f, 239.875f, 0.1f), new Vector3(0, 0, 0));

            /*Ranger*/
            CreateWorldInteractable(Enums.InteractableType.Character, 4, region.Id, new Vector3(235.625f, 242.375f, 0.2f), new Vector3(0, 0, 125));

            /*Mage*/
            CreateWorldInteractable(Enums.InteractableType.Character, 5, region.Id, new Vector3(240.625f, 242.375f, 0f), new Vector3(0, 0, 235));

            /*Pool*/
            CreateWorldInteractable(Enums.InteractableType.Object, 7, region.Id, new Vector3(238.125f, 242.375f, 0f), new Vector3(0, 0, 0));

            var regionSize = GetRegionSize(region.Id);

            for (int i = 3; i < objectsInWorld; i++)
            {
                CreateWorldObject(Random.Range(16, 21), i, region.Id, new Vector3(Random.Range(0, (regionSize - 1)), Random.Range(0, (regionSize - 1)), 0f), new Vector3(0, 0, Random.Range(0, 359)));
            }
        }
    }

    static public void CreateWorldInteractable(Enums.InteractableType type, int interactableId, int regionId, Vector3 position, Vector3 rotation)
    {
        var worldInteractable = new WorldInteractable();

        int id = worldInteractableList.Count > 0 ? (worldInteractableList[worldInteractableList.Count - 1].Id + 1) : 1;

        worldInteractable.Id = id;

        worldInteractable.type = (int)type;

        worldInteractable.interactableId = interactableId;
        
        for (int index = 0; index < baseTasks; index++)
        {
            CreateTask(worldInteractable, 0, index, regionId, position, rotation);
        }
        
        worldInteractableList.Add(worldInteractable);
    }

    static public void CreateTask(WorldInteractable worldInteractable, int objectiveId, int taskIndex, int regionId, Vector3 position, Vector3 rotation)
    {
        var task = new Task();

        int id = taskList.Count > 0 ? (taskList[taskList.Count - 1].Id + 1) : 1;

        task.Id = id;
        task.Index = taskIndex;

        task.worldInteractableId = worldInteractable.Id;
        task.objectiveId = objectiveId;
        
        task.name = "Just a task" + (objectiveId == 0 ? "" : " with an objective " + task.objectiveId);

        task.publicNotes = "I belong to Interactable " + worldInteractable.Id + ". This is definitely a test";
        
        CreateInteraction(task, true, 0, 0, regionId, position, rotation);
        CreateInteraction(task, false, 0, 4, regionId, position, rotation);
        CreateInteraction(task, false, 9, 15, regionId, position, rotation);
        
        taskList.Add(task);
    }

    static public void CreateInteraction(Task task, bool isDefault, int startTime, int endTime, int regionId, Vector3 position, Vector3 rotation)
    {
        var interaction = new Interaction();

        int id = interactionList.Count > 0 ? (interactionList[interactionList.Count - 1].Id + 1) : 1;
        
        interaction.Id = id;

        interaction.taskId = task.Id;

        interaction.isDefault = isDefault;

        interaction.startTime = startTime;
        interaction.endTime = endTime;

        interaction.publicNotes = "These are public interaction notes";

        interaction.positionX = position.x;
        interaction.positionY = position.y;
        interaction.positionZ = position.z;

        interaction.regionId = regionId;
        interaction.terrainId = GetTerrain(interaction.regionId, interaction.positionX, interaction.positionY);
        interaction.terrainTileId = GetTerrainTile(interaction.terrainId, interaction.positionX, interaction.positionY);

        interaction.rotationX = (int)rotation.x;
        interaction.rotationY = (int)rotation.y;
        interaction.rotationZ = (int)rotation.z;

        interaction.scaleMultiplier = 1;
        
        CreateOutcome(interaction, Enums.OutcomeType.Positive);

        interactionList.Add(interaction);
    }

    static public void CreateOutcome(Interaction interaction, Enums.OutcomeType type)
    {
        var outcome = new Outcome();

        int id = outcomeList.Count > 0 ? (outcomeList[outcomeList.Count - 1].Id + 1) : 1;

        outcome.Id = id;

        outcome.type = (int)type;

        outcome.interactionId = interaction.Id;

        outcomeList.Add(outcome);
    }

    static public void CreateWorldObject(int objectGraphicId, int index, int regionId, Vector3 position, Vector3 rotation)
    {
        var worldObject = new WorldObject();

        int id = worldObjectList.Count > 0 ? (worldObjectList[worldObjectList.Count - 1].Id + 1) : 1;

        worldObject.Id = id;
        worldObject.Index = index;

        worldObject.objectGraphicId = objectGraphicId;
        worldObject.regionId = regionId;
        
        worldObject.positionX = position.x;
        worldObject.positionY = position.y;
        worldObject.positionZ = position.z;

        worldObject.terrainId = GetTerrain(worldObject.regionId, worldObject.positionX, worldObject.positionY);
        worldObject.terrainTileId = GetTerrainTile(worldObject.terrainId, worldObject.positionX, worldObject.positionY);
        
        worldObject.rotationX = (int)rotation.x;
        worldObject.rotationY = (int)rotation.y;
        worldObject.rotationZ = (int)rotation.z;

        worldObject.scaleMultiplier = 1;

        worldObjectList.Add(worldObject);
    }

    static public void LoadChapters()
    {
        for (int i = 0; i < chapters; i++)
        {
            var chapter = new Chapter();

            int chapterId = (i + 1);

            chapter.Id = chapterId;
            chapter.Index = i;

            chapter.name = "Chapter " + chapterId;
            chapter.publicNotes = "This is a pretty regular sentence. The structure is something you'd expect. Nothing too long though!";
            
            chapterList.Add(chapter);
        }
    }

    static public void LoadChapterPartyMembers()
    {
        foreach (Chapter chapter in chapterList)
        {
            List<int> randomInteractables = new List<int>();

            var chapterInteractableIds = chapterInteractableList.Where(x => x.chapterId == chapter.Id).Select(x => x.interactableId).Distinct().ToList();
            interactableList.Where(x => !chapterInteractableIds.Contains(x.Id)).Distinct().ToList().ForEach(x => randomInteractables.Add(x.Id));

            for (int i = 0; i < partyMembersInChapter; i++)
            {
                var partyMember = new PartyMember();

                int id = partyMemberList.Count > 0 ? (partyMemberList[partyMemberList.Count - 1].Id + 1) : 1;

                partyMember.Id = id;
                partyMember.Index = i;

                partyMember.chapterId = chapter.Id;

                int randomInteractable = Random.Range(0, randomInteractables.Count);

                partyMember.interactableId = randomInteractables[randomInteractable];

                randomInteractables.RemoveAt(randomInteractable);

                partyMemberList.Add(partyMember);
            }
        }
    }

    static public void LoadChapterInteractables()
    {
        foreach(Chapter chapter in chapterList)
        {
            List<int> randomInteractables = new List<int>();

            var partyMemberIds = partyMemberList.Where(x => x.chapterId == chapter.Id).Select(x => x.interactableId).Distinct().ToList();
            interactableList.Where(x => !partyMemberIds.Contains(x.Id)).Distinct().ToList().ForEach(x => randomInteractables.Add(x.Id));

            for (int i = 0; i < chapterInteractablesInChapter; i++)
            {
                var chapterInteractable = new ChapterInteractable();

                int id = chapterInteractableList.Count > 0 ? (chapterInteractableList[chapterInteractableList.Count - 1].Id + 1) : 1;

                chapterInteractable.Id = id;
                chapterInteractable.Index = i;

                chapterInteractable.chapterId = chapter.Id;

                int randomInteractable = Random.Range(0, randomInteractables.Count);

                chapterInteractable.interactableId = randomInteractables[randomInteractable];

                randomInteractables.RemoveAt(randomInteractable);

                chapterInteractableList.Add(chapterInteractable);
            }
        }
    }

    static public void LoadChapterRegions()
    {
        foreach (Chapter chapter in chapterList)
        {
            List<int> randomRegions = new List<int>();

            regionList.ForEach(x => randomRegions.Add(x.Id));

            //int randomRegionAmount = Random.Range(1, regionList.Count + 1);

            for (int i = 0; i < /*randomRegionAmount*/1; i++)
            {
                var chapterRegion = new ChapterRegion();

                int id = chapterRegionList.Count > 0 ? (chapterRegionList[chapterRegionList.Count - 1].Id + 1) : 1;

                chapterRegion.Id = id;
                chapterRegion.Index = i;

                chapterRegion.chapterId = chapter.Id;

                int randomRegion = Random.Range(0, randomRegions.Count);

                chapterRegion.regionId = randomRegions[randomRegion];

                randomRegions.RemoveAt(randomRegion);

                chapterRegionList.Add(chapterRegion);
            }
        }
    }

    static public void LoadPhases()
    {
        foreach (Chapter chapter in chapterList)
        {
            for (int i = 0; i < phasesInChapter; i++)
            {
                var phase = new Phase();

                int id = phaseList.Count > 0 ? (phaseList[phaseList.Count - 1].Id + 1) : 1;

                phase.Id = id;
                phase.Index = i;

                phase.chapterId = chapter.Id;
                phase.name = "Phase " + (i + 1);
                phase.publicNotes = "I belong to Chapter " + chapter.Id + ". This is definitely a test";
                
                phaseList.Add(phase);
            }
        }
    }

    static public void LoadPhaseRegions()
    {
        foreach(Phase phase in phaseList)
        {
            foreach(ChapterRegion chapterRegion in chapterRegionList.Where(x => x.chapterId == phase.chapterId).Distinct().ToList())
            {
                var regionSource = regionList.Where(x => x.Id == chapterRegion.regionId).FirstOrDefault();

                var region = new Region();

                int regionId = regionList.Count > 0 ? (regionList[regionList.Count - 1].Id + 1) : 1;

                region.Id = regionId;
                region.phaseId = phase.Id;
                region.chapterRegionId = chapterRegion.Id;

                region.Index = chapterRegion.Index;
                region.tileSetId = regionSource.tileSetId;

                region.name = regionSource.name;
                region.regionSize = regionSource.regionSize;
                region.terrainSize = regionSource.terrainSize;

                regionList.Add(region);

                //Get all world interactables belonging to this region
                var tempInteractionSourceList = interactionList.Where(x => x.regionId == regionSource.Id);
                var tempTaskSourceList = taskList.Where(x => tempInteractionSourceList.Select(y => y.taskId).Contains(x.Id));
                var worldInteractableSourceList = worldInteractableList.Where(x => tempTaskSourceList.Select(y => y.worldInteractableId).Contains(x.Id)).Distinct().ToList();

                var terrainSourceList = terrainList.Where(x => x.regionId == regionSource.Id).OrderBy(x => x.Index).Distinct().ToList();

                foreach (Terrain terrainSource in terrainSourceList)
                {
                    var terrain = new Terrain();

                    int terrainId = terrainList.Count > 0 ? (terrainList[terrainList.Count - 1].Id + 1) : 1;

                    terrain.Id = terrainId;
                    terrain.regionId = region.Id;

                    terrain.Index = terrainSource.Index;

                    terrain.iconId = terrainSource.iconId;
                    terrain.name = terrainSource.name;

                    var atmosphereSourceList = atmosphereList.Where(x => x.terrainId == terrainSource.Id).OrderByDescending(x => x.isDefault).ThenBy(x => x.startTime).ToList();

                    foreach (Atmosphere atmosphereSource in atmosphereSourceList)
                    {
                        CreateAtmosphere(terrain, atmosphereSource.isDefault, atmosphereSource.startTime, atmosphereSource.endTime);
                    }

                    var terrainTileSourceList = terrainTileList.Where(x => x.terrainId == terrainSource.Id).OrderBy(x => x.Index).Distinct().ToList();

                    foreach(TerrainTile terrainTileSource in terrainTileSourceList)
                    {
                        var terrainTile = new TerrainTile();
                        
                        int terrainTileId = terrainTileList.Count > 0 ? (terrainTileList[terrainTileList.Count - 1].Id + 1) : 1;

                        terrainTile.Id = terrainTileId;
                        terrainTile.terrainId = terrain.Id;

                        terrainTile.Index = terrainTileSource.Index;

                        terrainTile.tileId = terrainTileSource.tileId;
                        
                        terrainTileList.Add(terrainTile);
                    }

                    terrainList.Add(terrain);
                }

                foreach (WorldInteractable worldInteractableSource in worldInteractableSourceList)
                {
                    var worldInteractable = new WorldInteractable();

                    int worldInteractableId = worldInteractableList.Count > 0 ? (worldInteractableList[worldInteractableList.Count - 1].Id + 1) : 1;

                    worldInteractable.Id = worldInteractableId;

                    worldInteractable.type = worldInteractableSource.type;

                    worldInteractable.interactableId = worldInteractableSource.interactableId;
                    
                    worldInteractable.interactionIndex = worldInteractableSource.interactionIndex;

                    var taskSourceList = taskList.Where(x => x.worldInteractableId == worldInteractableSource.Id).OrderBy(x => x.Index).Distinct().ToList();

                    foreach(Task taskSource in taskSourceList)
                    {
                        var task = new Task();

                        int id = taskList.Count > 0 ? (taskList[taskList.Count - 1].Id + 1) : 1;

                        task.Id = id;
                        task.Index = taskSource.Index;

                        task.worldInteractableId = worldInteractable.Id;
                        task.objectiveId = taskSource.objectiveId;

                        task.name = "Just a task" + (taskSource.objectiveId == 0 ? "" : " with an objective " + task.objectiveId);
                        
                        var interactionSourceList = interactionList.Where(x => x.taskId == taskSource.Id).OrderBy(x => x.Index).Distinct().ToList();

                        foreach (Interaction interactionSource in interactionSourceList)
                        {
                            var interaction = new Interaction();

                            int interactionId = interactionList.Count > 0 ? (interactionList[interactionList.Count - 1].Id + 1) : 1;

                            interaction.Id = interactionId;
                            interaction.taskId = task.Id;
                            
                            interaction.regionId = region.Id;

                            interaction.Index = interactionSource.Index;

                            interaction.isDefault = interactionSource.isDefault;

                            interaction.startTime = interactionSource.startTime;
                            interaction.endTime = interactionSource.endTime;

                            interaction.publicNotes = interactionSource.publicNotes;
                            interaction.privateNotes = interactionSource.privateNotes;

                            interaction.positionX = interactionSource.positionX;
                            interaction.positionY = interactionSource.positionY;
                            interaction.positionZ = interactionSource.positionZ;

                            interaction.terrainId = GetTerrain(interaction.regionId, interaction.positionX, interaction.positionY);
                            interaction.terrainTileId = GetTerrainTile(interaction.terrainId, interaction.positionX, interaction.positionY);

                            interaction.rotationX = interactionSource.rotationX;
                            interaction.rotationY = interactionSource.rotationY;
                            interaction.rotationZ = interactionSource.rotationZ;

                            interaction.scaleMultiplier = interactionSource.scaleMultiplier;

                            var outcomeSourceList = outcomeList.Where(x => x.interactionId == interactionSource.Id).OrderBy(x => x.type).Distinct().ToList();

                            foreach(Outcome outcomeSource in outcomeSourceList)
                            {
                                var outcome = new Outcome();

                                int outcomeId = outcomeList.Count > 0 ? (outcomeList[outcomeList.Count - 1].Id + 1) : 1;

                                outcome.Id = outcomeId;

                                outcome.type = outcomeSource.type;

                                outcome.interactionId = interaction.Id;

                                outcomeList.Add(outcome);
                            }

                            interactionList.Add(interaction);
                        }

                        taskList.Add(task);
                    }
                    
                    worldInteractableList.Add(worldInteractable);
                }

                var worldObjectSourceList = worldObjectList.Where(x => x.regionId == regionSource.Id).Distinct().ToList();

                foreach (WorldObject worldObjectSource in worldObjectSourceList)
                {
                    var worldObject = new WorldObject();

                    int worldObjectId = worldObjectList.Count > 0 ? (worldObjectList[worldObjectList.Count - 1].Id + 1) : 1;

                    worldObject.Id = worldObjectId;
                    worldObject.regionId = region.Id;

                    worldObject.positionX = worldObjectSource.positionX;
                    worldObject.positionY = worldObjectSource.positionY;
                    worldObject.positionZ = worldObjectSource.positionZ;

                    worldObject.terrainId = GetTerrain(worldObject.regionId, worldObject.positionX, worldObject.positionY);
                    worldObject.terrainTileId = GetTerrainTile(worldObject.terrainId, worldObject.positionX, worldObject.positionY);

                    worldObject.rotationX = worldObjectSource.rotationX;
                    worldObject.rotationY = worldObjectSource.rotationY;
                    worldObject.rotationZ = worldObjectSource.rotationZ;

                    worldObject.scaleMultiplier = worldObjectSource.scaleMultiplier;

                    worldObject.Index = worldObjectSource.Index;
                    worldObject.objectGraphicId = worldObjectSource.objectGraphicId;
                    
                    worldObjectList.Add(worldObject);
                }
            }
        }
    }

    static public void LoadQuests()
    {
        foreach (Phase phase in phaseList)
        {
            for (int i = 0; i < questsInPhase; i++)
            {
                var quest = new Quest();

                int id = questList.Count > 0 ? (questList[questList.Count - 1].Id + 1) : 1;

                quest.Id = id;
                quest.Index = i;

                quest.phaseId = phase.Id;
                quest.name = "Quest " + (i + 1);
                quest.publicNotes = "I belong to Phase " + phase.Id + ". This is definitely a test";
                
                questList.Add(quest);
            }
        }
    }

    static public void LoadPhaseInteractables()
    {
        foreach (Chapter chapter in chapterList)
        {
            foreach (Phase phase in phaseList.Where(x => x.chapterId == chapter.Id).Distinct().ToList())
            {
                var chapterInteractables = chapterInteractableList.Where(x => x.chapterId == chapter.Id).Distinct().ToList();
                var questIds = questList.Where(x => x.phaseId == phase.Id).Select(x => x.Id).Distinct().ToList();

                for (int i = 0; i < chapterInteractables.Count; i++)
                {
                    var phaseInteractable = new PhaseInteractable();

                    int id = phaseInteractableList.Count > 0 ? (phaseInteractableList[phaseInteractableList.Count - 1].Id + 1) : 1;

                    phaseInteractable.Id = id;

                    phaseInteractable.phaseId = phase.Id;
                    phaseInteractable.chapterInteractableId = chapterInteractables[i].Id;

                    int randomQuestId = Random.Range(0, questIds.Count);

                    phaseInteractable.questId = questIds[randomQuestId];

                    questIds.RemoveAt(randomQuestId);

                    phaseInteractableList.Add(phaseInteractable);
                }
            }
        }
    }

    static public void LoadObjectives()
    {
        foreach (Quest quest in questList)
        {
            for (int i = 0; i < objectivesInQuest; i++)
            {
                var objective = new Objective();

                int id = objectiveList.Count > 0 ? (objectiveList[objectiveList.Count - 1].Id + 1) : 1;

                objective.Id = id;
                objective.Index = i;

                objective.questId = quest.Id;
                objective.name = "Objective " + (i + 1);
                objective.publicNotes = "I belong to Quest " + quest.Id + ". This is definitely a test";
                
                objectiveList.Add(objective);
            }
        }
    }





    static public void LoadObjectiveWorldInteractables()
    {
        foreach (Objective objective in objectiveList)
        {
            List<int> randomInteractables = new List<int>();

            interactableList.ForEach(x => randomInteractables.Add(x.Id));

            //Fetch the interactables belonging to the quest and create an objective interactable for each
            var chapterInteractableIds = chapterInteractableList.Where(x => phaseInteractableList.Where(y => y.questId == objective.questId).Select(y => y.chapterInteractableId).Contains(x.Id))
                                                                .Select(x => x.interactableId).ToList();

            int index = 0;

            for (int i = 0; i < chapterInteractableIds.Count; i++)
            {
                var worldInteractable = new WorldInteractable();

                int id = worldInteractableList.Count > 0 ? (worldInteractableList[worldInteractableList.Count - 1].Id + 1) : 1;

                worldInteractable.Id = id;
                worldInteractable.Index = index;

                worldInteractable.type = (int)Enums.InteractableType.Character;

                worldInteractable.objectiveId = objective.Id;

                worldInteractable.interactableId = chapterInteractableIds[i];

                worldInteractable.isDefault = true;

                worldInteractableList.Add(worldInteractable);

                index++;
            }

            for(int i = 0; i < interactablesInObjective; i++)
            {
                var worldInteractable = new WorldInteractable();

                int id = worldInteractableList.Count > 0 ? (worldInteractableList[worldInteractableList.Count - 1].Id + 1) : 1;

                worldInteractable.Id = id;
                worldInteractable.Index = index;

                worldInteractable.type = (int)Enums.InteractableType.Object;

                worldInteractable.objectiveId = objective.Id;

                int randomInteractable = Random.Range(0, randomInteractables.Count);
                worldInteractable.interactableId = randomInteractables[randomInteractable];

                worldInteractableList.Add(worldInteractable);

                index++;
            }
        }
    }

    static public void LoadTasks()
    {
        foreach (Objective objective in objectiveList)
        {
            var worldInteractables = worldInteractableList.Where(x => x.objectiveId == objective.Id).Distinct().ToList();

            var phaseId = phaseList.Where(x => questList.Where(y => y.Id == objective.questId).Select(y => y.phaseId).Contains(x.Id)).Select(x => x.Id).FirstOrDefault();
            var regions = regionList.Where(x => x.phaseId == phaseId).Distinct().ToList();
            
            foreach (WorldInteractable worldInteractable in worldInteractables)
            {
                for (int index = 0; index < baseTasks; index++)
                {
                    var randomRegion = regions[Random.Range(0, regions.Count)];

                    var regionSize = GetRegionSize(randomRegion.Id);

                    var randomPosition = new Vector3(Random.Range(0, (regionSize - 1)), Random.Range(0, (regionSize - 1)), 0);

                    CreateTask(worldInteractable, objective.Id, index, randomRegion.Id, randomPosition, Vector3.zero);
                }
            }
        }
    }
    
    static public float GetRegionSize(int regionId)
    {
        var region = regionList.Where(x => x.Id == regionId).FirstOrDefault();
        var tileSet = tileSetList.Where(x => x.Id == region.tileSetId).FirstOrDefault();
        var terrains = terrainList.Where(x => x.regionId == region.Id).Distinct().ToList();

        var regionSize = region.regionSize * region.terrainSize * tileSet.tileSize;

        return regionSize;
    }

    static public int GetTerrain(int regionId, float posX, float posY)
    {
        var region = regionList.Where(x => x.Id == regionId).FirstOrDefault();
        var tileSet = tileSetList.Where(x => x.Id == region.tileSetId).FirstOrDefault();
        var terrains = terrainList.Where(x => x.regionId == region.Id).Distinct().ToList();

        var terrainSize = region.terrainSize * tileSet.tileSize;

        var terrainCoordinates = new Vector2(Mathf.Floor(posX / terrainSize),
                                             Mathf.Floor(posY / terrainSize));

        var terrainIndex = (region.regionSize * terrainCoordinates.y) + terrainCoordinates.x;

        var terrainId = terrains.Where(x => x.Index == terrainIndex).Select(x => x.Id).FirstOrDefault();

        return terrainId;
    }

    static public int GetTerrainTile(int terrainId, float posX, float posY)
    {
        var terrain = terrainList.Where(x => x.Id == terrainId).FirstOrDefault();
        var region = regionList.Where(x => x.Id == terrain.regionId).FirstOrDefault();
        var tileSet = tileSetList.Where(x => x.Id == region.tileSetId).FirstOrDefault();
        
        var terrainSize = region.terrainSize * tileSet.tileSize;

        var terrainCoordinates = new Vector2(Mathf.Floor(posX / terrainSize),
                                             Mathf.Floor(posY / terrainSize));

        var terrainTiles = terrainTileList.Where(x => x.terrainId == terrainId).Distinct().ToList();

        var terrainPosition = new Vector2(terrainCoordinates.x * terrainSize,
                                          terrainCoordinates.y * terrainSize);

        var localPosition = new Vector2(posX - terrainPosition.x,
                                        posY - terrainPosition.y);

        var tileCoordinates = new Vector2(Mathf.Floor(localPosition.x / tileSet.tileSize),
                                          Mathf.Floor(localPosition.y / tileSet.tileSize));

        var tileIndex = (region.terrainSize * tileCoordinates.y) + tileCoordinates.x;

        var terrainTileId = terrainTiles.Where(x => x.Index == tileIndex).Select(x => x.Id).FirstOrDefault();

        return terrainTileId;
    }

    static public void CreateSaveFile()
    {
        CreateGameSave();
    }

    static public void CreateGameSave()
    {
        var gameSave = new GameSave();

        int id = gameSaveList.Count > 0 ? (gameSaveList[gameSaveList.Count - 1].Id + 1) : 1;

        gameSave.Id = id;
        gameSave.Index = gameSaveList.Count;

        CreateStageSaves(gameSave);

        gameSaveList.Add(gameSave);
    }

    static private void CreateStageSaves(GameSave gameSave)
    {
        LoadChapterSaves(gameSave);
        LoadPhaseRegionWorldInteractableTasks();
    }

    static public void LoadChapterSaves(GameSave gameSave)
    {
        foreach (Chapter chapter in chapterList)
        {
            var chapterSave = new ChapterSave();

            int chapterSaveId = chapterSaveList.Count > 0 ? (chapterSaveList[chapterSaveList.Count - 1].Id + 1) : 1;

            chapterSave.Id = chapterSaveId;

            chapterSave.gameSaveId = gameSave.Id;
            chapterSave.chapterId = chapter.Id;

            LoadPhaseSaves(chapter, chapterSave);

            chapterSaveList.Add(chapterSave);
        }
    }

    static public void LoadPhaseSaves(Chapter chapter, ChapterSave chapterSave)
    {
        foreach(Phase phase in phaseList.Where(x => x.chapterId == chapter.Id))
        {
            var phaseSave = new PhaseSave();

            int id = phaseSaveList.Count > 0 ? (phaseSaveList[phaseSaveList.Count - 1].Id + 1) : 1;

            phaseSave.Id = id;

            phaseSave.chapterSaveId = chapterSave.Id;
            phaseSave.phaseId = phase.Id;

            LoadQuestSaves(phase, phaseSave);

            phaseSaveList.Add(phaseSave);
        }
    }

    static public void LoadQuestSaves(Phase phase, PhaseSave phaseSave)
    {
        foreach (Quest quest in questList.Where(x => x.phaseId == phase.Id))
        {
            var questSave = new QuestSave();

            int id = questSaveList.Count > 0 ? (questSaveList[questSaveList.Count - 1].Id + 1) : 1;

            questSave.Id = id;

            questSave.phaseSaveId = phaseSave.Id;
            questSave.questId = quest.Id;

            LoadObjectiveSaves(quest, questSave);

            questSaveList.Add(questSave);
        }
    }

    static public void LoadObjectiveSaves(Quest quest, QuestSave questSave)
    {
        foreach (Objective objective in objectiveList.Where(x => x.questId == quest.Id))
        {
            var objectiveSave = new ObjectiveSave();

            int id = objectiveSaveList.Count > 0 ? (objectiveSaveList[objectiveSaveList.Count - 1].Id + 1) : 1;

            objectiveSave.Id = id;

            objectiveSave.questSaveId = questSave.Id;
            objectiveSave.objectiveId = objective.Id;
            
            LoadObjectiveTaskSaves(objective, objectiveSave);

            objectiveSaveList.Add(objectiveSave);
        }
    }

    static public void LoadObjectiveTaskSaves(Objective objective, ObjectiveSave objectiveSave)
    {
        foreach (Task task in taskList.Where(x => x.objectiveId == objective.Id))
        {
            var taskSave = new TaskSave();

            int id = taskSaveList.Count > 0 ? (taskSaveList[taskSaveList.Count - 1].Id + 1) : 1;

            taskSave.Id = id;

            taskSave.worldInteractableId = task.worldInteractableId;
            taskSave.objectiveSaveId = objectiveSave.Id;
            taskSave.taskId = task.Id;

            LoadInteractionSaves(task, taskSave);

            taskSaveList.Add(taskSave);
        }
    }

    static public void LoadWorldInteractableTaskSaves(WorldInteractable worldInteractable)
    {
        foreach (Task task in taskList.Where(x => x.worldInteractableId == worldInteractable.Id))
        {
            var taskSave = new TaskSave();

            int id = taskSaveList.Count > 0 ? (taskSaveList[taskSaveList.Count - 1].Id + 1) : 1;

            taskSave.Id = id;

            taskSave.worldInteractableId = worldInteractable.Id;
            taskSave.taskId = task.Id;

            LoadInteractionSaves(task, taskSave);

            taskSaveList.Add(taskSave);
        }
    }

    static public void LoadInteractionSaves(Task task, TaskSave taskSave)
    {
        foreach (Interaction interaction in interactionList.Where(x => x.taskId == task.Id))
        {
            var interactionSave = new InteractionSave();

            int id = interactionSaveList.Count > 0 ? (interactionSaveList[interactionSaveList.Count - 1].Id + 1) : 1;

            interactionSave.Id = id;

            interactionSave.taskSaveId = taskSave.Id;
            interactionSave.interactionId = interaction.Id;

            interactionSaveList.Add(interactionSave);
        }
    }

    static private void LoadPhaseRegionWorldInteractableTasks()
    {
        foreach (PhaseSave phaseSave in phaseSaveList)
        {
            var phaseRegionSourceList = regionList.Where(x => x.phaseId == phaseSave.phaseId);

            foreach (Region phaseRegionSource in phaseRegionSourceList)
            {
                var interactionIdList = interactionList.Where(x => x.regionId == phaseRegionSource.Id).Select(x => x.taskId).ToList();
                var taskSourceIdList = taskList.Where(x => interactionIdList.Contains(x.Id)).Select(x => x.worldInteractableId).ToList();
                var worldInteractableSourceList = worldInteractableList.Where(x => taskSourceIdList.Contains(x.Id)).Distinct().ToList();
                
                foreach (WorldInteractable worldInteractableSource in worldInteractableSourceList)
                {
                    LoadWorldInteractableTaskSaves(worldInteractableSource);
                }
            }
        }
    }

    static private void Query()
    {
    }
}
