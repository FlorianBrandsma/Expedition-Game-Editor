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
    static public int regions = 5;
    static public int chapters = 3;
    static public int elementsInChapter = 3;
    static public int regionsInChapter = 2;
    static public int phasesInChapter = 4;
    static public int questsInPhase = 4;
    static public int objectivesInQuest = 3;
    static public int elementsInObjective = 2;
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
    static public List<ChapterRegion> chapterRegionList = new List<ChapterRegion>();
    static public List<Phase> phaseList = new List<Phase>();
    static public List<PhaseElement> phaseElementList = new List<PhaseElement>();
    static public List<PhaseRegion> phaseRegionList = new List<PhaseRegion>();
    static public List<Quest> questList = new List<Quest>();
    static public List<Objective> objectiveList = new List<Objective>();
    static public List<Task> taskList = new List<Task>();

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

    public class TerrainElement : GeneralData
    {
        public int chapterId;
        public int objectiveId;
        public int elementId;
        public int taskIndex;
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
        LoadObjectGraphics();
        LoadItems();
        LoadElements();
        LoadRegions();
        LoadChapters();
        LoadChapterTerrainElements();
        LoadChapterRegions();
        LoadPhases();
        LoadQuests();
        LoadPhaseElements();
        LoadObjectives();
        LoadObjectiveTerrainElements();
        LoadTasks();
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

    static public void LoadChapterTerrainElements()
    {
        foreach(Chapter chapter in chapterList)
        {
            List<int> randomElements = new List<int>();

            elementList.Where(x => x.id != chapter.elementId).Distinct().ToList().ForEach(x => randomElements.Add(x.id));

            for (int i = 0; i < elementsInChapter; i++)
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
