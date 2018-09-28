using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class ElementData 
{
    public string table;
    public int id;
    public int type;

    public ElementData()
    {
        table   = "";
        id      = 0;
        type    = 0;
    }

    public ElementData(string new_table, int new_id, int new_type)
    {
        table   = new_table;
        id      = new_id;
        type    = new_type;
    }

    public bool Equals(ElementData data)
    {
        if (table != data.table)
            return false;

        if (id != data.id)
            return false;
        
        if (type != data.type)
            return false;

        return true;
    }

    public ElementData Copy()
    {
        return new ElementData(table, id, type);
    }
}
