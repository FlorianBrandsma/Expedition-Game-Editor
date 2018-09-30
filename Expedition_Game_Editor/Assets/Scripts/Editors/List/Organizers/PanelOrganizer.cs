﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class PanelOrganizer : MonoBehaviour, IOrganizer
{
    private List<ElementData> local_data_list;

    static public List<SelectionElement> element_list = new List<SelectionElement>();
    private List<SelectionElement> element_list_local = new List<SelectionElement>();

    //private bool visible_only;
    private bool zigzag;

    private Path select_path;
    private Path edit_path;

    public float element_size { get; set; }

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
        //visible_only = listProperties.visible_only;
        zigzag = listProperties.zigzag;
    }

    public void SetListSize()
    {
        x_anchors.Clear();

        float[] new_anchors = new float[] { 0, 1 };

        if (zigzag)
            new_anchors = right_anchor;

        for (int i = 0; i < listManager.listData.list.Count; i++)
        {
            string new_header = listManager.listData.list[i].ToString();

            if (i > 0)
            {
                if (new_header == "id: " + listManager.listData.list[i - 1].id)
                    new_header = "";

                if (zigzag)
                    new_anchors = (new_header != "" ? SwapAnchors(x_anchors[i - 1]) : x_anchors[i - 1]);
            }

            x_anchors.Add(new_anchors);
        }

        SetList(listManager.list_area.anchorMax.x);
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

        for (int i = 0; i < listManager.listData.list.Count; i++)
        {
            string new_header = listManager.listData.data.table + " " + i;

            if (i > 0)
            {   //If header is the same as the previous one
                if (new_header == listManager.listData.data.table + " " + (i - 1))
                    new_header = "";
            }

            element_size = (listManager.base_size / rect_width) + (new_header != "" ? bonus_height : 0);
            row_height.Add(element_size);

            position_sum += element_size;
            row_offset_max.Add(position_sum - element_size);
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

        SelectionElement element_prefab = Resources.Load<SelectionElement>("Editor/Organizer/Panel/Panel_Prefab");

        for (int i = 0; i < local_data_list.Count; i++)
        {
            SelectionElement element = listManager.SpawnElement(element_list, element_prefab, local_data_list[i]);
            element_list_local.Add(element);

            listManager.element_list.Add(element);

            string new_header = element.data.table + " " + i;
            string content = "This is a pretty regular sentence. The structure is something you'd expect. Nothing too long though!";

            element.id_text.text = element.data.id.ToString();
            element.header.text = new_header;
            element.content.text = content;

            //Debugging
            element.name = new_header;

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

        int index = listManager.listData.list.IndexOf(element.data);

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
