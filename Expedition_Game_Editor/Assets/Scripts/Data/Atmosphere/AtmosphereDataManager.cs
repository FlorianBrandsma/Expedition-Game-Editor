using UnityEngine;
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
            if (searchParameters.id.Count           > 0 && !searchParameters.id.Contains(atmosphere.Id))                continue;
            if (searchParameters.terrainId.Count    > 0 && !searchParameters.terrainId.Contains(atmosphere.TerrainId))  continue;

            atmosphereDataList.Add(atmosphere);
        }
    }

    private static void GetTerrainData()
    {
        var searchParameters = new Search.Terrain();
        searchParameters.id = atmosphereDataList.Select(x => x.TerrainId).Distinct().ToList();

        terrainDataList = DataManager.GetTerrainData(searchParameters);
    }

    private static void GetRegionData()
    {
        var searchParameters = new Search.Region();
        searchParameters.id = terrainDataList.Select(x => x.RegionId).Distinct().ToList();

        regionDataList = DataManager.GetRegionData(searchParameters);
    }

    private static void GetTileSetData()
    {
        var searchParameters = new Search.TileSet();
        searchParameters.id = regionDataList.Select(x => x.TileSetId).Distinct().ToList();

        tileSetDataList = DataManager.GetTileSetData(searchParameters);
    }

    private static void GetTileData()
    {
        var searchParameters = new Search.Tile();
        searchParameters.tileSetId = tileSetDataList.Select(x => x.Id).Distinct().ToList();

        tileDataList = DataManager.GetTileData(searchParameters);
    }

    private static void GetIconData()
    {
        var searchParameters = new Search.Icon();
        searchParameters.id = terrainDataList.Select(x => x.IconId).Distinct().ToList();

        iconDataList = DataManager.GetIconData(searchParameters);
    }

    public static void UpdateData(AtmosphereElementData elementData, DataRequest dataRequest)
    {
        var data = Fixtures.atmosphereList.Where(x => x.Id == elementData.Id).FirstOrDefault();
        
        if (elementData.ChangedStartTime)
        {
            if (dataRequest.requestType == Enums.RequestType.Execute)
                data.StartTime = elementData.StartTime;
            else { }
        }
        
        if (elementData.ChangedEndTime)
        {
            if (dataRequest.requestType == Enums.RequestType.Execute)
                data.EndTime = elementData.EndTime;
            else { }
        }
        
        if (elementData.ChangedPublicNotes)
        {
            if (dataRequest.requestType == Enums.RequestType.Execute)
                data.PublicNotes = elementData.PublicNotes;
            else { }
        }
        
        if (elementData.ChangedPrivateNotes)
        {
            if (dataRequest.requestType == Enums.RequestType.Execute)
                data.PrivateNotes = elementData.PrivateNotes;
            else { }
        }
    }
}
