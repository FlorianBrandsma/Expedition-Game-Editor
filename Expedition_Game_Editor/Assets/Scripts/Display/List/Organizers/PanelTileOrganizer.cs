using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Collections;

public class PanelTileOrganizer : MonoBehaviour, IOrganizer, IList
{
    private Vector2 listSize;

    private ListProperties listProperties;
    private PanelTileProperties panelTileProperties;

    private bool horizontal, vertical;

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

        ElementList = new List<SelectionElement>();
    }

    public void InitializeProperties()
    {
        listProperties = (ListProperties)DisplayManager.Display;
        panelTileProperties = (PanelTileProperties)DisplayManager.Display.Properties;

        horizontal = listProperties.horizontal;
        vertical = listProperties.vertical;
    }

    public void SelectData()
    {
        SelectionManager.SelectData(dataController.DataList);
    }
    
    public void SetData()
    {
        SetData(dataController.DataList);
    }

    public void SetData(List<IDataElement> list)
    {
        generalDataList = list.Cast<GeneralData>().ToList();

        SelectionElement elementPrefab = Resources.Load<SelectionElement>("UI/PanelTile");

        listSize = GetListSize(list.Count, false);

        foreach (IDataElement data in list)
        {
            SelectionElement element = SelectionElementManager.SpawnElement(elementPrefab, listManager.listParent, 
                                                                            Enums.ElementType.PanelTile, DisplayManager, 
                                                                            DisplayManager.Display.SelectionType,
                                                                            DisplayManager.Display.SelectionProperty);
            ElementList.Add(element);

            data.SelectionElement = element;
            element.data = new SelectionElement.Data(dataController, data);

            element.GetComponent<EditorPanelTile>().InitializeChildElement();

            //Debugging
            GeneralData generalData = (GeneralData)data;
            element.name = generalData.DebugName + generalData.Id;
            //
            
            SetElement(element);
        }
    }

    public void SetElementSize()
    {
        ElementSize = listProperties.elementSize;
    }

    public Vector2 GetListSize(int element_count, bool exact)
    {
        Vector2 new_size;

        int list_width = GetListWidth();
        int list_height = GetListHeight();

        if (list_width > element_count)
            list_width = element_count;

        if (list_height > element_count)
            list_height = element_count;

        //No cases where a PanelTile only has a vertical slider. Calculation will be added if or when necessary
        new_size = new Vector2(horizontal ? ((element_count + (element_count % list_height)) * ElementSize.x) / list_height : list_width * ElementSize.y,
                                vertical ? 0 : list_height * ElementSize.y);

        if (exact)
            return new Vector2(new_size.x - listManager.RectTransform.rect.width, new_size.y);
        else
            return new Vector2(new_size.x / ElementSize.x, new_size.y / ElementSize.y);
    }

    public int GetListWidth()
    {
        int x = 0;

        while (-(x * ElementSize.x / 2f) + (x * ElementSize.x) < listManager.RectTransform.rect.max.x)
            x++;

        return x - 1;
    }

    public int GetListHeight()
    {
        int y = 0;

        while (-(y * ElementSize.y / 2f) + (y * ElementSize.y) < listManager.RectTransform.rect.max.y)
            y++;

        return y - 1;
    }

    public void UpdateData()
    {
        ResetData(null);
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

        rect.sizeDelta = new Vector2(ElementSize.x, ElementSize.y);

        rect.transform.localPosition = new Vector2(-((ElementSize.x * 0.5f) * (listSize.x - 1)) + Mathf.Floor(index / listSize.y) * ElementSize.x, 
                                                     (ElementSize.y * 0.5f) - (index % listSize.y * ElementSize.y));

        element.gameObject.SetActive(true);

        element.SetElement();
    }

    float ListPosition(int i)
    {
        return 0;
    }

    public void CloseList()
    {
        SelectionElementManager.CloseElement(ElementList);
    }

    public void ClearOrganizer() { }

    private void CancelSelection()
    {
        SelectionManager.CancelSelection(dataController.DataList);
    }

    public void CloseOrganizer()
    {
        CloseList();

        CancelSelection();

        DestroyImmediate(this);
    }
}
