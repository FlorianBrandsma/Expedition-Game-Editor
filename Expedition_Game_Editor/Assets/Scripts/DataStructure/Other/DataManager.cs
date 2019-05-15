using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DataManager
{
    #region Functions

    public List<ObjectGraphicData> GetObjectGraphicData(List<int> idList, bool searchById = false)
    {
        List<ObjectGraphicData> dataList = new List<ObjectGraphicData>();

        string[] objectNames = new string[] { "Nothing", "Polearm", "Warrior", "Blue", "Green" };

        for (int id = 0; id < objectNames.Length; id++)
        {
            var data = new ObjectGraphicData();

            data.id = id;

            data.table = "ObjectGraphic";
            data.name = objectNames[id];
            data.icon = "Textures/Icons/Objects/" + objectNames[id];

            if (searchById)
            {
                if (idList.Contains(id))
                    dataList.Add(data);
            } else {
                dataList.Add(data);
            }
        }

        return dataList;
    }

    public List<ElementData> GetElementData()
    {
        return GetElementData(new List<int>());
    }

    public List<ElementData> GetElementData(List<int> idList, bool searchById = false)
    {
        List<ElementData> dataList = new List<ElementData>();

        var objectList = new List<int> { 0, 2, 3, 4 };

        for (int i = 0; i < objectList.Count; i++)
        {
            var data = new ElementData();

            int id = (i + 1);

            data.id = id;
            data.table = "Element";

            data.objectGraphicId = objectList[i];

            if (searchById)
            {
                if (idList.Contains(id))
                    dataList.Add(data);
            }
            else
            {
                dataList.Add(data);
            }
        }

        return dataList;
    }

    #endregion

    #region Classes

    public class ObjectGraphicData : GeneralData
    {
        public string name;
        public string icon;
    }

    public class ElementData : GeneralData
    {
        public int objectGraphicId;
    }

    #endregion
}