using UnityEngine;
using System.Collections.Generic;

public class Search
{
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
    public class ChapterElement : SearchParameters
    {
        public List<int> chapterId = new List<int>();

        public enum RequestType
        {
            Custom,
            GetChapterElementsById
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
            Custom
        }

        public RequestType requestType;
    }

    [System.Serializable]
    public class Quest : SearchParameters
    {
        public List<int> index = new List<int>();
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
        public List<string> name = new List<string>();
        public List<string> description = new List<string>();

        public enum RequestType
        {
            Custom
        }

        public RequestType requestType;
    }

    [System.Serializable]
    public class ObjectiveElement : SearchParameters
    {
        public List<int> index = new List<int>();
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
        public List<string> name = new List<string>();
        public List<string> description = new List<string>();

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

        public enum RequestType
        {
            Custom
        }

        public RequestType requestType;
    }
}
