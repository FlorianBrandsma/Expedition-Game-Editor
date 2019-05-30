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

    static public List<SelectionElement> elementList = new List<SelectionElement>();

    public Vector2 ElementSize { get; set; }

    private ButtonProperties properties;

    private IDataController dataController;
    private List<GeneralData> generalDataList;

    public void InitializeOrganizer()
    {
        dataController = listManager.listProperties.DataController;
    }

    public void SetProperties()
    {
        properties = listManager.listProperties.GetComponent<ButtonProperties>();
    }

    public void SetElementSize()
    {
        ElementSize = listManager.listProperties.elementSize;
    }

    public Vector2 GetListSize(int element_count, bool exact)
    {
        return new Vector2(0, ElementSize.y * element_count);
    }

    public void SetData()
    {
        SetData(dataController.DataList);
    }

    public void SetData(List<IDataElement> list)
    {
        generalDataList = list.Cast<GeneralData>().ToList();

        SelectionElement elementPrefab = Resources.Load<SelectionElement>("UI/Button");

        foreach (IDataElement data in list)
        {
            SelectionElement element = listManager.SpawnElement(elementList, elementPrefab, Enums.ElementType.Button);
            listManager.elementList.Add(element);

            data.SelectionElement = element;
            element.route.data = new Data(dataController, data);

            //Debugging
            GeneralData generalData = (GeneralData)data;
            element.name = generalData.table + generalData.id;
            //

            SetElement(element);
        }
    }

    public void UpdateData()
    {
        ResetData(dataController.DataList);

        SelectionManager.ResetSelection(listManager);
    }

    public void ResetData(List<IDataElement> filter)
    {
        CloseList();
        SetData(filter);
    }

    void SetElement(SelectionElement element)
    {
        element.SetElement();

        RectTransform rect = element.GetComponent<RectTransform>();

        int index = generalDataList.FindIndex(x => x.id == element.GeneralData().id);

        rect.anchorMax = new Vector2(1, 1);

        rect.sizeDelta = new Vector2(rect.sizeDelta.x, ElementSize.y);

        rect.transform.localPosition = new Vector2(0, (listManager.listParent.sizeDelta.y / 2) - (ElementSize.y * index) - (ElementSize.y * 0.5f));

        rect.gameObject.SetActive(true);
    }

    public SelectionElement GetElement(int index)
    {
        return listManager.elementList[index];
    }

    float ListPosition(int i)
    {
        return listManager.listParent.TransformPoint(new Vector2(0, (listManager.listParent.sizeDelta.y / 2.222f) - (ElementSize.y * i))).y;
    }

    public void CloseList()
    {
        listManager.ResetElement();
    }

    public void ClearOrganizer() { }

    public void CloseOrganizer()
    {
        CloseList();

        DestroyImmediate(this);
    }
}
