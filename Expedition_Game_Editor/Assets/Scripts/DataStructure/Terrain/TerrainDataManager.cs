using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class TerrainDataManager : IDataManager
{
    public IDataController DataController { get; set; }
    private List<TerrainData> terrainDataList;

    private DataManager dataManager = new DataManager();

    private List<DataManager.RegionData> regionDataList;
    private List<DataManager.TileSetData> tileSetDataList;
    private List<DataManager.TileData> tileDataList;

    private List<DataManager.IconData> iconDataList;

    public TerrainDataManager(TerrainController terrainController)
    {
        DataController = terrainController;
    }

    public List<IDataElement> GetDataElements(IEnumerable searchParameters)
    {
        var objectiveSearchData = searchParameters.Cast<Search.Terrain>().FirstOrDefault();

        GetTerrainData(objectiveSearchData);

        GetRegionData();
        GetTileSetData();
        GetTileData();

        GetIconData();
        
        var list = (from terrainData    in terrainDataList
                    join regionData     in regionDataList   on terrainData.regionId equals regionData.Id
                    join tileSetData    in tileSetDataList  on regionData.tileSetId equals tileSetData.Id

                    join leftJoin in (from tileData in tileDataList
                                      select new { tileData }) on tileSetData.Id equals leftJoin.tileData.tileSetId into tileData

                    join iconData       in iconDataList     on terrainData.iconId   equals iconData.Id

                    select new TerrainDataElement()
                    {
                        Id = terrainData.Id,
                        Index = terrainData.Index,

                        RegionId = terrainData.regionId,
                        IconId = terrainData.iconId,
                        Name = terrainData.name,

                        iconPath = iconData.path,

                        tileSetId = regionData.tileSetId,
                        baseTilePath = tileData.First().tileData.iconPath

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
            
            var terrainData = new TerrainData();

            terrainData.Id = terrain.Id;
            terrainData.Index = terrain.Index;

            terrainData.regionId = terrain.regionId;
            terrainData.iconId = terrain.iconId;
            terrainData.name = terrain.name;

            terrainDataList.Add(terrainData);
        }
    }

    internal void GetRegionData()
    {
        var regionSearchParameters = new Search.Region();
        regionSearchParameters.id = terrainDataList.Select(x => x.regionId).Distinct().ToList();

        regionDataList = dataManager.GetRegionData(regionSearchParameters);
    }

    private void GetTileSetData()
    {
        var tileSetSearchParameters = new Search.TileSet();
        tileSetSearchParameters.id = regionDataList.Select(x => x.tileSetId).Distinct().ToList();

        tileSetDataList = dataManager.GetTileSetData(tileSetSearchParameters);
    }

    private void GetTileData()
    {
        var tileSearchParameters = new Search.Tile();
        tileSearchParameters.tileSetId = tileSetDataList.Select(x => x.Id).Distinct().ToList();

        tileDataList = dataManager.GetTileData(tileSearchParameters);
    }

    internal void GetIconData()
    {
        var iconSearchParameters = new Search.Icon();
        iconSearchParameters.id = terrainDataList.Select(x => x.iconId).Distinct().ToList();

        iconDataList = dataManager.GetIconData(iconSearchParameters);
    }

    internal class TerrainData : GeneralData
    {
        public int regionId;
        public int iconId;
        public string name;
    }
}
