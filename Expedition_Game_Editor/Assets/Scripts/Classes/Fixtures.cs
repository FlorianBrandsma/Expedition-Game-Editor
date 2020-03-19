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
    static public int worldInteractablesInChapter = 3;
    static public int phasesInChapter = 4;
    static public int questsInPhase = 4;
    static public int objectivesInQuest = 3;
    static public int interactablesInObjective = 2;
    static public int sideSceneInteractables = 1;
    static public int interactionsInSceneInteractables = 3;
    static public int baseInteractions = 2;
    static public int tileSets = 2;
    static public int tilesInTileSet = 10;
    static public int objectsInScene = 3;

    static public List<Icon> iconList = new List<Icon>();
    static public List<ObjectGraphic> objectGraphicList = new List<ObjectGraphic>();
    static public List<Item> itemList = new List<Item>();
    static public List<Interactable> interactableList = new List<Interactable>();
    static public List<TileSet> tileSetList = new List<TileSet>();
    static public List<Tile> tileList = new List<Tile>();
    static public List<Region> regionList = new List<Region>();
    static public List<Terrain> terrainList = new List<Terrain>();
    static public List<TerrainTile> terrainTileList = new List<TerrainTile>();
    static public List<Chapter> chapterList = new List<Chapter>();
    static public List<ChapterRegion> chapterRegionList = new List<ChapterRegion>();
    static public List<Phase> phaseList = new List<Phase>();
    static public List<PhaseInteractable> phaseInteractableList = new List<PhaseInteractable>();
    static public List<Quest> questList = new List<Quest>();
    static public List<Objective> objectiveList = new List<Objective>();
    static public List<Interaction> interactionList = new List<Interaction>();

    static public List<PartyMember> partyMemberList = new List<PartyMember>();
    static public List<SceneInteractable> sceneInteractableList = new List<SceneInteractable>();
    static public List<SceneObject> sceneObjectList = new List<SceneObject>();

    public class Item : GeneralData
    {
        public int objectGraphicId;
        public int type;
        public string name;
    }

    public class Interactable : GeneralData
    {
        public int objectGraphicId;
        public string name;
    }

    public class PartyMember : GeneralData
    {
        public int chapterId;
        public int interactableId;
    }

    public class SceneObject : GeneralData
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

    public class SceneInteractable : GeneralData
    {
        public int chapterId;
        public int objectiveId;
        public int interactableId;
        public int interactionIndex;
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
        public int interactableId;
        public string name;
        public string notes;
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
        public string notes;
    }

    public class PhaseInteractable : GeneralData
    {
        public int phaseId;
        public int questId;
        public int sceneInteractableId;
    }

    public class Quest : GeneralData
    {
        public int phaseId;
        public string name;
        public string notes;
    }

    public class Objective : GeneralData
    {
        public int questId;
        public string name;
        public string journal;
        public string notes;
    }
    
    public class Interaction : GeneralData
    {
        public int sceneInteractableId;
        public int objectiveId;
        public int regionId;
        public int terrainId;
        public int terrainTileId;

        public string description;

        public float positionX;
        public float positionY;
        public float positionZ;

        public int rotationX;
        public int rotationY;
        public int rotationZ;

        public float scaleMultiplier;

        public int animation;
    }

    static public void LoadFixtures()
    {
        LoadIcons();
        LoadObjectGraphics();
        LoadTileSets();
        LoadTiles();
        LoadItems();
        LoadInteractables();
        LoadRegions();
        LoadTerrains();
        LoadTerrainTiles();
        LoadSceneObjects();
        LoadChapters();
        LoadChapterPartyMembers();
        LoadChapterSceneInteractables();
        LoadChapterRegions();
        LoadPhases();
        LoadPhaseRegions();
        LoadQuests();
        LoadPhaseInteractables();
        LoadObjectives();
        LoadObjectiveSceneInteractables();
        LoadInteractions();

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

            item.type = (int)Enums.ItemType.Supplies;
            item.Index = index;

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

            item.type = (int)Enums.ItemType.Gear;
            item.Index = index;

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

            item.type = (int)Enums.ItemType.Spoils;
            item.Index = index;

            item.objectGraphicId = 1;
            item.name = "Item " + id;

            itemList.Add(item);

            index++;
        }
    }

    static public void LoadInteractables()
    {
        var objectList = new List<int> { 10, 11, 12, 13, 14, 15 };

        for (int i = 0; i < objectList.Count; i++)
        {
            var interactable = new Interactable();

            int id = (i + 1);

            interactable.Id = id;
            interactable.Index = i;

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

                terrainList.Add(terrain);
            }
        }
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

    static public void LoadSceneObjects()
    {
        foreach (Region region in regionList)
        {
            var terrains = terrainList.Where(x => x.regionId == region.Id).Distinct().ToList();
            var middleTerrain = terrains[terrains.Count / 2];
            var terrainTiles = terrainTileList.Where(x => x.terrainId == (middleTerrain.Id)).Distinct().ToList();
            var middleTile = terrainTiles[terrainTiles.Count / 2];

            /*Skull*/
            CreateSceneObject(16, 0, region.Id, new Vector3(245f, 236.5f, -0.2f), new Vector3(5, 0, 25));

            /*Rock*/
            CreateSceneObject(17, 1, region.Id, new Vector3(230f, 241.75f, 0f), new Vector3(0, 0, 180f));

            /*Cactus*/
            CreateSceneObject(18, 2, region.Id, new Vector3(246.5f, 236.75f, 0f), new Vector3(0, 0, 0));

            /*Red warrior*/
            CreateSceneInteractable(1, region.Id, new Vector3(238.125f, 239.875f, 0.1f), new Vector3(0, 0, 0));

            /*Ranger*/
            CreateSceneInteractable(4, region.Id, new Vector3(235.625f, 242.375f, 0.2f), new Vector3(0, 0, 125));

            /*Mage*/
            CreateSceneInteractable(5, region.Id, new Vector3(240.625f, 242.375f, 0f), new Vector3(0, 0, 235));

            var regionSize = GetRegionSize(region.Id);

            for (int i = 3; i < objectsInScene; i++)
            {
                CreateSceneObject(Random.Range(16, 21), i, region.Id, new Vector3(Random.Range(0, (regionSize - 1)), Random.Range(0, (regionSize - 1)), 0f), new Vector3(0, 0, Random.Range(0, 359)));
            }
        }
    }

    static public void CreateSceneInteractable(int interactableId, int regionId, Vector3 position, Vector3 rotation)
    {
        var sceneInteractable = new SceneInteractable();

        int id = sceneInteractableList.Count > 0 ? (sceneInteractableList[sceneInteractableList.Count - 1].Id + 1) : 1;

        sceneInteractable.Id = id;
        sceneInteractable.interactableId = interactableId;

        for(int index = 0; index < interactionsInSceneInteractables; index++)
        {
            CreateInteraction(sceneInteractable, index, regionId, position, rotation);
        }
        
        sceneInteractableList.Add(sceneInteractable);
    }

    static public void CreateInteraction(SceneInteractable sceneInteractable, int index, int regionId, Vector3 position, Vector3 rotation)
    {
        var interaction = new Interaction();

        int id = interactionList.Count > 0 ? (interactionList[interactionList.Count - 1].Id + 1) : 1;
        
        interaction.Id = id;
        interaction.Index = index;

        interaction.sceneInteractableId = sceneInteractable.Id;
        interaction.regionId = regionId;

        interaction.description = "Talk to {" + sceneInteractable.interactableId + "}";

        interaction.positionX = position.x;
        interaction.positionY = position.y;
        interaction.positionZ = position.z;

        interaction.terrainId = GetTerrain(interaction.regionId, interaction.positionX, interaction.positionY);
        interaction.terrainTileId = GetTerrainTile(interaction.terrainId, interaction.positionX, interaction.positionY);

        interaction.rotationX = (int)rotation.x;
        interaction.rotationY = (int)rotation.y;
        interaction.rotationZ = (int)rotation.z;

        interaction.scaleMultiplier = 1;
        
        interactionList.Add(interaction);
    }

    static public void CreateSceneObject(int objectGraphicId, int index, int regionId, Vector3 position, Vector3 rotation)
    {
        var sceneObject = new SceneObject();

        int id = sceneObjectList.Count > 0 ? (sceneObjectList[sceneObjectList.Count - 1].Id + 1) : 1;

        sceneObject.Id = id;
        sceneObject.Index = index;

        sceneObject.objectGraphicId = objectGraphicId;
        sceneObject.regionId = regionId;
        
        sceneObject.positionX = position.x;
        sceneObject.positionY = position.y;
        sceneObject.positionZ = position.z;

        sceneObject.terrainId = GetTerrain(sceneObject.regionId, sceneObject.positionX, sceneObject.positionY);
        sceneObject.terrainTileId = GetTerrainTile(sceneObject.terrainId, sceneObject.positionX, sceneObject.positionY);
        
        sceneObject.rotationX = (int)rotation.x;
        sceneObject.rotationY = (int)rotation.y;
        sceneObject.rotationZ = (int)rotation.z;

        sceneObject.scaleMultiplier = 1;

        sceneObjectList.Add(sceneObject);
    }

    static public void LoadChapters()
    {
        List<int> randomInteractables = new List<int>();

        interactableList.ForEach(x => randomInteractables.Add(x.Id));

        for (int i = 0; i < chapters; i++)
        {
            var chapter = new Chapter();

            int id = (i + 1);

            chapter.Id = id;
            chapter.Index = i;

            int randomInteractable = Random.Range(0, randomInteractables.Count);

            chapter.interactableId = randomInteractables[randomInteractable];

            chapter.name = "Chapter " + id;
            chapter.notes = "This is a pretty regular sentence. The structure is something you'd expect. Nothing too long though!";

            chapterList.Add(chapter);
        }
    }

    static public void LoadChapterPartyMembers()
    {
        foreach (Chapter chapter in chapterList)
        {
            List<int> randomInteractables = new List<int>();

            var sceneInteractableIds = sceneInteractableList.Where(x => x.chapterId == chapter.Id).Select(x => x.interactableId).Distinct().ToList();
            interactableList.Where(x => !sceneInteractableIds.Contains(x.Id)).Distinct().ToList().ForEach(x => randomInteractables.Add(x.Id));

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

    static public void LoadChapterSceneInteractables()
    {
        foreach(Chapter chapter in chapterList)
        {
            List<int> randomInteractables = new List<int>();

            var partyMemberIds = partyMemberList.Where(x => x.chapterId == chapter.Id).Select(x => x.interactableId).Distinct().ToList();
            interactableList.Where(x => !partyMemberIds.Contains(x.Id)).Distinct().ToList().ForEach(x => randomInteractables.Add(x.Id));

            for (int i = 0; i < worldInteractablesInChapter; i++)
            {
                var chapterInteractable = new SceneInteractable();

                int id = sceneInteractableList.Count > 0 ? (sceneInteractableList[sceneInteractableList.Count - 1].Id + 1) : 1;

                chapterInteractable.Id = id;
                chapterInteractable.Index = i;

                chapterInteractable.chapterId = chapter.Id;

                int randomInteractable = Random.Range(0, randomInteractables.Count);

                chapterInteractable.interactableId = randomInteractables[randomInteractable];

                randomInteractables.RemoveAt(randomInteractable);

                sceneInteractableList.Add(chapterInteractable);
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
                phase.notes = "I belong to Chapter " + chapter.Id + ". This is definitely a test";

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

                var sceneInteractableSourceList = sceneInteractableList.Where(x => interactionList.Where(y => y.regionId == regionSource.Id).Select(y => y.sceneInteractableId).Contains(x.Id)).Distinct().ToList();
                
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

                foreach (SceneInteractable sceneInteractableSource in sceneInteractableSourceList)
                {
                    var sceneInteractable = new SceneInteractable();

                    int sceneInteractableId = sceneInteractableList.Count > 0 ? (sceneInteractableList[sceneInteractableList.Count - 1].Id + 1) : 1;

                    sceneInteractable.Id = sceneInteractableId;

                    sceneInteractable.chapterId = sceneInteractableSource.chapterId;
                    sceneInteractable.objectiveId = sceneInteractableSource.objectiveId;
                    sceneInteractable.interactableId = sceneInteractableSource.interactableId;
                    sceneInteractable.interactionIndex = sceneInteractableSource.interactionIndex;

                    var interactionSourceList = interactionList.Where(x => x.sceneInteractableId == sceneInteractableSource.Id).OrderBy(x => x.Index).Distinct().ToList();

                    foreach (Interaction interactionSource in interactionSourceList)
                    {
                        var interaction = new Interaction();

                        int interactionId = interactionList.Count > 0 ? (interactionList[interactionList.Count - 1].Id + 1) : 1;

                        interaction.Id = interactionId;
                        interaction.sceneInteractableId = sceneInteractable.Id;
                        interaction.objectiveId = interactionSource.objectiveId;
                        interaction.regionId = region.Id;

                        interaction.Index = interactionSource.Index;
                        interaction.description = interactionSource.description;

                        interaction.positionX = interactionSource.positionX;
                        interaction.positionY = interactionSource.positionY;
                        interaction.positionZ = interactionSource.positionZ;

                        interaction.terrainId = GetTerrain(interaction.regionId, interaction.positionX, interaction.positionY);
                        interaction.terrainTileId = GetTerrainTile(interaction.terrainId, interaction.positionX, interaction.positionY);

                        interaction.rotationX = interactionSource.rotationX;
                        interaction.rotationY = interactionSource.rotationY;
                        interaction.rotationZ = interactionSource.rotationZ;

                        interaction.scaleMultiplier = interactionSource.scaleMultiplier;

                        interactionList.Add(interaction);
                    }

                    sceneInteractableList.Add(sceneInteractable);
                }

                var sceneObjectSourceList = sceneObjectList.Where(x => x.regionId == regionSource.Id).Distinct().ToList();

                foreach (SceneObject sceneObjectSource in sceneObjectSourceList)
                {
                    var sceneObject = new SceneObject();

                    int sceneObjectId = sceneObjectList.Count > 0 ? (sceneObjectList[sceneObjectList.Count - 1].Id + 1) : 1;

                    sceneObject.Id = sceneObjectId;
                    sceneObject.regionId = region.Id;

                    sceneObject.positionX = sceneObjectSource.positionX;
                    sceneObject.positionY = sceneObjectSource.positionY;
                    sceneObject.positionZ = sceneObjectSource.positionZ;

                    sceneObject.terrainId = GetTerrain(sceneObject.regionId, sceneObject.positionX, sceneObject.positionY);
                    sceneObject.terrainTileId = GetTerrainTile(sceneObject.terrainId, sceneObject.positionX, sceneObject.positionY);

                    sceneObject.rotationX = sceneObjectSource.rotationX;
                    sceneObject.rotationY = sceneObjectSource.rotationY;
                    sceneObject.rotationZ = sceneObjectSource.rotationZ;

                    sceneObject.scaleMultiplier = sceneObjectSource.scaleMultiplier;

                    sceneObject.Index = sceneObjectSource.Index;
                    sceneObject.objectGraphicId = sceneObjectSource.objectGraphicId;
                    
                    sceneObjectList.Add(sceneObject);
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
                quest.notes = "I belong to Phase " + phase.Id + ". This is definitely a test";

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
                var chapterInteractables = sceneInteractableList.Where(x => x.chapterId == chapter.Id).Distinct().ToList();
                var questIds = questList.Where(x => x.phaseId == phase.Id).Select(x => x.Id).Distinct().ToList();

                for (int i = 0; i < chapterInteractables.Count; i++)
                {
                    var phaseInteractable = new PhaseInteractable();

                    int id = phaseInteractableList.Count > 0 ? (phaseInteractableList[phaseInteractableList.Count - 1].Id + 1) : 1;

                    phaseInteractable.Id = id;

                    phaseInteractable.phaseId = phase.Id;
                    phaseInteractable.sceneInteractableId = chapterInteractables[i].Id;

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
                objective.notes = "I belong to Quest " + quest.Id + ". This is definitely a test";

                objectiveList.Add(objective);
            }
        }
    }

    static public void LoadObjectiveSceneInteractables()
    {
        foreach (Objective objective in objectiveList)
        {
            List<int> randomInteractables = new List<int>();

            interactableList.ForEach(x => randomInteractables.Add(x.Id));

            for(int i = 0; i < interactablesInObjective; i++)
            {
                var objectiveInteractable = new SceneInteractable();

                int id = sceneInteractableList.Count > 0 ? (sceneInteractableList[sceneInteractableList.Count - 1].Id + 1) : 1;

                objectiveInteractable.Id = id;
                objectiveInteractable.Index = i;

                objectiveInteractable.objectiveId = objective.Id;

                int randomInteractable = Random.Range(0, randomInteractables.Count);
                objectiveInteractable.interactableId = randomInteractables[randomInteractable];

                sceneInteractableList.Add(objectiveInteractable);
            }
        }
    }

    static public void LoadInteractions()
    {
        foreach (Objective objective in objectiveList)
        {
            var phaseInteractableSceneInteractableIds = phaseInteractableList.Where(x => x.questId == objective.questId).Select(x => x.sceneInteractableId).Distinct().ToList();
            var sceneInteractables = sceneInteractableList.Where(x => phaseInteractableSceneInteractableIds.Contains(x.Id) || x.objectiveId == objective.Id).Distinct().ToList();

            var phaseId = phaseList.Where(x => questList.Where(y => y.Id == objective.questId).Select(y => y.phaseId).Contains(x.Id)).Select(x => x.Id).FirstOrDefault();
            var regions = regionList.Where(x => x.phaseId == phaseId).Distinct().ToList();

            foreach(SceneInteractable sceneInteractable in sceneInteractables)
            {
                for (int i = 0; i < baseInteractions; i++)
                {
                    var interaction = new Interaction();

                    int id = interactionList.Count > 0 ? (interactionList[interactionList.Count - 1].Id + 1) : 1;

                    interaction.Id = id;
                    interaction.Index = i;

                    interaction.objectiveId = objective.Id;
                    interaction.sceneInteractableId = sceneInteractable.Id;

                    int randomRegion = Random.Range(0, regions.Count);

                    interaction.regionId = regions[randomRegion].Id;

                    var regionSize = GetRegionSize(interaction.regionId);

                    interaction.description = "Basically a task description. Property of objective" + objective.Id;

                    interaction.positionX = Random.Range(0, (regionSize - 1));
                    interaction.positionY = Random.Range(0, (regionSize - 1));
                    interaction.positionZ = 0;

                    interaction.terrainId = GetTerrain(interaction.regionId, interaction.positionX, interaction.positionY);
                    interaction.terrainTileId = GetTerrainTile(interaction.terrainId, interaction.positionX, interaction.positionY);
                    
                    interaction.scaleMultiplier = 1;

                    interactionList.Add(interaction);
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

    static private void Query() { }
}
