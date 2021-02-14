using UnityEngine;
using System.Collections.Generic;

public class Search
{
    public class Notification
    {
        public Enums.NotificationType notificationType;
    }

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

    public class Model
    {
        public List<int> id = new List<int>();
        public List<int> excludeId = new List<int>();

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
        public bool includeAddElement;
        public bool includeRemoveElement;

        public List<int> id = new List<int>();
        public List<int> excludeId = new List<int>();

        public List<int> type = new List<int>();

        public enum RequestType
        {
            Custom
        }

        public RequestType requestType;
    }

    public class Item
    {
        public bool includeAddElement;

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
        public bool includeAddElement;

        public List<int> id = new List<int>();
        
        public enum RequestType
        {
            Custom
        }

        public RequestType requestType;
    }

    public class ChapterInteractable
    {
        public bool includeAddElement;

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
        public bool includeAddElement;

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
        public bool includeAddElement;

        public List<int> id = new List<int>();

        public List<int> chapterId = new List<int>();
        public List<int> defaultRegionId = new List<int>();

        public enum RequestType
        {
            Custom,
            GetPhaseWithQuests
        }

        public RequestType requestType;
    }

    public class Quest
    {
        public bool includeAddElement;

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
        public bool includeAddElement;

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
        public bool includeAddElement;
        public bool includeRemoveElement;

        public List<int> id = new List<int>();
        public List<int> excludeId = new List<int>();

        public List<int> regionId = new List<int>();

        public List<int> chapterInteractableId = new List<int>();

        public List<int> chapterId = new List<int>();
        public List<int> phaseId = new List<int>();
        public List<int> questId = new List<int>();
        public List<int> objectiveId = new List<int>();
        public List<int> interactableId = new List<int>();
        public List<int> modelId = new List<int>();

        public List<int> type = new List<int>();

        public List<int> interactionIndex = new List<int>();

        public enum RequestType
        {
            Custom,
            GetRegionWorldInteractables,
            GetQuestAndObjectiveWorldInteractables,
            GetSceneActorWorldInteractables
        }

        public RequestType requestType;
    }

    public class Task
    {
        public bool includeAddElement;

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
        public bool includeAddElement;

        public List<int> id = new List<int>();

        public List<int> taskId = new List<int>();

        public enum RequestType
        {
            Custom
        }

        public RequestType requestType;
    }

    public class InteractionDestination
    {
        public bool includeAddElement;

        public List<int> id = new List<int>();

        public List<int> interactionId = new List<int>();
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

    public class Scene
    {
        public bool includeAddElement;

        public List<int> id = new List<int>();

        public List<int> outcomeId = new List<int>();
        public List<int> regionId = new List<int>();

        public enum RequestType
        {
            Custom
        }

        public RequestType requestType;
    }

    public class SceneShot
    {
        public List<int> id = new List<int>();

        public List<int> sceneId = new List<int>();

        public List<int> positionTargetSceneActorId = new List<int>();
        public List<int> rotationTargetSceneActorId = new List<int>();

        public enum RequestType
        {
            Custom
        }

        public RequestType requestType;
    }

    public class CameraFilter
    {
        public bool includeRemoveElement;

        public List<int> id = new List<int>();
        public List<int> excludeId = new List<int>();
        
        public enum RequestType
        {
            Custom
        }

        public RequestType requestType;
    }

    public class SceneActor
    {
        public bool includeAddElement;
        public bool includeRemoveElement;

        public List<int> id = new List<int>();
        public List<int> excludeId = new List<int>();

        public List<int> sceneId = new List<int>();
        public List<int> worldInteractableId = new List<int>();

        public List<int> targetSceneActorId = new List<int>();

        public enum RequestType
        {
            Custom
        }

        public RequestType requestType;
    }

    public class SceneProp
    {
        public bool includeAddElement;

        public List<int> id = new List<int>();
        public List<int> excludeId = new List<int>();

