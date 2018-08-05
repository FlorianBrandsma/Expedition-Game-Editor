using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class ListOrganizer : MonoBehaviour, IOrganizer
{
    static public List<SelectionElement> element_list = new List<SelectionElement>();
    private List<SelectionElement> element_list_local = new List<SelectionElement>();

    //private Enums.SelectionProperty selectionProperty;
    private Enums.SelectionType selectionType;

    private float base_size;

    //private bool visible_only;

    ListManager listManager;

    public void InitializeOrganizer()
    {
        listManager = GetComponent<ListManager>();
    }

    public void SetProperties(ListProperties listProperties)
    {
        //selectionProperty = listProperties.selectionProperty;
        selectionType = listProperties.selectionType;

        //visible_only = listProperties.visible_only;
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
        SelectionElement element_prefab = Resources.Load<SelectionElement>("Editor/Organizer/List/List_Prefab");

        for (int i = 0; i < id_list.Count; i++)
        {
            SelectionElement element = listManager.SpawnElement(element_list, element_prefab, i);
            element_list_local.Add(element);

            string header = listManager.listData.data.table + " " + i;
            element.header.text = header;

            //Debugging
            element.name = header;

            //Review
            //element.GetComponent<Button>().onClick.AddListener(delegate { SelectElement(element); });

            SetElement(element);
        }
    }

    public void ResetRows(List<int> filter)
    {
        CloseList();
        SetRows(filter);
    }

    void SetElement(SelectionElement element)
    {
        RectTransform rect = element.GetComponent<RectTransform>();

        int index = listManager.id_list.IndexOf(element.data.id);

        rect.anchorMax = new Vector2(1, 1);

        rect.sizeDelta = new Vector2(rect.sizeDelta.x, base_size);

        rect.transform.localPosition = new Vector2(0, (listManager.list_parent.sizeDelta.y / 2) - (base_size * index) - (base_size * 0.5f));

        rect.gameObject.SetActive(true);
    }

    public SelectionElement GetElement(int index)
    {
        return element_list_local[index];
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

    public void CloseList()
    {
        listManager.ResetElement(element_list_local);

        DestroyImmediate(this);
    }
}
