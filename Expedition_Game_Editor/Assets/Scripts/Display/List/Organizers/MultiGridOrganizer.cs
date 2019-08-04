using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public class MultiGridOrganizer : MonoBehaviour, IOrganizer, IList
{
    private IDataController primaryDataController;
    private IDataController secondaryDataController;

    private List<GeneralData> generalDataList;

    private MultiGridProperties multiGridProperties;

    int primaryDimension;

    private Vector2 secondaryElementSize;

    private ListManager ListManager { get { return GetComponent<ListManager>(); } }

    public List<SelectionElement> ElementList { get; set; }
    public Vector2 ElementSize { get; set; }

    public void InitializeOrganizer()
    {
        ElementList = new List<SelectionElement>();
    }

    public void InitializeProperties()
    {
        multiGridProperties = ListManager.listProperties.GetComponent<MultiGridProperties>();

        multiGridProperties.elementSize = ListManager.listProperties.elementSize;

        primaryDataController = multiGridProperties.PrimaryDataController;
        secondaryDataController = multiGridProperties.SecondaryDataController;
    }

    public void SetElementSize()
    {
        primaryDimension = (int)Mathf.Sqrt(primaryDataController.DataList.Count);

        if(multiGridProperties.elementType == Enums.ElementType.CompactMultiGrid)
        {
            ElementSize = new Vector2(  ListManager.listProperties.elementSize.x + multiGridProperties.margin, 
                                        ListManager.listProperties.elementSize.y + multiGridProperties.margin);
        } else {

            secondaryElementSize = new Vector2( ListManager.listProperties.elementSize.x,
                                                ListManager.listProperties.elementSize.y);

            ElementSize = new Vector2(  secondaryElementSize.x * (Mathf.Sqrt(secondaryDataController.DataList.Count) / primaryDimension) + multiGridProperties.margin,
                                        secondaryElementSize.y * (Mathf.Sqrt(secondaryDataController.DataList.Count) / primaryDimension) + multiGridProperties.margin);
        }
    }

    public Vector2 GetListSize(int elementCount, bool exact)
    {
        Vector2 primaryListSize = new Vector2(  (Mathf.Sqrt(elementCount) * ElementSize.x), 
                                                (Mathf.Sqrt(elementCount) * ElementSize.y));

        if (exact)
        {
            return new Vector2( primaryListSize.x - ListManager.rectTransform.rect.width, 
                                primaryListSize.y);
        } else {
            return new Vector2( primaryListSize.x / ElementSize.x, 
                                primaryListSize.y / ElementSize.y);
        }   
    }

    public void SetData()
    {
        SetData(primaryDataController.DataList);
    }

    public void SetData(List<IDataElement> primaryList)
    {
        generalDataList = primaryList.Cast<GeneralData>().ToList();

        string elementType = Enum.GetName(typeof(Enums.ElementType), multiGridProperties.elementType);

        SelectionElement elementPrefab = Resources.Load<SelectionElement>("UI/" + elementType);

        foreach (IDataElement data in primaryList)
        {
            SelectionElement element = SelectionElementManager.SpawnElement(elementPrefab, multiGridProperties.elementType,
                                                                            ListManager, ListManager.selectionType, ListManager.selectionProperty, ListManager.listParent);

            ElementList.Add(element);

            data.SelectionElement = element;
            element.data = new SelectionElement.Data(primaryDataController, data);

            //Debugging
            GeneralData generalData = (GeneralData)data;
            //element.name = generalData.DebugName + generalData.id;
            //

            SetElement(element);
        }
    }

    public void UpdateData()
    {
        ResetData(primaryDataController.DataList);

        SelectionManager.SelectElements();
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

        rect.sizeDelta = ElementSize;

        rect.transform.localPosition = new Vector2( -((ElementSize.x * 0.5f) * (primaryDimension - 1)) + (index % primaryDimension * ElementSize.x),
                                                     -(ElementSize.y * 0.5f) + (ListManager.listParent.sizeDelta.y / 2f) - (Mathf.Floor(index / primaryDimension) * ElementSize.y));

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
