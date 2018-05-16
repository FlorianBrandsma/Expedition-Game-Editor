using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class ListOrganizer : MonoBehaviour, IOrganizer
{
    private List<RectTransform> element_list = new List<RectTransform>();

    public RectTransform element_prefab;
    public RectTransform element_selection;

    private Path edit_path;

    private float base_size;

    public bool get_select, set_select;

    ListManager list_manager;

    public void SetProperties(Path new_edit_path, bool new_get_select, bool new_set_select)
    {
        edit_path = new_edit_path;

        get_select = new_get_select;
        set_select = new_set_select;
    }

    public void OpenList(float new_height)
    {
        list_manager = GetComponent<ListManager>();

        base_size = new_height;
    }

    public Vector2 GetListSize()
    {
        return new Vector2(list_manager.list_parent.sizeDelta.x, base_size * list_manager.id_list.Count);
    }

    public void SetRows()
    {
        for (int i = 0; i < list_manager.id_list.Count; i++)
        {
            //if (ListPosition(i) > listMin.y)
            //    break;

            RectTransform new_element = list_manager.SpawnElement(element_list, element_prefab);

            new_element.transform.SetParent(list_manager.list_parent, false);

            SetElement(new_element, i);

            string header = list_manager.table + " " + i;

            new_element.name = header;

            ListElement list_element = new_element.GetComponent<ListElement>();
            list_element.header.text = header;

            //OpenEditor
            int index = i;
            
            //Review
            new_element.GetComponent<Button>().onClick.AddListener(delegate { list_manager.SelectElement(list_manager.id_list[index], (edit_path.editor.Count > 0)); });

            new_element.gameObject.SetActive(true);
        }
    }

    void SetElement(RectTransform rect, int index)
    {
        rect.anchorMax = new Vector2(1, 1);

        rect.sizeDelta = new Vector2(rect.sizeDelta.x, base_size);

        rect.transform.localPosition = new Vector2(0, (list_manager.list_parent.sizeDelta.y / 2) - (base_size * index) - (base_size * 0.5f));

        if (GetComponent<ListManager>().slider.gameObject.activeInHierarchy)
            rect.anchorMax = new Vector2(0.9f, 1);
    }

    float ListPosition(int i)
    {
        return list_manager.list_parent.TransformPoint(new Vector2(0, (list_manager.list_parent.sizeDelta.y / 2.222f) - (base_size * i))).y;
    }

    public void SelectElement(int id)
    {
        SetElement(element_selection, id - 1);

        element_selection.gameObject.SetActive(true);  
    }

    public void ResetSelection()
    {
        element_selection.gameObject.SetActive(false);
    }

    public void CloseList()
    {
        list_manager.ResetElement(element_list);

        ResetSelection();

        gameObject.SetActive(false);
    }
}
