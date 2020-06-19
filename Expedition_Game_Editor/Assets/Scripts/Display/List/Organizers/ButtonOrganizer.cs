using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public class ButtonOrganizer : MonoBehaviour, IOrganizer, IList
{
    private IDisplayManager DisplayManager  { get { return GetComponent<IDisplayManager>(); } }
    private ListManager ListManager         { get { return (ListManager)DisplayManager; } }

    private ListProperties ListProperties   { get { return (ListProperties)DisplayManager.Display; } }
    private ButtonProperties ButtonProperties { get { return (ButtonProperties)DisplayManager.Display.Properties; } }
    
    private IDataController DataController  { get { return DisplayManager.Display.DataController; } }
    
    public List<SelectionElement> ElementList { get; set; }

    public Vector2 ElementSize { get { return ListProperties.elementSize; } }

    public void InitializeOrganizer()
    {
        ElementList = new List<SelectionElement>();
    }

    public void SelectData()
    {
        SelectionManager.SelectData(DataController.DataList, DisplayManager);
    }
    
    public void SetData()
    {
        SetData(DataController.DataList);
    }

    public void UpdateData()
    {
        ResetData(DataController.DataList);
    }

    public void ResetData(List<IDataElement> filter)
    {
        ClearOrganizer();
        SetData(filter);
    }

    public void SetData(List<IDataElement> list)
    {
        SelectionElement elementPrefab = Resources.Load<SelectionElement>("UI/Button");

        foreach (IDataElement data in list)
        {
            //SelectionElement element = SelectionElementManager.SpawnElement(elementPrefab, ListManager.listParent,
            //                                                                Enums.ElementType.Button, DisplayManager, 
            //                                                                DisplayManager.Display.SelectionType,
            //                                                                DisplayManager.Display.SelectionProperty);
            //ElementList.Add(element);

            //data.SelectionElement = element;
            //element.data = new SelectionElement.Data(DataController, data);

            ////Debugging
            //GeneralData generalData = (GeneralData)data;
            //element.name = generalData.DebugName + generalData.Id;
            ////

            //SetElement(element);
        }
    }

    private void SetElement(SelectionElement element)
    {
        element.RectTransform.sizeDelta = new Vector2(element.RectTransform.sizeDelta.x, ElementSize.y);

        element.RectTransform.anchorMax = new Vector2(1, 1);

        int index = DataController.DataList.FindIndex(x => x.Id == element.GeneralData.Id);
        element.transform.localPosition = GetElementPosition(index);
        
        element.gameObject.SetActive(true);

        element.SetElement();
        element.SetOverlay();
    }

    public Vector2 GetElementPosition(int index)
    {
        var position = new Vector2(0, (ListManager.listParent.sizeDelta.y / 2) - (ElementSize.y * index) - (ElementSize.y * 0.5f));

        return position;
    }

    public Vector2 GetListSize(int element_count, bool exact)
    {
        return new Vector2(0, ElementSize.y * element_count);
    }
    
    public void ClearOrganizer()
    {
        SelectionElementManager.CloseElement(ElementList);
    }

    private void CancelSelection()
    {
        SelectionManager.CancelSelection(DataController.DataList);
    }

    public void CloseOrganizer()
    {
        ClearOrganizer();

        CancelSelection();

        DestroyImmediate(this);
    }
}
