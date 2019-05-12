using UnityEngine;
using System.Collections;

public class Search
{
    [System.Serializable]
    public class Chapter : SearchParameters
    {
        public enum IntegerColumn
        {
            none,
            id,
            index,
        }

        public enum StringColumn
        {
            none,
            name,
            description
        }

        public IntegerColumn integerColumn;
        public StringColumn stringColumn;
    }

    [System.Serializable]
    public class Phase : SearchParameters
    {
        public enum IntegerColumn
        {
            none,
            id,
            index,
            chapterId
        }

        public enum StringColumn
        {
            none,
            name,
            description
        }

        public IntegerColumn integerColumn;
        public StringColumn stringColumn;
    }

    [System.Serializable]
    public class ObjectGraphic : SearchParameters
    {

    }

    [System.Serializable]
    public class Element : SearchParameters
    {
        public enum RequestType
        {
            Custom,
            GetAllElements,
        }

        public RequestType requestType;
    }

    [System.Serializable]
    public class ChapterElement : SearchParameters
    {
        public enum RequestType
        {
            Custom,
            GetChapterElementsById
        }

        public RequestType requestType;
    }

    [System.Serializable]
    public class Item : SearchParameters
    {
        public enum RequestType
        {
            Custom,
            GetItemsByType,
        }

        public RequestType requestType;
    }
}
