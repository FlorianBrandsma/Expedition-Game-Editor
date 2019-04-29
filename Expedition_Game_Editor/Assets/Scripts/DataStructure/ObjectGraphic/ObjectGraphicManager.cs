using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class ObjectGraphicManager
{
    private List<int> id_list;

    private List<ObjectGraphicData> objectGraphicData_list;

    public List<ObjectGraphicDataElement> GetObjectGraphicDataElements(List<int> id_list)
    {
        this.id_list = id_list;

        GetObjectGraphicData();

        var list = (from objectGraphicData in objectGraphicData_list
                    select new ObjectGraphicDataElement()
                    {
                        id = objectGraphicData.id,
                        table = objectGraphicData.table,
                        type = objectGraphicData.type,

                        name = objectGraphicData.name,
                        icon = objectGraphicData.icon

                    }).OrderBy(x => x.name).ToList();

        list.ForEach(x => x.SetOriginalValues());

        return list;
    }

    public void GetObjectGraphicData()
    {
        objectGraphicData_list = new List<ObjectGraphicData>();

        string[] object_names = new string[] { "Null", "Warrior", "Polearm" };

        //Temporary
        foreach (int id in id_list)
        {
            var objectGraphicData = new ObjectGraphicData();

            objectGraphicData.id = id;
            objectGraphicData.table = "ObjectGraphic";

            objectGraphicData.name = object_names[id];
            objectGraphicData.icon = "Textures/Icons/Objects/" + object_names[id];

            objectGraphicData_list.Add(objectGraphicData);
        }
    }

    internal class ObjectGraphicData : GeneralData
    {
        public string name;
        public string icon;
    }
}
