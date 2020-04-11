using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class AtmosphereDataManager : IDataManager
{
    public IDataController DataController { get; set; }
    private List<AtmosphereData> atmosphereDataList;

    private DataManager dataManager = new DataManager();

    private List<DataManager.TerrainData> terrainDataList;

    private List<DataManager.RegionData> regionDataList;
    private List<DataManager.TileSetData> tileSetDataList;
    private List<DataManager.TileData> tileDataList;

    private List<DataManager.IconData> iconDataList;
    
    public AtmosphereDataManager(IDataController dataController)
    {
        DataController = dataController;
    }

    public List<IDataElement> GetDataElements(IEnumerable searchParameters)
    {
        var atmosphereSearchData = searchParameters.Cast<Search.Atmosphere>().FirstOrDefault();

        GetAtmosphereData(atmosphereSearchData);

        GetTerrainData();

        GetRegionData();
        GetTileSetData();
        GetTileData();

        GetIconData();
        
        var list = (from atmosphereData in atmosphereDataList
                    join terrainData    in terrainDataList  on atmosphereData.terrainId equals terrainData.Id
                    join regionData     in regionDataList   on terrainData.regionId     equals regionData.Id
                    join tileSetData    in tileSetDataList  on regionData.tileSetId     equals tileSetData.Id

                    join leftJoin in (from tileData in tileDataList
                                      select new { tileData }) on tileSetData.Id equals leftJoin.tileData.tileSetId into tileData

                    join iconData       in iconDataList     on terrainData.iconId       equals iconData.Id

                    select new AtmosphereDataElement()
                    {
                        Id = atmosphereData.Id,

                        TerrainId = atmosphereData.terrainId,

                        Default = atmosphereData.isDefault,

                        StartTime = atmosphereData.startTime,
                        EndTime = atmosphereData.endTime,

                        regionName = regionData.name,
                        terrainName = terrainData.name,

                        iconPath = iconData.path,
                        baseTilePath = tileData.First().tileData.iconPath

                    }).OrderByDescending(x => x.Default).ThenBy(x => x.StartTime).ToList();

        list.ForEach(x => x.SetOriginalValues());

        return list.Cast<IDataElement>().ToList();
    }

    public void GetAtmosphereData(Search.Atmosphere searchParameters)
    {
        atmosphereDataList = new List<AtmosphereData>();

        foreach (Fixtures.Atmosphere atmosphere in Fixtures.atmosphereList)
        {
            if (searchParameters.id.Count > 0 && !searchParameters.id.Contains(atmosphere.Id)) continue;
            if (searchParameters.terrainId.Count > 0 && !searchParameters.terrainId.Contains(atmosphere.terrainId)) continue;

            var atmosphereData = new AtmosphereData();

            atmosphereData.Id = atmosphere.Id;

            atmosphereData.terrainId = atmosphere.terrainId;

            atmosphereData.isDefault = atmosphere.isDefault;

            atmosphereData.startTime = atmosphere.startTime;
            atmosphereData.endTime = atmosphere.endTime;

            atmosphereDataList.Add(atmosphereData);
        }
    }

    internal void GetTerrainData()
    {
        var terrainSearchParameters = new Search.Terrain();
        terrainSearchParameters.id = atmosphereDataList.Select(x => x.terrainId).Distinct().ToList();

        terrainDataList = dataManager.GetTerrainData(terrainSearchParameters);
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
    
    internal class AtmosphereData : GeneralData
    {
        public int terrainId;

        public bool isDefault;

        public int startTime;
        public int endTime;
    }
}
