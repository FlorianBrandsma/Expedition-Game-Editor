using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class TileDataManager : IDataManager
{
    public IDataController DataController { get; set; }

    private List<TileData> tileDataList;

    private DataManager dataManager = new DataManager();

    private List<DataManager.IconData> iconDataList;

    public TileDataManager(TileController tileController)
    {
        DataController = tileController;
    }

    public List<IElementData> GetData(SearchProperties searchProperties)
    {
        var searchParameters = searchProperties.searchParameters.Cast<Search.Tile>().First();

        GetTileData(searchParameters);

        if (tileDataList.Count == 0) return new List<IElementData>();

        var list = (from tileData in tileDataList

                    select new TileElementData()
                    {
                        Id = tileData.id,

                        icon = tileData.iconPath
                        
                    }).OrderBy(x => x.Index).ToList();

        list.ForEach(x => x.SetOriginalValues());

        return list.Cast<IElementData>().ToList();
    }

    public void GetTileData(Search.Tile searchParameters)
    {
        tileDataList = new List<TileData>();

        foreach (Fixtures.Tile tile in Fixtures.tileList)
        {
            if (searchParameters.id.Count           > 0 && !searchParameters.id.Contains(tile.id)) continue;
            if (searchParameters.tileSetId.Count    > 0 && !searchParameters.tileSetId.Contains(tile.tileSetId)) continue;

            var tileData = new TileData();

            tileData.id = tile.id;

            tileData.tileSetId = tile.tileSetId;
            tileData.iconPath = tile.iconPath;
            
            tileDataList.Add(tileData);
        }
    }

    internal class TileData
    {
        public int id;

        public int tileSetId;
        public string iconPath;
    }
}
