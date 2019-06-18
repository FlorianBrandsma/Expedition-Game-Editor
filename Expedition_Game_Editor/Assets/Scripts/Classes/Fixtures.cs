using UnityEngine;
using System.Collections.Generic;
using System.Linq;

static public class Fixtures
{
    static public int timeFrames = 2;
 
    static public int supplies = 4;
    static public int gear = 6;
    static public int spoils = 10;
    static public int elements = 6;
    static public int objectGraphics = 15;
    static public int icons = 9;
    static public int regions = 2;
    static public int terrainsInRegions = 3;
    static public int terrainTilesInTerrains = 5;
    static public int chapters = 3;
    static public int partyElementsInChapter = 1;
    static public int worldElementsInChapter = 3;
    static public int phasesInChapter = 4;
    static public int questsInPhase = 4;
    static public int objectivesInQuest = 3;
    static public int elementsInObjective = 2;
    static public int sideTerrainElements = 1;
    static public int baseTasks = 4;
    static public int tileSets = 2;
    static public int tilesInTileSet = 10;
    static public int baseTerrainObjects = 5;

    static public List<Icon> iconList = new List<Icon>();
    static public List<ObjectGraphic> objectGraphicList = new List<ObjectGraphic>();
    static public List<Item> itemList = new List<Item>();
    static public List<Element> elementList = new List<Element>();
    static public List<TileSet> tileSetList = new List<TileSet>();
    static public List<Tile> tileList = new List<Tile>();
    static public List<Region> regionList = new List<Region>();
    static public List<Terrain> terrainList = new List<Terrain>();
    static public List<TerrainTile> terrainTileList = new List<TerrainTile>();
    static public List<Chapter> chapterList = new List<Chapter>();
    static public List<ChapterRegion> chapterRegionList = new List<ChapterRegion>();
    static public List<Phase> phaseList = new List<Phase>();
    static public List<PhaseElement> phaseElementList = new List<PhaseElement>();
    static public List<PhaseRegion> phaseRegionList = new List<PhaseRegion>();
    static public List<Quest> questList = new List<Quest>();
    static public List<Objective> objectiveList = new List<Objective>();
    static public List<Task> taskList = new List<Task>();

    static public List<PartyElement> partyElementList = new List<PartyElement>();
    static public List<TerrainElement> terrainElementList = new List<TerrainElement>();

    public class Item : GeneralData
    {
        public int objectGraphicId;
        public int type;
        public string name;
    }

    public class Element : GeneralData
    {
        public int objectGraphicId;
        public string name;
    }

    public class PartyElement : GeneralData
    {
        public int chapterId;
        public int elementId;
    }

    public class TerrainElement : GeneralData
    {
        public int chapterId;
        public int objectiveId;
        public int elementId;
        public int taskIndex;
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
        public int elementId;
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

    public class PhaseElement : GeneralData
    {
        public int phaseId;
        public int questId;
        public int terrainElementId;
    }

    public class PhaseRegion : GeneralData
    {
        public int phaseId;
        public int regionId;
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
    
    public class Task : GeneralData
    {
        public int terrainElementId;
        public int objectiveId;
        public string description;
    }

    static public void LoadFixtures()
    {
        LoadIcons();
        LoadObjectGraphics();
        LoadTileSets();
        LoadTiles();
        LoadItems();
        LoadElements();
        LoadRegions();
        LoadTerrains();
        LoadTerrainTiles();
        LoadChapters();
        LoadChapterPartyElements();
        LoadChapterTerrainElements();
        LoadChapterRegions();
        LoadPhases();
        LoadPhaseRegions();
        LoadQuests();
        LoadPhaseElements();
        LoadObjectives();
        LoadObjectiveTerrainElements();
        LoadTasks();
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

    static public void LoadElements()
    {
        var objectList = new List<int> { 10, 11, 12, 13, 14, 15 };

        for (int i = 0; i < objectList.Count; i++)
        {
            var element = new Element();

            int id = (i + 1);

            element.id = id;
            element.index = i;

            element.objectGraphicId = objectList[i];
            element.name = "Element " + id;

            elementList.Add(element);
        }
    }

    static public void LoadRegions()
    {
        for (int i = 0; i < regions; i++)
        {
            var region = new Region();

            int id = regionList.Count > 0 ? (regionList[regionList.Count - 1].id + 1) : 1;

            region.id = id;
            region.table = "Region";
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
                terrain.table = "Terrain";
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
                terrainTile.table = "TerrainTile";
                terrainTile.index = i;

                terrainTile.terrainId = terrain.id;
                terrainTile.tileId = tileData.FirstOrDefault().id;

                terrainTileList.Add(terrainTile);
            }
        }
    }

    static public void LoadChapters()
    {
        List<int> randomElements = new List<int>();

        elementList.ForEach(x => randomElements.Add(x.id));

        for (int i = 0; i < chapters; i++)
        {
            var chapter = new Chapter();

            int id = (i + 1);

            chapter.id = id;
            chapter.index = i;

            int randomElement = Random.Range(0, randomElements.Count);

            chapter.elementId = randomElements[randomElement];

            chapter.name = "Chapter " + id;
            chapter.notes = "This is a pretty regular sentence. The structure is something you'd expect. Nothing too long though!";

            chapterList.Add(chapter);
        }
    }

