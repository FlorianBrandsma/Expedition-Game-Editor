using UnityEngine;
using System.Collections.Generic;

public class Search
{
    [System.Serializable]
    public class Icon : SearchParameters
    {
        public List<int> category = new List<int>();

        public enum RequestType
        {
            Custom
        }

        public RequestType requestType;
    }

    [System.Serializable]
    public class ObjectGraphic : SearchParameters
    {
        public enum RequestType
        {
            Custom
        }

        public RequestType requestType;
    }

    [System.Serializable]
    public class Tile : SearchParameters
    {
        public List<int> tileSetId = new List<int>();

        public enum RequestType
        {
            Custom
        }

        public RequestType requestType;
    }

    [System.Serializable]
    public class Interactable : SearchParameters
    {
        public List<int> index = new List<int>();
        public List<string> name = new List<string>();

        public enum RequestType
        {
            Custom,
            GetAllInteractables,
        }

        public RequestType requestType;
    }

    [System.Serializable]
    public class Item : SearchParameters
    {
        public List<int> index = new List<int>();
        public List<int> type = new List<int>();
        public List<string> name = new List<string>();

        public enum RequestType
        {
            Custom,
            GetItemsByType,
        }

        public RequestType requestType;
    }

    [System.Serializable]
    public class Chapter : SearchParameters
    {
        public List<int> index          = new List<int>();
        public List<string> name        = new List<string>();
        public List<string> description = new List<string>();

        public enum RequestType
        {
            Custom
        }

        public RequestType requestType;
    }

    [System.Serializable]
    public class ChapterRegion : SearchParameters
    {
        public List<int> chapterId = new List<int>();
        public List<int> regionId = new List<int>();

        public enum RequestType
        {
            Custom
        }

        public RequestType requestType;
    }

    [System.Serializable]
    public class Phase : SearchParameters
    {
        public List<int> index          = new List<int>();
        public List<int> chapterId      = new List<int>();
        public List<string> name        = new List<string>();
        public List<string> description = new List<string>();

        public enum RequestType
        {
            Custom,
            GetPhaseWithQuests
        }

        public RequestType requestType;
    }

    [System.Serializable]
    public class PhaseInteractable : SearchParameters
    {
        public List<int> phaseId = new List<int>();
        public List<int> questId = new List<int>();
        public List<int> terrainInteractableId = new List<int>();

        public enum RequestType
        {
            Custom
        }

        public RequestType requestType;
    }

    [System.Serializable]
    public class Quest : SearchParameters
    {
        public List<int> index = new List<int>();
        public List<int> phaseId = new List<int>();
        public List<string> name = new List<string>();
        public List<string> description = new List<string>();

        public enum RequestType
        {
            Custom
        }

        public RequestType requestType;
    }

    [System.Serializable]
    public class Objective : SearchParameters
    {
        public List<int> index = new List<int>();
        public List<int> questId = new List<int>();
        public List<string> name = new List<string>();
        public List<string> description = new List<string>();

        public enum RequestType
        {
            Custom
        }

        public RequestType requestType;
    }

    [System.Serializable]
    public class Interaction : SearchParameters
    {
        public List<int> index = new List<int>();
        public List<int> objectiveId = new List<int>();
        public List<int> terrainInteractableId = new List<int>();

        public enum RequestType
        {
            Custom
        }

        public RequestType requestType;
    }

    [System.Serializable]
    public class Region : SearchParameters
    {
        public List<int> index = new List<int>();
        public List<int> phaseId = new List<int>();
        public List<string> name = new List<string>();

        public enum RequestType
        {
            Custom
        }

        public RequestType requestType;
    }

    [System.Serializable]
    public class Terrain : SearchParameters
    {
        public List<int> index = new List<int>();
        public List<int> regionId = new List<int>();
        public List<string> name = new List<string>();
        
        public enum RequestType
        {
            Custom
        }

        public RequestType requestType;
    }

    [System.Serializable]
    public class TerrainTile : SearchParameters
    {
        public List<int> index = new List<int>();

        public List<int> regionId = new List<int>();

        public enum RequestType
        {
            Custom
        }

        public RequestType requestType;
    }

    [System.Serializable]
    public class TerrainObject : SearchParameters
    {
        public enum RequestType
        {
            Custom
        }

        public RequestType requestType;
    }

    public class TerrainInteractable : SearchParameters
    {
        public List<int> regionId = new List<int>();
        public List<int> chapterId = new List<int>();
        public List<int> questId = new List<int>();
        public List<int> objectiveId = new List<int>();
        public List<int> interactableId = new List<int>();
        public List<int> interactionIndex = new List<int>();

        public enum RequestType
        {
            Custom,
            GetQuestAndObjectiveInteractables,
            GetInteractablesFromInteractionRegion
        }

        public RequestType requestType;
    }

    public class PartyMember : SearchParameters
    {
        public List<int> chapterId = new List<int>();
        public List<int> interactableId = new List<int>();

        public enum RequestType
        {
            Custom
        }

        public RequestType requestType;
    }
}
