using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public static class TileDataManager
{
    private static List<TileBaseData> tileDataList;

    public static List<IElementData> GetData(SearchProperties searchProperties)
    {
        var searchParameters = searchProperties.searchParameters.Cast<Search.Tile>().First();

        GetTileData(searchParameters);

        if (tileDataList.Count == 0) return new List<IElementData>();
        
        var list = (from tileData in tileDataList
                    select new TileElementData()
                    {
                        Id = tileData.Id,

                        Icon = tileData.IconPath

                    }).OrderBy(x => x.Id).ToList();
        

        list.ForEach(x => x.SetOriginalValues());

        return list.Cast<IElementData>().ToList();
    }

    private static void GetTileData(Search.Tile searchParameters)
    {
        tileDataList = new List<TileBaseData>();

        foreach (TileBaseData tile in Fixtures.tileList)
        {
            if (searchParameters.id.Count           > 0 && !searchParameters.id.Contains(tile.Id))                  continue;
            if (searchParameters.tileSetId.Count    > 0 && !searchParameters.tileSetId.Contains(tile.TileSetId))    continue;

            tileDataList.Add(tile);
        }
    }
}
