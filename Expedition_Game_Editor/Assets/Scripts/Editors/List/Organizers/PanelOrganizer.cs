﻿using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class PanelOrganizer : MonoBehaviour, IOrganizer
{
    private PanelProperties properties;

    private List<ElementData> local_data_list;

    static public List<SelectionElement> element_list = new List<SelectionElement>();
    private List<SelectionElement> element_list_local = new List<SelectionElement>();

    public Vector2 element_size { get; set; }

    private float bonus_height = 25;

    private List<float> row_height     = new List<float>(); //Individual heights
    private List<float> row_offset_max = new List<float>(); //Combined heights

    private List<float[]> x_anchors    = new List<float[]>();

    private float[] left_anchor  = new float[] { 0.1f, 1 };
    private float[] right_anchor = new float[] { 0, 0.9f };

    ListManager listManager;

    public void InitializeOrganizer()
    {
        listManager = GetComponent<ListManager>();
    }

    public void SetProperties(ListProperties listProperties)
    {
        properties = listProperties.GetComponent<PanelProperties>();
    }

    public void SetElementSize()
    {
        element_size = listManager.listProperties.element_size;

        x_anchors.Clear();

        float[] new_anchors = new float[] { 0, 1 };

        if (properties.zigzag)
            new_anchors = right_anchor;

        for (int i = 0; i < listManager.listProperties.listData.list.Count; i++)
        {
            string new_header = listManager.listProperties.listData.list[i].ToString();

            if (i > 0)
            {
                if (new_header == "id: " + listManager.listProperties.listData.list[i - 1].id)
                    new_header = "";

                if (properties.zigzag)
                    new_anchors = (new_header != "" ? SwapAnchors(x_anchors[i - 1]) : x_anchors[i - 1]);
            }

            x_anchors.Add(new_anchors);
        }

        SetList(properties.reference_area.anchorMax.x);
    }

    float[] SwapAnchors(float[] old_anchors)
    {
        if (old_anchors == left_anchor)
            return right_anchor;
        else
            return left_anchor;
    }

    public void SetList(float rect_width)
    {
        //Simplify this. Remove "bonus height" from name removal
        row_offset_max.Clear();
        row_height.Clear();

        float position_sum = 0;

        for (int i = 0; i < listManager.listProperties.listData.list.Count; i++)
        {
            string new_header = listManager.listProperties.listData.data.table + " " + i;

            if (i > 0)
            {   //If header is the same as the previous one
                if (new_header == listManager.listProperties.listData.data.table + " " + (i - 1))
                    new_header = "";
            }

            float new_size = (element_size.y / rect_width) + (new_header != "" ? bonus_height : 0);
            row_height.Add(new_size);

            position_sum += new_size;
            row_offset_max.Add(position_sum - new_size);
        }
    }

    public Vector2 GetListSize(List<ElementData> data_list, bool exact)
    {
        if (exact)
            return new Vector2(0, row_height.Sum());
        else
            return new Vector2(0, data_list.Count);
    }

    public void SetRows(List<ElementData> data_list)
    {
        local_data_list = data_list;

        SelectionElement element_prefab = Resources.Load<SelectionElement>("UI/Panel");

        for (int i = 0; i < local_data_list.Count; i++)
        {
            ElementData element_data = local_data_list[i];

            SelectionElement element = listManager.SpawnElement(element_list, element_prefab, element_data);

            element_list_local.Add(element);

            listManager.element_list.Add(element);

            //Temporary
            string new_header = element.data.table + " " + i;

            EditorPanel panel = element.GetComponent<EditorPanel>();

            panel.header.text = new_header;
            panel.description.text = properties.temp_description;

            if(properties.icon)  
                panel.icon.texture = Resources.Load<Texture2D>("Textures/Characters/1");

            if (properties.edit)
            {
                ElementData edit_data = new ElementData(properties.edit_data.table, element_data.id, properties.edit_data.type);
                panel.edit_button.InitializeSelection(listManager, edit_data, SelectionManager.Property.Edit);
            }

            //Debugging
            element.name = new_header;
            //

            SetElement(element);
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

        int index = listManager.listProperties.listData.list.IndexOf(element.data);

        rect.offsetMin = new Vector2(rect.offsetMin.x, listManager.list_parent.sizeDelta.y - (row_offset_max[index] + row_height[index]));
        rect.offsetMax = new Vector2(rect.offsetMax.x, -row_offset_max[index]);

        //Zigzag enabled
        rect.anchorMin = new Vector2(x_anchors[index][0], 0);
        rect.anchorMax = new Vector2(x_anchors[index][1], 1);

        rect.gameObject.SetActive(true);
    }

    public SelectionElement GetElement(int index)
    {
        return element_list_local[index];
    }

    float ListPosition(int i)
    {
        return listManager.list_parent.TransformPoint(new Vector2(0, (listManager.list_parent.sizeDelta.y / 2.222f) - row_offset_max[i])).y;
    }

    public void CloseList()
    {
        listManager.ResetElement(element_list_local);

        DestroyImmediate(this);
    }
}
