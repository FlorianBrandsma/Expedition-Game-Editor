using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class TileDataManager
{
    private TileController tileController;
    private List<TileData> tileDataList;

    private DataManager dataManager = new DataManager();

    //private List<DataManager.ObjectGraphicData> objectGraphicDataList;
    private List<DataManager.IconData> iconDataList;

    public void InitializeManager(TileController tileController)
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
                        id = tileData.id,
                        table = tileData.table,
                        Index = tileData.index,

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
            if (searchParameters.tileSetId.Count > 0 && !searchParameters.tileSetId.Contains(tile.tileSetId)) continue;

            var tileData = new TileData();

            tileData.id = tile.id;
            tileData.table = "Tile";
            tileData.index = tile.index;

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
