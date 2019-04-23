using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class TileOrganizer : MonoBehaviour, IOrganizer, IList
{
    private ListManager listManager { get { return GetComponent<ListManager>(); } }

    static public List<SelectionElement> element_list = new List<SelectionElement>();

    public Vector2 element_size { get; set; }
    private Vector2 list_size;

    private TileProperties properties;
    private bool horizontal, vertical;

    private IDataController dataController;
    private List<GeneralData> generalData_list;

    public void InitializeOrganizer()
    {
        dataController = listManager.listProperties.segmentController.dataController;
    }

    public void SetProperties()
    {
        properties = listManager.listProperties.GetComponent<TileProperties>();

        horizontal = listManager.listProperties.horizontal;
        vertical = listManager.listProperties.vertical;
    }

    public void SetElementSize()
    {
        element_size = listManager.listProperties.element_size;
    }

    public Vector2 GetListSize(int element_count, bool exact)
    {
        Vector2 new_size;

        if(horizontal && vertical)
        {
            new_size = new Vector2( horizontal  ? properties.grid_size.x * element_size.x : element_size.x,
                                    vertical    ? properties.grid_size.y * element_size.y : element_size.y);
        } else {

            int list_width  = GetListWidth(element_count);
            int list_height = GetListHeight(element_count);

            //No cases where a Tile only has a horizontal slider. Calculation will be added if or when necessary
            new_size = new Vector2( horizontal  ? 0                                                                 : list_width  * element_size.x,
                                    vertical    ? (Mathf.Ceil(element_count / (float)list_width) * element_size.y)  : list_height * element_size.y);
        }

        if (exact)
            return new Vector2(new_size.x - listManager.rectTransform.rect.width, new_size.y);
        else
            return new Vector2(new_size.x / element_size.x, new_size.y / element_size.y);
    }

    public int GetListWidth(int element_count)
    {
        int x = 0;

        while (x <= element_count && -(x * element_size.x / 2f) + (x * element_size.x) < listManager.rectTransform.rect.max.x)
            x++;

        return x - 1;
    }

    public int GetListHeight(int element_count)
    {
        int y = 0;

        while (y <= element_count && -(y * element_size.y / 2f) + (y * element_size.y) < listManager.rectTransform.rect.max.y)
            y++;

        return y - 1;
    }

    public void UpdateData()
    {
        ResetData(null);
        SetData();
    }

    public void SetData()
    {
        SetData(dataController.data_list);
    }

    public void SetData(ICollection list)
    {
        generalData_list = list.Cast<GeneralData>().ToList();

        SelectionElement element_prefab = Resources.Load<SelectionElement>("UI/Tile");

        list_size = GetListSize(list.Count, false);

        foreach (var data in list)
        {
            SelectionElement element = listManager.SpawnElement(element_list, element_prefab);
            listManager.element_list.Add(element);

            element.route.data = new Data(dataController, new[] { data });

            //Debugging
            GeneralData generalData = (GeneralData)data;
            element.name = generalData.table + generalData.id;
            //

            SetElement(element);
        }
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

        rect.sizeDelta = new Vector2(element_size.x, element_size.y);
        
        rect.transform.localPosition = new Vector2( -((element_size.x * 0.5f) * (list_size.x - 1)) + (index % list_size.x * element_size.x),
                                                     -(element_size.y * 0.5f) + (listManager.list_parent.sizeDelta.y / 2f) - (Mathf.Floor(index / list_size.x) * element_size.y));

        rect.gameObject.SetActive(true);
    }

    public SelectionElement GetElement(int index)
    {
        return listManager.element_list[index];
    }

    public void CloseList()
    {
        listManager.ResetElement();
    }

    public void CloseOrganizer()
    {
        CloseList();

        DestroyImmediate(this);
    }
}
