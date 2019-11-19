using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public class MultiGridOrganizer : MonoBehaviour, IOrganizer, IList
{
    private int primaryDimension;

    private Vector2 secondaryElementSize;
    
    private IDisplayManager DisplayManager  { get { return GetComponent<IDisplayManager>(); } }
    private ListManager ListManager         { get { return (ListManager)DisplayManager; } }

    private ListProperties ListProperties   { get { return (ListProperties)DisplayManager.Display; } }
    private MultiGridProperties MultiGridProperties { get { return (MultiGridProperties)DisplayManager.Display.Properties; } }

    private IDataController PrimaryDataController   { get { return MultiGridProperties.PrimaryDataController; } }
    private IDataController SecondaryDataController { get { return MultiGridProperties.SecondaryDataController; } }

    public List<SelectionElement> ElementList { get; set; }

    public Vector2 ElementSize { get; set; }

    public void InitializeOrganizer()
    {
        ElementList = new List<SelectionElement>();

        SetElementSize();
    }

    public void SelectData()
    {
        if(PrimaryDataController != null)
            SelectionManager.SelectData(PrimaryDataController.DataList, DisplayManager);
        
        if(SecondaryDataController != null)
            SelectionManager.SelectData(SecondaryDataController.DataList, DisplayManager);
    }

    public void UpdateData()
    {
        ResetData(PrimaryDataController.DataList);
    }

    public void ResetData(List<IDataElement> filter)
    {
        ClearOrganizer();
        SetData(filter);
    }

    public void SetData()
    {
        SetData(PrimaryDataController.DataList);
    }

    public void SetData(List<IDataElement> primaryList)
    {
        string elementType = Enum.GetName(typeof(Enums.ElementType), MultiGridProperties.elementType);

        SelectionElement elementPrefab = Resources.Load<SelectionElement>("UI/" + elementType);

        foreach (IDataElement data in primaryList)
        {
            SelectionElement element = SelectionElementManager.SpawnElement(elementPrefab, ListManager.listParent,
                                                                            MultiGridProperties.elementType, DisplayManager, 
                                                                            DisplayManager.Display.SelectionType,
                                                                            DisplayManager.Display.SelectionProperty);

            ElementList.Add(element);

            data.SelectionElement = element;
            element.data = new SelectionElement.Data(PrimaryDataController, data);

            //Debugging
            GeneralData generalData = (GeneralData)data;
            //element.name = generalData.DebugName + generalData.id;
            //

            SetElement(element);
        }
    }

    private void SetElement(SelectionElement element)
    {
        RectTransform rect = element.GetComponent<RectTransform>();

        int index = PrimaryDataController.DataList.FindIndex(x => x.Id == element.GeneralData.Id);

        rect.sizeDelta = ElementSize;

        rect.transform.localPosition = GetElementPosition(index);

        element.gameObject.SetActive(true);

        element.SetElement();
    }

    public Vector2 GetElementPosition(int index)
    {
        var position = new Vector2(-((ElementSize.x * 0.5f) * (primaryDimension - 1)) + (index % primaryDimension * ElementSize.x),
                                    -(ElementSize.y * 0.5f) + (ListManager.listParent.sizeDelta.y / 2f) - (Mathf.Floor(index / primaryDimension) * ElementSize.y));

        return position;
    }

    private void SetElementSize()
    {
        MultiGridProperties.elementSize = ListProperties.elementSize;

        primaryDimension = (int)Mathf.Sqrt(PrimaryDataController.DataList.Count);

        if (MultiGridProperties.elementType == Enums.ElementType.CompactMultiGrid)
        {
            ElementSize = new Vector2(  ListProperties.elementSize.x + MultiGridProperties.margin,
                                        ListProperties.elementSize.y + MultiGridProperties.margin);
        } else {

            secondaryElementSize = new Vector2( ListProperties.elementSize.x,
                                                ListProperties.elementSize.y);

            ElementSize = new Vector2(  secondaryElementSize.x * (Mathf.Sqrt(SecondaryDataController.DataList.Count) / primaryDimension) + MultiGridProperties.margin,
                                        secondaryElementSize.y * (Mathf.Sqrt(SecondaryDataController.DataList.Count) / primaryDimension) + MultiGridProperties.margin);
        }
    }

    public Vector2 GetListSize(int elementCount, bool exact)
    {
        Vector2 primaryListSize = new Vector2(  (Mathf.Sqrt(elementCount) * ElementSize.x),
                                                (Mathf.Sqrt(elementCount) * ElementSize.y));

        if (exact)
        {
            return new Vector2( primaryListSize.x - ListManager.RectTransform.rect.width,
                                primaryListSize.y);
        }
        else
        {
            return new Vector2( primaryListSize.x / ElementSize.x,
                                primaryListSize.y / ElementSize.y);
        }
    }
    
    public void ClearOrganizer()
    {
        SelectionElementManager.CloseElement(ElementList);
    }

    private void CancelSelection()
    {
        if (PrimaryDataController != null)
            SelectionManager.CancelSelection(PrimaryDataController.DataList);

        if (SecondaryDataController != null)
            SelectionManager.CancelSelection(SecondaryDataController.DataList);
    }

    public void CloseOrganizer()
    {
        ClearOrganizer();

        CancelSelection();

        DestroyImmediate(this);
    }
}
