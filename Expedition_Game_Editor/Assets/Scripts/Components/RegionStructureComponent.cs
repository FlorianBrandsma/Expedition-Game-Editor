using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class RegionStructureComponent : MonoBehaviour, IComponent
{
    private Enums.RegionType type;

    public RegionDisplayManager default_display;

    private PathController pathController { get { return GetComponent<PathController>(); } }

    public EditorComponent component;
    private List<Data> data_list = new List<Data>();
    private List<Dropdown> dropdown_list = new List<Dropdown>();

    public List<string> structure_list;

    private Route region;

    private Path active_path;

    public void InitializeComponent(Path path)
    {
        active_path = path;

        region = active_path.FindLastRoute("Region").Copy();
        type = (Enums.RegionType)region.GeneralData().type;

        InitializeData();

        if (path.route.Count < (pathController.route.path.route.Count + 1))
        {
            //The region route gets added at the end when the component is initialized.
            //It tries to add another route every time it gets opened, causing the selection to appear.
            //This attempt gets blocked by using the max_length variable

            int index = (int)RegionDisplayManager.active_display;

            if (index > (pathController.controllers.Length - 1))
                index = (pathController.controllers.Length - 1);

            region.controller = index;

            path.Add(region);
        }     
    }

    private void InitializeData()
    {
        if (active_path.type != Path.Type.New) return;

        if(type == Enums.RegionType.Task)
            RegionDisplayManager.active_display = 0;

        data_list.Clear();

        InitializeStructureData();
        InitializeRegionData();

        //TEMPORARY
        if (region.GeneralData().id == 0)
            region.GeneralData().id = 1;
    }

    private void InitializeStructureData()
    {
        for (int i = 0; i < structure_list.Count; i++)
        {
            Data data = active_path.FindLastRoute(structure_list[i]).data;

            data_list.Add(data);
        }
    }

    private void InitializeRegionData()
    {
        Data data = active_path.FindFirstRoute("Region").data;

        if (type == Enums.RegionType.Task)
            ((RegionController)data.controller).InitializeController();

        data_list.Add(data);
    }

    public void SetComponent(Path new_path)
    {
        for (int i = 0; i < data_list.Count; i++)
            SetStructureComponent(i);      
    }

    private void SetStructureComponent(int index)
    {
        Dropdown dropdown = ComponentManager.componentManager.AddDropdown(component);
        dropdown_list.Add(dropdown);

        Data data = data_list[index];

        dropdown.gameObject.AddComponent(data.controller.GetType());

        switch (data.controller.DataType)
        {
            case Enums.DataType.Chapter:        SetChapterOptions(dropdown, data);      break;
            case Enums.DataType.Phase:          SetPhaseOptions(dropdown, data);        break;
            case Enums.DataType.Quest:          SetQuestOptions(dropdown, data);        break;
            case Enums.DataType.Step:           SetStepOptions(dropdown, data);         break;
            case Enums.DataType.StepElement:    SetStepElementOptions(dropdown, data);  break;
            case Enums.DataType.Task:           SetTaskOptions(dropdown, data);         break;
            case Enums.DataType.Region:         SetRegionOptions(dropdown, data);       break;
            default: Debug.Log("YOU ARE MISSING A CASE");                               break;
        }

        int selected_index = data.controller.DataList.Cast<GeneralData>().ToList().FindIndex(x => x.id == data.element.Cast<GeneralData>().FirstOrDefault().id);

        dropdown.value = selected_index;
        dropdown.captionText.text = dropdown.options[selected_index].text;

        dropdown.onValueChanged.AddListener(delegate { InitializePath(pathController.route.path, 
            new Data(   data.controller, 
                        GetEnumerable(dropdown, data.controller.DataList)),                     
            index);
        });
    }

    #region Dropdown options

    private void SetChapterOptions(Dropdown dropdown, Data data)
    {
        List<ChapterDataElement> dataElements = data.controller.DataList.Cast<ChapterDataElement>().ToList();

        foreach (ChapterDataElement dataElement in dataElements)
            dropdown.options.Add(new Dropdown.OptionData(dataElement.Name));

        dropdown.GetComponent<ChapterController>().temp_id_count = ((ChapterController)data.controller).temp_id_count;
    }

    private void SetPhaseOptions(Dropdown dropdown, Data data)
    {
        List<PhaseDataElement> dataElements = data.controller.DataList.Cast<PhaseDataElement>().ToList();

        foreach (PhaseDataElement dataElement in dataElements)
            dropdown.options.Add(new Dropdown.OptionData(dataElement.name));

        dropdown.GetComponent<PhaseController>().temp_id_count = ((PhaseController)data.controller).temp_id_count;
    }

    private void SetQuestOptions(Dropdown dropdown, Data data)
    {
        List<QuestDataElement> dataElements = data.controller.DataList.Cast<QuestDataElement>().ToList();

        foreach (QuestDataElement dataElement in dataElements)
            dropdown.options.Add(new Dropdown.OptionData(dataElement.name));

        dropdown.GetComponent<QuestController>().temp_id_count = ((QuestController)data.controller).temp_id_count;
    }

    private void SetStepOptions(Dropdown dropdown, Data data)
    {
        List<StepDataElement> dataElements = data.controller.DataList.Cast<StepDataElement>().ToList();

        foreach (StepDataElement dataElement in dataElements)
            dropdown.options.Add(new Dropdown.OptionData(dataElement.name));

        dropdown.GetComponent<StepController>().temp_id_count = ((StepController)data.controller).temp_id_count;
    }

    private void SetStepElementOptions(Dropdown dropdown, Data data)
    {
        List<StepElementDataElement> dataElements = data.controller.DataList.Cast<StepElementDataElement>().ToList();

        foreach (StepElementDataElement dataElement in dataElements)
            dropdown.options.Add(new Dropdown.OptionData(dataElement.name));

        dropdown.GetComponent<StepElementController>().temp_id_count = ((StepElementController)data.controller).temp_id_count;
    }

    private void SetTaskOptions(Dropdown dropdown, Data data)
    {
        List<TaskDataElement> dataElements = data.controller.DataList.Cast<TaskDataElement>().ToList();

        foreach (TaskDataElement dataElement in dataElements)
            dropdown.options.Add(new Dropdown.OptionData(dataElement.description));

        dropdown.GetComponent<TaskController>().temp_id_count = ((TaskController)data.controller).temp_id_count;
    }

    private void SetRegionOptions(Dropdown dropdown, Data data)
    {
        List<RegionDataElement> dataElements = data.controller.DataList.Cast<RegionDataElement>().ToList();

        foreach (RegionDataElement dataElement in dataElements)
            dropdown.options.Add(new Dropdown.OptionData(dataElement.name));

        dropdown.GetComponent<RegionController>().temp_id_count = ((RegionController)data.controller).temp_id_count;
    }

    #endregion

    private void InitializePath(Path path, Data data, int index)
    {
        path.ReplaceAllRoutes(data);

        data_list[index] = data;
        
        SetStructureData(path, data, index + 1);

        EditorManager.editorManager.InitializePath(path);
    }

    private void SetStructureData(Path path, Data data, int index)
    {
        for (int i = index; i < data_list.Count; i++)
        {
            Data structure_data = new Data(dropdown_list[i].GetComponent<IDataController>(), data_list[i].element);

            data_list[i] = structure_data;

            structure_data.controller.InitializeController();

            //(TASK belongs to an ELEMENT which is placed on a TERRAIN which belongs to a REGION and a PHASE)
            //TASK: load ELEMENT > TERRAIN > REGION

            structure_data.element = GetEnumerable(0, data_list[i].controller.DataList);

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
        dropdown_list.Clear();
    }
}
