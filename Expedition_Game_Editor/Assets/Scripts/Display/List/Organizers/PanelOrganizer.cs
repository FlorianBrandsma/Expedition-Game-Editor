using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PanelOrganizer : MonoBehaviour, IOrganizer, IList
{
    private ListManager listManager { get { return GetComponent<ListManager>(); } }

    static public List<SelectionElement> element_list = new List<SelectionElement>();

    public Vector2 elementSize { get; set; }
    private List<float> row_height      = new List<float>(); //Individual heights
    private List<float> row_offset_max  = new List<float>(); //Combined heights

    private PanelProperties properties;

    private IDataController dataController;
    private List<GeneralData> generalData_list;

    public void InitializeOrganizer()
    {
        dataController = listManager.listProperties.segmentController.dataController;
    }

    public void SetProperties()
    {
        properties = listManager.listProperties.GetComponent<PanelProperties>();
    }

    public void SetElementSize()
    {
        elementSize = new Vector2( listManager.listProperties.element_size.x,
                                    properties.constant_height ?    listManager.listProperties.element_size.y : 
                                                                    listManager.listProperties.element_size.y / properties.reference_area.anchorMax.x);

        SetList();
    }

    public Vector2 GetListSize(int element_count, bool exact)
    {
        if (exact)
            return new Vector2(0, row_height.Sum());
        else
            return new Vector2(0, element_count);
    }

    public void SetList()
    {
        float position_sum = 0;

        for (int i = 0; i < listManager.listProperties.segmentController.dataController.dataList.Count; i++)
        {
            row_height.Add(elementSize.y);

            position_sum += elementSize.y;
            row_offset_max.Add(position_sum - elementSize.y);
        }
    }

    public void SetData()
    {
        SetData(dataController.dataList);
    }

    public void SetData(ICollection list)
    {
        generalData_list = list.Cast<GeneralData>().ToList();

        SelectionElement element_prefab = Resources.Load<SelectionElement>("UI/Panel");

        foreach (var data in list)
        {
            SelectionElement element = listManager.SpawnElement(element_list, element_prefab);
            listManager.elementList.Add(element);

            element.route.data = new Data(dataController, new[] { data });

            //Debugging
            GeneralData generalData = (GeneralData)data;
            element.name = generalData.table + generalData.id;
            //

            SetElement(element);
        }
    }

    public void UpdateData()
    {
        ResetData(dataController.dataList);

        SelectionManager.ResetSelection(listManager);
    }

    public void ResetData(ICollection filter)
    {
        CloseList();
        SetData(filter);
    }

    void SetElement(SelectionElement element)
    {
        element.SetElement();

        RectTransform rect = element.GetComponent<RectTransform>();

        int index = generalData_list.FindIndex(x => x.id == element.GeneralData().id);

        rect.offsetMin = new Vector2(rect.offsetMin.x, listManager.listParent.sizeDelta.y - (row_offset_max[index] + row_height[index]));
        rect.offsetMax = new Vector2(rect.offsetMax.x, -row_offset_max[index]);

        rect.gameObject.SetActive(true);     
    }

    public SelectionElement GetElement(int index)
    {
        return listManager.elementList[index];
    }

    float ListPosition(int i)
    {
        return listManager.listParent.TransformPoint(new Vector2(0, (listManager.listParent.sizeDelta.y / 2.222f) - row_offset_max[i])).y;
    }

    public void CloseList()
    {
        listManager.ResetElement();
    }

    public void CloseOrganizer()
    {
        CloseList();

        DestroyImmediate(this);
    }
}
