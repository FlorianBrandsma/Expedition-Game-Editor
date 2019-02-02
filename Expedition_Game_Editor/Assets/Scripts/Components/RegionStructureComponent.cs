using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class RegionStructureComponent : MonoBehaviour, IComponent
{
    private RegionManager.Type type;

    public RegionDisplayManager default_display;

    private EditorController controller;

    public EditorComponent component;
    private List<DataList> structure_dataList = new List<DataList>();

    public List<ElementData> structure_list;

    private Route region;
    private Route structure;

    private Path active_path;

    public void InitializeComponent(Path new_path)
    {
        controller = GetComponent<EditorController>();

        active_path = new_path;

        region = active_path.FindLastRoute("Region").Copy();
        type = (RegionManager.Type)region.data.type;

        InitializeData();

        if (new_path.route.Count < (controller.path.route.Count + 1))
        {
            //The region route gets added at the end when the component is initialized.
            //It tries to add another route every time it gets opened, causing the selection to appear.
            //This attempt gets blocked by using the max_length variable

            int index = (int)RegionDisplayManager.active_display;

            if (index > (controller.controllers.Length - 1))
                index = (controller.controllers.Length - 1);

            region.controller = index;

            new_path.Add(region);
        }     
    }

    private void InitializeData()
    {
        if (active_path.type != Path.Type.New) return;

        if(type == RegionManager.Type.Task)
            RegionDisplayManager.active_display = 0;

        structure_dataList.Clear();

        InitializeStructureData();
        InitializeRegionData();

        if (region.data.id == 0)
            region.data.id = 1;
    }

    private void InitializeStructureData()
    {
        Route previous_route = null;
        int target_id = 0;

        for (int i = 0; i < structure_list.Count; i++)
        {
            Route structure_route = active_path.FindLastRoute(structure_list[i].table);
            target_id = structure_route.data.id;

            string sql = "";

            sql += "SELECT * FROM " + structure_route.data.table;

            if (previous_route != null)
                sql += " WHERE " + previous_route.data.table + "Id = " + target_id;

            DataList dataList = GetData(sql, structure_route.data);

            structure_dataList.Add(dataList);

            previous_route = structure_route;
        }
    }

    private void InitializeRegionData()
    {
        Route structure_route = active_path.FindLastRoute("Region");
        int target_id = structure_route.data.id;

        string sql = "";

        sql += "SELECT * FROM Region ";

        if(type != RegionManager.Type.Base)
            sql += "WHERE Phase" + "Id = " + target_id;

        DataList dataList = GetData(sql, structure_route.data);

        structure_dataList.Add(dataList);        
    }

    public void SetComponent(Path new_path)
    {
        for (int i = 0; i < structure_dataList.Count; i++)
        {
            SetStructureComponent(i);
        }
    }

    private void SetStructureComponent(int index)
    {
        DataList dataList = structure_dataList[index];
        
        Dropdown dropdown = ComponentManager.componentManager.AddDropdown(component);

        dropdown.options.Clear();
        dropdown.onValueChanged.RemoveAllListeners();
        
        for (int i = 0; i < dataList.list.Count; i++)
        {
            dropdown.options.Add(new Dropdown.OptionData(dataList.data.table + " " + i));
        }

        List<ElementData> new_list = structure_list.ToList();
        new_list.Add(region.data);

        Route structure_route = active_path.FindLastRoute(new_list[index].table);

        int selected_index = dataList.list.FindIndex(x => x.id == structure_route.data.id);

        dropdown.captionText.text = dataList.list[selected_index].table + " " + selected_index;
        dropdown.value = selected_index;

        Path path = controller.path;
        dropdown.onValueChanged.AddListener(delegate { OpenPath(path, dataList.list[dropdown.value], (index + 1)); });
    }

    private DataList GetData(string sql, ElementData data)
    {
        DataList dataList = new DataList();

        dataList.data = data; //placeholder. Will be made using sql (FROM)
        dataList.id_count = 15; //Placeholder
        dataList.GetData(sql);

        return dataList;
    }

    private void OpenPath(Path path, ElementData data, int index)
    {
        path.ReplaceAllRoutes(data);

        SetStructureData(path, data, index);

        EditorManager.editorManager.OpenPath(path);
    }

    private void SetStructureData(Path path, ElementData data, int index)
    {
        for (int i = index; i < structure_dataList.Count; i++)
        {
            ElementData structure_data = structure_dataList[i].data;

            string sql = "";

            if(structure_data.table == "Region")
            {
                sql += "SELECT * FROM Region WHERE Phase" + "Id = " + data.id;
            } else {
                sql += "SELECT * FROM " + structure_data.table + " WHERE " + data.table + "Id = " + data.id;
            }

            structure_dataList[i] = GetData(sql, structure_data);

            //If type = task and table = region, data comes from the element, which could be placed on a region
            data = structure_dataList[i].list[0];

            path.ReplaceAllRoutes(data);
        }
    }

    public void CloseComponent() { }
}
