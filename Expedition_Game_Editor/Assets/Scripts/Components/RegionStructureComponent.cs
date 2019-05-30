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

    private Path activePath;

    public void InitializeComponent(Path path)
    {
        activePath = path;

        route = activePath.FindLastRoute("Region").Copy();

        var regionDataElement = (RegionDataElement)route.data.DataElement;
        type = (Enums.RegionType)regionDataElement.type;

        InitializeData();

        if (path.route.Count < (pathController.route.path.route.Count + 1))
        {
            //The region route gets added at the end when the component is initialized.
            //It tries to add another route every time it gets opened, causing the selection to appear.
            //This attempt gets blocked by using the max_length variable

            int index = (int)RegionDisplayManager.activeDisplay;

            if (index > (pathController.controllers.Length - 1))
                index = (pathController.controllers.Length - 1);

            route.controller = index;

            path.Add(route);
        }     
    }

    private void InitializeData()
    {
        if (activePath.type != Path.Type.New) return;

        if (type == Enums.RegionType.Task)
            RegionDisplayManager.activeDisplay = 0;

        dataList.Clear();

        InitializeStructureData();
        InitializeRegionData();
    }

    private void InitializeStructureData()
    {
        foreach(string structure in structureList)
        {
            Data data = activePath.FindLastRoute(structure).data;

            dataList.Add(data);
        }
    }

    private void InitializeRegionData()
    {
        Data data = activePath.FindFirstRoute("Region").data;

        dataList.Add(data);
    }

    public void SetComponent(Path path)
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
        dropdown.GetComponent<IDataController>().InitializeController();

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

        int selectedIndex = data.DataController.DataList.Cast<GeneralData>().ToList().FindIndex(x => x.id == ((GeneralData)data.DataElement).id);

        dropdown.value = selectedIndex;
        dropdown.captionText.text = dropdown.options[selectedIndex].text;

        dropdown.onValueChanged.AddListener(delegate {  InitializePath( pathController.route.path, 
                                                                        new Data(data.DataController,  data.DataController.DataList[dropdown.value]),
                                                                        index);
        });
    }

    #region Dropdown options

    private void SetChapterOptions(Dropdown dropdown, Data data)
    {
        var dataElements = data.DataController.DataList.Cast<ChapterDataElement>().ToList();

        dataElements.ForEach(x => dropdown.options.Add(new Dropdown.OptionData(x.Name)));
    }

    private void SetPhaseOptions(Dropdown dropdown, Data data)
    {
        var dataElements = data.DataController.DataList.Cast<PhaseDataElement>().ToList();

        dataElements.ForEach(x => dropdown.options.Add(new Dropdown.OptionData(x.Name)));
    }

    private void SetQuestOptions(Dropdown dropdown, Data data)
    {
        var dataElements = data.DataController.DataList.Cast<QuestDataElement>().ToList();

        dataElements.ForEach(x => dropdown.options.Add(new Dropdown.OptionData(x.Name)));
    }

    private void SetObjectiveOptions(Dropdown dropdown, Data data)
    {
        var dataElements = data.DataController.DataList.Cast<ObjectiveDataElement>().ToList();

        dataElements.ForEach(x => dropdown.options.Add(new Dropdown.OptionData(x.Name)));
    }

    private void SetTerrainElementOptions(Dropdown dropdown, Data data)
    {
        var dataElements = data.DataController.DataList.Cast<TerrainElementDataElement>().ToList();

        dataElements.ForEach(x => dropdown.options.Add(new Dropdown.OptionData(x.elementName)));
    }

    private void SetTaskOptions(Dropdown dropdown, Data data)
    {
        var dataElements = data.DataController.DataList.Cast<TaskDataElement>().ToList();

        dataElements.ForEach(x => dropdown.options.Add(new Dropdown.OptionData(x.Description)));
    }

    private void SetRegionOptions(Dropdown dropdown, Data data)
    {
        var dataElements = data.DataController.DataList.Cast<RegionDataElement>().ToList();

        dataElements.ForEach(x => dropdown.options.Add(new Dropdown.OptionData(x.Name)));
    }

    #endregion

    private void InitializePath(Path path, Data data, int index)
    {
        path.ReplaceAllRoutes(data);

        dataList[index] = data;
        
        SetStructureData(path, index + 1);

        EditorManager.editorManager.InitializePath(path);
    }

    private void SetStructureData(Path path, int index)
    {
        for (int i = index; i < dataList.Count; i++)
        {
            var data = dataList[i];

            switch (data.DataController.DataType)
            {
                case Enums.DataType.Chapter:        GetChapterData(i);          break;
                case Enums.DataType.Phase:          GetPhaseData(i);            break;
                case Enums.DataType.Quest:          GetQuestData(i);            break;
                case Enums.DataType.Objective:      GetObjectiveData(i);        break;
                case Enums.DataType.TerrainElement: GetTerrainElementData(i);   break;
                case Enums.DataType.Task:           GetTaskData(i);             break;
                case Enums.DataType.Region:         GetRegionData(i);           break;

                default: Debug.Log("CASE MISSING"); break;
            }

            var dataController = dropdownList[i].GetComponent<IDataController>();

            dataList[i] = new Data(dataController, dataController.DataList.FirstOrDefault());

            //(TASK belongs to an TERRAINELEMENT which is placed on a TERRAIN which belongs to a REGION and a PHASE)
            //TASK: load TERRAINELEMENT > TERRAIN > REGION

            path.ReplaceAllRoutes(dataList[i]);
        }
    }

    private void GetChapterData(int index)
    {
        var dataController = dropdownList[index].GetComponent<IDataController>();

        var searchParameters = new Search.Chapter();

        dataController.GetData(new[] { searchParameters });
    }

    private void GetPhaseData(int index)
    {
        var dataController = dropdownList[index].GetComponent<IDataController>();

        var searchParameters = new Search.Phase();

        //searchParameters.requestType = Search.Phase.RequestType.GetPhaseWithQuests;

        //var chapterData = dataList[index - 1].ElementData.Cast<ChapterDataElement>().FirstOrDefault();
        //searchParameters.chapterId = new List<int>() { chapterData.id };

        searchParameters.temp_id_count = 15;

        dataController.GetData(new[] { searchParameters });
    }

    private void GetQuestData(int index)
    {
        var dataController = dropdownList[index].GetComponent<IDataController>();

        var searchParameters = new Search.Quest();
        searchParameters.temp_id_count = 15;

        dataController.GetData(new[] { searchParameters });
    }

    private void GetObjectiveData(int index)
    {
        var dataController = dropdownList[index].GetComponent<IDataController>();

        var searchParameters = new Search.Objective();
        searchParameters.temp_id_count = 15;

        dataController.GetData(new[] { searchParameters });
    }

    private void GetTerrainElementData(int index)
    {
        var dataController = dropdownList[index].GetComponent<IDataController>();

        var searchParameters = new Search.TerrainElement();
        searchParameters.temp_id_count = 15;

        dataController.GetData(new[] { searchParameters });
    }

    private void GetTaskData(int index)
    {
        var dataController = dropdownList[index].GetComponent<IDataController>();

        var searchParameters = new Search.Task();
        searchParameters.temp_id_count = 15;

        dataController.GetData(new[] { searchParameters });
    }

    private void GetRegionData(int index)
    {
        var dataController = dropdownList[index].GetComponent<IDataController>();

        ((RegionController)dataController).regionType = type;

        var searchParameters = new Search.Region();

        dataController.GetData(new[] { searchParameters });
    }

    //public IEnumerable GetEnumerable(int index, ICollection list)
    //{
    //    int i = 0;

    //    foreach(var data in list)
    //    {
    //        if (i == index)
    //            return new[] { data };

    //        i++;
    //    }

    //    return null;
    //}

    //public IEnumerable GetEnumerable(Dropdown dropdown, ICollection list)
    //{
    //    int i = 0;

    //    foreach (var data in list)
    //    {
    //        if (i == dropdown.value)
    //            return new[] { data };

    //        i++;
    //    }

    //    return null;
    //}

    public void CloseComponent()
    {
        dropdownList.Clear();
    }
}
