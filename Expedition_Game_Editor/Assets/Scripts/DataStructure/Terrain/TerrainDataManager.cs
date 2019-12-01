using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class TerrainDataManager : IDataManager
{
    public IDataController DataController { get; set; }
    private List<TerrainData> terrainDataList;

    private DataManager dataManager = new DataManager();

    private List<DataManager.IconData> iconDataList;

    public TerrainDataManager(TerrainController terrainController)
    {
        DataController = terrainController;
    }

    public List<IDataElement> GetDataElements(IEnumerable searchParameters)
    {
        var objectiveSearchData = searchParameters.Cast<Search.Terrain>().FirstOrDefault();

        GetTerrainData(objectiveSearchData);
        GetIconData();

        var list = (from terrainData in terrainDataList
                    join iconData in iconDataList on terrainData.iconId equals iconData.Id

                    select new TerrainDataElement()
                    {
                        DataType = Enums.DataType.Terrain,

                        Id = terrainData.Id,
                        Index = terrainData.Index,

                        RegionId = terrainData.regionId,
                        IconId = terrainData.iconId,
                        Name = terrainData.name,

                        iconPath = iconData.path,

                        tileSetId = terrainData.tileSetId,
                        baseTilePath = terrainData.baseTileIconPath

                    }).ToList();

        list.ForEach(x => x.SetOriginalValues());

        return list.Cast<IDataElement>().ToList();
    }

    public void GetTerrainData(Search.Terrain searchParameters)
    {
        terrainDataList = new List<TerrainData>();

        foreach (Fixtures.Terrain terrain in Fixtures.terrainList)
        {
            if (searchParameters.id.Count > 0 && !searchParameters.id.Contains(terrain.Id)) continue;
            if (searchParameters.regionId.Count > 0 && !searchParameters.regionId.Contains(terrain.regionId)) continue;

            var region = Fixtures.regionList.Where(x => x.Id == terrain.regionId).FirstOrDefault();

            var tileList = Fixtures.terrainTileList.Where(x => x.terrainId == terrain.Id).Distinct().ToList();
            var mostCommonTileId = tileList.GroupBy(x => x.tileId).OrderByDescending(x => x.Count()).Select(x => x.Key).FirstOrDefault();

            var baseTile = Fixtures.tileList.Where(x => x.Id == mostCommonTileId).Distinct().FirstOrDefault();

            var terrainData = new TerrainData();

            terrainData.Id = terrain.Id;
            terrainData.Index = terrain.Index;

            terrainData.regionId = terrain.regionId;
            terrainData.iconId = terrain.iconId;
            terrainData.name = terrain.name;

            terrainData.tileSetId = region.tileSetId;
            terrainData.baseTileIconPath = baseTile.iconPath;

            terrainDataList.Add(terrainData);
        }
    }

    internal void GetIconData()
    {
        iconDataList = dataManager.GetIconData(terrainDataList.Select(x => x.iconId).Distinct().ToList(), true);
    }

    internal class TerrainData : GeneralData
    {
        public int regionId;
        public int iconId;
        public string name;

        public int tileSetId;
        public string baseTileIconPath;
    }
}
