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
    private ListManager listManager { get { return GetComponent<ListManager>(); } }

    static public List<SelectionElement> element_list = new List<SelectionElement>();

    public Vector2 element_size { get; set; }

    private ButtonProperties properties;

    List<GeneralData> generalData_list;

    public void InitializeOrganizer() { }

    public void SetProperties()
    {
        properties = listManager.listProperties.GetComponent<ButtonProperties>();
    }

    public void SetElementSize()
    {
        element_size = listManager.listProperties.element_size;
    }

    public Vector2 GetListSize(int element_count, bool exact)
    {
        return new Vector2(0, element_size.y * element_count);
    }

    public void SetData()
    {
        var dataController = listManager.listProperties.dataController;
        generalData_list = dataController.data_list.Cast<GeneralData>().ToList();

        SelectionElement element_prefab = Resources.Load<SelectionElement>("UI/Button");

        foreach (var data in dataController.data_list)
        {
            SelectionElement element = listManager.SpawnElement(element_list, element_prefab);
            listManager.element_list.Add(element);

            element.SetElementData(new[] { data }, dataController.data_type);

            //Debugging
            GeneralData generalData = (GeneralData)data;
            element.name = generalData.table + generalData.id;
            //

            SetElement(element);
        }
    }

    public void UpdateData()
    {
        ResetData(null);

        SelectionManager.ResetSelection(listManager);
    }

    public void ResetData(ICollection filter)
    {
        CloseList();
        SetData();
    }

    public void CloseData()
    {
        listManager.element_list.Clear();
    }

    void SetElement(SelectionElement element)
    {
        element.SetElement();

        RectTransform rect = element.GetComponent<RectTransform>();

        int index = generalData_list.FindIndex(x => x.id == element.GeneralData().id);

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
