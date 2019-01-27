using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System.IO;

[System.Serializable]
public class ListData
{
    public ElementData data;

    public List<ElementData> list { get; set; }
    //Placeholder
    public int id_count;

    public Path select_path { get; set; }
    public Path edit_path   { get; set; }

    public void GetData(Route route)
    {
        GetData("sql");
    }

    public void GetData(string sql)
    {
        list = new List<ElementData>();

        for (int i = 0; i < id_count; i++)
            list.Add(new ElementData(data.table, (i + 1), data.type));
    }
}
