using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class RegionStructureComponent : MonoBehaviour, IComponent
{
    private RegionManager.Type type;

    private EditorController controller;

    public EditorComponent component;
    private List<ListData> structure_dataList = new List<ListData>();

    //Structure components
    public List<ElementData> structure_data;

    private Route region;
    private Route structure;

    private Path active_path;

    private int max_route_length;

    public void InitializeComponent(Path new_path)
    {
        controller = GetComponent<EditorController>();

        active_path = new_path;

        max_route_length = controller.path.route.Count + 1;

        if (new_path.route.Count < max_route_length)
        {
            //The region route gets added at the end when the component is initialized.
            //It tries to add another route every time it gets opened, causing the selection to appear.
            //This attempt gets blocked by using the max_length variable

            Route new_region = new Route(0, new_path.GetLastRoute().data, new_path.GetLastRoute().origin);
            region = new_region;

            type = (RegionManager.Type)region.data.type;

            new_path.Add(region);

            if (!new_path.loaded)
            {
                //Path gets checked as loaded when it is reset
                //Initialization should only happen once when the sub editor is opened manually
                
                InitializeStructure();
            }
        }

        if (new_path.route.Count == max_route_length || controller.loaded)
            region = new_path.FindLastRoute("Region");

        controller.loaded = true;
    }

    private void InitializeStructure()
    {
        structure_dataList.Clear();

        if(structure_data.Count > 0)
        {
            switch (type)
            {
                case RegionManager.Type.Phase:
                    structure = active_path.FindLastRoute("Phase");
                    break;
                case RegionManager.Type.Task:
                    structure = active_path.FindLastRoute("Task");
                    break;
            }

            //Debug.Log(structure.data.table + ":" + structure.data.id);
        }
        
        InitializeStructureComponent();
        InitializeRegionComponent();
    }

    public void SetComponent(Path new_path)
    {
        SetStructure();
        SetRegionComponent();
    }

    private void SetStructure()
    {
        foreach (ElementData data in structure_data)
            SetStructureComponent(data);
    }

    private void InitializeStructureComponent()
    {

    }

    private void InitializeRegionComponent()
    {

    }

    private void SetStructureComponent(ElementData data)
    {
        //GetData
        ListData dataList = GetData(data);
        structure_dataList.Add(dataList);

        Dropdown dropdown = ComponentManager.componentManager.AddDropdown(component);

        dropdown.options.Clear();
        dropdown.onValueChanged.RemoveAllListeners();
        
        for (int i = 0; i < dataList.list.Count; i++)
        {
            dropdown.options.Add(new Dropdown.OptionData(dataList.data.table + " " + i));
        }

        int selected_index = 0;

        dropdown.captionText.text = dataList.list[selected_index].table + " " + selected_index;
        dropdown.value = selected_index;

        //Path path = controller.path;
        //dropdown.onValueChanged.AddListener(delegate { OpenPath(path, dataList.list[dropdown.value]); });
    }

    private void SetRegionComponent()
    {
        ListData dataList = GetData(region.data);

        Dropdown dropdown = ComponentManager.componentManager.AddDropdown(component);

        dropdown.options.Clear();
        dropdown.onValueChanged.RemoveAllListeners();
        
        for (int i = 0; i < dataList.list.Count; i++)
        {
            dropdown.options.Add(new Dropdown.OptionData(dataList.data.table + " " + i));
        }

        int selected_index = dataList.list.FindIndex(x => x.id == region.data.id);

        dropdown.captionText.text = dataList.list[selected_index].table + " " + selected_index;
        dropdown.value = selected_index;

        Path path = controller.path;
        dropdown.onValueChanged.AddListener(delegate { OpenPath(path, dataList.list[dropdown.value]); });
    }

    private ListData GetData(ElementData data)
    {
        ListData dataList = new ListData();

        if (region.data.id == 0)
            region.data.id = 1;

        dataList.data = data;
        dataList.id_count = 15; //Placeholder
        dataList.GetData("sql");

        return dataList;
    }

    private void OpenPath(Path path, ElementData data)
    {
        EditorManager.editorManager.OpenPath(PathManager.ReloadPath(path, data));
    }

    public void CloseComponent() { }
}
