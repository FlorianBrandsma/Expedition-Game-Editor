using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public static class TerrainDataManager
{
    private static List<TerrainBaseData> terrainDataList;

    private static List<RegionBaseData> regionDataList;
    private static List<TileSetBaseData> tileSetDataList;
    private static List<TileBaseData> tileDataList;

    private static List<IconBaseData> iconDataList;

    public static List<IElementData> GetData(SearchProperties searchProperties)
    {
        var searchParameters = searchProperties.searchParameters.Cast<Search.Terrain>().First();

        GetTerrainData(searchParameters);

        if (terrainDataList.Count == 0) return new List<IElementData>();
        
        GetRegionData();
        GetTileSetData();
        GetTileData();

        GetIconData();
        
        var list = (from terrainData    in terrainDataList
                    join regionData     in regionDataList   on terrainData.RegionId equals regionData.Id
                    join tileSetData    in tileSetDataList  on regionData.TileSetId equals tileSetData.Id

                    join leftJoin in (from tileData in tileDataList
                                      select new { tileData }) on tileSetData.Id equals leftJoin.tileData.TileSetId into tileData

                    join iconData       in iconDataList     on terrainData.IconId   equals iconData.Id

                    select new TerrainElementData()
                    {
                        Id = terrainData.Id,
                        Index = terrainData.Index,

                        RegionId = terrainData.RegionId,
                        IconId = terrainData.IconId,
                        Name = terrainData.Name,

                        IconPath = iconData.Path,

                        TileSetId = regionData.TileSetId,
                        BaseTilePath = tileData.First().tileData.IconPath

                    }).ToList();

        list.ForEach(x => x.SetOriginalValues());

        return list.Cast<IElementData>().ToList();
    }

    private static void GetTerrainData(Search.Terrain searchParameters)
    {
        terrainDataList = new List<TerrainBaseData>();

        foreach (TerrainBaseData terrain in Fixtures.terrainList)
        {
            if (searchParameters.id.Count       > 0 && !searchParameters.id.Contains(terrain.Id))               continue;
            if (searchParameters.regionId.Count > 0 && !searchParameters.regionId.Contains(terrain.RegionId))   continue;

            terrainDataList.Add(terrain);
        }
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

    public static void AddData(TerrainElementData elementData, DataRequest dataRequest)
    {
        if (dataRequest.requestType == Enums.RequestType.Execute)
        {
            elementData.Id = Fixtures.terrainList.Count > 0 ? (Fixtures.terrainList[Fixtures.terrainList.Count - 1].Id + 1) : 1;
            Fixtures.terrainList.Add(((TerrainData)elementData).Clone());
        }
        else { }
    }

    public static void UpdateData(TerrainElementData elementData, DataRequest dataRequest)
    {
        var data = Fixtures.terrainList.Where(x => x.Id == elementData.Id).FirstOrDefault();
        
        if (elementData.ChangedIconId)
        {
            if (dataRequest.requestType == Enums.RequestType.Execute)
                data.IconId = elementData.IconId;
            else { }
        }

        if (elementData.ChangedName)
        {
            if (dataRequest.requestType == Enums.RequestType.Execute)
                data.Name = elementData.Name;
            else { }
        }
    }
}
