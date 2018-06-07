using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class DisplayOrganizer : MonoBehaviour, IOrganizer
{
    private List<RectTransform> element_list = new List<RectTransform>();

    public RectTransform element_prefab;
    public RectTransform element_selection;

    private bool visible_only;
    private bool show_numbers;
    private bool zigzag;

    private Path select_path;
    private Path edit_path;

    private float base_size;
    private float bonus_height = 25;

    private List<float> row_height     = new List<float>(); //Individual heights
    private List<float> row_offset_max = new List<float>(); //Combined heights

    private List<float[]> x_anchors    = new List<float[]>();

    private float[] left_anchor  = new float[] { 0.1f, 1 };
    private float[] right_anchor = new float[] { 0, 0.9f };

    ListManager listManager;

    public void SetProperties(Path new_select_path, Path new_edit_path, bool new_visible_only, bool new_show_numbers, bool new_zigzag)
    {
        listManager = GetComponent<ListManager>();

        select_path = new_select_path;
        edit_path = new_edit_path;

        visible_only = new_visible_only;
        show_numbers = new_show_numbers;
        zigzag = new_zigzag;
    }

    public void SetListSize(float new_size)
    {
        base_size = new_size;

        SetAnchors();
    }

    void SetAnchors()
    {
        x_anchors.Clear();

        float[] new_anchors = new float[] { 0, 1 };

        if (zigzag)
            new_anchors = right_anchor;

        for (int i = 0; i < listManager.id_list.Count; i++)
        {
            string new_header = listManager.id_list[i].ToString();

            if (i > 0)
            {
                if (new_header == "id: " + listManager.id_list[i - 1])
                    new_header = "";

                if (zigzag)
                    new_anchors = (new_header != "" ? SwapAnchors(x_anchors[i - 1]) : x_anchors[i - 1]);
            }

            x_anchors.Add(new_anchors);
        }
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
        row_offset_max.Clear();
        row_height.Clear();

        float position_sum = 0;

        for (int i = 0; i < listManager.id_list.Count; i++)
        {
            string new_header = listManager.table + " " + i;

            if (i > 0)
            {   //If header is the same as the previous one
                if (new_header == listManager.table + " " + (i - 1))
                    new_header = "";
            }

            float new_height = (base_size / rect_width) + (new_header != "" ? bonus_height : 0);
            row_height.Add(new_height);

            position_sum += new_height;
            row_offset_max.Add(position_sum - new_height);
        }
    }

    public Vector2 GetListSize()
    {
        return new Vector2(listManager.list_parent.sizeDelta.x, row_height.Sum());
    }

    public void SetRows()
    {
        for (int i = 0; i < listManager.id_list.Count; i++)
        {
            //if (ListPosition(i) > listMin.y)
            //    break;
            
            RectTransform new_element = listManager.SpawnElement(element_list, element_prefab);

            //new_element.transform.SetParent(list_manager.list_parent, false);

            SetElement(new_element, i);

            string new_header = listManager.table + " " + i;
            string content = "This is a pretty regular sentence. The structure is something you'd expect. Nothing too long though!";

            if (i > 0 && new_header == listManager.table + " " + (i - 1))
                new_header = "";

            new_element.name = new_header;

            ListElement list_element = new_element.GetComponent<ListElement>();

            list_element.id.text = listManager.id_list[i].ToString();
            list_element.header.text = new_header;
            list_element.content.text = content;

            //Crops content nicely
            list_element.SetOffset();
            
            //OpenEditor
            int id = listManager.id_list[i];

            new_element.GetComponent<Button>().onClick.AddListener(delegate { listManager.OpenEditor(listManager.NewPath(select_path, id)); });
            new_element.GetComponent<ListElement>().edit_button.onClick.AddListener(delegate { listManager.OpenEditor(listManager.NewPath(edit_path, id)); });

            new_element.gameObject.SetActive(true);  
        }
    }

    void SetElement(RectTransform rect, int index)
    {
        rect.offsetMin = new Vector2(rect.offsetMin.x, listManager.list_parent.sizeDelta.y - (row_offset_max[index] + row_height[index]));
        rect.offsetMax = new Vector2(rect.offsetMax.x, -row_offset_max[index]);

        //Zigzag enabled
        rect.anchorMin = new Vector2(x_anchors[index][0], 0);
        rect.anchorMax = new Vector2(x_anchors[index][1], 1);

        if (show_numbers)
            listManager.SetNumbers(listManager.vertical_number_parent, index, new Vector2(0, rect.transform.localPosition.y));
    }

    float ListPosition(int i)
    {
        return listManager.list_parent.TransformPoint(new Vector2(0, (listManager.list_parent.sizeDelta.y / 2.222f) - row_offset_max[i])).y;
    }

    public void SelectElement(int id)
    {
        element_selection.gameObject.SetActive(true);
        //Get correct index based off the ID
        SetElement(element_selection, id-1);

        //If selection is below X ...
        //Move list up
        //main_list.GetComponent<ScrollRect>().verticalNormalizedPosition = 0.75f;
    }

    public void CloseList()
    {
        element_selection.gameObject.SetActive(false);
        
        listManager.ResetElement(element_list);
        listManager.ResetText();

        gameObject.SetActive(false);
    }  
}
