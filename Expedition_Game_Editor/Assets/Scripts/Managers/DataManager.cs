using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DataManager
{
    public List<ObjectGraphicData> GetObjectGraphicData(List<int> id_list, bool search_by_id = false)
    {
        List<ObjectGraphicData> data_list = new List<ObjectGraphicData>();

        //Temporary
        string[] object_names = new string[] { "Null", "Warrior", "Polearm" };

        for (int id = 0; id <= 2; id++)
        {
            var data = new ObjectGraphicData();

            data.id = id;

            data.table = "ObjectGraphic";
            data.name = object_names[id];
            data.icon = "Textures/Icons/Objects/" + object_names[id];

            if (search_by_id && id_list[0] == (id))
                data_list.Add(data);
            else
                data_list.Add(data);
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