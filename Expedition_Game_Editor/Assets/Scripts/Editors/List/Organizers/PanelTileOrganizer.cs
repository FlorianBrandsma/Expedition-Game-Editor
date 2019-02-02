using UnityEngine;
using System.Collections.Generic;

public class PanelTileOrganizer : MonoBehaviour, IOrganizer
{
    private PanelTileProperties properties;

    private List<ElementData> local_data_list;

    static public List<SelectionElement> element_list = new List<SelectionElement>();
    private List<SelectionElement> element_list_local = new List<SelectionElement>();

    public Vector2 element_size { get; set; }
    private Vector2 list_size;

    private bool horizontal, vertical;

    ListManager listManager;

    public void InitializeOrganizer()
    {
        listManager = GetComponent<ListManager>();
    }

    public void SetProperties(ListProperties listProperties)
    {
        properties = listProperties.GetComponent<PanelTileProperties>();

        horizontal = listProperties.horizontal;
        vertical = listProperties.vertical;
    }

    public void SetElementSize()
    {
        element_size = listManager.listProperties.element_size;
    }

    public Vector2 GetListSize(List<ElementData> data_list, bool exact)
    {
        Vector2 new_size;

        int list_width = GetListWidth();
        int list_height = GetListHeight();

        if (list_width > data_list.Count)
            list_width = data_list.Count;

        if (list_height > data_list.Count)
            list_height = data_list.Count;

        //No cases where a PanelTile only has a vertical slider. Calculation will be added if or when necessary
        new_size = new Vector2( horizontal  ? ((data_list.Count + (data_list.Count % list_height)) * element_size.x) / list_height  : list_width  * element_size.y,
                                vertical    ? 0                                                                                     : list_height * element_size.y);

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

    public void SetRows(List<ElementData> data_list)
    {
        local_data_list = data_list;

        SelectionElement element_prefab = Resources.Load<SelectionElement>("UI/PanelTile");

        int i = 0;

        list_size = GetListSize(local_data_list, false);

        for (int x = 0; x < list_size.x; x++)
        {
            for (int y = 0; y < list_size.y; y++)
            {
                if (i == local_data_list.Count) break;

                ElementData element_data = local_data_list[i];

                SelectionElement element = listManager.SpawnElement(element_list, element_prefab, element_data);
                element_list_local.Add(element);

                listManager.element_list.Add(element);

                //Temporary
                string new_header = element.data.table + " " + i;

                EditorPanelTile panel = element.GetComponent<EditorPanelTile>();

                panel.header.text = new_header;
                panel.icon.texture = Resources.Load<Texture2D>("Textures/Characters/1");

                if (properties.edit)
                {
                    ElementData edit_data = new ElementData(properties.edit_data.table, element_data.id, properties.edit_data.type);
                    panel.edit_button.InitializeSelection(listManager, edit_data, SelectionManager.Property.Edit);
                }

                //Debugging
                element.name = listManager.listProperties.dataList.data.table + " " + i;

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

        rect.transform.localPosition = new Vector2(-((element_size.x * 0.5f) * (list_size.x - 1)) + Mathf.Floor(index / list_size.y) * element_size.x, 
                                                     (element_size.y * 0.5f) - (index % list_size.y * element_size.y));

        rect.gameObject.SetActive(true);
    }

    public SelectionElement GetElement(int index)
    {
        return element_list_local[index];
    }

    float ListPosition(int i)
    {
        return 0;
    }

    public void CloseList()
    {
        listManager.ResetElement(element_list_local);

        DestroyImmediate(this);
    }
}
