using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class ElementData 
{
    public string table;
    public int id;
    public int type;
    public Path path;

    public ElementData()
    {
        table   = "";
        id      = 0;
        type    = 0;
        path    = new Path(null, new List<int>(), new List<ElementData>());
    }

    public ElementData(string new_table, int new_id, int new_type, Path new_path)
    {
        table   = new_table;
        id      = new_id;
        type    = new_type;
        path    = new_path;
    }
    /*
    public ElementData Copy(ElementData data)
    {
        Path new_path = PathManager

        ElementData new_data = new ElementData(data.table);


        return new_data;
    }
    */
}
