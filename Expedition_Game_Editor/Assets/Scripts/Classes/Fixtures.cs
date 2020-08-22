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

    static public int saves = 1;

    static public List<Icon>                    iconList                    = new List<Icon>();
    static public List<ObjectGraphic>           objectGraphicList           = new List<ObjectGraphic>();
    static public List<Item>                    itemList                    = new List<Item>();
    static public List<Interactable>            interactableList            = new List<Interactable>();
    static public List<TileSet>                 tileSetList                 = new List<TileSet>();
    static public List<Tile>                    tileList                    = new List<Tile>();
    static public List<Region>                  regionList                  = new List<Region>();
    static public List<Atmosphere>              atmosphereList              = new List<Atmosphere>();
    static public List<WorldObject>             worldObjectList             = new List<WorldObject>();
    static public List<Terrain>                 terrainList                 = new List<Terrain>();
    static public List<TerrainTile>             terrainTileList             = new List<TerrainTile>();
    static public List<Chapter>                 chapterList                 = new List<Chapter>();
    static public List<PartyMember>             partyMemberList             = new List<PartyMember>();
    static public List<ChapterInteractable>     chapterInteractableList     = new List<ChapterInteractable>();
    static public List<ChapterRegion>           chapterRegionList           = new List<ChapterRegion>();
    static public List<Phase>                   phaseList                   = new List<Phase>();
    static public List<Quest>                   questList                   = new List<Quest>();
    static public List<Objective>               objectiveList               = new List<Objective>();
    static public List<WorldInteractable>       worldInteractableList       = new List<WorldInteractable>();
    static public List<Task>                    taskList                    = new List<Task>();
    static public List<Interaction>             interactionList             = new List<Interaction>();
    static public List<InteractionDestination>  interactionDestinationList  = new List<InteractionDestination>();
    static public List<Outcome>                 outcomeList                 = new List<Outcome>();

    static public List<Save>                    saveList                    = new List<Save>();
    static public List<PlayerSave>              playerSaveList              = new List<PlayerSave>();
    static public List<InteractableSave>        interactableSaveList        = new List<InteractableSave>();
    static public List<ChapterSave>             chapterSaveList             = new List<ChapterSave>();
    static public List<PhaseSave>               phaseSaveList               = new List<PhaseSave>();
    static public List<QuestSave>               questSaveList               = new List<QuestSave>();
    static public List<ObjectiveSave>           objectiveSaveList           = new List<ObjectiveSave>();
    static public List<TaskSave>                taskSaveList                = new List<TaskSave>();
    static public List<InteractionSave>         interactionSaveList         = new List<InteractionSave>();

    public class Item
    {
        public int id;
        public int index;
        
        public int type;

        public int objectGraphicId;

        public string name;
    }

    public class Interactable
    {
        public int id;
        public int index;

        public int type;

        public int objectGraphicId;
        
        public string name;

        public float scaleMultiplier;

        public int health;
        public int hunger;
        public int thirst;

        public float weight;
        public float speed;
        public float stamina;
    }
    
    public class WorldObject
    {
        public int id;

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
    
    public class ObjectGraphic
    {
        public int id;

        public int iconId;

        public string name;
        public string path;

        public float height;
        public float width;
        public float depth;
    }

    public class Icon
    {
        public int id;
        
        public int category;
        public string path;
    }

    public class TileSet
    {
        public int id;
        
        public string name;
        public float tileSize;
    }

    public class Tile
    {
        public int id;

        public int tileSetId;
        public string iconPath;
    }
    
    public class Region
    {
        public int id;
        public int index;

        public int chapterRegionId;
        public int phaseId;
        public int tileSetId;

        public string name;
        public int regionSize;
        public int terrainSize;
    }

    public class Atmosphere
    {
        public int id;

        public int terrainId;

        public bool isDefault;

        public int startTime;
        public int endTime;

        public string publicNotes;
        public string privateNotes;
    }

    public class Terrain
    {
        public int id;
        public int index;

        public int regionId;
        public int iconId;

        public string name;
    }

    public class TerrainTile
    {
        public int id;
        public int index;

        public int terrainId;
        public int tileId;
    }

    public class Chapter
    {
        public int id;
        public int index;

        public string name;

        public float timeSpeed;

        public string publicNotes;
        public string privateNotes;
    }

    //Party member might be merged with ChapterInteractable and sorted by type
    public class PartyMember
    {
        public int id;

        public int chapterId;
        public int interactableId;
    }

    public class ChapterInteractable
    {
        public int id;
        
        public int chapterId;
        public int interactableId;
    }

    public class ChapterRegion
    {
        public int id;
        public int index;
        
        public int chapterId;
        public int regionId;
    }

    public class Phase
    {
        public int id;
        public int index;

        public int chapterId;

        public string name;
        
        public int defaultRegionId;

        public float defaultPositionX;
        public float defaultPositionY;
        public float defaultPositionZ;

        public int defaultRotationX;
        public int defaultRotationY;
        public int defaultRotationZ;

        public int defaultTime;

        public string publicNotes;
        public string privateNotes;
    }

    public class Quest
    {
        public int id;
        public int index;

        public int phaseId;

        public string name;

        public string publicNotes;
        public string privateNotes;
    }

    public class Objective
    {
        public int id;
        public int index;

        public int questId;

        public string name;
        public string journal;

        public string publicNotes;
        public string privateNotes;
    }

    public class WorldInteractable
    {
        public int id;
        public int index;
        
        public int type;
        
        public int phaseId;
        public int questId;
        public int objectiveId;

        public int chapterInteractableId;
        public int interactableId;
    }

    public class Task
    {
        public int id;
        public int index;

        public int worldInteractableId;
        public int objectiveId;

        public string name;

        public bool completeObjective;
        public bool repeatable;

        public string publicNotes;
        public string privateNotes;
    }

    public class Interaction
    {
        public int id;
        
        public int taskId;
        
        public bool isDefault;

        public int startTime;
        public int endTime;
        
        public bool triggerAutomatically;
        public bool beNearDestination;
        public bool faceAgent;
        public bool facePartyLeader;
        public bool hideInteractionIndicator;

        public float interactionRange;

        public int delayMethod;
        public int delayDuration;
        public bool hideDelayIndicator;

        public bool cancelDelayOnInput;
        public bool cancelDelayOnMovement;
        public bool cancelDelayOnHit;

        public string publicNotes;
        public string privateNotes;
    }

    public class InteractionDestination
    {
        public int id;
        
        public int interactionId;

        public int regionId;
        public int terrainId;
        public int terrainTileId;
        
        public float positionX;
        public float positionY;
        public float positionZ;

        public float positionVariance;

        public bool freeRotation;

        public int rotationX;
        public int rotationY;
        public int rotationZ;
        
        public int animation;
        public float patience;
    }

    public class Outcome
    {
        public int id;
        
        public int type;

        public int interactionId;
    }

    #region Save data
    public class Game
    {
        public int id;
        
        public string name;
    }

    public class Save
    {
        public int id;
        public int index;

        public int gameId;
    }

    public class PlayerSave
    {
        public int id;
        
        public int saveId;
        public int regionId;
        public int partyMemberId;
        
        public float positionX;
        public float positionY;
        public float positionZ;

        public float scaleMultiplier;

        public int gameTime;
        public int playedTime;
    }

    public class InteractableSave
    {
        public int id;
        public int index;
        
        public int saveId;
        public int interactableId;
    }

    public class ChapterSave
    {
        public int id;
        public int index;

        public int saveId;
        public int chapterId;

        public bool complete;
    }

    public class PhaseSave
    {
        public int id;
        public int index;

        public int saveId;
        public int chapterSaveId;
        public int phaseId;

        public bool complete;
    }

    public class QuestSave
    {
        public int id;
        public int index;

        public int saveId;
        public int phaseSaveId;
        public int questId;

        public bool complete;
    }

    public class ObjectiveSave
    {
        public int id;
        public int index;

        public int saveId;
        public int questSaveId;
        public int objectiveId;

        public bool complete;
    }

    public class TaskSave
    {
        public int id;
        public int index;

        public int saveId;
        public int worldInteractableId;
        public int objectiveSaveId;
        public int taskId;

        public bool complete;
    }

    public class InteractionSave
    {
        public int id;
        public int index;

        public int saveId;
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
        LoadQuests();
        LoadPhaseWorldInteractables();
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

        int id = iconList.Count > 0 ? (iconList[iconList.Count - 1].id + 1) : 1;

        icon.id = id;
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

        int id = objectGraphicList.Count > 0 ? (objectGraphicList[objectGraphicList.Count - 1].id + 1) : 1;

        objectGraphic.id = id;
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

        int id = tileSetList.Count > 0 ? (tileSetList[tileSetList.Count - 1].id + 1) : 1;

        tileSet.id = id;
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

                int id = tileList.Count > 0 ? (tileList[tileList.Count - 1].id + 1) : 1;

                tile.id = id;
                tile.tileSetId = tileSet.id;
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

            int id = itemList.Count > 0 ? (itemList[itemList.Count - 1].id + 1) : 1;

            item.id = id;
            item.index = index;

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

            int id = itemList.Count > 0 ? (itemList[itemList.Count - 1].id + 1) : 1;

            item.id = id;
            item.index = index;

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

            int id = itemList.Count > 0 ? (itemList[itemList.Count - 1].id + 1) : 1;

            item.id = id;
            item.index = index;

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

            int id = interactableList.Count > 0 ? (interactableList[interactableList.Count - 1].id + 1) : 1;

            interactable.id = id;
            interactable.index = i;

            interactable.type = (int)Enums.InteractableType.Agent;

            interactable.objectGraphicId = objectList[i];

            interactable.name = "Interactable " + id;

            interactable.scaleMultiplier = 1;

            interactable.health = 100;
            interactable.hunger = 100;
            interactable.thirst = 100;

            interactable.weight = 80f;
            interactable.speed = 5f;
            interactable.stamina = 50f;

            interactableList.Add(interactable);
        }
    }

    static public void LoadInteractableObjects()
    {
        var objectList = new List<int> { 20 };

        for (int i = 0; i < objectList.Count; i++)
        {
            var interactable = new Interactable();

            int id = interactableList.Count > 0 ? (interactableList[interactableList.Count - 1].id + 1) : 1;

            interactable.id = id;
            interactable.index = i;

            interactable.type = (int)Enums.InteractableType.Object;

            interactable.objectGraphicId = objectList[i];

            interactable.name = "Interactable " + id;

            interactable.scaleMultiplier = 1;

            interactableList.Add(interactable);
        }
    }

    static public void LoadRegions()
    {
        for (int i = 0; i < regions; i++)
        {
            var region = new Region();

            int id = regionList.Count > 0 ? (regionList[regionList.Count - 1].id + 1) : 1;

            region.id = id;
            region.index = i;

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

                int id = terrainList.Count > 0 ? (terrainList[terrainList.Count - 1].id + 1) : 1;

                terrain.id = id;
                terrain.index = i;

                terrain.regionId = region.id;
                terrain.iconId = 1;
                terrain.name = "Terrain " + (i + 1);

                CreateAtmosphere(terrain, true, 0, 0);
                CreateAtmosphere(terrain, false, 8 * TimeManager.secondsInHour, (13 * TimeManager.secondsInHour) - 1);
                CreateAtmosphere(terrain, false, 16 * TimeManager.secondsInHour, (21 * TimeManager.secondsInHour) - 1);

                terrainList.Add(terrain);
            }
        }
    }

    static public void CreateAtmosphere(Terrain terrain, bool isDefault, int startTime, int endTime)
    {
        var atmosphere = new Atmosphere();

        int id = atmosphereList.Count > 0 ? (atmosphereList[atmosphereList.Count - 1].id + 1) : 1;

        atmosphere.id = id;

        atmosphere.terrainId = terrain.id;

        atmosphere.isDefault = isDefault;
        
        atmosphere.startTime = startTime ;
        atmosphere.endTime = endTime;

        atmosphereList.Add(atmosphere);
    }

    static public void LoadTerrainTiles()
    {
        foreach(Terrain terrain in terrainList)
        {
            var regionData = regionList.Where(x => x.id == terrain.regionId).FirstOrDefault();
            var tileData = tileList.Where(x => x.tileSetId == regionData.tileSetId).ToList();

            for (int i = 0; i < (regionData.terrainSize * regionData.terrainSize); i++)
            {
                var terrainTile = new TerrainTile();

                int id = terrainTileList.Count > 0 ? (terrainTileList[terrainTileList.Count - 1].id + 1) : 1;

                terrainTile.id = id;
                terrainTile.index = i;

                terrainTile.terrainId = terrain.id;
                terrainTile.tileId = tileData.FirstOrDefault().id;

                terrainTileList.Add(terrainTile);
            }
        }
    }

    static public void LoadWorldObjects()
    {
        foreach (Region region in regionList)
        {
            var terrains = terrainList.Where(x => x.regionId == region.id).Distinct().ToList();
            var middleTerrain = terrains[terrains.Count / 2];
            var terrainTiles = terrainTileList.Where(x => x.terrainId == (middleTerrain.id)).Distinct().ToList();
            var middleTile = terrainTiles[terrainTiles.Count / 2];

            /*Skull*/
            CreateWorldObject(16, region.id, new Vector3(245f, -0.2f, 236.5f), new Vector3(355, 155, 0));

            /*Rock*/
            CreateWorldObject(17, region.id, new Vector3(230f, 0f, 241.75f), new Vector3(0, 180, 0));

            /*Cactus*/
            CreateWorldObject(18, region.id, new Vector3(246.5f, 0f, 236.75f), new Vector3(0, 180, 0));

            /*Red warrior*/
            //CreateWorldInteractable(Enums.InteractableType.Agent, 1, region.Id, new Vector3(238.125f, 0.1f, 239.875f), new Vector3(0, 180, 0));

            /*Ranger*/
            var rangerDestinationList = new List<InteractionDestination>()
            {
                new InteractionDestination()
                {
                    positionX = 235.625f,
                    positionY = 0.2f,
                    positionZ = 242.375f,

                    rotationX = 0,
                    rotationY = 130,
                    rotationZ = 0,

                    positionVariance = 0
                },
                new InteractionDestination()
                {
                    positionX = 230f,
                    positionY = 0.2f,
                    positionZ = 235f,

                    rotationX = 0,
                    rotationY = 130,
                    rotationZ = 0,

                    positionVariance = 0
                },
                new InteractionDestination()
                {
                    positionX = 223f,
                    positionY = 0.2f,
                    positionZ = 246f,

                    rotationX = 0,
                    rotationY = 130,
                    rotationZ = 0,

                    positionVariance = 0
                }
            };

            CreateWorldInteractable(Enums.InteractableType.Agent, 4, region.id, rangerDestinationList);

            /*Mage*/
            var mageDestinationList = new List<InteractionDestination>()
            {
                new InteractionDestination()
                {
                    positionX = 240.625f,
                    positionY = 0f,
                    positionZ = 242.375f,

                    freeRotation = true,

                    rotationX = 0,
                    rotationY = 255,
                    rotationZ = 0,

                    positionVariance = 5,
                    patience = 3
                }
            };

            CreateWorldInteractable(Enums.InteractableType.Agent, 5, region.id, mageDestinationList);

            /*Pool*/
            var poolDestinationList = new List<InteractionDestination>()
            {
                new InteractionDestination()
                {
                    positionX = 238.125f,
                    positionY = 0f,
                    positionZ = 242.375f,

                    rotationX = 0,
                    rotationY = 180,
                    rotationZ = 0,

                    positionVariance = 0
                }
            };

            CreateWorldInteractable(Enums.InteractableType.Object, 7, region.id, poolDestinationList);

            var regionSize = GetRegionSize(region.id);

            for (int i = 3; i < objectsInWorld; i++)
            {
                CreateWorldObject(Random.Range(16, 21), region.id, new Vector3(Random.Range(0, (regionSize - 1)), 0f, Random.Range(0, (regionSize - 1))), new Vector3(0, Random.Range(0, 359), 0));
            }
        }
    }

    static public void CreateWorldInteractable(Enums.InteractableType type, int interactableId, int regionId, List<InteractionDestination> interactionDestinationList)
    {
        var worldInteractable = new WorldInteractable();

        int id = worldInteractableList.Count > 0 ? (worldInteractableList[worldInteractableList.Count - 1].id + 1) : 1;

        worldInteractable.id = id;

        worldInteractable.type = (int)type;

        worldInteractable.interactableId = interactableId;
        
        for (int index = 0; index < baseTasks; index++)
        {
            CreateTask(worldInteractable, 0, index, regionId, interactionDestinationList);
        }
        
        worldInteractableList.Add(worldInteractable);
    }

    static public void CreateTask(WorldInteractable worldInteractable, int objectiveId, int taskIndex, int regionId, List<InteractionDestination> interactionDestinationList)
    {
        var task = new Task();

        int id = taskList.Count > 0 ? (taskList[taskList.Count - 1].id + 1) : 1;

        task.id = id;
        task.index = taskIndex;

        task.worldInteractableId = worldInteractable.id;
        task.objectiveId = objectiveId;
        
        task.name = "Just a task" + (objectiveId == 0 ? "" : " with an objective " + task.objectiveId);

        task.publicNotes = "I belong to Interactable " + worldInteractable.id + ". This is definitely a test";
        
        CreateInteraction(task, true, 0, 0, regionId, interactionDestinationList);
        CreateInteraction(task, false, 0, (5 * TimeManager.secondsInHour) - 1, regionId, interactionDestinationList);
        CreateInteraction(task, false, 9 * TimeManager.secondsInHour, (16 * TimeManager.secondsInHour) - 1, regionId, interactionDestinationList);
        
        taskList.Add(task);
    }

    static public void CreateInteraction(Task task, bool isDefault, int startTime, int endTime, int regionId, List<InteractionDestination> interactionDestinationList)
    {
        var interaction = new Interaction();

        int id = interactionList.Count > 0 ? (interactionList[interactionList.Count - 1].id + 1) : 1;
        
        interaction.id = id;

        interaction.taskId = task.id;

        interaction.isDefault = isDefault;

        interaction.startTime = startTime;
        interaction.endTime = endTime;

        interaction.triggerAutomatically = false;
        interaction.beNearDestination = true;
        interaction.faceAgent = true;
        interaction.facePartyLeader = false;
        interaction.hideInteractionIndicator = false;

        interaction.interactionRange = 2;

        interaction.delayMethod = 0;
        interaction.delayDuration = 0;
        interaction.hideDelayIndicator = true;

        interaction.cancelDelayOnInput = true;
        interaction.cancelDelayOnMovement = true;
        interaction.cancelDelayOnHit = false;

        interaction.publicNotes = "These are public interaction notes";

        interactionDestinationList.ForEach(interactionDestination =>
        {
            CreateInteractionDestination(interaction, regionId, interactionDestination);
        });

        CreateOutcome(interaction, Enums.OutcomeType.Positive);

        interactionList.Add(interaction);
    }

    static public void CreateInteractionDestination(Interaction interaction, int regionId, InteractionDestination interactionDestinationSource)
    {
        var interactionDestination = new InteractionDestination();

        int id = interactionDestinationList.Count > 0 ? (interactionDestinationList[interactionDestinationList.Count - 1].id + 1) : 1;

        interactionDestination.id = id;

        interactionDestination.interactionId = interaction.id;
        
        interactionDestination.positionX = interactionDestinationSource.positionX;
        interactionDestination.positionY = interactionDestinationSource.positionY;
        interactionDestination.positionZ = interactionDestinationSource.positionZ;

        interactionDestination.positionVariance = interactionDestinationSource.positionVariance;

        interactionDestination.regionId = regionId;

        interactionDestination.terrainId = GetTerrain(interactionDestination.regionId, interactionDestination.positionX, interactionDestination.positionZ);
        interactionDestination.terrainTileId = GetTerrainTile(interactionDestination.terrainId, interactionDestination.positionX, interactionDestination.positionZ);

        interactionDestination.freeRotation = interactionDestinationSource.freeRotation;

        interactionDestination.rotationX = interactionDestinationSource.rotationX;
        interactionDestination.rotationY = interactionDestinationSource.rotationY;
        interactionDestination.rotationZ = interactionDestinationSource.rotationZ;

        interactionDestination.animation = interactionDestinationSource.animation;
        interactionDestination.patience = interactionDestinationSource.patience;

        interactionDestinationList.Add(interactionDestination);
    }

    static public void CreateOutcome(Interaction interaction, Enums.OutcomeType type)
    {
        var outcome = new Outcome();

        int id = outcomeList.Count > 0 ? (outcomeList[outcomeList.Count - 1].id + 1) : 1;

        outcome.id = id;

        outcome.type = (int)type;

        outcome.interactionId = interaction.id;

        outcomeList.Add(outcome);
    }

    static public void CreateWorldObject(int objectGraphicId, int regionId, Vector3 position, Vector3 rotation)
    {
        var worldObject = new WorldObject();

        int id = worldObjectList.Count > 0 ? (worldObjectList[worldObjectList.Count - 1].id + 1) : 1;

        worldObject.id = id;

        worldObject.objectGraphicId = objectGraphicId;
        worldObject.regionId = regionId;
        
        worldObject.positionX = position.x;
        worldObject.positionY = position.y;
        worldObject.positionZ = position.z;

        worldObject.terrainId = GetTerrain(worldObject.regionId, worldObject.positionX, worldObject.positionZ);
        worldObject.terrainTileId = GetTerrainTile(worldObject.terrainId, worldObject.positionX, worldObject.positionZ);
        
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

            chapter.id = chapterId;
            chapter.index = i;

            chapter.name = "Chapter " + chapterId + " Name";

            chapter.timeSpeed = 240;

            chapter.publicNotes = "This is a pretty regular sentence. The structure is something you'd expect. Nothing too long though!";
            
            chapterList.Add(chapter);
        }
    }

    static public void LoadChapterPartyMembers()
    {
        foreach (Chapter chapter in chapterList)
        {
            for (int i = 0; i < partyMembersInChapter; i++)
            {
                var partyMember = new PartyMember();

                int id = partyMemberList.Count > 0 ? (partyMemberList[partyMemberList.Count - 1].id + 1) : 1;

                partyMember.id = id;

                partyMember.chapterId = chapter.id;

                var partyMemberInteractableId = 1;
                partyMember.interactableId = partyMemberInteractableId;

                partyMemberList.Add(partyMember);
            }
        }
    }

    static public void LoadChapterInteractables()
    {
        foreach(Chapter chapter in chapterList)
        {
            List<int> randomInteractables = new List<int>();

            var partyMemberIds = partyMemberList.Where(x => x.chapterId == chapter.id).Select(x => x.interactableId).Distinct().ToList();
            interactableList.Where(x => !partyMemberIds.Contains(x.id)).Distinct().ToList().ForEach(x => randomInteractables.Add(x.id));

            for (int i = 0; i < chapterInteractablesInChapter; i++)
            {
                var chapterInteractable = new ChapterInteractable();

                int id = chapterInteractableList.Count > 0 ? (chapterInteractableList[chapterInteractableList.Count - 1].id + 1) : 1;

                chapterInteractable.id = id;

                chapterInteractable.chapterId = chapter.id;

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

            regionList.ForEach(x => randomRegions.Add(x.id));

            //int randomRegionAmount = Random.Range(1, regionList.Count + 1);

            for (int i = 0; i < /*randomRegionAmount*/1; i++)
            {
                var chapterRegion = new ChapterRegion();

                int id = chapterRegionList.Count > 0 ? (chapterRegionList[chapterRegionList.Count - 1].id + 1) : 1;

                chapterRegion.id = id;
                chapterRegion.index = i;

                chapterRegion.chapterId = chapter.id;

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

                int id = phaseList.Count > 0 ? (phaseList[phaseList.Count - 1].id + 1) : 1;

                phase.id = id;
                phase.index = i;

                phase.chapterId = chapter.id;

                phase.name = "Phase " + (i + 1) + " Name";

                foreach (ChapterRegion chapterRegion in chapterRegionList.Where(x => x.chapterId == phase.chapterId).Distinct().ToList())
                {
                    var regionSource = regionList.Where(x => x.id == chapterRegion.regionId).FirstOrDefault();

                    var region = new Region();

                    int regionId = regionList.Count > 0 ? (regionList[regionList.Count - 1].id + 1) : 1;

                    region.id = regionId;
                    region.phaseId = phase.id;
                    region.chapterRegionId = chapterRegion.id;

                    region.index = chapterRegion.index;
                    region.tileSetId = regionSource.tileSetId;

                    region.name = regionSource.name;
                    region.regionSize = regionSource.regionSize;
                    region.terrainSize = regionSource.terrainSize;

                    regionList.Add(region);

                    //Get all world interactables belonging to this region                
                    var tempInteractionDestinationSourceList = interactionDestinationList.Where(x => x.regionId == regionSource.id).ToList();
                    var tempInteractionSourceList = interactionList.Where(x => tempInteractionDestinationSourceList.Select(y => y.interactionId).Contains(x.id)).ToList();
                    var tempTaskSourceList = taskList.Where(x => tempInteractionSourceList.Select(y => y.taskId).Contains(x.id)).ToList();
                    
                    var terrainSourceList = terrainList.Where(x => x.regionId == regionSource.id).OrderBy(x => x.index).Distinct().ToList();

                    foreach (Terrain terrainSource in terrainSourceList)
                    {
                        var terrain = new Terrain();

                        int terrainId = terrainList.Count > 0 ? (terrainList[terrainList.Count - 1].id + 1) : 1;

                        terrain.id = terrainId;
                        terrain.regionId = region.id;

                        terrain.index = terrainSource.index;

                        terrain.iconId = terrainSource.iconId;
                        terrain.name = terrainSource.name;

                        var atmosphereSourceList = atmosphereList.Where(x => x.terrainId == terrainSource.id).OrderByDescending(x => x.isDefault).ThenBy(x => x.startTime).ToList();

                        foreach (Atmosphere atmosphereSource in atmosphereSourceList)
                        {
                            CreateAtmosphere(terrain, atmosphereSource.isDefault, atmosphereSource.startTime, atmosphereSource.endTime);
                        }

                        var terrainTileSourceList = terrainTileList.Where(x => x.terrainId == terrainSource.id).OrderBy(x => x.index).Distinct().ToList();

                        foreach (TerrainTile terrainTileSource in terrainTileSourceList)
                        {
                            var terrainTile = new TerrainTile();

                            int terrainTileId = terrainTileList.Count > 0 ? (terrainTileList[terrainTileList.Count - 1].id + 1) : 1;

                            terrainTile.id = terrainTileId;
                            terrainTile.terrainId = terrain.id;

                            terrainTile.index = terrainTileSource.index;

                            terrainTile.tileId = terrainTileSource.tileId;

                            terrainTileList.Add(terrainTile);
                        }

                        terrainList.Add(terrain);
                    }

                    var worldInteractableSourceList = worldInteractableList.Where(x => tempTaskSourceList.Select(y => y.worldInteractableId).Contains(x.id)).Distinct().ToList();

                    foreach (WorldInteractable worldInteractableSource in worldInteractableSourceList)
                    {
                        var worldInteractable = new WorldInteractable();

                        int worldInteractableId = worldInteractableList.Count > 0 ? (worldInteractableList[worldInteractableList.Count - 1].id + 1) : 1;

                        worldInteractable.id = worldInteractableId;

                        worldInteractable.type = worldInteractableSource.type;

                        worldInteractable.interactableId = worldInteractableSource.interactableId;

                        var taskSourceList = taskList.Where(x => x.worldInteractableId == worldInteractableSource.id).OrderBy(x => x.index).Distinct().ToList();

                        foreach (Task taskSource in taskSourceList)
                        {
                            var task = new Task();

                            int taskId = taskList.Count > 0 ? (taskList[taskList.Count - 1].id + 1) : 1;

                            task.id = taskId;
                            task.index = taskSource.index;

                            task.worldInteractableId = worldInteractable.id;
                            task.objectiveId = taskSource.objectiveId;

                            task.name = "Just a task" + (taskSource.objectiveId == 0 ? "" : " with an objective " + task.objectiveId);

                            var interactionSourceList = interactionList.Where(x => x.taskId == taskSource.id).OrderBy(x => x.isDefault).ThenBy(x => x.startTime).Distinct().ToList();

                            foreach (Interaction interactionSource in interactionSourceList)
                            {
                                var interaction = new Interaction();

                                int interactionId = interactionList.Count > 0 ? (interactionList[interactionList.Count - 1].id + 1) : 1;

                                interaction.id = interactionId;
                                interaction.taskId = task.id;

                                interaction.isDefault = interactionSource.isDefault;

                                interaction.startTime = interactionSource.startTime;
                                interaction.endTime = interactionSource.endTime;

                                interaction.triggerAutomatically = interactionSource.triggerAutomatically;
                                interaction.beNearDestination = interactionSource.beNearDestination;
                                interaction.faceAgent = interactionSource.faceAgent;
                                interaction.facePartyLeader = interactionSource.facePartyLeader;
                                interaction.hideInteractionIndicator = interactionSource.hideInteractionIndicator;

                                interaction.interactionRange = interactionSource.interactionRange;

                                interaction.delayMethod = interactionSource.delayMethod;
                                interaction.delayDuration = interactionSource.delayDuration;
                                interaction.hideDelayIndicator = interactionSource.hideDelayIndicator;

                                interaction.cancelDelayOnInput = interactionSource.cancelDelayOnInput;
                                interaction.cancelDelayOnMovement = interactionSource.cancelDelayOnMovement;
                                interaction.cancelDelayOnHit = interactionSource.cancelDelayOnHit;

                                interaction.publicNotes = interactionSource.publicNotes;
                                interaction.privateNotes = interactionSource.privateNotes;

                                var interactionDestinationSourceList = interactionDestinationList.Where(x => x.interactionId == interactionSource.id).Distinct().ToList();

                                foreach (InteractionDestination interactionDestinationSource in interactionDestinationSourceList)
                                {
                                    var interactionDestination = new InteractionDestination();

                                    int interactionDestinationId = interactionDestinationList.Count > 0 ? (interactionDestinationList[interactionDestinationList.Count - 1].id + 1) : 1;

                                    interactionDestination.id = interactionDestinationId;

                                    interactionDestination.interactionId = interaction.id;

                                    interactionDestination.positionX = interactionDestinationSource.positionX;
                                    interactionDestination.positionY = interactionDestinationSource.positionY;
                                    interactionDestination.positionZ = interactionDestinationSource.positionZ;

                                    interactionDestination.positionVariance = interactionDestinationSource.positionVariance;

                                    interactionDestination.regionId = region.id;

                                    interactionDestination.terrainId = GetTerrain(interactionDestination.regionId, interactionDestination.positionX, interactionDestination.positionZ);
                                    interactionDestination.terrainTileId = GetTerrainTile(interactionDestination.terrainId, interactionDestination.positionX, interactionDestination.positionZ);

                                    interactionDestination.freeRotation = interactionDestinationSource.freeRotation;

                                    interactionDestination.rotationX = interactionDestinationSource.rotationX;
                                    interactionDestination.rotationY = interactionDestinationSource.rotationY;
                                    interactionDestination.rotationZ = interactionDestinationSource.rotationZ;

                                    interactionDestination.animation = interactionDestinationSource.animation;
                                    interactionDestination.patience = interactionDestinationSource.patience;

                                    interactionDestinationList.Add(interactionDestination);
                                }

                                var outcomeSourceList = outcomeList.Where(x => x.interactionId == interactionSource.id).OrderBy(x => x.type).Distinct().ToList();

                                foreach (Outcome outcomeSource in outcomeSourceList)
                                {
                                    var outcome = new Outcome();

                                    int outcomeId = outcomeList.Count > 0 ? (outcomeList[outcomeList.Count - 1].id + 1) : 1;

                                    outcome.id = outcomeId;

                                    outcome.type = outcomeSource.type;

                                    outcome.interactionId = interaction.id;

                                    outcomeList.Add(outcome);
                                }

                                interactionList.Add(interaction);
                            }

                            taskList.Add(task);
                        }

                        worldInteractableList.Add(worldInteractable);
                    }

                    var worldObjectSourceList = worldObjectList.Where(x => x.regionId == regionSource.id).Distinct().ToList();

                    foreach (WorldObject worldObjectSource in worldObjectSourceList)
                    {
                        var worldObject = new WorldObject();

                        int worldObjectId = worldObjectList.Count > 0 ? (worldObjectList[worldObjectList.Count - 1].id + 1) : 1;

                        worldObject.id = worldObjectId;
                        worldObject.regionId = region.id;

                        worldObject.positionX = worldObjectSource.positionX;
                        worldObject.positionY = worldObjectSource.positionY;
                        worldObject.positionZ = worldObjectSource.positionZ;

                        worldObject.terrainId = GetTerrain(worldObject.regionId, worldObject.positionX, worldObject.positionZ);
                        worldObject.terrainTileId = GetTerrainTile(worldObject.terrainId, worldObject.positionX, worldObject.positionZ);

                        worldObject.rotationX = worldObjectSource.rotationX;
                        worldObject.rotationY = worldObjectSource.rotationY;
                        worldObject.rotationZ = worldObjectSource.rotationZ;

                        worldObject.scaleMultiplier = worldObjectSource.scaleMultiplier;

                        worldObject.objectGraphicId = worldObjectSource.objectGraphicId;

                        worldObjectList.Add(worldObject);
                    }
                }
                
                phase.defaultRegionId = regionList.Where(x => x.phaseId == phase.id).First().id;

                phase.defaultPositionX = 238.125f;
                phase.defaultPositionY = 0.1f;
                phase.defaultPositionZ = 245.5f;

                phase.defaultRotationX = 0;
                phase.defaultRotationY = 0;
                phase.defaultRotationZ = 0;

                phase.defaultTime = 7 * TimeManager.secondsInHour;

                phase.publicNotes = "I belong to Chapter " + chapter.id + ". This is definitely a test";
                
                phaseList.Add(phase);
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

                int id = questList.Count > 0 ? (questList[questList.Count - 1].id + 1) : 1;

                quest.id = id;
                quest.index = i;

                quest.phaseId = phase.id;
                quest.name = "Quest " + (i + 1) + " Name";
                quest.publicNotes = "I belong to Phase " + phase.id + ". This is definitely a test";
                
                questList.Add(quest);
            }
        }
    }

    static public void LoadPhaseWorldInteractables()
    {
        foreach (Chapter chapter in chapterList)
        {
            foreach (Phase phase in phaseList.Where(x => x.chapterId == chapter.id).Distinct().ToList())
            {
                var chapterInteractables = chapterInteractableList.Where(x => x.chapterId == chapter.id).Distinct().ToList();
                var questIds = questList.Where(x => x.phaseId == phase.id).Select(x => x.id).Distinct().ToList();

                for (int i = 0; i < chapterInteractables.Count; i++)
                {
                    var worldInteractable = new WorldInteractable();

                    int id = worldInteractableList.Count > 0 ? (worldInteractableList[worldInteractableList.Count - 1].id + 1) : 1;

                    worldInteractable.id = id;

                    worldInteractable.type = (int)Enums.InteractableType.Agent;

                    var chapterInteractable = chapterInteractables[i];

                    worldInteractable.chapterInteractableId = chapterInteractable.id;
                    worldInteractable.phaseId = phase.id;
                    
                    int randomQuestId = Random.Range(0, questIds.Count);

                    worldInteractable.questId = questIds[randomQuestId];

                    questIds.RemoveAt(randomQuestId);

                    worldInteractable.interactableId = chapterInteractable.interactableId;

                    worldInteractableList.Add(worldInteractable);
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

                int id = objectiveList.Count > 0 ? (objectiveList[objectiveList.Count - 1].id + 1) : 1;

                objective.id = id;
                objective.index = i;

                objective.questId = quest.id;
                objective.name = "Objective " + (i + 1) + " Name";
                objective.publicNotes = "I belong to Quest " + quest.id + ". This is definitely a test";
                
                objectiveList.Add(objective);
            }
        }
    }
    
    static public void LoadObjectiveWorldInteractables()
    {
        foreach (Objective objective in objectiveList)
        {
            List<int> randomInteractables = new List<int>();

            interactableList.ForEach(x => randomInteractables.Add(x.id));

            for (int i = 0; i < interactablesInObjective; i++)
            {
                var worldInteractable = new WorldInteractable();

                int id = worldInteractableList.Count > 0 ? (worldInteractableList[worldInteractableList.Count - 1].id + 1) : 1;

                worldInteractable.id = id;
                worldInteractable.index = i;

                worldInteractable.type = (int)Enums.InteractableType.Object;

                worldInteractable.objectiveId = objective.id;

                int randomInteractable = Random.Range(0, randomInteractables.Count);
                worldInteractable.interactableId = randomInteractables[randomInteractable];

                worldInteractableList.Add(worldInteractable);
            }
        }
    }

    static public void LoadTasks()
    {
        foreach (Objective objective in objectiveList)
        {
            var questWorldInteractables = worldInteractableList.Where(x => x.questId == objective.questId).Distinct().ToList();
            var objectiveWorldInteractables = worldInteractableList.Where(x => x.objectiveId == objective.id).Distinct().ToList();
            
            var worldInteractables = questWorldInteractables.Concat(objectiveWorldInteractables);

            var phaseId = phaseList.Where(x => questList.Where(y => y.id == objective.questId).Select(y => y.phaseId).Contains(x.id)).Select(x => x.id).FirstOrDefault();
            var regions = regionList.Where(x => x.phaseId == phaseId).Distinct().ToList();
            
            foreach (WorldInteractable worldInteractable in worldInteractables)
            {
                for (int index = 0; index < baseTasks; index++)
                {
                    var randomRegion = regions[Random.Range(0, regions.Count)];

                    var regionSize = GetRegionSize(randomRegion.id);

                    var randomPosition = new Vector3(Random.Range(0, (regionSize - 1)), 0, Random.Range(0, (regionSize - 1)));
                    var rotation = new Vector3(0, 180, 0);

                    var randomDestinationList = new List<InteractionDestination>()
                    {
                        new InteractionDestination()
                        {
                            positionX = randomPosition.x,
                            positionY = randomPosition.y,
                            positionZ = randomPosition.z,

                            rotationX = 0,
                            rotationY = 180,
                            rotationZ = 0,

                            positionVariance = 0
                        }
                    };

                    CreateTask(worldInteractable, objective.id, index, randomRegion.id, randomDestinationList);
                }
            }
        }
    }
    
    static public float GetRegionSize(int regionId)
    {
        var region = regionList.Where(x => x.id == regionId).FirstOrDefault();
        var tileSet = tileSetList.Where(x => x.id == region.tileSetId).FirstOrDefault();
        var terrains = terrainList.Where(x => x.regionId == region.id).Distinct().ToList();

        var regionSize = region.regionSize * region.terrainSize * tileSet.tileSize;

        return regionSize;
    }

    static public int GetTerrain(int regionId, float posX, float posZ)
    {
        var region = regionList.Where(x => x.id == regionId).FirstOrDefault();
        var tileSet = tileSetList.Where(x => x.id == region.tileSetId).FirstOrDefault();
        var terrains = terrainList.Where(x => x.regionId == region.id).Distinct().ToList();

        var terrainSize = region.terrainSize * tileSet.tileSize;

        var terrainCoordinates = new Vector2(Mathf.Floor(posX / terrainSize),
                                             Mathf.Floor(posZ / terrainSize));

        var terrainIndex = (region.regionSize * terrainCoordinates.y) + terrainCoordinates.x;

        var terrainId = terrains.Where(x => x.index == terrainIndex).Select(x => x.id).FirstOrDefault();

        return terrainId;
    }

    static public int GetTerrainTile(int terrainId, float posX, float posZ)
    {
        var terrain = terrainList.Where(x => x.id == terrainId).FirstOrDefault();
        var region = regionList.Where(x => x.id == terrain.regionId).FirstOrDefault();
        var tileSet = tileSetList.Where(x => x.id == region.tileSetId).FirstOrDefault();
        
        var terrainSize = region.terrainSize * tileSet.tileSize;

        var terrainCoordinates = new Vector2(Mathf.Floor(posX / terrainSize),
                                             Mathf.Floor(posZ / terrainSize));

        var terrainTiles = terrainTileList.Where(x => x.terrainId == terrainId).Distinct().ToList();

        var terrainPosition = new Vector2(terrainCoordinates.x * terrainSize,
                                          terrainCoordinates.y * terrainSize);

        var localPosition = new Vector2(posX - terrainPosition.x,
                                        posZ - terrainPosition.y);

        var tileCoordinates = new Vector2(Mathf.Floor(localPosition.x / tileSet.tileSize),
                                          Mathf.Floor(localPosition.y / tileSet.tileSize));

        var tileIndex = (region.terrainSize * tileCoordinates.y) + tileCoordinates.x;

        var terrainTileId = terrainTiles.Where(x => x.index == tileIndex).Select(x => x.id).FirstOrDefault();

        return terrainTileId;
    }

    static public void CreateSaveFile()
    {
        CreateSave();
    }

    static public void CreateSave()
    {
        var save = new Save();

        int id = saveList.Count > 0 ? (saveList[saveList.Count - 1].id + 1) : 1;

        save.id = id;
        save.index = saveList.Count;

        CreatePlayerSave(save);
        CreateInteractableSaves(save);
        CreateStageSaves(save);

        saveList.Add(save);
    }

    static private void CreatePlayerSave(Save save)
    {
        var playerSave = new PlayerSave();

        int playerSaveId = playerSaveList.Count > 0 ? (playerSaveList[playerSaveList.Count - 1].id + 1) : 1;

        playerSave.id = playerSaveId;

        playerSave.saveId = save.id;

        var firstChapter        = chapterList.OrderBy(x => x.index).First();
        var firstPhase          = phaseList.Where(x => x.chapterId == firstChapter.id).OrderBy(x => x.index).First();
        var firstPartyMember    = partyMemberList.Where(x => x.chapterId == firstChapter.id).First();

        playerSave.regionId = firstPhase.defaultRegionId;
        playerSave.partyMemberId = firstPartyMember.id;

        playerSave.positionX = firstPhase.defaultPositionX;
        playerSave.positionY = firstPhase.defaultPositionY;
        playerSave.positionZ = firstPhase.defaultPositionZ;

        playerSave.scaleMultiplier = 1;

        playerSave.gameTime = firstPhase.defaultTime;

        //Test
        //playerSave.playedSeconds = 123456;
        //34 hours
        //17 minutes
        //36 seconds
        //----

        playerSaveList.Add(playerSave);
    }

    static private void CreateInteractableSaves(Save save)
    {
        foreach(Interactable interactable in interactableList)
        {
            var interactableSave = new InteractableSave();

            int interactableSaveId = interactableSaveList.Count > 0 ? (interactableSaveList[interactableSaveList.Count - 1].id + 1) : 1;

            interactableSave.id = interactableSaveId;
            interactableSave.index = interactable.index;

            interactableSave.saveId = save.id;
            interactableSave.interactableId = interactable.id;

            interactableSaveList.Add(interactableSave);
        }
    }

    static private void CreateStageSaves(Save save)
    {
        LoadChapterSaves(save);
        LoadPhaseRegionWorldInteractableTasks();
    }

    static public void LoadChapterSaves(Save save)
    {
        foreach (Chapter chapter in chapterList)
        {
            var chapterSave = new ChapterSave();

            int chapterSaveId = chapterSaveList.Count > 0 ? (chapterSaveList[chapterSaveList.Count - 1].id + 1) : 1;

            chapterSave.id = chapterSaveId;

            chapterSave.saveId = save.id;
            chapterSave.chapterId = chapter.id;

            LoadPhaseSaves(chapter, chapterSave);

            chapterSaveList.Add(chapterSave);
        }
    }

    static public void LoadPhaseSaves(Chapter chapter, ChapterSave chapterSave)
    {
        foreach(Phase phase in phaseList.Where(x => x.chapterId == chapter.id))
        {
            var phaseSave = new PhaseSave();

            int id = phaseSaveList.Count > 0 ? (phaseSaveList[phaseSaveList.Count - 1].id + 1) : 1;

            phaseSave.id = id;

            phaseSave.saveId = chapterSave.saveId;
            phaseSave.chapterSaveId = chapterSave.id;
            phaseSave.phaseId = phase.id;

            LoadQuestSaves(phase, phaseSave);

            phaseSaveList.Add(phaseSave);
        }
    }

    static private void LoadPhaseRegionWorldInteractableTasks()
    {
        foreach (PhaseSave phaseSave in phaseSaveList)
        {
            var phaseRegionSourceList = regionList.Where(x => x.phaseId == phaseSave.phaseId);

            foreach (Region phaseRegionSource in phaseRegionSourceList)
            {
                var interactionIdList = interactionDestinationList.Where(x => x.regionId == phaseRegionSource.id).Select(x => x.interactionId).ToList();
                var taskIdList = interactionList.Where(x => interactionIdList.Contains(x.id)).Select(x => x.taskId).ToList();
                var worldInteractableIdList = taskList.Where(x => taskIdList.Contains(x.id)).Select(x => x.worldInteractableId).ToList();
                var worldInteractableSourceList = worldInteractableList.Where(x => worldInteractableIdList.Contains(x.id)).Distinct().ToList();
                
                foreach (WorldInteractable worldInteractableSource in worldInteractableSourceList)
                {
                    LoadWorldInteractableTaskSaves(worldInteractableSource, phaseSave);
                }
            }
        }
    }

    static public void LoadWorldInteractableTaskSaves(WorldInteractable worldInteractable, PhaseSave phaseSave)
    {
        foreach (Task task in taskList.Where(x => x.worldInteractableId == worldInteractable.id))
        {
            var taskSave = new TaskSave();

            int id = taskSaveList.Count > 0 ? (taskSaveList[taskSaveList.Count - 1].id + 1) : 1;

            taskSave.id = id;

            taskSave.saveId = phaseSave.saveId;
            taskSave.worldInteractableId = worldInteractable.id;
            taskSave.taskId = task.id;

            LoadInteractionSaves(task, taskSave);

            taskSaveList.Add(taskSave);
        }
    }

    static public void LoadQuestSaves(Phase phase, PhaseSave phaseSave)
    {
        foreach (Quest quest in questList.Where(x => x.phaseId == phase.id))
        {
            var questSave = new QuestSave();

            int id = questSaveList.Count > 0 ? (questSaveList[questSaveList.Count - 1].id + 1) : 1;

            questSave.id = id;

            questSave.saveId = phaseSave.saveId;
            questSave.phaseSaveId = phaseSave.id;
            questSave.questId = quest.id;

            LoadObjectiveSaves(quest, questSave);

            questSaveList.Add(questSave);
        }
    }

    static public void LoadObjectiveSaves(Quest quest, QuestSave questSave)
    {
        foreach (Objective objective in objectiveList.Where(x => x.questId == quest.id))
        {
            var objectiveSave = new ObjectiveSave();

            int id = objectiveSaveList.Count > 0 ? (objectiveSaveList[objectiveSaveList.Count - 1].id + 1) : 1;

            objectiveSave.id = id;

            objectiveSave.saveId = questSave.saveId;
            objectiveSave.questSaveId = questSave.id;
            objectiveSave.objectiveId = objective.id;
            
            LoadObjectiveTaskSaves(objective, objectiveSave);

            objectiveSaveList.Add(objectiveSave);
        }
    }

    static public void LoadObjectiveTaskSaves(Objective objective, ObjectiveSave objectiveSave)
    {
        foreach (Task task in taskList.Where(x => x.objectiveId == objective.id))
        {
            var taskSave = new TaskSave();

            int id = taskSaveList.Count > 0 ? (taskSaveList[taskSaveList.Count - 1].id + 1) : 1;

            taskSave.id = id;

            taskSave.saveId = objectiveSave.saveId;
            taskSave.worldInteractableId = task.worldInteractableId;
            taskSave.objectiveSaveId = objectiveSave.id;
            taskSave.taskId = task.id;

            LoadInteractionSaves(task, taskSave);

            taskSaveList.Add(taskSave);
        }
    }
    
    static public void LoadInteractionSaves(Task task, TaskSave taskSave)
    {
        foreach (Interaction interaction in interactionList.Where(x => x.taskId == task.id))
        {
            var interactionSave = new InteractionSave();

            int id = interactionSaveList.Count > 0 ? (interactionSaveList[interactionSaveList.Count - 1].id + 1) : 1;

            interactionSave.id = id;

            interactionSave.saveId = taskSave.saveId;
            interactionSave.taskSaveId = taskSave.id;
            interactionSave.interactionId = interaction.id;

            interactionSaveList.Add(interactionSave);
        }
    }
    
    static private void Query()
    {

    }
}
