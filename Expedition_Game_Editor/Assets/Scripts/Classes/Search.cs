using UnityEngine;
using System.Collections.Generic;

public class Search
{
    [System.Serializable]
    public class Icon : SearchParameters
    {
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
    public class Element : SearchParameters
    {
        public List<int> index = new List<int>();
        public List<string> name = new List<string>();

        public enum RequestType
        {
            Custom,
            GetAllElements,
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
    public class PhaseElement : SearchParameters
    {
        public List<int> phaseId = new List<int>();
        public List<int> questId = new List<int>();
        public List<int> terrainElementId = new List<int>();

        public enum RequestType
        {
            Custom
        }

        public RequestType requestType;
    }

    [System.Serializable]
    public class PhaseRegion : SearchParameters
    {
        public List<int> phaseId = new List<int>();
        public List<int> regionId = new List<int>();

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
    public class Task : SearchParameters
    {
        public List<int> index = new List<int>();
        public List<int> objectiveId = new List<int>();
        public List<int> terrainElementId = new List<int>();

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
        public List<string> name = new List<string>();

        public enum RequestType
        {
            Custom
        }

        public RequestType requestType;
    }

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

    public class TerrainTile : SearchParameters
    {
        public List<int> index = new List<int>();

        public enum RequestType
        {
            Custom
        }

        public RequestType requestType;
    }

    public class TerrainObject : SearchParameters
    {
        public enum RequestType
        {
            Custom
        }

        public RequestType requestType;
    }

    public class TerrainElement : SearchParameters
    {
        public List<int> chapterId = new List<int>();
        public List<int> objectiveId = new List<int>();
        public List<int> elementId = new List<int>();
        public List<int> taskIndex = new List<int>();

        public enum RequestType
        {
            Custom,
            GetQuestAndObjectiveElements
        }

        public RequestType requestType;
    }

    public class PartyElement : SearchParameters
    {
        public List<int> chapterId = new List<int>();
        public List<int> elementId = new List<int>();

        public enum RequestType
        {
            Custom
        }

        public RequestType requestType;
    }
}
