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

    public List<EditorElement> ElementList  { get; set; }

    public Vector2 ElementSize { get; set; }

    public void InitializeOrganizer()
    {
        ElementList = new List<EditorElement>();

        SetElementSize();
    }

    public void SelectData()
    {
        if(PrimaryDataController != null)
            SelectionManager.SelectData(PrimaryDataController.Data.dataList, DisplayManager);
        
        if(SecondaryDataController != null)
            SelectionManager.SelectData(SecondaryDataController.Data.dataList, DisplayManager);
    }

    public void UpdateData()
    {
        ResetData(PrimaryDataController.Data.dataList);
    }

    public void ResetData(List<IElementData> filter)
    {
        ClearOrganizer();
        SetData(filter);
    }

    public void SetData()
    {
        SetData(PrimaryDataController.Data.dataList);
    }

    public void SetData(List<IElementData> primaryList)
    {
        string elementType = Enum.GetName(typeof(Enums.ElementType), MultiGridProperties.elementType);

        var prefab = Resources.Load<ExMultiGrid>("Elements/UI/" + elementType);

        foreach (IElementData elementData in primaryList)
        {
            var multiGrid = (ExMultiGrid)PoolManager.SpawnObject(prefab);

            SelectionElementManager.InitializeElement(  multiGrid.EditorElement.DataElement, ListManager.listParent,
                                                        DisplayManager,
                                                        DisplayManager.Display.SelectionType,
                                                        DisplayManager.Display.SelectionProperty);

            ElementList.Add(multiGrid.EditorElement);

            elementData.DataElement = multiGrid.EditorElement.DataElement;

            multiGrid.EditorElement.DataElement.Data = PrimaryDataController.Data;
            multiGrid.EditorElement.DataElement.Id = elementData.Id;

            //Debugging
            multiGrid.name = elementData.DebugName + elementData.Id;

            SetElement(multiGrid.EditorElement);
        }
    }

    private void SetElement(EditorElement element)
    {
        element.RectTransform.sizeDelta = ElementSize;
        
        int index = PrimaryDataController.Data.dataList.FindIndex(x => x.Id == element.DataElement.ElementData.Id);
        element.transform.localPosition = GetElementPosition(index);

        element.gameObject.SetActive(true);

        element.DataElement.SetElement();
        element.SetOverlay();
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

        primaryDimension = (int)Mathf.Sqrt(PrimaryDataController.Data.dataList.Count);

        if (MultiGridProperties.elementType == Enums.ElementType.CompactMultiGrid)
        {
            ElementSize = new Vector2(  ListProperties.elementSize.x + MultiGridProperties.margin,
                                        ListProperties.elementSize.y + MultiGridProperties.margin);
        } else {

            secondaryElementSize = new Vector2( ListProperties.elementSize.x,
                                                ListProperties.elementSize.y);

            ElementSize = new Vector2(  secondaryElementSize.x * (Mathf.Sqrt(SecondaryDataController.Data.dataList.Count) / primaryDimension) + MultiGridProperties.margin,
                                        secondaryElementSize.y * (Mathf.Sqrt(SecondaryDataController.Data.dataList.Count) / primaryDimension) + MultiGridProperties.margin);
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
        ElementList.ForEach(x => PoolManager.ClosePoolObject(x.DataElement.Poolable));
        SelectionElementManager.CloseElement(ElementList);
    }

    private void CancelSelection()
    {
        if (PrimaryDataController != null)
            SelectionManager.CancelSelection(PrimaryDataController.Data.dataList);

        if (SecondaryDataController != null)
            SelectionManager.CancelSelection(SecondaryDataController.Data.dataList);
    }

    public void CloseOrganizer()
    {
        CancelSelection();

        ClearOrganizer();
        
        DestroyImmediate(this);
    }
}
