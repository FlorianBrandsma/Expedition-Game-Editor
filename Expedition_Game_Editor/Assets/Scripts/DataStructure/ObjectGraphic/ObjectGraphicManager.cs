using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class ObjectGraphicManager
{
    private List<ObjectGraphicData> objectGraphicDataList;

    public List<ObjectGraphicDataElement> GetObjectGraphicDataElements(List<int> idList)
    {
        GetObjectGraphicData(idList);

        var list = (from objectGraphicData in objectGraphicDataList
                    select new ObjectGraphicDataElement()
                    {
                        id = objectGraphicData.id,
                        table = objectGraphicData.table,
                        type = objectGraphicData.type,

                        Name = objectGraphicData.name,
                        Icon = objectGraphicData.icon

                    }).OrderByDescending(x => x.id == 0).ThenBy(x => x.Name).ToList();

        list.ForEach(x => x.SetOriginalValues());

        return list;
    }

    public void GetObjectGraphicData(List<int> idList)
    {
        objectGraphicDataList = new List<ObjectGraphicData>();

        string[] object_names = new string[] { "Nothing", "Polearm", "Warrior", "Blue", "Green" };

        //Temporary
        foreach (int id in idList)
        {
            var objectGraphicData = new ObjectGraphicData();

            objectGraphicData.id = id;
            objectGraphicData.table = "ObjectGraphic";

            objectGraphicData.name = object_names[id];
            objectGraphicData.icon = "Textures/Icons/Objects/" + object_names[id];

            objectGraphicDataList.Add(objectGraphicData);
        }
    }

    internal class ObjectGraphicData : GeneralData
    {
        public string name;
        public string icon;
    }
}
