using UnityEngine;
using System.Collections.Generic;
using System;

public class PanelOrganizer : MonoBehaviour, IOrganizer, IList
{
    private IDisplayManager DisplayManager  { get { return GetComponent<IDisplayManager>(); } }
    private ListManager ListManager         { get { return (ListManager)DisplayManager; } }
    
    private ListProperties ListProperties   { get { return (ListProperties)DisplayManager.Display; } }
    private PanelProperties PanelProperties { get { return (PanelProperties)DisplayManager.Display.Properties; } }

    private IDataController DataController  { get { return DisplayManager.Display.DataController; } }
    
    public List<EditorElement> ElementList  { get; set; }

    public Vector2 ElementSize
    {
        get
        {
            return new Vector2( ListManager.RectTransform.rect.width,
                                PanelProperties.constantHeight ? ListProperties.elementSize.y :
                                                                 ListProperties.elementSize.y / PanelProperties.referenceArea.anchorMax.x);
        }
    }

    public void InitializeOrganizer()
    {
        ElementList = new List<EditorElement>();
    }

    public void SelectData()
    {
        SelectionManager.SelectData(DataController.Data.dataList, DisplayManager);
    }

    public void UpdateData()
    {
        ResetData(DataController.Data.dataList);
    }

    public void ResetData(List<IElementData> filter)
    {
        ClearOrganizer();

        SetData(filter);
    }

    public void SetData()
    {
        SetData(DataController.Data.dataList);
    }
    
    public void SetData(List<IElementData> list)
    {
        string elementType = Enum.GetName(typeof(Enums.ElementType), PanelProperties.elementType);

        var prefab = Resources.Load<ExPanel>("Elements/UI/" + elementType);

        foreach (IElementData elementData in list)
        {
            var panel = (ExPanel)PoolManager.SpawnObject(prefab);

            SelectionElementManager.InitializeElement(  panel.EditorElement.DataElement, ListManager.listParent,
                                                        DisplayManager, 
                                                        DisplayManager.Display.SelectionType, 
                                                        DisplayManager.Display.SelectionProperty,
                                                        DisplayManager.Display.UniqueSelection);

            ElementList.Add(panel.EditorElement);

            elementData.DataElement = panel.EditorElement.DataElement;

            panel.EditorElement.DataElement.Data = DataController.Data;
            panel.EditorElement.DataElement.Id = elementData.Id;

            panel.EditorElement.DataElement.Path = DisplayManager.Display.DataController.SegmentController.Path;

            SetProperties(panel);

            panel.GetComponent<ExPanel>().InitializeChildElement();

            //Debugging
            panel.name = elementData.DebugName + elementData.Id;

            SetElement(panel.EditorElement);
        }
    }
    
    private void SetElement(EditorElement element)
    {
        element.RectTransform.sizeDelta = new Vector2(element.RectTransform.sizeDelta.x, ElementSize.y);

        element.RectTransform.offsetMin = new Vector2(0, element.RectTransform.offsetMin.y);
        element.RectTransform.offsetMax = new Vector2(0, element.RectTransform.offsetMax.y);
        
        int index = DataController.Data.dataList.FindIndex(x => x.Id == element.DataElement.ElementData.Id);
        element.transform.localPosition = GetElementPosition(index);

        element.gameObject.SetActive(true);
        
        element.DataElement.SetElement();
        element.SetOverlay();
    }

    private void SetProperties(ExPanel panel)
    {
        panel.iconType = PanelProperties.iconType;
        panel.childProperty = PanelProperties.childProperty;
    }

    public Vector2 GetElementPosition(int index)
    {
        var position = new Vector2(0, (ListManager.listParent.sizeDelta.y / 2) + (-ElementSize.y * index) - (ElementSize.y / 2));

        return position;
    }

    public Vector2 GetListSize(int elementCount, bool exact)
    {
        if (exact)
            return new Vector2(0, DataController.Data.dataList.Count * ElementSize.y);
        else
            return new Vector2(0, elementCount);
    }

    public void ClearOrganizer()
    {
        ElementList.ForEach(x => PoolManager.ClosePoolObject(x.DataElement.Poolable));
        SelectionElementManager.CloseElement(ElementList);
    }

    private void CancelSelection()
    {
        SelectionManager.CancelSelection(DataController.Data.dataList);
    }

    public void CloseOrganizer()
    {
        CancelSelection();

        ClearOrganizer();
        
        DestroyImmediate(this);
    }
}
