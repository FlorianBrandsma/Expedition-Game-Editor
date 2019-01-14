using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class TileOrganizer : MonoBehaviour, IOrganizer
{
    private TileProperties properties;

    private List<ElementData> local_data_list;

    static public List<SelectionElement> element_list = new List<SelectionElement>();
    private List<SelectionElement> element_list_local = new List<SelectionElement>();

    //private Enums.SelectionProperty selectionProperty;
    //private SelectionManager.Type selectionType;

    public Vector2 element_size { get; set; }

    private Vector2 list_size;

    //private bool visible_only;

    private bool horizontal, vertical;

    private ListManager listManager;

    public void InitializeOrganizer()
    {
        listManager = GetComponent<ListManager>();
    }

    public void SetProperties(ListProperties listProperties)
    {
        properties = listProperties.GetComponent<TileProperties>();

        //selectionProperty = listProperties.selectionProperty;
        //selectionType = listProperties.selectionType;

        //visible_only = listProperties.visible_only;

        horizontal = listProperties.horizontal;
        vertical = listProperties.vertical;
    }

    public void SetElementSize()
    {
        element_size = listManager.listData.listProperties.element_size;
    }

    public Vector2 GetListSize(List<ElementData> data_list, bool exact)
    {
        Vector2 new_size;

        if(horizontal && vertical)
        {
            new_size = new Vector2( horizontal  ? properties.grid_size.x * element_size.x : element_size.x,
                                    vertical    ? properties.grid_size.y * element_size.y : element_size.y);
        } else
        {
            int list_width  = GetListWidth(data_list.Count);
            int list_height = GetListHeight(data_list.Count);

            //No cases where a Tile only has a horizontal slider. Calculation will be added if or when necessary
            new_size = new Vector2( horizontal  ? 0                                                                     : list_width  * element_size.x,
                                    vertical    ? (Mathf.Ceil(data_list.Count / (float)list_width) * element_size.y)    : list_height * element_size.y);
        }

        if (exact)
            return new Vector2(new_size.x - listManager.rectTransform.rect.width, new_size.y);
        else
            return new Vector2(new_size.x / element_size.x, new_size.y / element_size.y);
    }

    public int GetListWidth(int elements)
    {
        int x = 0;

        while (x <= elements && -(x * element_size.x / 2f) + (x * element_size.x) < listManager.rectTransform.rect.max.x)
            x++;

        return x - 1;
    }

    public int GetListHeight(int elements)
    {
        int y = 0;

        while (y <= elements && -(y * element_size.y / 2f) + (y * element_size.y) < listManager.rectTransform.rect.max.y)
            y++;

        return y - 1;
    }

    public void SetRows(List<ElementData> data_list)
    {
        local_data_list = data_list;

        SelectionElement element_prefab = Resources.Load<SelectionElement>("UI/Tile");

        int i = 0;

        list_size = GetListSize(local_data_list, false);

        for (int y = 0; y < list_size.y; y++)
        {
            for (int x = 0; x < list_size.x; x++)
            {
                if (i == local_data_list.Count) break;

                SelectionElement element = listManager.SpawnElement(element_list, element_prefab, local_data_list[i]);
                element_list_local.Add(element);

                listManager.element_list.Add(element);

                //Debugging
                element.name = listManager.listData.data.table + " " + i;

                SetElement(element);

                i++;
            }
        }
    }

    public void ResetRows(List<ElementData> filter)
    {
        CloseList();
        SetRows(filter);
    }

    void SetElement(SelectionElement element)
    {
        RectTransform rect = element.GetComponent<RectTransform>();

        int index = local_data_list.IndexOf(element.data);

        rect.sizeDelta = new Vector2(element_size.x, element_size.y);
        
        rect.transform.localPosition = new Vector2( -((element_size.x * 0.5f) * (list_size.x - 1)) + (index % list_size.x * element_size.x),
                                                     -(element_size.y * 0.5f) + (listManager.list_parent.sizeDelta.y / 2f) - (Mathf.Floor(index / list_size.x) * element_size.y));

        rect.gameObject.SetActive(true);
    }

    public SelectionElement GetElement(int index)
    {
        return element_list_local[index];
    }

    public void CloseList()
    {
        listManager.ResetElement(element_list_local);

        DestroyImmediate(this);
    } 
}
