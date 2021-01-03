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

    public static List<IElementData> GetData(Search.Terrain searchParameters)
    {
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

    public static TerrainElementData DefaultData(int regionId, int regionIndex)
    {
        return new TerrainElementData()
        {
            RegionId = regionId,
            IconId = 1,

            Index = regionIndex,
        };
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

    public static void AddData(TerrainElementData elementData, DataRequest dataRequest, bool copy = false)
    {
        if (dataRequest.requestType == Enums.RequestType.Execute)
        {
            elementData.Id = Fixtures.terrainList.Count > 0 ? (Fixtures.terrainList[Fixtures.terrainList.Count - 1].Id + 1) : 1;
            Fixtures.terrainList.Add(((TerrainData)elementData).Clone());

            elementData.SetOriginalValues();

            if (copy) return;

            var atmosphereElementData = AtmosphereDataManager.DefaultData(elementData.Id, true);
            atmosphereElementData.Add(dataRequest);

        } else { }
    }

    public static void UpdateData(TerrainElementData elementData, DataRequest dataRequest)
    {
        if (!elementData.Changed) return;

        var data = Fixtures.terrainList.Where(x => x.Id == elementData.Id).FirstOrDefault();

        if (dataRequest.requestType == Enums.RequestType.Execute)
        {
            if (elementData.ChangedIconId)
            {
                data.IconId = elementData.IconId;
            }

            if (elementData.ChangedName)
            {
                data.Name = elementData.Name;
            }

            elementData.SetOriginalValues();

        } else { }
    }

    public static void RemoveData(TerrainElementData elementData, DataRequest dataRequest)
    {
        if (dataRequest.requestType == Enums.RequestType.Execute)
        {
            RemoveDependencies(elementData, dataRequest);

            Fixtures.terrainList.RemoveAll(x => x.Id == elementData.Id);

        } else {

            RemoveDependencies(elementData, dataRequest);
        }
    }

    private static void RemoveDependencies(TerrainElementData elementData, DataRequest dataRequest)
    {
        RemoveTerrainTileData(elementData, dataRequest);
        RemoveAtmosphereData(elementData, dataRequest);
    }

    private static void RemoveTerrainTileData(TerrainElementData elementData, DataRequest dataRequest)
    {
        var terrainTileSearchParameters = new Search.TerrainTile()
        {
            terrainId = new List<int>() { elementData.Id }
        };

        var terrainTileDataList = DataManager.GetTerrainTileData(terrainTileSearchParameters);

        terrainTileDataList.ForEach(terrainTileData =>
        {
            var terrainTileElementData = new TerrainTileElementData()
            {
                Id = terrainTileData.Id
            };

            terrainTileElementData.Remove(dataRequest);
        });
    }

    private static void RemoveAtmosphereData(TerrainElementData elementData, DataRequest dataRequest)
    {
        var atmosphereSearchParameters = new Search.Atmosphere()
        {
            terrainId = new List<int>() { elementData.Id }
        };

        var atmosphereDataList = DataManager.GetAtmosphereData(atmosphereSearchParameters);

        atmosphereDataList.ForEach(atmosphereData =>
        {
            var atmosphereElementData = new AtmosphereElementData()
            {
                Id = atmosphereData.Id
            };

            atmosphereElementData.Remove(dataRequest);
        });
    }
}
