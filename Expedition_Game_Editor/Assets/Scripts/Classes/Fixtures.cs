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
    static public int baseInteractions = 4;
    static public int tileSets = 2;
    static public int tilesInTileSet = 10;
    static public int baseSceneObjects = 5;

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
        /*01*/CreateObjectGraphic("Nothing",        1);
        /*02*/CreateObjectGraphic("Polearm",        2);
        /*03*/CreateObjectGraphic("Mighty Polearm", 3);
        /*04*/CreateObjectGraphic("Shortbow",       4);
        /*05*/CreateObjectGraphic("Longbow",        5);
        /*06*/CreateObjectGraphic("Crossbow",       6);
        /*07*/CreateObjectGraphic("Strong Crossbow",7);
        /*08*/CreateObjectGraphic("Staff",          8);
        /*09*/CreateObjectGraphic("Menacing Staff", 9);
        /*10*/CreateObjectGraphic("Red Warrior",    10);
        /*11*/CreateObjectGraphic("Blue Warrior",   11);
        /*12*/CreateObjectGraphic("Green Warrior",  12);
        /*13*/CreateObjectGraphic("Ranger",         13);
        /*14*/CreateObjectGraphic("Mage",           14);
        /*15*/CreateObjectGraphic("Drake",          15);
        /*16*/CreateObjectGraphic("Skull",          17);
        /*17*/CreateObjectGraphic("Rock",           18);
        /*18*/CreateObjectGraphic("Cactus",         19);
        /*19*/CreateObjectGraphic("Tree",           20);
        /*20*/CreateObjectGraphic("Pool",           21);
    }

    static public void CreateObjectGraphic(string name, int iconId)
    {
        var objectGraphic = new ObjectGraphic();

        int id = objectGraphicList.Count > 0 ? (objectGraphicList[objectGraphicList.Count - 1].id + 1) : 1;

        objectGraphic.id = id;
        objectGraphic.iconId = iconId;
        objectGraphic.name = name;
        objectGraphic.path = "Objects/" + name;

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

            item.type = (int)Enums.ItemType.Supplies;
            item.index = index;

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

            item.type = (int)Enums.ItemType.Gear;
            item.index = index;

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

            item.type = (int)Enums.ItemType.Spoils;
            item.index = index;

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

            interactable.id = id;
            interactable.index = i;

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

                terrainList.Add(terrain);
            }
        }
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

    static public void LoadSceneObjects()
    {
        foreach (Region region in regionList)
        {
            var terrains = terrainList.Where(x => x.regionId == region.id).Distinct().ToList();
            var middleTerrain = terrains[terrains.Count / 2];
            var terrainTiles = terrainTileList.Where(x => x.terrainId == (middleTerrain.id)).Distinct().ToList();
            var middleTile = terrainTiles[terrainTiles.Count / 2];

            /*Skull*/
            CreateSceneObject(17, region.id, new Vector3(0.1f, 0.2f, 0.3f), new Vector3(1, 2, 3));

            /*Rock*/
            CreateSceneObject(18, region.id, new Vector3(0.4f, 0.5f, 0.6f), new Vector3(4, 5, 6));

            /*Cactus*/
            CreateSceneObject(19, region.id, new Vector3(0.7f, 0.8f, 0.9f), new Vector3(7, 8, 9));

            /*Red warrior*/
            CreateSceneInteractable(1, region.id, new Vector3(0.1f, 0.2f, 0.3f), new Vector3(1, 2, 3));

            /*Ranger*/
            CreateSceneInteractable(4, region.id, new Vector3(0.4f, 0.5f, 0.6f), new Vector3(4, 5, 6));

            /*Mage*/
            CreateSceneInteractable(5, region.id, new Vector3(0.1f, 0.2f, 0.3f), new Vector3(1, 2, 3));
        }
    }

    static public void CreateSceneInteractable(int interactableId, int regionId, Vector3 position, Vector3 rotation)
    {
        var sceneInteractable = new SceneInteractable();

        int id = sceneInteractableList.Count > 0 ? (sceneInteractableList[sceneInteractableList.Count - 1].id + 1) : 1;

        sceneInteractable.id = id;
        sceneInteractable.interactableId = interactableId;

        CreateInteraction(sceneInteractable, regionId, position, rotation);

        sceneInteractableList.Add(sceneInteractable);
    }

    static public void CreateInteraction(SceneInteractable sceneInteractable, int regionId, Vector3 position, Vector3 rotation)
    {
        var interaction = new Interaction();

        int id = interactionList.Count > 0 ? (interactionList[interactionList.Count - 1].id + 1) : 1;

        interaction.id = id;
        interaction.sceneInteractableId = sceneInteractable.id;
        interaction.regionId = regionId;

        interaction.description = "Talk to {" + sceneInteractable.interactableId + "}";

        interaction.positionX = position.x;
        interaction.positionY = position.y;
        interaction.positionZ = position.z;

        interaction.rotationX = (int)rotation.x;
        interaction.rotationY = (int)rotation.y;
        interaction.rotationZ = (int)rotation.z;

        interaction.scaleMultiplier = 1;
        
        interactionList.Add(interaction);
    }

    static public void CreateSceneObject(int objectGraphicId, int regionId, Vector3 position, Vector3 rotation)
    {
        var sceneObject = new SceneObject();

        int id = sceneObjectList.Count > 0 ? (sceneObjectList[sceneObjectList.Count - 1].id + 1) : 1;

        sceneObject.id = id;
        sceneObject.objectGraphicId = objectGraphicId;
        sceneObject.regionId = regionId;
        
        sceneObject.positionX = position.x;
        sceneObject.positionY = position.y;
        sceneObject.positionZ = position.z;

        //sceneObject.terrainTileId = terrainTileId;

        sceneObject.rotationX = (int)rotation.x;
        sceneObject.rotationY = (int)rotation.y;
        sceneObject.rotationZ = (int)rotation.z;

        sceneObject.scaleMultiplier = 1;

        sceneObjectList.Add(sceneObject);
    }

    static public void LoadChapters()
    {
        List<int> randomInteractables = new List<int>();

        interactableList.ForEach(x => randomInteractables.Add(x.id));

        for (int i = 0; i < chapters; i++)
        {
            var chapter = new Chapter();

            int id = (i + 1);

            chapter.id = id;
            chapter.index = i;

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

            var sceneInteractableIds = sceneInteractableList.Where(x => x.chapterId == chapter.id).Select(x => x.interactableId).Distinct().ToList();
            interactableList.Where(x => !sceneInteractableIds.Contains(x.id)).Distinct().ToList().ForEach(x => randomInteractables.Add(x.id));

            for (int i = 0; i < partyMembersInChapter; i++)
            {
                var partyMember = new PartyMember();

                int id = partyMemberList.Count > 0 ? (partyMemberList[partyMemberList.Count - 1].id + 1) : 1;

                partyMember.id = id;
                partyMember.chapterId = chapter.id;

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

            var partyMemberIds = partyMemberList.Where(x => x.chapterId == chapter.id).Select(x => x.interactableId).Distinct().ToList();
            interactableList.Where(x => !partyMemberIds.Contains(x.id)).Distinct().ToList().ForEach(x => randomInteractables.Add(x.id));

            for (int i = 0; i < worldInteractablesInChapter; i++)
            {
                var chapterInteractable = new SceneInteractable();

                int id = sceneInteractableList.Count > 0 ? (sceneInteractableList[sceneInteractableList.Count - 1].id + 1) : 1;

                chapterInteractable.id = id;
                chapterInteractable.chapterId = chapter.id;

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
        //for (int i = 0; i < phasesInChapter; i++)
        //{
        //    var phase = new Phase();

        //    int id = phaseList.Count > 0 ? (phaseList[phaseList.Count - 1].id + 1) : 1;

        //    phase.id = id;
        //    phase.index = i;

        //    phase.chapterId = 1;
        //    phase.name = "Phase " + (i + 1);
        //    phase.notes = "I belong to Chapter " + 1 + ". This is definitely a test";

        //    phaseList.Add(phase);
        //}

        foreach (Chapter chapter in chapterList)
        {
            for (int i = 0; i < phasesInChapter; i++)
            {
                var phase = new Phase();

                int id = phaseList.Count > 0 ? (phaseList[phaseList.Count - 1].id + 1) : 1;

                phase.id = id;
                phase.index = i;

                phase.chapterId = chapter.id;
                phase.name = "Phase " + (i + 1);
                phase.notes = "I belong to Chapter " + chapter.id + ". This is definitely a test";

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

                var sceneInteractableSourceList = sceneInteractableList.Where(x => interactionList.Where(y => y.regionId == regionSource.id).Select(y => y.sceneInteractableId).Contains(x.id)).Distinct().ToList();

                foreach (SceneInteractable sceneInteractableSource in sceneInteractableSourceList)
                {
                    var sceneInteractable = new SceneInteractable();

                    int sceneInteractableId = sceneInteractableList.Count > 0 ? (sceneInteractableList[sceneInteractableList.Count - 1].id + 1) : 1;
                    
                    sceneInteractable.id = sceneInteractableId;

                    sceneInteractable.chapterId = sceneInteractableSource.chapterId;
                    sceneInteractable.objectiveId = sceneInteractableSource.objectiveId;
                    sceneInteractable.interactableId = sceneInteractableSource.interactableId;
                    sceneInteractable.interactionIndex = sceneInteractableSource.interactionIndex;

                    var interactionSourceList = interactionList.Where(x => x.sceneInteractableId == sceneInteractableSource.id).OrderBy(x => x.index).Distinct().ToList();

                    foreach (Interaction interactionSource in interactionSourceList)
                    {
                        var interaction = new Interaction();

                        int interactionId = interactionList.Count > 0 ? (interactionList[interactionList.Count - 1].id + 1) : 1;

                        interaction.id = interactionId;
                        interaction.sceneInteractableId = sceneInteractable.id;
                        interaction.objectiveId = interactionSource.objectiveId;
                        interaction.regionId = region.id;

                        interaction.terrainTileId = interactionSource.terrainTileId;

                        interaction.index = interactionSource.index;
                        interaction.description = interactionSource.description;

                        interaction.positionX = interactionSource.positionX;
                        interaction.positionY = interactionSource.positionY;
                        interaction.positionZ = interactionSource.positionZ;

                        interaction.rotationX = interactionSource.rotationX;
                        interaction.rotationY = interactionSource.rotationY;
                        interaction.rotationZ = interactionSource.rotationZ;
                        
                        interaction.scaleMultiplier = interactionSource.scaleMultiplier;

                        interactionList.Add(interaction);
                    }

                    sceneInteractableList.Add(sceneInteractable);
                }

                var sceneObjectSourceList = sceneObjectList.Where(x => x.regionId == regionSource.id).Distinct().ToList();
                
                foreach(SceneObject sceneObjectSource in sceneObjectSourceList)
                {
                    var sceneObject = new SceneObject();

                    int sceneObjectId = sceneObjectList.Count > 0 ? (sceneObjectList[sceneObjectList.Count - 1].id + 1) : 1;

                    sceneObject.id = sceneObjectId;
                    sceneObject.regionId = region.id;

                    sceneObject.terrainTileId = sceneObjectSource.terrainTileId;

                    sceneObject.positionX = sceneObjectSource.positionX;
                    sceneObject.positionY = sceneObjectSource.positionY;
                    sceneObject.positionZ = sceneObjectSource.positionZ;

                    sceneObject.rotationX = sceneObjectSource.rotationX;
                    sceneObject.rotationY = sceneObjectSource.rotationY;
                    sceneObject.rotationZ = sceneObjectSource.rotationZ;

                    sceneObject.scaleMultiplier = sceneObjectSource.scaleMultiplier;

                    sceneObject.index = sceneObjectSource.index;
                    sceneObject.objectGraphicId = sceneObjectSource.objectGraphicId;

                    sceneObjectList.Add(sceneObject);
                }

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
                    
                    var terrainTileSourceList = terrainTileList.Where(x => x.terrainId == terrainSource.id).OrderBy(x => x.index).Distinct().ToList();

                    foreach(TerrainTile terrainTileSource in terrainTileSourceList)
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
                
                regionList.Add(region);
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
                quest.name = "Quest " + (i + 1);
                quest.notes = "I belong to Phase " + phase.id + ". This is definitely a test";

                questList.Add(quest);
            }
        }
    }

    static public void LoadPhaseInteractables()
    {
        foreach (Chapter chapter in chapterList)
        {
            foreach (Phase phase in phaseList.Where(x => x.chapterId == chapter.id).Distinct().ToList())
            {
                var chapterInteractables = sceneInteractableList.Where(x => x.chapterId == chapter.id).Distinct().ToList();
                var questIds = questList.Where(x => x.phaseId == phase.id).Select(x => x.id).Distinct().ToList();

                for (int i = 0; i < chapterInteractables.Count; i++)
                {
                    var phaseInteractable = new PhaseInteractable();

                    int id = phaseInteractableList.Count > 0 ? (phaseInteractableList[phaseInteractableList.Count - 1].id + 1) : 1;

                    phaseInteractable.id = id;

                    phaseInteractable.phaseId = phase.id;
                    phaseInteractable.sceneInteractableId = chapterInteractables[i].id;

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

                int id = objectiveList.Count > 0 ? (objectiveList[objectiveList.Count - 1].id + 1) : 1;

                objective.id = id;
                objective.index = i;

                objective.questId = quest.id;
                objective.name = "Objective " + (i + 1);
                objective.notes = "I belong to Quest " + quest.id + ". This is definitely a test";

                objectiveList.Add(objective);
            }
        }
    }

    static public void LoadObjectiveSceneInteractables()
    {
        foreach (Objective objective in objectiveList)
        {
            List<int> randomInteractables = new List<int>();

            interactableList.ForEach(x => randomInteractables.Add(x.id));

            for(int i = 0; i < interactablesInObjective; i++)
            {
                var objectiveInteractable = new SceneInteractable();

                int id = sceneInteractableList.Count > 0 ? (sceneInteractableList[sceneInteractableList.Count - 1].id + 1) : 1;

                objectiveInteractable.id = id;
                objectiveInteractable.index = i;

                objectiveInteractable.objectiveId = objective.id;

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
            var sceneInteractables = sceneInteractableList.Where(x => phaseInteractableSceneInteractableIds.Contains(x.id) || x.objectiveId == objective.id).Distinct().ToList();

            var phaseId = phaseList.Where(x => questList.Where(y => y.id == objective.questId).Select(y => y.phaseId).Contains(x.id)).Select(x => x.id).FirstOrDefault();
            var regions = regionList.Where(x => x.phaseId == phaseId).Distinct().ToList();

            foreach(SceneInteractable sceneInteractable in sceneInteractables)
            {
                for (int i = 0; i < baseInteractions; i++)
                {
                    var interaction = new Interaction();

                    int id = interactionList.Count > 0 ? (interactionList[interactionList.Count - 1].id + 1) : 1;

                    interaction.id = id;
                    interaction.index = i;

                    interaction.objectiveId = objective.id;
                    interaction.sceneInteractableId = sceneInteractable.id;

                    int randomRegion = Random.Range(0, regions.Count);

                    interaction.regionId = regions[randomRegion].id;
                    
                    interaction.description = "Basically a task description. Property of objective" + objective.id;

                    interaction.scaleMultiplier = 1;

                    interactionList.Add(interaction);
                }
            }
        }
    }
}
