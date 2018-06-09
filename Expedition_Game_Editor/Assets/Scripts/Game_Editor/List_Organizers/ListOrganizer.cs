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

    private bool visible_only;
    private bool show_numbers;

    private bool get_select, set_select;

    ListManager listManager;

    public void InitializeOrganizer(Path new_select_path, Path new_edit_path)
    {
        listManager = GetComponent<ListManager>();

        edit_path = new_edit_path;
    }

    public void SetProperties(ListProperties listProperties)
    {
        visible_only = listProperties.visible_only;
        show_numbers = listProperties.enable_numbers;
    }

    public void SetListSize(float new_size)
    {
        base_size = new_size;
    }

    public Vector2 GetListSize()
    {
        return new Vector2(0, base_size * listManager.id_list.Count);
    }

    public void SetRows()
    {
        for (int i = 0; i < listManager.id_list.Count; i++)
        {
            //if (ListPosition(i) > listMin.y)
            //    break;

            RectTransform new_element = listManager.SpawnElement(element_list, element_prefab);

            new_element.transform.SetParent(listManager.list_parent, false);

            SetElement(new_element, i);

            string header = listManager.table + " " + i;

            new_element.name = header;

            ListElement list_element = new_element.GetComponent<ListElement>();
            list_element.header.text = header;

            //OpenEditor
            int index = i;
            
            //Review
            new_element.GetComponent<Button>().onClick.AddListener(delegate { listManager.SelectElement(listManager.id_list[index], listManager.editable); });

            new_element.gameObject.SetActive(true);
        }
    }

    void SetElement(RectTransform rect, int index)
    {
        rect.anchorMax = new Vector2(1, 1);

        rect.sizeDelta = new Vector2(rect.sizeDelta.x, base_size);

        rect.transform.localPosition = new Vector2(0, (listManager.list_parent.sizeDelta.y / 2) - (base_size * index) - (base_size * 0.5f));
    }

    float ListPosition(int i)
    {
        return listManager.list_parent.TransformPoint(new Vector2(0, (listManager.list_parent.sizeDelta.y / 2.222f) - (base_size * i))).y;
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
        listManager.ResetText();

        listManager.ResetElement(element_list);

        ResetSelection();
    }
}
