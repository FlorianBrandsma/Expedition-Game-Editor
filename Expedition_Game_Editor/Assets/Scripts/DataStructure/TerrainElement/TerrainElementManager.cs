using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class TerrainElementManager
{
    private TerrainElementController dataController;
    private List<TerrainElementData> terrainElementData_list;

    public List<TerrainElementDataElement> GetTerrainElementDataElements(TerrainElementController dataController)
    {
        this.dataController = dataController;

        GetTerrainElementData();
        //GetIconData()?

        var list = (from oCore in terrainElementData_list
                    select new TerrainElementDataElement()
                    {
                        id = oCore.id,
                        table = oCore.table,
                        type = oCore.type,
                        index = oCore.index,
                        name = oCore.name,
                        description = oCore.description,

                        icon = "Textures/Icons/Objects/Nothing"

                    }).OrderBy(x => x.index).ToList();

        list.ForEach(x => x.SetOriginalValues());

        return list;
    }

    public void GetTerrainElementData()
    {
        terrainElementData_list = new List<TerrainElementData>();

        //Temporary
        for (int i = 0; i < dataController.temp_id_count; i++)
        {
            var terrainElementData = new TerrainElementData();

            terrainElementData.id = (i + 1);
            terrainElementData.table = "TerrainElement";
            terrainElementData.index = i;

            terrainElementData.name = "TerrainElement " + (i + 1);
            terrainElementData.description = "This is a pretty regular sentence. The structure is something you'd expect. Nothing too long though!";

            terrainElementData_list.Add(terrainElementData);
        }
    }

    internal class TerrainElementData : GeneralData
    {
        public int index;
        public string name;
        public string description;
    }
}
