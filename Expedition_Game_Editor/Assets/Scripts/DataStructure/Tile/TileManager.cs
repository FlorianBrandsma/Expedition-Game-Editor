using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class TileManager
{
    private TileController dataController;
    private List<TileData> tileData_list;

    public List<TileDataElement> GetTileDataElements(TileController dataController)
    {
        this.dataController = dataController;

        GetTileData();
        //GetIconData()?

        var list = (from oCore in tileData_list
                    select new TileDataElement()
                    {
                        id = oCore.id,
                        table = oCore.table,
                        type = oCore.type,
                        index = oCore.index,
                        name = oCore.name,
                        description = oCore.description,

                        icon = "Textures/Icons/Objects/Nothing"

                    }).OrderBy(x => x.index).ToList();

        list.ForEach(x => x.SetOriginalValues());

        return list;
    }

    public void GetTileData()
    {
        tileData_list = new List<TileData>();

        //Temporary
        for (int i = 0; i < dataController.temp_id_count; i++)
        {
            var tileData = new TileData();

            tileData.id = (i + 1);
            tileData.table = "Tile";
            tileData.index = i;

            tileData.name = "Tile " + (i + 1);
            tileData.description = "This is a pretty regular sentence. The structure is something you'd expect. Nothing too long though!";

            tileData_list.Add(tileData);
        }
    }

    internal class TileData : GeneralData
    {
        public int index;
        public string name;
        public string description;
    }
}
