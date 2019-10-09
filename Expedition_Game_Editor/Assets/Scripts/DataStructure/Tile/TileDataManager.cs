using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class TileDataManager
{
    private TileController tileController;
    private List<TileData> tileDataList;

    private DataManager dataManager = new DataManager();

    private List<DataManager.IconData> iconDataList;

    public TileDataManager(TileController tileController)
    {
        this.tileController = tileController;
    }

    public List<IDataElement> GetTileDataElements(IEnumerable searchParameters)
    {
        var tileSearchData = searchParameters.Cast<Search.Tile>().FirstOrDefault();

        GetTileData(tileSearchData);

        var list = (from tileData in tileDataList

                    select new TileDataElement()
                    {
                        dataType = Enums.DataType.Tile,

                        Id = tileData.Id,
                        Index = tileData.Index,

                        icon = tileData.iconPath
                        
                    }).OrderBy(x => x.Index).ToList();

        list.ForEach(x => x.SetOriginalValues());

        return list.Cast<IDataElement>().ToList();
    }

    public void GetTileData(Search.Tile searchParameters)
    {
        tileDataList = new List<TileData>();

        foreach (Fixtures.Tile tile in Fixtures.tileList)
        {
            if (searchParameters.id.Count > 0 && !searchParameters.id.Contains(tile.Id)) continue;
            if (searchParameters.tileSetId.Count > 0 && !searchParameters.tileSetId.Contains(tile.tileSetId)) continue;

            var tileData = new TileData();

            tileData.Id = tile.Id;
            tileData.Index = tile.Index;

            tileData.tileSetId = tile.tileSetId;
            tileData.iconPath = tile.iconPath;
            
            tileDataList.Add(tileData);
        }
    }

    internal class TileData : GeneralData
    {
        public int tileSetId;
        public string iconPath;
    }
}
