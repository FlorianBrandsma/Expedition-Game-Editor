using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class TerrainDataManager
{
    private TerrainController terrainController;
    private List<TerrainData> terrainDataList;

    public void InitializeManager(TerrainController terrainController)
    {
        this.terrainController = terrainController;
    }

    public List<IDataElement> GetTerrainDataElements(IEnumerable searchParameters)
    {
        var objectiveSearchData = searchParameters.Cast<Search.Terrain>().FirstOrDefault();

        GetTerrainData(objectiveSearchData);

        var list = (from terrainData in terrainDataList
                    select new TerrainDataElement()
                    {
                        id = terrainData.id,
                        table = terrainData.table,

                        icon = "Textures/Icons/Objects/Nothing"

                    }).ToList();

        list.ForEach(x => x.SetOriginalValues());

        return list.Cast<IDataElement>().ToList();
    }

    public void GetTerrainData(Search.Terrain searchParameters)
    {
        terrainDataList = new List<TerrainData>();

        int index = 0;

        for (int i = 0; i < searchParameters.temp_id_count; i++)
        {
            var terrainData = new TerrainData();

            int id = (i + 1);

            terrainData.id = id;
            terrainData.table = "Terrain";
            terrainData.index = index;

            terrainData.name = "Terrain " + id;

            terrainDataList.Add(terrainData);

            index++;
        }
    }

    internal class TerrainData : GeneralData
    {
        public int index;
        public string name;
    }
}
