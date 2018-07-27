using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class ListOrganizer : MonoBehaviour, IOrganizer
{
    static public List<RectTransform> element_list = new List<RectTransform>();
    private List<RectTransform> element_list_local = new List<RectTransform>();

    static public List<RectTransform> selection_list = new List<RectTransform>();
    private RectTransform element_selection;

    private Enums.SelectionType selectionType;

    private float base_size;

    private bool visible_only;

    ListManager listManager;

    public void InitializeOrganizer(Path new_select_path, Path new_edit_path)
    {
        listManager = GetComponent<ListManager>();
    }

    public void SetProperties(ListProperties listProperties)
    {
        selectionType = listProperties.selectionType;

        visible_only = listProperties.visible_only;
    }

    public void SetListSize(float new_size)
    {
        base_size = new_size;
    }

    public Vector2 GetListSize(List<int> id_list, bool exact)
    {
        return new Vector2(0, base_size * id_list.Count);
    }

    public void SetRows(List<int> id_list)
    {
        RectTransform element_prefab = Resources.Load<RectTransform>("Editor/Organizer/List/List_Prefab");

        for (int i = 0; i < id_list.Count; i++)
        {
            //if (ListPosition(i) > listMin.y)
            //    break;

            RectTransform new_element = listManager.SpawnElement(element_list, element_prefab);
            element_list_local.Add(new_element);

            SelectionElement selectionElement = new_element.GetComponent<SelectionElement>();
            selectionElement.InitializeSelection(listManager, i, selectionType);

            string header = listManager.table + " " + i;
            selectionElement.header.text = header;

            //Debugging
            new_element.name = header;

            //Review
            new_element.GetComponent<Button>().onClick.AddListener(delegate { SelectElement(selectionElement); });

            SetElement(new_element, selectionElement.id);
        }
    }

    public void ResetRows(List<int> filter)
    {
        CloseList();
        SetRows(filter);
    }

    void SetElement(RectTransform element, int id)
    {
        int index = listManager.id_list.IndexOf(id);

        element.anchorMax = new Vector2(1, 1);

        element.sizeDelta = new Vector2(element.sizeDelta.x, base_size);

        element.transform.localPosition = new Vector2(0, (listManager.list_parent.sizeDelta.y / 2) - (base_size * index) - (base_size * 0.5f));

        element.gameObject.SetActive(true);
    }

    public SelectionElement GetElement(int index)
    {
        return element_list_local[index].GetComponent<SelectionElement>();
    }

    float ListPosition(int i)
    {
        return listManager.list_parent.TransformPoint(new Vector2(0, (listManager.list_parent.sizeDelta.y / 2.222f) - (base_size * i))).y;
    }

    public void SelectElement(SelectionElement selection)
    {
        if(selectionType != Enums.SelectionType.None)
            selection.SelectElement();
    }

    public void ResetSelection()
    {
        element_selection.gameObject.SetActive(false);
    }

    public void CloseList()
    {
        if (element_selection != null)
            ResetSelection();

        listManager.ResetElement(element_list_local);

        DestroyImmediate(this);
    }
}
