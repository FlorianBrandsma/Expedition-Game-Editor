using UnityEngine;
using System.Collections;

[System.Serializable]
public class GeneralData
{
    public string table;
    public int id;
    public int type;
    public int index;

    //Temporary
    public int id_count;

    public GeneralData()
    {
        table = "";
        id = 0;
        type = 0;
        index = 0;
    }

    public GeneralData(string table, int id, int type, int index)
    {
        this.table = table;
        this.id = id;
        this.type = type;
        this.index = index;
    }

    public bool Equals(GeneralData data)
    {
        if (table != data.table)
            return false;

        if (id != data.id)
            return false;

        if (type != data.type)
            return false;

        if (index != data.index)
            return false;

        return true;
    }

    public GeneralData Copy()
    {
        return new GeneralData(table, id, type, index);
    }
}
