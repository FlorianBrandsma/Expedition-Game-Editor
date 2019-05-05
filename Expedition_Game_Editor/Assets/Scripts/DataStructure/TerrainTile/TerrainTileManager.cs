using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class TerrainTileManager
{
    private TerrainTileController dataController;
    private List<TerrainTileData> terrainTileData_list;

    public List<TerrainTileDataElement> GetTerrainTileDataElements(TerrainTileController dataController)
    {
        this.dataController = dataController;

        GetTerrainTileData();
        //GetIconData()?

        var list = (from oCore in terrainTileData_list
                    select new TerrainTileDataElement()
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

    public void GetTerrainTileData()
    {
        terrainTileData_list = new List<TerrainTileData>();

        //Temporary
        for (int i = 0; i < dataController.temp_id_count; i++)
        {
            var terrainTileData = new TerrainTileData();

            terrainTileData.id = (i + 1);
            terrainTileData.table = "TerrainTile";
            terrainTileData.index = i;

            terrainTileData.name = "TerrainTile " + (i + 1);
            terrainTileData.description = "This is a pretty regular sentence. The structure is something you'd expect. Nothing too long though!";

            terrainTileData_list.Add(terrainTileData);
        }
    }

    internal class TerrainTileData : GeneralData
    {
        public int index;
        public string name;
        public string description;
    }
}