    static public void LoadChapterPartyElements()
    {
        foreach (Chapter chapter in chapterList)
        {
            List<int> randomElements = new List<int>();

            var terrainElementIds = terrainElementList.Where(x => x.chapterId == chapter.id).Select(x => x.elementId).Distinct().ToList();
            elementList.Where(x => !terrainElementIds.Contains(x.id)).Distinct().ToList().ForEach(x => randomElements.Add(x.id));

            for (int i = 0; i < partyElementsInChapter; i++)
            {
                var partyElement = new PartyElement();

                int id = partyElementList.Count > 0 ? (partyElementList[partyElementList.Count - 1].id + 1) : 1;

                partyElement.id = id;
                partyElement.chapterId = chapter.id;

                int randomElement = Random.Range(0, randomElements.Count);

                partyElement.elementId = randomElements[randomElement];

                randomElements.RemoveAt(randomElement);

                partyElementList.Add(partyElement);
            }
        }
    }

    static public void LoadChapterTerrainElements()
    {
        foreach(Chapter chapter in chapterList)
        {
            List<int> randomElements = new List<int>();

            var partyElementIds = partyElementList.Where(x => x.chapterId == chapter.id).Select(x => x.elementId).Distinct().ToList();
            elementList.Where(x => !partyElementIds.Contains(x.id)).Distinct().ToList().ForEach(x => randomElements.Add(x.id));

            for (int i = 0; i < worldElementsInChapter; i++)
            {
                var chapterElement = new TerrainElement();

                int id = terrainElementList.Count > 0 ? (terrainElementList[terrainElementList.Count - 1].id + 1) : 1;

                chapterElement.id = id;
                chapterElement.chapterId = chapter.id;

                int randomElement = Random.Range(0, randomElements.Count);

                chapterElement.elementId = randomElements[randomElement];

                randomElements.RemoveAt(randomElement);

                terrainElementList.Add(chapterElement);
            }
        }
    }

    static public void LoadChapterRegions()
    {
        foreach (Chapter chapter in chapterList)
        {
            List<int> randomRegions = new List<int>();

            regionList.ForEach(x => randomRegions.Add(x.id));

            int randomRegionAmount = Random.Range(1, regionList.Count + 1);

            for (int i = 0; i < randomRegionAmount; i++)
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
        foreach(Chapter chapter in chapterList)
        { 
            for (int i = 0; i < phasesInChapter; i++)
            {
                var phase = new Phase();

                int id = phaseList.Count > 0 ? (phaseList[phaseList.Count - 1].id + 1) : 1;

                phase.id =  id;
                phase.index = i;

                phase.chapterId = chapter.id;
                phase.name = "Phase " + (i + 1);
                phase.notes = "I belong to Chapter "+ chapter.id +". This is definitely a test";

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

    static public void LoadPhaseElements()
    {
        foreach (Chapter chapter in chapterList)
        {
            foreach (Phase phase in phaseList.Where(x => x.chapterId == chapter.id).Distinct().ToList())
            {
                var chapterElements = terrainElementList.Where(x => x.chapterId == chapter.id).Distinct().ToList();
                var questIds = questList.Where(x => x.phaseId == phase.id).Select(x => x.id).Distinct().ToList();

                for (int i = 0; i < chapterElements.Count; i++)
                {
                    var phaseElement = new PhaseElement();

                    int id = phaseElementList.Count > 0 ? (phaseElementList[phaseElementList.Count - 1].id + 1) : 1;

                    phaseElement.id = id;

                    phaseElement.phaseId = phase.id;
                    phaseElement.terrainElementId = chapterElements[i].id;

                    int randomQuestId = Random.Range(0, questIds.Count);

                    phaseElement.questId = questIds[randomQuestId];

                    questIds.RemoveAt(randomQuestId);

                    phaseElementList.Add(phaseElement);
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

    static public void LoadObjectiveTerrainElements()
    {
        foreach (Objective objective in objectiveList)
        {
            List<int> randomElements = new List<int>();

            elementList.ForEach(x => randomElements.Add(x.id));

            for(int i = 0; i < elementsInObjective; i++)
            {
                var objectiveElement = new TerrainElement();

                int id = terrainElementList.Count > 0 ? (terrainElementList[terrainElementList.Count - 1].id + 1) : 1;

                objectiveElement.id = id;
                objectiveElement.index = i;

                objectiveElement.objectiveId = objective.id;

                int randomElement = Random.Range(0, randomElements.Count);
                objectiveElement.elementId = randomElements[randomElement];

                terrainElementList.Add(objectiveElement);
            }
        }
    }

    static public void LoadTasks()
    {
        foreach (Objective objective in objectiveList)
        {
            var phaseElementTerrainElementIds = phaseElementList.Where(x => x.questId == objective.questId).Select(x => x.terrainElementId).Distinct().ToList();
            var terrainElements = terrainElementList.Where(x => phaseElementTerrainElementIds.Contains(x.id) || x.objectiveId == objective.id).Distinct().ToList();

            foreach(TerrainElement terrainElement in terrainElements)
            {
                for (int i = 0; i < baseTasks; i++)
                {
                    var task = new Task();

                    int id = taskList.Count > 0 ? (taskList[taskList.Count - 1].id + 1) : 1;

                    task.id = id;
                    task.index = i;

                    task.objectiveId = objective.id;
                    task.terrainElementId = terrainElement.id;
                    task.description = "Perform a simple task. Property of objective" + objective.id;

                    taskList.Add(task);
                }
            }
        }
    }
}
