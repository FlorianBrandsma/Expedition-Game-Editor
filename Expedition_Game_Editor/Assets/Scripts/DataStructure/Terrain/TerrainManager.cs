using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class TerrainManager
{
    private TerrainController dataController;
    private List<TerrainData> terrainData_list;

    public List<TerrainDataElement> GetTerrainDataElements(TerrainController dataController)
    {
        this.dataController = dataController;

        GetTerrainData();
        //GetIconData()?

        var list = (from oCore in terrainData_list
                    select new TerrainDataElement()
                    {
                        id = oCore.id,
                        table = oCore.table,
                        type = oCore.type,

                        icon = "Textures/Characters/1"

                    }).ToList();

        list.ForEach(x => x.SetOriginalValues());

        return list;
    }

    public void GetTerrainData()
    {
        terrainData_list = new List<TerrainData>();

        //Temporary
        for (int i = 0; i < dataController.temp_id_count; i++)
        {
            var terrainData = new TerrainData();

            terrainData.id = (i + 1);
            terrainData.table = "Terrain";
            terrainData.index = i;

            terrainData.name = "Terrain " + (i + 1);
            terrainData.description = "This is a pretty regular sentence. The structure is something you'd expect. Nothing too long though!";

            terrainData_list.Add(terrainData);
        }
    }

    internal class TerrainData : GeneralData
    {
        public int index;
        public string name;
        public string description;
    }
}
