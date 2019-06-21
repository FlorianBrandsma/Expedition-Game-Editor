using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Collections;

public class PanelTileOrganizer : MonoBehaviour, IOrganizer, IList
{
    private Vector2 listSize;

    private PanelTileProperties properties;
    private bool horizontal, vertical;

    private IDataController dataController;
    private List<GeneralData> generalDataList;

    private ListManager ListManager { get { return GetComponent<ListManager>(); } }

    public List<SelectionElement> ElementList { get; set; }
    public Vector2 ElementSize { get; set; }

    public void InitializeOrganizer()
    {
        dataController = ListManager.listProperties.DataController;
        ElementList = new List<SelectionElement>();
    }

    public void InitializeProperties()
    {
        properties = ListManager.listProperties.GetComponent<PanelTileProperties>();

        horizontal = ListManager.listProperties.horizontal;
        vertical = ListManager.listProperties.vertical;
    }

    public void SetElementSize()
    {
        ElementSize = ListManager.listProperties.elementSize;
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
        new_size = new Vector2( horizontal  ? ((element_count + (element_count % list_height)) * ElementSize.x) / list_height  : list_width  * ElementSize.y,
                                vertical    ? 0                                                                                 : list_height * ElementSize.y);

        if (exact)
            return new Vector2(new_size.x - ListManager.rectTransform.rect.width, new_size.y);
        else
            return new Vector2(new_size.x / ElementSize.x, new_size.y / ElementSize.y);
    }

    public int GetListWidth()
    {
        int x = 0;

        while (-(x * ElementSize.x / 2f) + (x * ElementSize.x) < ListManager.rectTransform.rect.max.x)
            x++;

        return x - 1;
    }

    public int GetListHeight()
    {
        int y = 0;

        while (-(y * ElementSize.y / 2f) + (y * ElementSize.y) < ListManager.rectTransform.rect.max.y)
            y++;

        return y - 1;
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
            SelectionElement element = SelectionElementManager.SpawnElement(elementPrefab, Enums.ElementType.PanelTile,
                                                                            ListManager, ListManager.selectionType, ListManager.selectionProperty, ListManager.listParent);
            ElementList.Add(element);

            data.SelectionElement = element;
            element.route.data = new Data(dataController, data);

            //Debugging
            GeneralData generalData = (GeneralData)data;
            element.name = generalData.DebugName + generalData.id;
            //

            SetElement(element);
        }
    }

    public void UpdateData()
    {
        ResetData(null);

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

        rect.sizeDelta = new Vector2(ElementSize.x, ElementSize.y);

        rect.transform.localPosition = new Vector2(-((ElementSize.x * 0.5f) * (listSize.x - 1)) + Mathf.Floor(index / listSize.y) * ElementSize.x, 
                                                     (ElementSize.y * 0.5f) - (index % listSize.y * ElementSize.y));

        rect.gameObject.SetActive(true);

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

    public void CloseOrganizer()
    {
        CloseList();

        DestroyImmediate(this);
    }
}
