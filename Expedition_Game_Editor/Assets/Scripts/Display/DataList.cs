using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System.IO;

[System.Serializable]
public class DataList
{
    public GeneralData data;

    public List<GeneralData> list { get; set; }
    //Placeholder
    public int id_count;

    public void GetData(Route route)
    {
        GetData("sql");
    }

    public void GetData(string sql)
    {
        list = new List<GeneralData>();

        for (int i = 0; i < id_count; i++)
            list.Add(new GeneralData(data.table, (i + 1), data.type));
    }
}
