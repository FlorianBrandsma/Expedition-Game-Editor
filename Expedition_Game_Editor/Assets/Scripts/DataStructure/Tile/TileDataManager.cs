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
        GetIconData();

        var list = (from tileData in tileDataList
                    join iconData in iconDataList on tileData.iconId equals iconData.id

                    select new TileDataElement()
                    {
                        id = tileData.id,
                        table = tileData.table,
                        Index = tileData.index,

                        icon = iconData.path

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
            tileData.iconId = tile.iconId;
            
            tileDataList.Add(tileData);
        }
    }

    internal void GetIconData()
    {
        iconDataList = dataManager.GetIconData(tileDataList.Select(x => x.iconId).Distinct().ToList(), true);
    }

    internal class TileData : GeneralData
    {
        public int tileSetId;
        public int iconId;
    }
}
