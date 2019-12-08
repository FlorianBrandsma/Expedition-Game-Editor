using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public class PanelOrganizer : MonoBehaviour, IOrganizer, IList
{
    private IDisplayManager DisplayManager  { get { return GetComponent<IDisplayManager>(); } }
    private ListManager ListManager         { get { return (ListManager)DisplayManager; } }
    
    private ListProperties ListProperties   { get { return (ListProperties)DisplayManager.Display; } }
    private PanelProperties PanelProperties { get { return (PanelProperties)DisplayManager.Display.Properties; } }

    private IDataController DataController  { get { return DisplayManager.Display.DataController; } }
    
    public List<SelectionElement> ElementList { get; set; }

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
        ElementList = new List<SelectionElement>();
    }

    public void SelectData()
    {
        SelectionManager.SelectData(DataController.DataList, DisplayManager);
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

    public void SetData()
    {
        SetData(DataController.DataList);
    }
    
    public void SetData(List<IDataElement> list)
    {
        string elementType = Enum.GetName(typeof(Enums.ElementType), PanelProperties.elementType);

        SelectionElement elementPrefab = Resources.Load<SelectionElement>("UI/" + elementType);

        foreach (IDataElement data in list)
        {
            SelectionElement element = SelectionElementManager.SpawnElement(elementPrefab, ListManager.listParent, 
                                                                            PanelProperties.elementType, DisplayManager,
                                                                            DisplayManager.Display.SelectionType,
                                                                            DisplayManager.Display.SelectionProperty);

            ElementList.Add(element);
            
            data.SelectionElement = element;
            element.data = new SelectionElement.Data(DataController, data);

            element.GetComponent<EditorPanel>().InitializeChildElement();

            //Debugging
            GeneralData generalData = (GeneralData)data;
            element.name = generalData.DebugName + generalData.Id;
            //

            SetElement(element);
        }
    }
    
    private void SetElement(SelectionElement element)
    {
        RectTransform rect = element.GetComponent<RectTransform>();

        int index = DataController.DataList.FindIndex(x => x.Id == element.GeneralData.Id);

        rect.sizeDelta = new Vector2(rect.sizeDelta.x, ElementSize.y);

        element.transform.localPosition = GetElementPosition(index);

        element.gameObject.SetActive(true);

        element.SetElement();

        element.SetOverlay();
    }

    public Vector2 GetElementPosition(int index)
    {
        var position = new Vector2(0, (ListManager.listParent.sizeDelta.y / 2) + (-ElementSize.y * index) - (ElementSize.y / 2));

        return position;
    }

    public Vector2 GetListSize(int elementCount, bool exact)
    {
        if (exact)
            return new Vector2(0, DataController.DataList.Count * ElementSize.y);
        else
            return new Vector2(0, elementCount);
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
