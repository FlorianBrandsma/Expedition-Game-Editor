using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class ButtonOrganizer : MonoBehaviour, IOrganizer
{
    //private ButtonProperties properties;

    private List<ElementData> local_data_list;

    static public List<SelectionElement> element_list = new List<SelectionElement>();
    private List<SelectionElement> element_list_local = new List<SelectionElement>();

    //private Enums.SelectionProperty selectionProperty;
    //private SelectionManager.Type selectionType;

    public Vector2 element_size { get; set; }

    //private bool visible_only;

    ListManager listManager;

    public void InitializeOrganizer()
    {
        listManager = GetComponent<ListManager>();
    }

    public void SetProperties(ListProperties listProperties)
    {
        //properties = listProperties.GetComponent<ButtonProperties>();
        //selectionProperty = listProperties.selectionProperty;
        //selectionType = listProperties.selectionType;

        //visible_only = listProperties.visible_only;
    }

    public void SetElementSize()
    {
        element_size = listManager.listData.listProperties.element_size;
    }

    public Vector2 GetListSize(List<ElementData> data_list, bool exact)
    {
        return new Vector2(0, element_size.y * data_list.Count);
    }

    public void SetRows(List<ElementData> data_list)
    {
        local_data_list = data_list;

        SelectionElement element_prefab = Resources.Load<SelectionElement>("UI/Button");

        for (int i = 0; i < local_data_list.Count; i++)
        {
            SelectionElement element = listManager.SpawnElement(element_list, element_prefab, local_data_list[i]);
            element_list_local.Add(element);

            listManager.element_list.Add(element);

            string label = listManager.listData.data.table + " " + i;
            element.GetComponent<EditorButton>().label.text = label;

            //Debugging
            element.name = label;

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

        int index = local_data_list.IndexOf(element.data);

        rect.anchorMax = new Vector2(1, 1);

        rect.sizeDelta = new Vector2(rect.sizeDelta.x, element_size.y);

        rect.transform.localPosition = new Vector2(0, (listManager.list_parent.sizeDelta.y / 2) - (element_size.y * index) - (element_size.y * 0.5f));

        rect.gameObject.SetActive(true);
    }

    public SelectionElement GetElement(int index)
    {
        return element_list_local[index];
    }

    float ListPosition(int i)
    {
        return listManager.list_parent.TransformPoint(new Vector2(0, (listManager.list_parent.sizeDelta.y / 2.222f) - (element_size.y * i))).y;
    }

    public void CloseList()
    {
        listManager.ResetElement(element_list_local);

        DestroyImmediate(this);
    }
}
