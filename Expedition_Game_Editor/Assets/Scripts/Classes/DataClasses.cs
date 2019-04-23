using UnityEngine;
using System.Collections;

[System.Serializable]
public class GeneralData
{
    public string table;
    public int id;
    public int type;

    //Temporary
    public int id_count;

    public GeneralData()
    {
        table = "";
        id = 0;
        type = 0;
    }

    public GeneralData(string table, int id, int type)
    {
        this.table = table;
        this.id = id;
        this.type = type;
    }

    public bool Equals(GeneralData data)
    {
        if (table != data.table)
            return false;

        if (id != data.id)
            return false;

        if (type != data.type)
            return false;

        return true;
    }

    public GeneralData Copy()
    {
        return new GeneralData(table, id, type);
    }
}

//public class DefaultHeaderData : GeneralData
//{
//    public string name  { get; set; }
//    public int objectId { get; set; }
//}

//[System.Serializable]
//public class ItemData : GeneralData
//{
//    public int index    { get; set; }
//    public string name  { get; set; }
//    public int objectId { get; set; }
//}

//[System.Serializable]
//public class ElementData : GeneralData
//{

//}

//[System.Serializable]
//public class ObjectGraphicData : GeneralData
//{
//    public string objectPath;
//    public string iconPath;

//    public ObjectGraphicData()
//    {
//        table       = "ObjectGraphic";

//        objectPath  = "Objects/Item/";
//        iconPath    = "Textures/Objects/Icons/";
//    }  
//}

//[System.Serializable]
//public class RegionData : GeneralData
//{

//}

//[System.Serializable]
//public class TerrainData : GeneralData
//{

//}

//[System.Serializable]
//public class TileData : GeneralData
//{

//}
