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

    public List<IElementData> GetData(SearchProperties searchProperties)
    {
        var searchParameters = searchProperties.searchParameters.Cast<Search.Atmosphere>().First();

        GetAtmosphereData(searchParameters);

        if (atmosphereDataList.Count == 0) return new List<IElementData>();

        GetTerrainData();

        GetRegionData();
        GetTileSetData();

        GetTileData();

        GetIconData();
        
        var list = (from atmosphereData in atmosphereDataList
                    join terrainData    in terrainDataList  on atmosphereData.terrainId equals terrainData.id
                    join regionData     in regionDataList   on terrainData.regionId     equals regionData.id
                    join tileSetData    in tileSetDataList  on regionData.tileSetId     equals tileSetData.id

                    join leftJoin in (from tileData in tileDataList
                                      select new { tileData }) on tileSetData.id equals leftJoin.tileData.tileSetId into tileData

                    join iconData       in iconDataList     on terrainData.iconId       equals iconData.id

                    select new AtmosphereElementData()
                    {
                        Id = atmosphereData.id,

                        TerrainId = atmosphereData.terrainId,

                        Default = atmosphereData.isDefault,

                        StartTime = atmosphereData.startTime,
                        EndTime = atmosphereData.endTime,

                        PublicNotes = atmosphereData.publicNotes,
                        PrivateNotes = atmosphereData.privateNotes,

                        regionName = regionData.name,
                        terrainName = terrainData.name,

                        iconPath = iconData.path,
                        baseTilePath = tileData.First().tileData.iconPath

                    }).OrderByDescending(x => x.Default).ThenBy(x => x.StartTime).ToList();

        list.ForEach(x => x.SetOriginalValues());

        return list.Cast<IElementData>().ToList();
    }

    public void GetAtmosphereData(Search.Atmosphere searchParameters)
    {
        atmosphereDataList = new List<AtmosphereData>();

        foreach (Fixtures.Atmosphere atmosphere in Fixtures.atmosphereList)
        {
            if (searchParameters.id.Count > 0 && !searchParameters.id.Contains(atmosphere.id)) continue;
            if (searchParameters.terrainId.Count > 0 && !searchParameters.terrainId.Contains(atmosphere.terrainId)) continue;

            var atmosphereData = new AtmosphereData();

            atmosphereData.id = atmosphere.id;

            atmosphereData.terrainId = atmosphere.terrainId;

            atmosphereData.isDefault = atmosphere.isDefault;

            atmosphereData.startTime = atmosphere.startTime;
            atmosphereData.endTime = atmosphere.endTime;

            atmosphereData.publicNotes = atmosphere.publicNotes;
            atmosphereData.privateNotes = atmosphere.privateNotes;

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
        tileSearchParameters.tileSetId = tileSetDataList.Select(x => x.id).Distinct().ToList();

        tileDataList = dataManager.GetTileData(tileSearchParameters);
    }

    internal void GetIconData()
    {
        var iconSearchParameters = new Search.Icon();
        iconSearchParameters.id = terrainDataList.Select(x => x.iconId).Distinct().ToList();

        iconDataList = dataManager.GetIconData(iconSearchParameters);
    }
    
    internal class AtmosphereData
    {
        public int id;

        public int terrainId;

        public bool isDefault;

        public int startTime;
        public int endTime;

        public string publicNotes;
        public string privateNotes;
    }
}
