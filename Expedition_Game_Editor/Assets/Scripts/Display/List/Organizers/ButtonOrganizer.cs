using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class ButtonOrganizer : MonoBehaviour, IOrganizer, IList
{
    private List<GeneralData> local_data_list;

    static public List<SelectionElement> element_list = new List<SelectionElement>();

    //private Enums.SelectionProperty selectionProperty;
    //private SelectionManager.Type selectionType;

    public Vector2 element_size { get; set; }

    //private bool visible_only;

    private ListManager listManager;

    public void InitializeOrganizer()
    {
        listManager = GetComponent<ListManager>();
    }

    public void SetProperties()
    {
        //properties = listProperties.GetComponent<ButtonProperties>();
        //selectionProperty = listProperties.selectionProperty;
        //selectionType = listProperties.selectionType;

        //visible_only = listProperties.visible_only;
    }

    public void SetElementSize()
    {
        element_size = listManager.listProperties.element_size;
    }

    public Vector2 GetListSize(int element_count, bool exact)
    {
        return new Vector2(0, element_size.y * element_count);
    }

    public void GetData()
    {
        //Need to know base dataclass for query (attached to listproperties)
        //listManager.listProperties.dataList.GetData(listManager.listProperties.route);
    }

    public void UpdateData()
    {
        ResetData(null);
        SetData();
    }

    public void SetData()
    {
        //var new_data = data_list.Cast<GeneralData>().ToList();

        //local_data_list = (from data in new_data
        //                   select new UIElementData()
        //                   {
        //                       id = data.id,
        //                       table = data.table,
        //                       type = data.type,
        //                       index = data.index,
        //                       name = (data.table + " " + data.index)
        //                   }).ToList();

        SelectionElement element_prefab = Resources.Load<SelectionElement>("UI/Button");

        foreach(GeneralData data in local_data_list)
        {
            SelectionElement element = listManager.SpawnElement(element_list, element_prefab);
            listManager.element_list.Add(element);

            //This should come from data. Button likely uses ItemData or ElementData which
            //both have "name" stored inside. No matter what type of list the data_list is

            //element.SetElement(data, new[] { data });

            //string label = data.name;

            //Debugging
            //element.name = label;

            SetElement(element);
        }
    }

    public void ResetData(ICollection filter)
    {
        CloseList();
        SetData();
    }

    void SetElement(SelectionElement element)
    {
        RectTransform rect = element.GetComponent<RectTransform>();

        int index = local_data_list.FindIndex(x => x.id == element.data.Cast<GeneralData>().ToList().FirstOrDefault().id);

        rect.anchorMax = new Vector2(1, 1);

        rect.sizeDelta = new Vector2(rect.sizeDelta.x, element_size.y);

        rect.transform.localPosition = new Vector2(0, (listManager.list_parent.sizeDelta.y / 2) - (element_size.y * index) - (element_size.y * 0.5f));

        rect.gameObject.SetActive(true);
    }

    public SelectionElement GetElement(int index)
    {
        return listManager.element_list[index];
    }

    float ListPosition(int i)
    {
        return listManager.list_parent.TransformPoint(new Vector2(0, (listManager.list_parent.sizeDelta.y / 2.222f) - (element_size.y * i))).y;
    }

    public void CloseList()
    {
        listManager.ResetElement(listManager.element_list);

        listManager.element_list.Clear();
    }

    public void CloseOrganizer()
    {
        CloseList();

        DestroyImmediate(this);
    }
}
