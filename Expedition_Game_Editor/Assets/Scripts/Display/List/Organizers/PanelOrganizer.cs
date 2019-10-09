using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public class PanelOrganizer : MonoBehaviour, IOrganizer, IList
{
    private IDataController dataController;
    private List<GeneralData> generalDataList;

    private List<float> rowHeight       = new List<float>(); //Individual heights
    private List<float> rowOffsetMax    = new List<float>(); //Combined heights

    private ListProperties listProperties;
    private PanelProperties panelProperties;

    private ListManager listManager;
    private IDisplayManager DisplayManager { get { return GetComponent<IDisplayManager>(); } }
    
    public List<SelectionElement> ElementList { get; set; }
    public Vector2 ElementSize { get; set; }

    public void InitializeOrganizer()
    {
        listManager = (ListManager)DisplayManager;

        dataController = DisplayManager.Display.DataController;

        ElementList = new List<SelectionElement>();
    }

    public void InitializeProperties()
    {
        listProperties = (ListProperties)DisplayManager.Display;
        panelProperties = (PanelProperties)DisplayManager.Display.Properties;
    }

    public void SetElementSize()
    {
        ElementSize = new Vector2(  listProperties.elementSize.x,
                                    panelProperties.constantHeight ?    listProperties.elementSize.y : 
                                                                        listProperties.elementSize.y / panelProperties.referenceArea.anchorMax.x);

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

        for (int i = 0; i < dataController.DataList.Count; i++)
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

    public void SetData(List<IDataElement> list)
    {
        generalDataList = list.Cast<GeneralData>().ToList();
        
        string elementType = Enum.GetName(typeof(Enums.ElementType), panelProperties.elementType);

        SelectionElement elementPrefab = Resources.Load<SelectionElement>("UI/" + elementType);

        foreach (IDataElement data in list)
        {
            SelectionElement element = SelectionElementManager.SpawnElement(elementPrefab, listManager.listParent, 
                                                                            panelProperties.elementType, DisplayManager,
                                                                            DisplayManager.Display.SelectionType,
                                                                            DisplayManager.Display.SelectionProperty);

            ElementList.Add(element);
            
            data.SelectionElement = element;
            element.data = new SelectionElement.Data(dataController, data);

            element.GetComponent<EditorPanel>().InitializeChildElement();

            //Debugging
            GeneralData generalData = (GeneralData)data;
            element.name = generalData.DebugName + generalData.Id;
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

        int index = generalDataList.FindIndex(x => x.Id == element.GeneralData.Id);

        rect.sizeDelta = new Vector2(rect.sizeDelta.x, ElementSize.y);

        element.transform.localPosition = new Vector2(element.transform.localPosition.x, 
                                                     (listManager.listParent.sizeDelta.y / 2) + (-ElementSize.y * index) - (ElementSize.y / 2));

        element.gameObject.SetActive(true);

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
