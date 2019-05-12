using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public class PanelOrganizer : MonoBehaviour, IOrganizer, IList
{
    static public List<SelectionElement> elementList = new List<SelectionElement>();

    private IDataController dataController;
    private List<GeneralData> generalDataList;

    private List<float> rowHeight       = new List<float>(); //Individual heights
    private List<float> rowOffsetMax    = new List<float>(); //Combined heights

    private PanelProperties properties;

    private ListManager ListManager { get { return GetComponent<ListManager>(); } }

    public Vector2 ElementSize { get; set; }

    public void InitializeOrganizer()
    {
        dataController = ListManager.listProperties.DataController;
    }

    public void SetProperties()
    {
        properties = ListManager.listProperties.GetComponent<PanelProperties>();
    }

    public void SetElementSize()
    {
        ElementSize = new Vector2(  ListManager.listProperties.elementSize.x,
                                    properties.constantHeight ? ListManager.listProperties.elementSize.y : 
                                                                ListManager.listProperties.elementSize.y / properties.referenceArea.anchorMax.x);

        SetList();
    }

    public Vector2 GetListSize(int elementCount, bool exact)
    {
        if (exact)
            return new Vector2(0, rowHeight.Sum());
        else
            return new Vector2(0, elementCount);
    }

    public void SetList()
    {
        float positionSum = 0;

        for (int i = 0; i < ListManager.listProperties.DataController.DataList.Count; i++)
        {
            rowHeight.Add(ElementSize.y);

            positionSum += ElementSize.y;
            rowOffsetMax.Add(positionSum - ElementSize.y);
        }
    }

    public void SetData()
    {
        SetData(dataController.DataList);
    }

    public void SetData(ICollection list)
    {
        generalDataList = list.Cast<GeneralData>().ToList();
        
        string element = Enum.GetName(typeof(Enums.ElementType), properties.elementType);

        SelectionElement elementPrefab = Resources.Load<SelectionElement>("UI/" + element);

        foreach (var data in list)
        {
            SelectionElement selectionElement = ListManager.SpawnElement(elementList, elementPrefab, properties.elementType);
            ListManager.elementList.Add(selectionElement);

            selectionElement.route.data = new Data(dataController, new[] { data });

            //Debugging
            GeneralData generalData = (GeneralData)data;
            selectionElement.name = generalData.table + generalData.id;
            //

            SetElement(selectionElement);
        }
    }

    public void UpdateData()
    {
        ResetData(dataController.DataList);

        SelectionManager.ResetSelection(ListManager);
    }

    public void ResetData(ICollection filter)
    {
        CloseList();
        SetData(filter);
    }

    void SetElement(SelectionElement element)
    {
        element.SetElement();

        RectTransform rect = element.GetComponent<RectTransform>();

        int index = generalDataList.FindIndex(x => x.id == element.GeneralData().id);

        rect.offsetMin = new Vector2(rect.offsetMin.x, ListManager.listParent.sizeDelta.y - (rowOffsetMax[index] + rowHeight[index]));
        rect.offsetMax = new Vector2(rect.offsetMax.x, -rowOffsetMax[index]);

        rect.gameObject.SetActive(true);     
    }

    public SelectionElement GetElement(int index)
    {
        return ListManager.elementList[index];
    }

    float ListPosition(int i)
    {
        return ListManager.listParent.TransformPoint(new Vector2(0, (ListManager.listParent.sizeDelta.y / 2.222f) - rowOffsetMax[i])).y;
    }

    public void CloseList()
    {
        ListManager.ResetElement();
    }

    public void ClearOrganizer() { }

    public void CloseOrganizer()
    {
        CloseList();

        DestroyImmediate(this);
    }
}
