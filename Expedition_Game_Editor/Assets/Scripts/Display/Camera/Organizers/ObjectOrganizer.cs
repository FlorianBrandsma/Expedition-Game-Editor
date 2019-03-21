using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Collections;

public class ObjectOrganizer : MonoBehaviour, IOrganizer
{
    private CameraManager manager;
    private Route route;
    private ObjectProperties properties;

    //static public List<SelectionElement[]> object_list;
    //private SelectionElement local_object;

    public void InitializeOrganizer()
    {
        manager = GetComponent<CameraManager>();
        route = manager.properties.route;
    }

    public void SetProperties()
    {
        properties = manager.properties.GetComponent<ObjectProperties>();
    }

    public void GetData()
    {
        //Debug.Log(route.data.table + ":" + route.data.id);

        //Element/ItemData has an objectId
        //Required: "AssetData" (create when opening Section B)
        //(Change object when changing header icon)

        //manager.properties.dataList.GetData(route);
    }

    public void UpdateData()
    {
        SetData();
    }

    public void SetData()
    {
        //ItemController itemController = data_list.Cast<ItemController>().ToList().FirstOrDefault();

        //Debug.Log(itemController.itemData.id);

        //local_data_list = data_list;

        //SelectionElement element_prefab = Resources.Load<SelectionElement>("UI/Button");

        //for (int i = 0; i < local_data_list.Count; i++)
        //{
        //    SelectionElement element = listManager.SpawnElement(element_list, element_prefab, local_data_list[i]);
        //    element_list_local.Add(element);

        //    listManager.element_list.Add(element);

        //    string label = listManager.listProperties.dataList.data.table + " " + i;
        //    element.GetComponent<EditorButton>().label.text = label;

        //    //Debugging
        //    element.name = label;

        //    SetElement(element);
        //}
    }

    public void ResetData(ICollection filter)
    {
        CloseOrganizer();
        SetData();
    }

    public void CloseOrganizer()
    {
        //listManager.ResetElement(element_list_local);
        //local_element.gameObject.SetActive(false);

        DestroyImmediate(this);
    }
}
