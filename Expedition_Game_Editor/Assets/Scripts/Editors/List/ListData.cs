using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System.IO;

[RequireComponent(typeof(IController))]

public class ListData : MonoBehaviour
{
    public ElementData data;

    public ListProperties listProperties { get; set; }
    public IController controller;

    public List<ElementData> list { get; set; }
    //Placeholder
    public int id_count;

    public Path select_path { get; set; }
    public Path edit_path   { get; set; }

    public void GetData(Route route)
    {
        if(listProperties == null)
            listProperties = GetComponent<ListProperties>();

        listProperties.InitializeProperties(route);

        GetData("sql");
    }

    public void GetData(string sql)
    {
        controller = GetComponent<IController>();
        //Debug.Log("Load data");
        list = new List<ElementData>();

        for (int i = 0; i < id_count; i++)
            list.Add(new ElementData(data.table, (i + 1), data.type));
    }

    public void SetRows()
    {
        ListManager listManager = GetComponent<ListProperties>().listManager;

        listManager.InitializeList(this);

        GetComponent<ListProperties>().SetList();
    }

    public void CloseRows()
    {
        GetComponent<ListProperties>().CloseList();
    }
}
