using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public static class AtmosphereDataManager
{
    private static List<AtmosphereBaseData> atmosphereDataList;

    private static List<TerrainBaseData> terrainDataList;

    private static List<RegionBaseData> regionDataList;
    private static List<TileSetBaseData> tileSetDataList;
    private static List<TileBaseData> tileDataList;

    private static List<IconBaseData> iconDataList;

    public static List<IElementData> GetData(SearchProperties searchProperties)
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
                    join terrainData    in terrainDataList  on atmosphereData.TerrainId equals terrainData.Id
                    join regionData     in regionDataList   on terrainData.RegionId     equals regionData.Id
                    join tileSetData    in tileSetDataList  on regionData.TileSetId     equals tileSetData.Id

                    join leftJoin in (from tileData in tileDataList
                                      select new { tileData }) on tileSetData.Id equals leftJoin.tileData.TileSetId into tileData

                    join iconData       in iconDataList     on terrainData.IconId       equals iconData.Id

                    select new AtmosphereElementData()
                    {
                        Id = atmosphereData.Id,

                        TerrainId = atmosphereData.TerrainId,

                        Default = atmosphereData.Default,

                        StartTime = atmosphereData.StartTime,
                        EndTime = atmosphereData.EndTime,

                        PublicNotes = atmosphereData.PublicNotes,
                        PrivateNotes = atmosphereData.PrivateNotes,

                        RegionName = regionData.Name,
                        TerrainName = terrainData.Name,

                        IconPath = iconData.Path,
                        BaseTilePath = tileData.First().tileData.IconPath

                    }).OrderByDescending(x => x.Default).ThenBy(x => x.StartTime).ToList();

        list.ForEach(x => x.SetOriginalValues());

        return list.Cast<IElementData>().ToList();
    }

    private static void GetAtmosphereData(Search.Atmosphere searchParameters)
    {
        atmosphereDataList = new List<AtmosphereBaseData>();

        foreach (AtmosphereBaseData atmosphere in Fixtures.atmosphereList)
        {
            if (searchParameters.id.Count > 0 && !searchParameters.id.Contains(atmosphere.Id)) continue;
            if (searchParameters.terrainId.Count > 0 && !searchParameters.terrainId.Contains(atmosphere.TerrainId)) continue;

            var atmosphereData = new AtmosphereBaseData();

            atmosphereData.Id = atmosphere.Id;

            atmosphereData.TerrainId = atmosphere.TerrainId;

            atmosphereData.Default = atmosphere.Default;

            atmosphereData.StartTime = atmosphere.StartTime;
            atmosphereData.EndTime = atmosphere.EndTime;

            atmosphereData.PublicNotes = atmosphere.PublicNotes;
            atmosphereData.PrivateNotes = atmosphere.PrivateNotes;

            atmosphereDataList.Add(atmosphereData);
        }
    }

    private static void GetTerrainData()
    {
        var terrainSearchParameters = new Search.Terrain();
        terrainSearchParameters.id = atmosphereDataList.Select(x => x.TerrainId).Distinct().ToList();

        terrainDataList = DataManager.GetTerrainData(terrainSearchParameters);
    }

    private static void GetRegionData()
    {
        var regionSearchParameters = new Search.Region();
        regionSearchParameters.id = terrainDataList.Select(x => x.RegionId).Distinct().ToList();

        regionDataList = DataManager.GetRegionData(regionSearchParameters);
    }

    private static void GetTileSetData()
    {
        var tileSetSearchParameters = new Search.TileSet();
        tileSetSearchParameters.id = regionDataList.Select(x => x.TileSetId).Distinct().ToList();

        tileSetDataList = DataManager.GetTileSetData(tileSetSearchParameters);
    }

    private static void GetTileData()
    {
        var tileSearchParameters = new Search.Tile();
        tileSearchParameters.tileSetId = tileSetDataList.Select(x => x.Id).Distinct().ToList();

        tileDataList = DataManager.GetTileData(tileSearchParameters);
    }

    private static void GetIconData()
    {
        var iconSearchParameters = new Search.Icon();
        iconSearchParameters.id = terrainDataList.Select(x => x.IconId).Distinct().ToList();

        iconDataList = DataManager.GetIconData(iconSearchParameters);
    }

    public static void UpdateData(AtmosphereElementData elementData)
    {
        var data = Fixtures.atmosphereList.Where(x => x.Id == elementData.Id).FirstOrDefault();
        
        if (elementData.ChangedStartTime)
            data.StartTime = elementData.StartTime;

        if (elementData.ChangedEndTime)
            data.EndTime = elementData.EndTime;

        if (elementData.ChangedPublicNotes)
            data.PublicNotes = elementData.PublicNotes;

        if (elementData.ChangedPrivateNotes)
            data.PrivateNotes = elementData.PrivateNotes;
    }
}
