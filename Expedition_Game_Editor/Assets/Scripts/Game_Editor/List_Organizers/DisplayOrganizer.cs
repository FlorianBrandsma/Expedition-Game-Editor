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
    
    private Path select_path;
    private Path edit_path;

    private float base_size;
    private float bonus_height = 25;

    private bool zigzag;

    public List<float> row_height     = new List<float>(); //Individual heights
    public List<float> row_offset_max = new List<float>(); //Combined heights

    public List<float[]> x_anchors    = new List<float[]>();

    private float[] left_anchor  = new float[] { 0.1f, 1 };
    private float[] right_anchor = new float[] { 0, 0.9f };

    ListManager list_manager;

    public void SetProperties(Path base_select_path, Path base_edit_path, bool zigzag)
    {
        select_path = base_select_path;
        edit_path = base_edit_path;

        zigzag = this.zigzag;
    }

    public void OpenList(float new_size)
    {
        list_manager = GetComponent<ListManager>();

        base_size = new_size;

        SetAnchors();
    }

    void SetAnchors()
    {
        x_anchors.Clear();

        float[] new_anchors = new float[] { 0, 1 };

        if (zigzag)
            new_anchors = right_anchor;

        for (int i = 0; i < list_manager.id_list.Count; i++)
        {
            string new_header = list_manager.id_list[i].ToString();

            if (i > 0)
            {
                if (new_header == "id: " + list_manager.id_list[i - 1])
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

        for (int i = 0; i < list_manager.id_list.Count; i++)
        {
            string new_header = list_manager.table + " " + i;

            if (i > 0)
            {   //If header is the same as the previous one
                if (new_header == list_manager.table + " " + (i - 1))
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
        return new Vector2(list_manager.list_parent.sizeDelta.x, row_height.Sum());
    }

    public void SetRows()
    {
        for (int i = 0; i < list_manager.id_list.Count; i++)
        {
            //if (ListPosition(i) > listMin.y)
            //    break;
            
            RectTransform new_element = list_manager.SpawnElement(element_list, element_prefab);

            //new_element.transform.SetParent(list_manager.list_parent, false);

            SetElement(new_element, i);

            string new_header = list_manager.table + " " + i;
            string content = "This is a pretty regular sentence. The structure is something you'd expect. Nothing too long though!";

            if (i > 0 && new_header == list_manager.table + " " + (i - 1))
                new_header = "";

            new_element.name = new_header;

            ListElement list_element = new_element.GetComponent<ListElement>();

            list_element.id.text = list_manager.id_list[i].ToString();
            list_element.header.text = new_header;
            list_element.content.text = content;

            //Crops content nicely
            list_element.SetOffset();
            
            //OpenEditor
            int index = i;

            new_element.GetComponent<Button>().onClick.AddListener(delegate { list_manager.OpenEditor(list_manager.NewPath(select_path, index)); });
            new_element.GetComponent<ListElement>().edit_button.onClick.AddListener(delegate { list_manager.OpenEditor(list_manager.NewPath(edit_path, index)); });

            new_element.gameObject.SetActive(true);
        }
    }

    void SetElement(RectTransform rect, int index)
    {
        rect.offsetMin = new Vector2(rect.offsetMin.x, list_manager.list_parent.sizeDelta.y - (row_offset_max[index] + row_height[index]));
        rect.offsetMax = new Vector2(rect.offsetMax.x, -row_offset_max[index]);

        //Zigzag enabled
        rect.anchorMin = new Vector2(x_anchors[index][0], 0);
        rect.anchorMax = new Vector2(x_anchors[index][1], 1);
    }

    float ListPosition(int i)
    {
        return list_manager.list_parent.TransformPoint(new Vector2(0, (list_manager.list_parent.sizeDelta.y / 2.222f) - row_offset_max[i])).y;
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

        list_manager.ResetElement(element_list);

        gameObject.SetActive(false);
    }  
}
