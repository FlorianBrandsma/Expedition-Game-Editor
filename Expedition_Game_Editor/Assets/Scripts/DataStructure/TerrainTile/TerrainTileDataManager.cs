using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class TerrainTileDataManager
{
    private TerrainTileController dataController;
    private List<TerrainTileData> terrainTileDataList;

    public List<TerrainTileDataElement> GetTerrainTileDataElements(TerrainTileController dataController)
    {
        this.dataController = dataController;

        GetTerrainTileData();
        //GetIconData()?

        var list = (from terrainTileData in terrainTileDataList
                    select new TerrainTileDataElement()
                    {
                        id = terrainTileData.id,
                        table = terrainTileData.table,

                        Index = terrainTileData.index,
                        icon = "Textures/Icons/Objects/Nothing"

                    }).OrderBy(x => x.Index).ToList();

        list.ForEach(x => x.SetOriginalValues());

        return list;
    }

    public void GetTerrainTileData()
    {
        terrainTileDataList = new List<TerrainTileData>();

        //Temporary
        for (int i = 0; i < dataController.temp_id_count; i++)
        {
            var terrainTileData = new TerrainTileData();

            terrainTileData.id = (i + 1);
            terrainTileData.table = "TerrainTile";
            terrainTileData.index = i;

            terrainTileDataList.Add(terrainTileData);
        }
    }

    internal class TerrainTileData : GeneralData
    {
        public int index;
    }
}
