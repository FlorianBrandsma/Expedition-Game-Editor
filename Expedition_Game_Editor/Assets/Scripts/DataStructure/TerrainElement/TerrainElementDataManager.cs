using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class TerrainElementDataManager
{
    private TerrainElementController terrainElementController;
    private List<TerrainElementData> terrainElementDataList;

    public void InitializeManager(TerrainElementController terrainElementController)
    {
        this.terrainElementController = terrainElementController;
    }

    public List<TerrainElementDataElement> GetTerrainElementDataElements(IEnumerable searchParameters)
    {
        var terrainElementSearchData = searchParameters.Cast<Search.TerrainElement>().FirstOrDefault();

        GetTerrainElementData(terrainElementSearchData);

        var list = (from terrainElementData in terrainElementDataList
                    select new TerrainElementDataElement()
                    {
                        id = terrainElementData.id,
                        table = terrainElementData.table,

                        Index = terrainElementData.index,

                        icon = "Textures/Icons/Objects/Nothing"

                    }).OrderBy(x => x.Index).ToList();

        list.ForEach(x => x.SetOriginalValues());

        return list;
    }

    public void GetTerrainElementData(Search.TerrainElement searchParameters)
    {
        terrainElementDataList = new List<TerrainElementData>();

        int index = 0;

        for (int i = 0; i < searchParameters.temp_id_count; i++)
        {
            var terrainElementData = new TerrainElementData();

            int id = (i + 1);

            terrainElementData.id = id;
            terrainElementData.table = "TerrainElement";
            terrainElementData.index = index;

            terrainElementDataList.Add(terrainElementData);

            index++;
        }
    }

    internal class TerrainElementData : GeneralData
    {
        public int index;
    }
}
