using UnityEngine;
using System.Collections;

public class GeneralData
{
    public string table;
    public int id;
    public int index;

    //Temporary
    public int id_count;

    public GeneralData()
    {
        table = "";
        id = 0;
        index = 0;
    }

    public GeneralData(string table, int id, int index)
    {
        this.table = table;
        this.id = id;
        this.index = index;
    }

    public bool Equals(GeneralData data)
    {
        if (table != data.table)
            return false;

        if (id != data.id)
            return false;

        return true;
    }

    public GeneralData Copy()
    {
        return new GeneralData(table, id, index);
    }
}
