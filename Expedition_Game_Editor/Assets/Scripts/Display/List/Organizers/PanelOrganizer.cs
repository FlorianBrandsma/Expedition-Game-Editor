using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PanelOrganizer : MonoBehaviour, IOrganizer, IList
{
    private ListManager listManager { get { return GetComponent<ListManager>(); } }

    static public List<SelectionElement> element_list = new List<SelectionElement>();

    public Vector2 element_size { get; set; }
    private List<float> row_height      = new List<float>(); //Individual heights
    private List<float> row_offset_max  = new List<float>(); //Combined heights

    private PanelProperties properties;

    List<GeneralData> generalData_list;

    public void InitializeOrganizer() { }

    public void SetProperties()
    {
        properties = listManager.listProperties.GetComponent<PanelProperties>();
    }

    public void SetElementSize()
    {
        element_size = new Vector2( listManager.listProperties.element_size.x,
                                    properties.constant_height ?    listManager.listProperties.element_size.y : 
                                                                    listManager.listProperties.element_size.y / properties.reference_area.anchorMax.x);

        SetList();
    }

    public void SetList()
    {
        float position_sum = 0;

        for (int i = 0; i < listManager.listProperties.dataController.data_list.Count; i++)
        {
            row_height.Add(element_size.y);

            position_sum += element_size.y;
            row_offset_max.Add(position_sum - element_size.y);
        }
    }

    public Vector2 GetListSize(int element_count, bool exact)
    {
        if (exact)
            return new Vector2(0, row_height.Sum());
        else
            return new Vector2(0, element_count);
    }

    public void UpdateData()
    {
        ResetData(null);

        SelectionManager.ResetSelection(listManager);
    }

    public void SetData()
    {
        var dataController = listManager.listProperties.dataController;
        generalData_list = dataController.data_list.Cast<GeneralData>().ToList();

        SelectionElement element_prefab = Resources.Load<SelectionElement>("UI/Panel");

        foreach (var data in dataController.data_list)
        {
            SelectionElement element = listManager.SpawnElement(element_list, element_prefab);
            listManager.element_list.Add(element);

            element.SetElementData(new[] { data }, dataController.data_type);

            //Debugging
            GeneralData generalData = (GeneralData)data;
            element.name = generalData.table + generalData.id;
            //

            SetElement(element);
        }
    }

    public void ResetData(ICollection filter)
    {
        CloseList();
        SetData();
    }

    public void CloseData()
    {
        listManager.element_list.Clear();
    }

    void SetElement(SelectionElement element)
    {
        element.SetElement();

        RectTransform rect = element.GetComponent<RectTransform>();

        int index = generalData_list.FindIndex(x => x.id == element.GeneralData().id);

        rect.offsetMin = new Vector2(rect.offsetMin.x, listManager.list_parent.sizeDelta.y - (row_offset_max[index] + row_height[index]));
        rect.offsetMax = new Vector2(rect.offsetMax.x, -row_offset_max[index]);

        rect.gameObject.SetActive(true);     
    }

    public SelectionElement GetElement(int index)
    {
        return listManager.element_list[index];
    }

    float ListPosition(int i)
    {
        return listManager.list_parent.TransformPoint(new Vector2(0, (listManager.list_parent.sizeDelta.y / 2.222f) - row_offset_max[i])).y;
    }

    public void CloseList()
    {
        listManager.ResetElement(listManager.element_list);

        listManager.element_list.Clear();
    }

    public void CloseOrganizer()
    {
        CloseList();

        DestroyImmediate(this);
    }
}
