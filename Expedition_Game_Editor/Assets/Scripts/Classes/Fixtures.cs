using UnityEngine;
using System.Collections.Generic;

static public class Fixtures
{
    static public int timeFrames = 2;

    static public int supplies = 4;
    static public int gear = 6;
    static public int spoils = 10;
    static public int elements = 6;
    static public int objectGraphics = 15;
    static public int icons = 9;
    static public int regions = 5;
    static public int chapters = 3;
    static public int elementsInChapter = 4;
    static public int regionsInChapter = 2;
    static public int phasesInChapter = 4;
    static public int questsInPhase = 4;
    static public int objectivesInQuest = 3;
    static public int sideObjectiveElements = 2;
    static public int sideTerrainElements = 1;
    static public int baseTasks = 4;
    static public int baseTerrains = 9;
    static public int tileSets = 2;
    static public int tiles = 20;
    static public int baseTerrainTiles = 25;
    static public int baseTerrainObjects = 5;


    static public List<Item> itemList = new List<Item>();
    static public List<Element> elementList = new List<Element>();
    static public List<ObjectGraphic> objectGraphicList = new List<ObjectGraphic>();
    static public List<Icon> iconList = new List<Icon>();
    static public List<Region> regionList = new List<Region>();
    static public List<Chapter> chapterList = new List<Chapter>();
    static public List<ChapterElement> chapterElementList = new List<ChapterElement>();
    static public List<ChapterRegion> chapterRegionList = new List<ChapterRegion>();
    static public List<Phase> phaseList = new List<Phase>();
    static public List<PhaseRegion> phaseRegionList = new List<PhaseRegion>();
    static public List<Quest> questList = new List<Quest>();
    static public List<Objective> objectiveList = new List<Objective>();

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

    public class ObjectGraphic : GeneralData
    {
        public string name;
        public string path;
        public string icon;
    }

    public class Icon : GeneralData
    {
        public string path;
    }

    public class Region : GeneralData
    {
        public string name;
        public int dimension;
    }

    public class Chapter : GeneralData
    {
        public string name;
        public string notes;
    }

    public class ChapterElement : GeneralData
    {
        public int chapterId;
        public int elementId;
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
    
    static public void LoadFixtures()
    {
        LoadObjectGraphics();
        LoadItems();
        LoadElements();
        LoadRegions();
        LoadChapters();
        LoadChapterElements();
        LoadChapterRegions();
        LoadPhases();
        LoadQuests();
        LoadObjectives();
    }

    static public void LoadObjectGraphics()
    {
        string[] objectNames = new string[] { "Nothing", "Polearm","MightyPolearm","Crossbow","StrongCrossbow","Shortbow","Longbow","Staff","MenacingStaff", "Warrior","Ranger","Mage", "Blue", "Green", "Drake" };
        //                                        0         1           2               3           4             5          6          7       8                9        10      11      12       13       14
        for (int id = 1; id <= objectNames.Length; id++)
        {
            var objectGraphic = new ObjectGraphic();

            objectGraphic.id = id;

            string path = objectNames[id - 1];

            var objectResource = Resources.Load<Source.ObjectGraphic>("Objects/" + path);

            objectGraphic.name = objectResource.name;
            objectGraphic.path = path;
            objectGraphic.icon = "Textures/Icons/Objects/" + path;

            objectGraphicList.Add(objectGraphic);
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

            int id = (i + 1);

            region.id = id;
            region.table = "Region";

            region.index = i;
            region.name = "Region " + id;

            regionList.Add(region);
        }
    }

    static public void LoadChapters()
    {
        int index = 0;

        for (int i = 0; i < chapters; i++)
        {
            var chapter = new Chapter();

            int id = (i + 1);

            chapter.id = id;
            chapter.index = index;
            chapter.name = "Chapter " + id;
            chapter.notes = "This is a pretty regular sentence. The structure is something you'd expect. Nothing too long though!";

            chapterList.Add(chapter);

            index++;
        }
    }

    static public void LoadChapterElements()
    {
        foreach(Chapter chapter in chapterList)
        {
            List<int> randomElements = new List<int>();

            elementList.ForEach(x => randomElements.Add(x.id));

            for (int i = 0; i < elementsInChapter; i++)
            {
                var chapterElement = new ChapterElement();

                int id = chapterElementList.Count > 0 ? (chapterElementList[chapterElementList.Count - 1].id + 1) : 1;

                chapterElement.id = id;
                
                chapterElement.chapterId = chapter.id;

                int randomElement = Random.Range(0, randomElements.Count);

                chapterElement.elementId = randomElements[randomElement];

                randomElements.RemoveAt(randomElement);

                chapterElementList.Add(chapterElement);
            }
        }
    }

    static public void LoadChapterRegions()
    {
        foreach (Chapter chapter in chapterList)
        {
            List<int> randomRegions = new List<int>();

            regionList.ForEach(x => randomRegions.Add(x.id));
            
            for (int i = 0; i < regionsInChapter; i++)
            {
                var chapterRegion = new ChapterRegion();

                int id = (i + 1);

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

    static public void CalculateFixtures()
    {
        for (int i = 0; i < 100000; i++)
            chapterList.Add(new Chapter());

        Debug.Log("done: " + chapterList.Count);

        //int total = 0;

        //int items = supplies + gear + spoils;
        
        //total += items;
        //total += elements;
        //total += objectGraphics;
        //total += icons;
        //total += regions;

        //total += chapters;

        ////Each chapter has a set of elements
        //int chapterElements = chapters * baseChapterElements;
        //total += chapterElements;
        //Debug.Log("chapterElements: " + chapterElements);

        ////Each chapter has a set of regions
        //int chapterRegions = chapters * baseChapterRegions;
        //total += chapterRegions;
        //Debug.Log("chapterRegions: " + chapterRegions);

        ////Each chapter has a set of phases
        //int phases = chapters * basePhases;
        //total += phases;
        //Debug.Log("phases: " + phases);

        ////Each phase has a copy of the chapter's regions, both day and night
        //int phaseRegions = phases * chapterRegions * timeFrames;
        //total += phaseRegions;
        //Debug.Log("phaseRegions: " + phaseRegions);

        ////Each phase has a set of quests
        //int quests = phases * baseQuests;
        //total += quests;
        //Debug.Log("quests: " + quests);

        ////Phase elements are divided over all the quests in the phase
        //int questElements = baseChapterElements * phases;
        //total += questElements;
        //Debug.Log("questElements: " + questElements);

        ////Each quest has a set of objectives
        //int objectives = quests * baseObjectives;
        //total += objectives;
        //Debug.Log("objectives: " + objectives);

        //int objectiveElements = sideObjectiveElements * objectives;
        //total += objectiveElements;
        //Debug.Log("objectiveElements: " + objectiveElements);

        //int terrainElements = questElements + objectiveElements + (sideTerrainElements * phaseRegions);
        //total += terrainElements;
        //Debug.Log("terrainElements: " + terrainElements);

        //int tasks = terrainElements * baseTasks * timeFrames;

        //Debug.Log(total);
    }
}
