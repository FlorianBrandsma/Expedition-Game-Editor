using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class TerrainObjectManager
{
    private TerrainObjectController dataController;
    private List<TerrainObjectData> terrainObjectData_list;

    public List<TerrainObjectDataElement> GetTerrainObjectDataElements(TerrainObjectController dataController)
    {
        this.dataController = dataController;

        GetTerrainObjectData();
        //GetIconData()?

        var list = (from oCore in terrainObjectData_list
                    select new TerrainObjectDataElement()
                    {
                        id = oCore.id,
                        table = oCore.table,
                        type = oCore.type,
                        index = oCore.index,
                        name = oCore.name,
                        description = oCore.description,

                        icon = "Textures/Icons/Objects/0"

                    }).OrderBy(x => x.index).ToList();

        list.ForEach(x => x.SetOriginalValues());

        return list;
    }

    public void GetTerrainObjectData()
    {
        terrainObjectData_list = new List<TerrainObjectData>();

        //Temporary
        for (int i = 0; i < dataController.temp_id_count; i++)
        {
            var terrainObjectData = new TerrainObjectData();

            terrainObjectData.id = (i + 1);
            terrainObjectData.table = "TerrainObject";
            terrainObjectData.index = i;

            terrainObjectData.name = "TerrainObject " + (i + 1);
            terrainObjectData.description = "This is a pretty regular sentence. The structure is something you'd expect. Nothing too long though!";

            terrainObjectData_list.Add(terrainObjectData);
        }
    }

    internal class TerrainObjectData : GeneralData
    {
        public int index;
        public string name;
        public string description;
    }
}
