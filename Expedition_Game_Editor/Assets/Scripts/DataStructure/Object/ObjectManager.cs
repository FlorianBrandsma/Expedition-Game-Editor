using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class ObjectManager
{
    private ObjectController dataController;
    private List<ObjectData> objectData_list;

    public List<ObjectDataElement> GetObjectDataElements(ObjectController dataController)
    {
        this.dataController = dataController;

        GetObjectData();
        //GetIconData()?

        var list = (from oCore in objectData_list
                    select new ObjectDataElement()
                    {
                        id = oCore.id,
                        table = oCore.table,
                        type = oCore.type,
                        index = oCore.index,
                        name = oCore.name,
                        description = oCore.description,

                        icon = "Textures/Characters/1"

                    }).OrderBy(x => x.index).ToList();

        list.ForEach(x => x.SetOriginalValues());

        return list;
    }

    public void GetObjectData()
    {
        objectData_list = new List<ObjectData>();

        //Temporary
        for (int i = 0; i < dataController.temp_id_count; i++)
        {
            var objectData = new ObjectData();

            objectData.id = (i + 1);
            objectData.table = "Object";
            objectData.index = i;

            objectData.name = "Object " + (i + 1);
            objectData.description = "This is a pretty regular sentence. The structure is something you'd expect. Nothing too long though!";

            objectData_list.Add(objectData);
        }
    }

    internal class ObjectData : GeneralData
    {
        public int index;
        public string name;
        public string description;
    }
}
