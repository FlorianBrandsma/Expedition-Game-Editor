using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class TileDataManager
{
    private TileController dataController;
    private List<TileData> tileDataList;

    public List<TileDataElement> GetTileDataElements(TileController dataController)
    {
        this.dataController = dataController;

        GetTileData();
        //GetIconData()?

        var list = (from tileData in tileDataList
                    select new TileDataElement()
                    {
                        id = tileData.id,
                        table = tileData.table,

                        Index = tileData.index,
                        icon = "Textures/Icons/Objects/Nothing"

                    }).OrderBy(x => x.Index).ToList();

        list.ForEach(x => x.SetOriginalValues());

        return list;
    }

    public void GetTileData()
    {
        tileDataList = new List<TileData>();

        //Temporary
        for (int i = 0; i < dataController.temp_id_count; i++)
        {
            var tileData = new TileData();

            tileData.id = (i + 1);
            tileData.table = "Tile";
            tileData.index = i;

            tileDataList.Add(tileData);
        }
    }

    internal class TileData : GeneralData
    {
        public int index;
    }
}
