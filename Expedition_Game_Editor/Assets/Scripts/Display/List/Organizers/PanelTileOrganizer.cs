using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Collections;

public class PanelTileOrganizer : MonoBehaviour, IOrganizer, IList
{
    private ListManager listManager { get { return GetComponent<ListManager>(); } }

    static public List<SelectionElement> element_list = new List<SelectionElement>();

    public Vector2 ElementSize { get; set; }
    private Vector2 list_size;

    private PanelTileProperties properties;
    private bool horizontal, vertical;

    private IDataController dataController;
    private List<GeneralData> generalData_list;

    public void InitializeOrganizer()
    {
        dataController = listManager.listProperties.SegmentController.DataController;
    }

    public void SetProperties()
    {
        properties = listManager.listProperties.GetComponent<PanelTileProperties>();

        horizontal = listManager.listProperties.horizontal;
        vertical = listManager.listProperties.vertical;
    }

    public void SetElementSize()
    {
        ElementSize = listManager.listProperties.elementSize;
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
            return new Vector2(new_size.x - listManager.rectTransform.rect.width, new_size.y);
        else
            return new Vector2(new_size.x / ElementSize.x, new_size.y / ElementSize.y);
    }

    public int GetListWidth()
    {
        int x = 0;

        while (-(x * ElementSize.x / 2f) + (x * ElementSize.x) < listManager.rectTransform.rect.max.x)
            x++;

        return x - 1;
    }

    public int GetListHeight()
    {
        int y = 0;

        while (-(y * ElementSize.y / 2f) + (y * ElementSize.y) < listManager.rectTransform.rect.max.y)
            y++;

        return y - 1;
    }

    public void SetData()
    {
        SetData(dataController.DataList);
    }

    public void SetData(ICollection list)
    {
        generalData_list = list.Cast<GeneralData>().ToList();

        SelectionElement element_prefab = Resources.Load<SelectionElement>("UI/PanelTile");

        list_size = GetListSize(list.Count, false);

        foreach (var data in list)
        {
            SelectionElement element = listManager.SpawnElement(element_list, element_prefab, Enums.ElementType.PanelTile);
            listManager.elementList.Add(element);

            element.route.data = new Data(dataController, new[] { data });

            //Debugging
            GeneralData generalData = (GeneralData)data;
            element.name = generalData.table + generalData.id;
            //

            SetElement(element);
        }
    }

    public void UpdateData()
    {
        ResetData(null);

        SelectionManager.ResetSelection(listManager);
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

        int index = generalData_list.FindIndex(x => x.id == element.GeneralData().id);

        rect.sizeDelta = new Vector2(ElementSize.x, ElementSize.y);

        rect.transform.localPosition = new Vector2(-((ElementSize.x * 0.5f) * (list_size.x - 1)) + Mathf.Floor(index / list_size.y) * ElementSize.x, 
                                                     (ElementSize.y * 0.5f) - (index % list_size.y * ElementSize.y));

        rect.gameObject.SetActive(true);
    }

    public SelectionElement GetElement(int index)
    {
        return listManager.elementList[index];
    }

    float ListPosition(int i)
    {
        return 0;
    }

    public void CloseList()
    {
        listManager.ResetElement();
    }

    public void ClearOrganizer() { }

    public void CloseOrganizer()
    {
        CloseList();

        DestroyImmediate(this);
    }
}
