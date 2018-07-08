using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class DisplayOrganizer : MonoBehaviour, IOrganizer
{
    static public List<RectTransform> element_list = new List<RectTransform>();
    private List<RectTransform> element_list_local = new List<RectTransform>();

    static public List<RectTransform> selection_list = new List<RectTransform>();
    private RectTransform element_selection;

    private bool visible_only;
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

    public void InitializeOrganizer(Path new_select_path, Path new_edit_path)
    {
        listManager = GetComponent<ListManager>();

        select_path = new_select_path;
        edit_path = new_edit_path;
    }

    public void SetProperties(ListProperties listProperties)
    {
        visible_only = listProperties.visible_only;
        zigzag = listProperties.zigzag;
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

    public Vector2 GetListSize(List<int> id_list, bool exact)
    {
        if (exact)
            return new Vector2(0, row_height.Sum());
        else
            return new Vector2(0, id_list.Count);
    }

    public void SetRows(List<int> id_list)
    {
        RectTransform element_prefab = Resources.Load<RectTransform>("Editor/Organizer/Display/Display_Prefab");

        for (int i = 0; i < id_list.Count; i++)
        {
            //if (ListPosition(i) > listMin.y)
            //    break;
            
            RectTransform new_element = listManager.SpawnElement(element_list, element_prefab);

            element_list_local.Add(new_element);

            SetElement(new_element, i);

            string new_header = listManager.table + " " + i;
            string content = "This is a pretty regular sentence. The structure is something you'd expect. Nothing too long though!";

            if (i > 0 && new_header == listManager.table + " " + (i - 1))
                new_header = "";

            new_element.name = new_header;

            ListElement list_element = new_element.GetComponent<ListElement>();

            list_element.id.text = id_list[i].ToString();
            list_element.header.text = new_header;
            list_element.content.text = content;

            //Crops content nicely
            list_element.SetOffset();
            
            //OpenEditor
            int id = id_list[i];

            new_element.GetComponent<Button>().onClick.AddListener(delegate { listManager.OpenPath(listManager.NewPath(select_path, id)); });
            new_element.GetComponent<ListElement>().edit_button.onClick.AddListener(delegate { listManager.OpenPath(listManager.NewPath(edit_path, id)); });
        }
    }

    public void ResetRows(List<int> filter)
    {
        CloseList();
        SetRows(filter);
    }

    void SetElement(RectTransform rect, int index)
    {
        rect.offsetMin = new Vector2(rect.offsetMin.x, listManager.list_parent.sizeDelta.y - (row_offset_max[index] + row_height[index]));
        rect.offsetMax = new Vector2(rect.offsetMax.x, -row_offset_max[index]);

        //Zigzag enabled
        rect.anchorMin = new Vector2(x_anchors[index][0], 0);
        rect.anchorMax = new Vector2(x_anchors[index][1], 1);

        rect.gameObject.SetActive(true);
    }

    float ListPosition(int i)
    {
        return listManager.list_parent.TransformPoint(new Vector2(0, (listManager.list_parent.sizeDelta.y / 2.222f) - row_offset_max[i])).y;
    }

    public void SelectElement(int index)
    {
        if(element_selection == null)
        {
            element_selection = listManager.SpawnElement(selection_list, Resources.Load<RectTransform>("Editor/Organizer/Display/Display_Selection"));
            element_selection.SetAsFirstSibling();
        }

        //element_selection.gameObject.SetActive(true);
        //Get correct index based off the ID
        SetElement(element_selection, index - 1);
        
        //If selection is below X ...
        //Move list up
        //main_list.GetComponent<ScrollRect>().verticalNormalizedPosition = 0.75f;
    }

    public void ResetSelection()
    {
        element_selection.gameObject.SetActive(false);
    }

    public void CloseList()
    {
        if(element_selection != null)
            ResetSelection();
        
        listManager.ResetElement(element_list_local);

        DestroyImmediate(this);
    }
}
