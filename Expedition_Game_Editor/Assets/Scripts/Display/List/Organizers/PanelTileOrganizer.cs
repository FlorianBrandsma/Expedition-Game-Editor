using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Collections;

public class PanelTileOrganizer : MonoBehaviour, IOrganizer, IList
{
    private PanelTileProperties properties;

    private List<GeneralData> local_data_list;

    static public List<SelectionElement> element_list = new List<SelectionElement>();

    public Vector2 element_size { get; set; }
    private Vector2 list_size;

    private bool horizontal, vertical;

    private ListManager listManager;
    
    public void InitializeOrganizer()
    {
        listManager = GetComponent<ListManager>(); 
    }

    public void SetProperties()
    {
        properties = listManager.listProperties.GetComponent<PanelTileProperties>();

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

        int list_width = GetListWidth();
        int list_height = GetListHeight();

        if (list_width > element_count)
            list_width = element_count;

        if (list_height > element_count)
            list_height = element_count;

        //No cases where a PanelTile only has a vertical slider. Calculation will be added if or when necessary
        new_size = new Vector2( horizontal  ? ((element_count + (element_count % list_height)) * element_size.x) / list_height  : list_width  * element_size.y,
                                vertical    ? 0                                                                                 : list_height * element_size.y);

        if (exact)
            return new Vector2(new_size.x - listManager.rectTransform.rect.width, new_size.y);
        else
            return new Vector2(new_size.x / element_size.x, new_size.y / element_size.y);
    }

    public int GetListWidth()
    {
        int x = 0;

        while (-(x * element_size.x / 2f) + (x * element_size.x) < listManager.rectTransform.rect.max.x)
            x++;

        return x - 1;
    }

    public int GetListHeight()
    {
        int y = 0;

        while (-(y * element_size.y / 2f) + (y * element_size.y) < listManager.rectTransform.rect.max.y)
            y++;

        return y - 1;
    }

    public void GetData()
    {
        //listManager.properties.dataList.GetData(listManager.properties.route);
    }

    public void UpdateData()
    {
        ResetData(null);

        SetData();
    }

    public void SetData()
    {
        //var new_list = data_list.Cast<GeneralData>().ToList();

        //local_data_list = (from data in new_list
        //                   select new UIElementData()
        //                   {
        //                       id = data.id,
        //                       table = data.table,
        //                       type = data.type,
        //                       index = data.index,
        //                       name = (data.table + " " + data.index),
        //                       icon = "Textures/Characters/1"
        //                   }).ToList();

        SelectionElement element_prefab = Resources.Load<SelectionElement>("UI/PanelTile");

        list_size = GetListSize(local_data_list.Count, false);

        foreach (GeneralData data in local_data_list)
        {
            SelectionElement element = listManager.SpawnElement(element_list, element_prefab);
            listManager.element_list.Add(element);

            //element.SetElement(data, new[] { data });

            //Debugging
            //element.name = data.name;

            SetElement(element);
        }
    }

    public void ResetData(ICollection filter)
    {
        CloseList();
        SetData();
    }

    void SetElement(SelectionElement element)
    {
        RectTransform rect = element.GetComponent<RectTransform>();

        int index = local_data_list.FindIndex(x => x.id == element.data.Cast<GeneralData>().ToList().FirstOrDefault().id);

        rect.sizeDelta = new Vector2(element_size.x, element_size.y);

        rect.transform.localPosition = new Vector2(-((element_size.x * 0.5f) * (list_size.x - 1)) + Mathf.Floor(index / list_size.y) * element_size.x, 
                                                     (element_size.y * 0.5f) - (index % list_size.y * element_size.y));

        rect.gameObject.SetActive(true);
    }

    public SelectionElement GetElement(int index)
    {
        return listManager.element_list[index];
    }

    float ListPosition(int i)
    {
        return 0;
    }

    public void CloseList()
    {
        listManager.ResetElement(listManager.element_list);

        listManager.element_list.Clear();
    }

    public void CloseOrganizer()
    {
        CloseList();

        DestroyImmediate(this);
    }
}
