using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class TerrainDataManager
{
    private TerrainController terrainController;
    private List<TerrainData> terrainDataList;

    private DataManager dataManager = new DataManager();

    private List<DataManager.IconData> iconDataList;

    public void InitializeManager(TerrainController terrainController)
    {
        this.terrainController = terrainController;
    }

    public List<IDataElement> GetTerrainDataElements(IEnumerable searchParameters)
    {
        var objectiveSearchData = searchParameters.Cast<Search.Terrain>().FirstOrDefault();

        GetTerrainData(objectiveSearchData);
        GetIconData();

        var list = (from terrainData in terrainDataList
                    join iconData in iconDataList on terrainData.iconId equals iconData.id
                    join baseIconData in iconDataList on terrainData.baseTileIconId equals baseIconData.id
                    
                    select new TerrainDataElement()
                    {
                        id = terrainData.id,
                        table = "Terrain",
                        index = terrainData.index,

                        RegionId = terrainData.regionId,
                        IconId = terrainData.iconId,
                        Name = terrainData.name,

                        iconPath = iconData.path,

                        baseTilePath = baseIconData.path

                    }).ToList();

        list.ForEach(x => x.SetOriginalValues());

        return list.Cast<IDataElement>().ToList();
    }

    public void GetTerrainData(Search.Terrain searchParameters)
    {
        terrainDataList = new List<TerrainData>();

        foreach (Fixtures.Terrain terrain in Fixtures.terrainList)
        {
            if (searchParameters.regionId.Count > 0 && !searchParameters.regionId.Contains(terrain.regionId)) continue;

            var tileList = Fixtures.terrainTileList.Where(x => x.terrainId == terrain.id).Distinct().ToList();
            var mostCommonTileId = tileList.GroupBy(x => x.tileId).OrderByDescending(x => x.Count()).Select(x => x.Key).FirstOrDefault();

            var baseTile = Fixtures.tileList.Where(x => x.id == mostCommonTileId).Distinct().FirstOrDefault();

            var terrainData = new TerrainData();

            terrainData.id = terrain.id;
            terrainData.index = terrain.index;

            terrainData.regionId = terrain.regionId;
            terrainData.iconId = terrain.iconId;
            terrainData.name = terrain.name;

            terrainData.baseTileIconId = baseTile.iconId;

            terrainDataList.Add(terrainData);
        }
    }

    internal void GetIconData()
    {
        iconDataList = dataManager.GetIconData(terrainDataList.Select(x => new List<int>() { x.iconId, x.baseTileIconId }).SelectMany(x => x).Distinct().ToList(), true);
    }

    internal class TerrainData : GeneralData
    {
        public int regionId;
        public int iconId;
        public string name;

        public int baseTileIconId;
    }
}
