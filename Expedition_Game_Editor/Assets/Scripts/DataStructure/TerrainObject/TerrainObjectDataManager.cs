using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class TerrainObjectDataManager
{
    private TerrainObjectController dataController;
    private List<TerrainObjectData> terrainObjectDataList;

    public List<TerrainObjectDataElement> GetTerrainObjectDataElements(TerrainObjectController dataController)
    {
        this.dataController = dataController;

        GetTerrainObjectData();
        //GetIconData()?

        var list = (from terrainObjectData in terrainObjectDataList
                    select new TerrainObjectDataElement()
                    {
                        id = terrainObjectData.id,
                        dataType = terrainObjectData.dataType,

                        Index = terrainObjectData.index,

                        icon = "Textures/Icons/Objects/Nothing"

                    }).OrderBy(x => x.Index).ToList();

        list.ForEach(x => x.SetOriginalValues());

        return list;
    }

    public void GetTerrainObjectData()
    {
        //terrainObjectDataList = new List<TerrainObjectData>();

        ////Temporary
        //for (int i = 0; i < dataController.temp_id_count; i++)
        //{
        //    var terrainObjectData = new TerrainObjectData();

        //    terrainObjectData.id = (i + 1);
        //    terrainObjectData.dataType = "TerrainObject";
        //    terrainObjectData.index = i;

        //    terrainObjectDataList.Add(terrainObjectData);
        //}
    }

    internal class TerrainObjectData : GeneralData
    {
        public int index;
    }
}