        public List<int> sceneId = new List<int>();

        public enum RequestType
        {
            Custom
        }

        public RequestType requestType;
    }

    public class EditorWorld
    {
        public bool includeAddWorldObjectElement;
        public bool includeAddInteractionDestinationElement;
        public bool includeAddSceneActorElement;
        public bool includeAddScenePropElement;

        public List<int> id = new List<int>();

        public List<int> regionId = new List<int>();
        public List<int> phaseId = new List<int>();
        public List<int> objectiveId = new List<int>();
        public List<int> interactionId = new List<int>();
        public List<int> sceneId = new List<int>();

        public Enums.RegionType regionType;

        public enum RequestType
        {
            Custom
        }

        public RequestType requestType;
    }

    public class Region
    {
        public bool includeAddElement;
        public bool includeRemoveElement;

        public List<int> id = new List<int>();
        public List<int> excludeId = new List<int>();

        public List<int> chapterRegionId = new List<int>();

        public List<int> phaseId = new List<int>();
        public List<int> excludePhaseId = new List<int>();
        
        public Enums.RegionType type;

        public enum RequestType
        {
            Custom
        }

        public RequestType requestType;
    }

    public class Atmosphere
    {
        public bool includeAddElement;

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
        public bool includeAddElement;

        public List<int> id = new List<int>();

        public List<int> regionId = new List<int>();

        public enum RequestType
        {
            Custom
        }

        public RequestType requestType;
    }

    public class GameWorld
    {
        public List<int> id = new List<int>();

        public List<int> phaseId = new List<int>();

        public enum RequestType
        {
            Custom
        }

        public RequestType requestType;
    }

    public class Save
    {
        public bool includeAddElement;

        public List<int> id = new List<int>();

        public List<int> gameId = new List<int>();

        public Enums.SaveType saveType;

        public enum RequestType
        {
            Custom
        }

        public RequestType requestType;
    }

    public class GameSave
    {
        public List<int> saveId = new List<int>();
        
        public enum RequestType
        {
            Custom
        }

        public RequestType requestType;
    }

    public class InteractableSave
    {
        public List<int> saveId = new List<int>();
        public List<int> interactableId = new List<int>();

        public List<int> type = new List<int>();

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

        public bool? complete;

        public enum RequestType
        {
            Custom
        }

        public RequestType requestType;
    }

    public class PhaseSave
    {
        public List<int> id = new List<int>();

        public List<int> saveId = new List<int>();
        public List<int> chapterId = new List<int>();
        public List<int> phaseId = new List<int>();

        public bool? complete;

        public enum RequestType
        {
            Custom
        }

        public RequestType requestType;
    }

    public class QuestSave
    {
        public List<int> id = new List<int>();

        public List<int> saveId = new List<int>();
        public List<int> phaseId = new List<int>();
        public List<int> questId = new List<int>();

        public bool? complete;

        public enum RequestType
        {
            Custom
        }

        public RequestType requestType;
    }

    public class ObjectiveSave
    {
        public List<int> id = new List<int>();

        public List<int> saveId = new List<int>();
        public List<int> questId = new List<int>();
        public List<int> objectiveId = new List<int>();

        public bool? complete;

        public enum RequestType
        {
            Custom
        }

        public RequestType requestType;
    }

    public class TaskSave
    {
        public List<int> id = new List<int>();

        public List<int> saveId = new List<int>();
        public List<int> objectiveId = new List<int>();
        public List<int> worldInteractableId = new List<int>();
        public List<int> taskId = new List<int>();

        public bool? complete;

        public enum RequestType
        {
            Custom
        }

        public RequestType requestType;
    }

    public class InteractionSave
    {
        public List<int> id = new List<int>();

        public List<int> saveId = new List<int>();
        public List<int> taskId = new List<int>();
        public List<int> interactionId = new List<int>();

        public bool? complete;

        public enum RequestType
        {
            Custom
        }

        public RequestType requestType;
    }
}
