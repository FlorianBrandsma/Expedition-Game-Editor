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
    private ListProperties listProperties;
    private ButtonProperties buttonProperties;
    
    private IDataController dataController;
    private List<GeneralData> generalDataList;

    private ListManager listManager;
    private IDisplayManager DisplayManager { get { return GetComponent<IDisplayManager>(); } }

    public List<SelectionElement> ElementList { get; set; }
    public Vector2 ElementSize { get; set; }

    public void InitializeOrganizer()
    {
        listManager = (ListManager)DisplayManager;

        dataController = DisplayManager.Display.DataController;
    }

    public void InitializeProperties()
    {
        listProperties = (ListProperties)DisplayManager.Display;
        buttonProperties = (ButtonProperties)DisplayManager.Display.Properties;
    }

    public void SetElementSize()
    {
        ElementSize = listProperties.elementSize;
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
            SelectionElement element = SelectionElementManager.SpawnElement(elementPrefab, Enums.ElementType.Button, 
                                                                            DisplayManager, listManager.selectionType,
                                                                            listManager.selectionProperty, listManager.listParent);
            ElementList.Add(element);

            data.SelectionElement = element;
            element.data = new SelectionElement.Data(dataController, data);

            //Debugging
            GeneralData generalData = (GeneralData)data;
            element.name = generalData.DebugName + generalData.id;
            //

            SetElement(element);
        }
    }

    public void UpdateData()
    {
        ResetData(dataController.DataList);
    }

    public void ResetData(List<IDataElement> filter)
    {
        CloseList();
        SetData(filter);
    }

    void SetElement(SelectionElement element)
    {
        RectTransform rect = element.GetComponent<RectTransform>();

        int index = generalDataList.FindIndex(x => x.id == element.GeneralData().id);

        rect.anchorMax = new Vector2(1, 1);

        rect.sizeDelta = new Vector2(rect.sizeDelta.x, ElementSize.y);

        rect.transform.localPosition = new Vector2(0, (listManager.listParent.sizeDelta.y / 2) - (ElementSize.y * index) - (ElementSize.y * 0.5f));

        rect.gameObject.SetActive(true);

        element.SetElement();
    }

    public void CloseList()
    {
        SelectionElementManager.CloseElement(ElementList);
    }

    public void ClearOrganizer() { }

    public void CloseOrganizer()
    {
        CloseList();

        DestroyImmediate(this);
    }
}
