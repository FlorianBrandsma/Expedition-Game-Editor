using UnityEngine;
using System.Collections.Generic;

public class Search
{
    public class Icon
    {
        public List<int> id = new List<int>();

        public List<int> category = new List<int>();

        public enum RequestType
        {
            Custom
        }

        public RequestType requestType;
    }

    public class ObjectGraphic
    {
        public List<int> id = new List<int>();

        public enum RequestType
        {
            Custom
        }

        public RequestType requestType;
    }

    public class TileSet
    {
        public List<int> id = new List<int>();

        public enum RequestType
        {
            Custom
        }

        public RequestType requestType;
    }

    public class Tile
    {
        public List<int> id = new List<int>();

        public List<int> tileSetId = new List<int>();

        public enum RequestType
        {
            Custom
        }

        public RequestType requestType;
    }

    [System.Serializable]
    public class Interactable
    {
        public List<int> id = new List<int>();

        public List<int> type = new List<int>();

        public enum RequestType
        {
            Custom
        }

        public RequestType requestType;
    }

    public class Item
    {
        public List<int> id = new List<int>();

        public List<int> type = new List<int>();

        public enum RequestType
        {
            Custom
        }

        public RequestType requestType;
    }

    public class Chapter
    {
        public List<int> id = new List<int>();

        public enum RequestType
        {
            Custom
        }

        public RequestType requestType;
    }

    public class PartyMember
    {
        public List<int> id = new List<int>();

        public List<int> chapterId = new List<int>();
        public List<int> interactableId = new List<int>();

        public enum RequestType
        {
            Custom
        }

        public RequestType requestType;
    }

    public class ChapterInteractable
    {
        public List<int> id = new List<int>();

        public List<int> chapterId = new List<int>();
        public List<int> interactableId = new List<int>();

        public enum RequestType
        {
            Custom
        }

        public RequestType requestType;
    }

    public class ChapterRegion
    {
        public List<int> id = new List<int>();

        public List<int> chapterId = new List<int>();
        public List<int> regionId = new List<int>();

        public enum RequestType
        {
            Custom
        }

        public RequestType requestType;
    }

    public class Phase
    {
        public List<int> id = new List<int>();

        public List<int> chapterId = new List<int>();

        public enum RequestType
        {
            Custom,
            GetPhaseWithQuests
        }

        public RequestType requestType;
    }

    public class Quest
    {
        public List<int> id = new List<int>();

        public List<int> phaseId = new List<int>();

        public enum RequestType
        {
            Custom
        }

        public RequestType requestType;
    }

    public class Objective
    {
        public List<int> id = new List<int>();

        public List<int> questId = new List<int>();

        public enum RequestType
        {
            Custom
        }

        public RequestType requestType;
    }

    public class WorldInteractable
    {
        public List<int> id = new List<int>();

        public List<int> type = new List<int>();

        public List<int> regionId = new List<int>();

        public List<int> chapterInteractableId = new List<int>();

        public List<int> phaseId = new List<int>();
        public List<int> questId = new List<int>();
        public List<int> objectiveId = new List<int>();
        public List<int> interactableId = new List<int>();
        public List<int> objectGraphicId = new List<int>();

        public int isDefault = -1;

        public List<int> interactionIndex = new List<int>();

        public enum RequestType
        {
            Custom,
            GetRegionWorldInteractables,
            GetQuestAndObjectiveWorldInteractables
        }

        public RequestType requestType;
    }

    public class Task
    {
        public List<int> id = new List<int>();

        public List<int> objectiveId = new List<int>();
        public List<int> worldInteractableId = new List<int>();

        public enum RequestType
        {
            Custom
        }
    }

    public class Interaction
    {
        public List<int> id = new List<int>();

        public List<int> taskId = new List<int>();
        public List<int> regionId = new List<int>();

        public enum RequestType
        {
            Custom
        }

        public RequestType requestType;
    }

    public class Outcome
    {
        public List<int> id = new List<int>();

        public List<int> interactionId = new List<int>();

        public enum RequestType
        {
            Custom
        }

        public RequestType requestType;
    }

    public class World
    {
        public List<int> id = new List<int>();

        public List<int> regionId = new List<int>();
        public List<int> objectiveId = new List<int>();

        public Enums.RegionType regionType;

        public enum RequestType
        {
            Custom
        }

        public RequestType requestType;
    }

    public class Region
    {
        public List<int> id = new List<int>();

        public List<int> phaseId = new List<int>();

        public enum RequestType
        {
            Custom
        }

        public RequestType requestType;
    }

    public class Atmosphere
    {
        public List<int> id = new List<int>();

        public List<int> terrainId = new List<int>();

        public enum RequestType
        {
            Custom
        }

        public RequestType requestType;
    }

    public class Terrain
    {
        public List<int> id = new List<int>();

        public List<int> regionId = new List<int>();

        public enum RequestType
        {
            Custom
        }

        public RequestType requestType;
    }

    public class TerrainTile
    {
        public List<int> id = new List<int>();

        public List<int> regionId = new List<int>();
        public List<int> terrainId = new List<int>();

        public enum RequestType
        {
            Custom
        }

        public RequestType requestType;
    }

    public class WorldObject
    {
        public List<int> id = new List<int>();

        public List<int> regionId = new List<int>();

        public enum RequestType
        {
            Custom
        }

        public RequestType requestType;
    }

    public class Save
    {
        public List<int> id = new List<int>();

        public List<int> gameId = new List<int>();

        public enum RequestType
        {
            Custom
        }

        public RequestType requestType;
    }

    public class ChapterSave
    {
        public List<int> id = new List<int>();

        public List<int> saveId = new List<int>();
        public List<int> chapterId = new List<int>();

        public enum RequestType
        {
            Custom
        }

        public RequestType requestType;
    }

    public class PhaseSave
    {
        public List<int> id = new List<int>();

        public List<int> chapterSaveId = new List<int>();
        public List<int> phaseId = new List<int>();

        public List<int> chapterId = new List<int>();
        
        public enum RequestType
        {
            Custom,
            GetPhaseSaveByChapter
        }

        public RequestType requestType;
    }

    public class QuestSave
    {
        public List<int> id = new List<int>();

        public List<int> phaseSaveId = new List<int>();
        public List<int> questId = new List<int>();

        public enum RequestType
        {
            Custom
        }

        public RequestType requestType;
    }

    public class ObjectiveSave
    {
        public List<int> id = new List<int>();

        public List<int> questSaveId = new List<int>();
        public List<int> objectiveId = new List<int>();

        public enum RequestType
        {
            Custom
        }

        public RequestType requestType;
    }

    public class TaskSave
    {
        public List<int> id = new List<int>();

        public List<int> objectiveSaveId = new List<int>();
        public List<int> worldInteractableId = new List<int>();
        public List<int> taskId = new List<int>();

        public enum RequestType
        {
            Custom
        }

        public RequestType requestType;
    }

    public class InteractionSave
    {
        public List<int> id = new List<int>();

        public List<int> taskSaveId = new List<int>();
        public List<int> interactionId = new List<int>();

        public enum RequestType
        {
            Custom
        }

        public RequestType requestType;
    }
}
