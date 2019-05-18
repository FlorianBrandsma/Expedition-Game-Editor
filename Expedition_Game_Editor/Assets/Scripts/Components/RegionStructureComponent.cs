using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class RegionStructureComponent : MonoBehaviour, IComponent
{
    private Enums.RegionType type;

    public RegionDisplayManager defaultDisplay;

    private PathController pathController { get { return GetComponent<PathController>(); } }

    public EditorComponent component;
    private List<Data> dataList = new List<Data>();
    private List<Dropdown> dropdownList = new List<Dropdown>();

    public List<string> structureList;

    private Route route;
    private RegionDataElement regionDataElement;

    private Path activePath;

    public void InitializeComponent(Path path)
    {
        activePath = path;

        route = activePath.FindLastRoute("Region").Copy();
        regionDataElement = route.data.ElementData.Cast<RegionDataElement>().FirstOrDefault();

        type = (Enums.RegionType)regionDataElement.type;

        InitializeData();

        if (path.route.Count < (pathController.route.path.route.Count + 1))
        {
            //The region route gets added at the end when the component is initialized.
            //It tries to add another route every time it gets opened, causing the selection to appear.
            //This attempt gets blocked by using the max_length variable

            int index = (int)RegionDisplayManager.active_display;

            if (index > (pathController.controllers.Length - 1))
                index = (pathController.controllers.Length - 1);

            route.controller = index;

            path.Add(route);
        }     
    }

    private void InitializeData()
    {
        if (activePath.type != Path.Type.New) return;

        if(type == Enums.RegionType.Task)
            RegionDisplayManager.active_display = 0;

        dataList.Clear();

        InitializeStructureData();
        InitializeRegionData();

        //TEMPORARY
        if (regionDataElement.id == 0)
            regionDataElement.id = 1;
    }

    private void InitializeStructureData()
    {
        for (int i = 0; i < structureList.Count; i++)
        {
            Data data = activePath.FindLastRoute(structureList[i]).data;

            dataList.Add(data);
        }
    }

    private void InitializeRegionData()
    {
        Data data = activePath.FindFirstRoute("Region").data;

        if (type == Enums.RegionType.Task)
            ((RegionController)data.DataController).InitializeController();

        dataList.Add(data);
    }

    public void SetComponent(Path new_path)
    {
        for (int i = 0; i < dataList.Count; i++)
            SetStructureComponent(i);      
    }

    private void SetStructureComponent(int index)
    {
        Dropdown dropdown = ComponentManager.componentManager.AddDropdown(component);
        dropdownList.Add(dropdown);

        Data data = dataList[index];

        dropdown.gameObject.AddComponent(data.DataController.GetType());

        switch (data.DataController.DataType)
        {
            case Enums.DataType.Chapter:        SetChapterOptions(dropdown, data);      break;
            case Enums.DataType.Phase:          SetPhaseOptions(dropdown, data);        break;
            case Enums.DataType.Quest:          SetQuestOptions(dropdown, data);        break;
            case Enums.DataType.Objective:      SetObjectiveOptions(dropdown, data);    break;
            case Enums.DataType.TerrainElement: SetTerrainElementOptions(dropdown, data); break;
            case Enums.DataType.Task:           SetTaskOptions(dropdown, data);         break;
            case Enums.DataType.Region:         SetRegionOptions(dropdown, data);       break;
            default:                            Debug.Log("CASE MISSING");              break;
        }

        int selectedIndex = data.DataController.DataList.Cast<GeneralData>().ToList().FindIndex(x => x.id == data.ElementData.Cast<GeneralData>().FirstOrDefault().id);

        dropdown.value = selectedIndex;
        dropdown.captionText.text = dropdown.options[selectedIndex].text;

        dropdown.onValueChanged.AddListener(delegate {  InitializePath(pathController.route.path, 
                                                        new Data(   data.DataController, 
                                                        GetEnumerable(dropdown, data.DataController.DataList)),
                                                        index);
        });
    }

    #region Dropdown options

    private void SetChapterOptions(Dropdown dropdown, Data data)
    {
        List<ChapterDataElement> dataElements = data.DataController.DataList.Cast<ChapterDataElement>().ToList();

        foreach (ChapterDataElement dataElement in dataElements)
            dropdown.options.Add(new Dropdown.OptionData(dataElement.Name));
        
        //dropdown.GetComponent<ChapterController>().searchParameters.temp_id_count = ((ChapterController)data.DataController).DataList.Count;
    }

    private void SetPhaseOptions(Dropdown dropdown, Data data)
    {
        List<PhaseDataElement> dataElements = data.DataController.DataList.Cast<PhaseDataElement>().ToList();

        foreach (PhaseDataElement dataElement in dataElements)
            dropdown.options.Add(new Dropdown.OptionData(dataElement.Name));

        //dropdown.GetComponent<PhaseController>().searchParameters.temp_id_count = ((PhaseController)data.DataController).DataList.Count;
    }

    private void SetQuestOptions(Dropdown dropdown, Data data)
    {
        List<QuestDataElement> dataElements = data.DataController.DataList.Cast<QuestDataElement>().ToList();

        foreach (QuestDataElement dataElement in dataElements)
            dropdown.options.Add(new Dropdown.OptionData(dataElement.Name));

        //dropdown.GetComponent<QuestController>().searchParameters.temp_id_count = ((QuestController)data.DataController).searchParameters.temp_id_count;
    }

    private void SetObjectiveOptions(Dropdown dropdown, Data data)
    {
        List<ObjectiveDataElement> dataElements = data.DataController.DataList.Cast<ObjectiveDataElement>().ToList();

        foreach (ObjectiveDataElement dataElement in dataElements)
            dropdown.options.Add(new Dropdown.OptionData(dataElement.Name));

        //dropdown.GetComponent<ObjectiveController>().searchParameters.temp_id_count = ((ObjectiveController)data.DataController).searchParameters.temp_id_count;
    }

    private void SetTerrainElementOptions(Dropdown dropdown, Data data)
    {
        List<TerrainElementDataElement> dataElements = data.DataController.DataList.Cast<TerrainElementDataElement>().ToList();

        foreach (TerrainElementDataElement dataElement in dataElements)
            dropdown.options.Add(new Dropdown.OptionData(dataElement.name));

        //dropdown.GetComponent<TerrainElementController>().searchParameters.temp_id_count = ((TerrainElementController)data.DataController).searchParameters.temp_id_count;
    }

    private void SetTaskOptions(Dropdown dropdown, Data data)
    {
        List<TaskDataElement> dataElements = data.DataController.DataList.Cast<TaskDataElement>().ToList();

        foreach (TaskDataElement dataElement in dataElements)
            dropdown.options.Add(new Dropdown.OptionData(dataElement.Description));

        //dropdown.GetComponent<TaskController>().searchParameters.temp_id_count = ((TaskController)data.DataController).searchParameters.temp_id_count;
    }

    private void SetRegionOptions(Dropdown dropdown, Data data)
    {
        List<RegionDataElement> dataElements = data.DataController.DataList.Cast<RegionDataElement>().ToList();

        foreach (RegionDataElement dataElement in dataElements)
            dropdown.options.Add(new Dropdown.OptionData(dataElement.Name));

        //dropdown.GetComponent<RegionController>().searchParameters.temp_id_count = ((RegionController)data.DataController).searchParameters.temp_id_count;
    }

    #endregion

    private void InitializePath(Path path, Data data, int index)
    {
        path.ReplaceAllRoutes(data);

        dataList[index] = data;
        
        SetStructureData(path, data, index + 1);

        EditorManager.editorManager.InitializePath(path);
    }

    private void SetStructureData(Path path, Data data, int index)
    {
        for (int i = index; i < dataList.Count; i++)
        {
            Data structure_data = new Data(dropdownList[i].GetComponent<IDataController>(), dataList[i].ElementData);

            dataList[i] = structure_data;

            structure_data.DataController.InitializeController();

            //(TASK belongs to an ELEMENT which is placed on a TERRAIN which belongs to a REGION and a PHASE)
            //TASK: load ELEMENT > TERRAIN > REGION

            structure_data.ElementData = GetEnumerable(0, dataList[i].DataController.DataList);

            path.ReplaceAllRoutes(structure_data);
        }
    }

    public IEnumerable GetEnumerable(int index, ICollection list)
    {
        int i = 0;

        foreach(var data in list)
        {
            if (i == index)
                return new[] { data };

            i++;
        }

        return null;
    }

    public IEnumerable GetEnumerable(Dropdown dropdown, ICollection list)
    {
        int i = 0;

        foreach (var data in list)
        {
            if (i == dropdown.value)
                return new[] { data };

            i++;
        }

        return null;
    }

    public void CloseComponent()
    {
        dropdownList.Clear();
    }
}
