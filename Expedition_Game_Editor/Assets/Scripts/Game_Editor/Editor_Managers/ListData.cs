using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System.IO;

[RequireComponent(typeof(EditorController))]

public class ListData : MonoBehaviour
{
    public ElementData data;

    public IController controller;

    public Enums.SortType sort_type;

    public List<ElementData> list { get; set; }
    //Placeholder
    public int id_count;

    public Path select_path { get; set; }
    public Path edit_path   { get; set; }

    public void InitializeRows()
    {
        InitializeRows("sql");
    }

    public void InitializeRows(string sql)
    {
        controller = GetComponent<IController>();

        list = new List<ElementData>();

        for (int i = 0; i < id_count; i++)
            list.Add(new ElementData(data.table, (i + 1), data.type, controller.data.path));
    }

    public void SetRows()
    {
        ListManager listManager = GetComponent<ListProperties>().main_list.GetComponent<ListManager>();

        listManager.InitializeList(this);

        GetComponent<ListProperties>().SetList();
    }

    public void CloseRows()
    {
        GetComponent<ListProperties>().main_list.GetComponent<ListManager>().CloseList();
    }
}
