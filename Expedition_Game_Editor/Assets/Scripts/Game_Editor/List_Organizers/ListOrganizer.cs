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

    private Path edit_path;

    private float base_size;

    private bool visible_only;

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
    }

    public void SetListSize(float new_size)
    {
        base_size = new_size;
    }

    public Vector2 GetListSize(bool exact)
    {
        return new Vector2(0, base_size * listManager.id_list.Count);
    }

    public void SetRows()
    {
        RectTransform element_prefab = Resources.Load<RectTransform>("Editor/Organizer/List/List_Prefab");

        for (int i = 0; i < listManager.id_list.Count; i++)
        {
            //if (ListPosition(i) > listMin.y)
            //    break;

            RectTransform new_element = listManager.SpawnElement(element_list, element_prefab);

            element_list_local.Add(new_element);
          
            SetElement(new_element, i);

            string header = listManager.table + " " + i;

            new_element.name = header;

            ListElement list_element = new_element.GetComponent<ListElement>();
            list_element.header.text = header;

            //OpenEditor
            int index = i;
            
            //Review
            new_element.GetComponent<Button>().onClick.AddListener(delegate { listManager.SelectElement(listManager.id_list[index], listManager.editable); });
        }
    }

    void SetElement(RectTransform element, int index)
    {
        element.anchorMax = new Vector2(1, 1);

        element.sizeDelta = new Vector2(element.sizeDelta.x, base_size);

        element.transform.localPosition = new Vector2(0, (listManager.list_parent.sizeDelta.y / 2) - (base_size * index) - (base_size * 0.5f));

        element.gameObject.SetActive(true);
    }

    float ListPosition(int i)
    {
        return listManager.list_parent.TransformPoint(new Vector2(0, (listManager.list_parent.sizeDelta.y / 2.222f) - (base_size * i))).y;
    }

    public void SelectElement(int id)
    {
        if (element_selection == null)
        {
            element_selection = listManager.SpawnElement(selection_list, Resources.Load<RectTransform>("Editor/Organizer/List/List_Selection"));
            element_selection.SetAsFirstSibling();
        }
            

        SetElement(element_selection, id - 1);
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
