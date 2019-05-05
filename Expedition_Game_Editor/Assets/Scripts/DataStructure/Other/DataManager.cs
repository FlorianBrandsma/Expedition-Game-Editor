using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DataManager
{
    public List<ObjectGraphicData> GetObjectGraphicData(List<int> idList, bool searchById = false)
    {
        List<ObjectGraphicData> data_list = new List<ObjectGraphicData>();

        //Temporary
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
                if (idList[0] == (id))
                    data_list.Add(data);
            } else {
                data_list.Add(data);
            }
        }
        //

        return data_list;
    }

    public class ObjectGraphicData : GeneralData
    {
        public string name;
        public string icon;
    }
}