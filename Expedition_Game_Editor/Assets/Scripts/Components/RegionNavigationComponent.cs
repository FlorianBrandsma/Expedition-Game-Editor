using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class NavigationData
{
    Enums.DataType dataType;
    public List<int> idList = new List<int>();
}

public class RegionNavigationComponent : MonoBehaviour, IComponent
{
    //On startup, get the data of all relevant routes in the path, based on the data types
    //Load the rest of the data "manually", to filter out dead ends

    //When changing selections, load data from every datacontroller AFTER the index of the selected tab

    internal class ComponentData
    {
        public Data data;
        public List<int> idList = new List<int>();

        public ComponentData(Data data)
        {
            this.data = data;
        }
    }

    private Enums.RegionType regionType;

    public RegionDisplayManager defaultDisplay;

    private PathController pathController { get { return GetComponent<PathController>(); } }

    public EditorComponent component;
    
    private List<ComponentData> componentDataList = new List<ComponentData>();

    private List<Enums.DataType> structureList;

    private Route regionRoute;

    public void InitializeComponent(Path path)
    {
        structureList = path.route.Where(x => x.data.DataController != null && x.data.DataController.DataCategory == Enums.DataCategory.Navigation).Select(x => x.data.DataController.DataType).Distinct().ToList();

        regionRoute = pathController.route.path.FindLastRoute(Enums.DataType.Region).Copy();

        var regionDataElement = (RegionDataElement)regionRoute.data.DataElement;
        regionType = regionDataElement.type;

        InitializeData();

        if (path.route.Count < (pathController.route.path.route.Count + 1))
        {
            //The region route gets added at the end when the component is initialized.
            //It tries to add another route every time it gets opened, causing the selection to appear.
            //This attempt gets blocked by using the max_length variable

            int index = (int)RegionDisplayManager.activeDisplay;
            
            regionRoute.controller = index;
            
            path.Add(regionRoute);

            if (RegionDisplayManager.activeDisplay == RegionDisplayManager.Display.Terrain)
                path.Add(regionRoute);
        }
    }

    private void InitializeData()
    {
        if (pathController.route.path.type != Path.Type.New) return;

        if (regionType == Enums.RegionType.Task)
            RegionDisplayManager.activeDisplay = RegionDisplayManager.Display.Terrain;
        else
            RegionDisplayManager.activeDisplay = RegionDisplayManager.Display.Tiles;

        componentDataList.Clear();

        InitializeStructureData();

        FilterData();
    }

    //Remove all dead ends from data
    #region Data Filter

    private void FilterData()
    {
        if (structureList.Contains(Enums.DataType.TerrainElement))
        {
            var terrainElementData = Fixtures.terrainElementList;//.Where(x => x.objectiveId != 0 || x.chapterId != 0).Distinct().ToList();
            var terrainElementComponent = FindComponentByDataType(Enums.DataType.TerrainElement);
            terrainElementComponent.idList = terrainElementData.Select(x => x.id).Distinct().ToList();
        }

        if (structureList.Contains(Enums.DataType.Task))
        {
            if (structureList.Contains(Enums.DataType.TerrainElement))
            {
                var terrainElementData = FindComponentByDataType(Enums.DataType.TerrainElement).idList;

                var taskData = Fixtures.taskList.Where(x => terrainElementData.Contains(x.terrainElementId)).Distinct().ToList();
                var taskComponent = FindComponentByDataType(Enums.DataType.Task);
                taskComponent.idList = taskData.Select(x => x.id).Distinct().ToList();
            }  
        }

        if (structureList.Contains(Enums.DataType.Objective))
        {
            if (structureList.Contains(Enums.DataType.Task))
            {
                var idList = FindComponentByDataType(Enums.DataType.Task).idList;
                var taskData = Fixtures.taskList.Where(x => idList.Contains(x.id)).Distinct().ToList();

                var objectiveData = Fixtures.objectiveList.Where(x => taskData.Select(y => y.objectiveId).Contains(x.id)).Distinct().ToList();
                var objectiveComponent = FindComponentByDataType(Enums.DataType.Objective);
                objectiveComponent.idList = objectiveData.Select(x => x.id).Distinct().ToList();
            }   
        }

        if (structureList.Contains(Enums.DataType.Quest))
        {
            if (structureList.Contains(Enums.DataType.Objective))
            {
                var idList = FindComponentByDataType(Enums.DataType.Objective).idList;
                var objectiveData = Fixtures.objectiveList.Where(x => idList.Contains(x.id)).Distinct().ToList();

                var questData = Fixtures.questList.Where(x => objectiveData.Select(y => y.questId).Contains(x.id)).Distinct().ToList();
                var questComponent = FindComponentByDataType(Enums.DataType.Quest);
                questComponent.idList = questData.Select(x => x.id).Distinct().ToList();
            }   
        }

        if (structureList.Contains(Enums.DataType.Phase))
        {
            if (structureList.Contains(Enums.DataType.Quest))
            {
                var idList = FindComponentByDataType(Enums.DataType.Quest).idList;
                var questData = Fixtures.questList.Where(x => idList.Contains(x.id)).Distinct().ToList();

                var phaseData = Fixtures.phaseList.Where(x => questData.Select(y => y.phaseId).Contains(x.id)).Distinct().ToList();
                var phaseComponent = FindComponentByDataType(Enums.DataType.Phase);
                phaseComponent.idList = phaseData.Select(x => x.id).Distinct().ToList();

            } else {

                var phaseRegionIds = Fixtures.regionList.Where(x => x.phaseId != 0).Distinct().ToList();

                var phaseData = Fixtures.phaseList.Where(x => phaseRegionIds.Select(y => y.phaseId).Contains(x.id)).Distinct().ToList();
                var phaseComponent = FindComponentByDataType(Enums.DataType.Phase);
                phaseComponent.idList = phaseData.Select(x => x.id).Distinct().ToList();
            }
        }

        if(structureList.Contains(Enums.DataType.Chapter))
        {
            if(structureList.Contains(Enums.DataType.Phase))
            {
                var idList = FindComponentByDataType(Enums.DataType.Phase).idList;
                var phaseData = Fixtures.phaseList.Where(x => idList.Contains(x.id)).Distinct().ToList();

                var chapterData = Fixtures.phaseList.Where(x => phaseData.Select(y => y.chapterId).Contains(x.id)).Distinct().ToList();
                var chapterComponent = FindComponentByDataType(Enums.DataType.Chapter);
                chapterComponent.idList = chapterData.Select(x => x.id).Distinct().ToList();
            }
        }

        if(structureList.Contains(Enums.DataType.Region))
        {
            if (structureList.Contains(Enums.DataType.Phase))
            {
                var phaseData = FindComponentByDataType(Enums.DataType.Phase).idList;

                var regionData = Fixtures.regionList.Where(x => phaseData.Contains(x.phaseId)).Distinct().ToList();
                var regionComponent = FindComponentByDataType(Enums.DataType.Region);
                regionComponent.idList = regionData.Select(x => x.id).Distinct().ToList();

            } else {

                var regionComponent = FindComponentByDataType(Enums.DataType.Region);
                var regionData = Fixtures.regionList.Where(x => x.phaseId == 0).Distinct().ToList();
                regionComponent.idList = regionData.Select(x => x.id).Distinct().ToList();
            }  
        }
    }

    #endregion

    private void InitializeStructureData()
    {
        foreach (Enums.DataType structure in structureList)
        {
            Data data = pathController.route.path.FindFirstRoute(structure).data;
            componentDataList.Add(new ComponentData(data));
        }
    }

    public void SetComponent(Path path)
    {
        foreach (Enums.DataType structure in structureList)
        {
            SetDropdown(structure);
        }
    }

    private void SetDropdown(Enums.DataType dataType)
    {
        Dropdown dropdown = ComponentManager.componentManager.AddDropdown(component);

        var componentData = FindComponentByDataType(dataType);

        componentData.data.DataController = InitializeDropdown(componentData, dropdown);

        SetStructureOptions(dropdown, componentData.data.DataController);
        
        dropdown.onValueChanged.AddListener(delegate
        {
            InitializePath(new Data(componentData.data.DataController, componentData.data.DataController.DataList[dropdown.value]), 
                                    componentDataList.IndexOf(componentData) + 1);
        });
    }

    private IDataController InitializeDropdown(ComponentData componentData, Dropdown dropdown)
    {
        IDataController dataController;

        Enums.DataType dataType = componentData.data.DataController.DataType;

        switch (dataType)
        {
            case Enums.DataType.Chapter:        dataController = dropdown.gameObject.AddComponent<ChapterController>();         break;
            case Enums.DataType.Phase:          dataController = dropdown.gameObject.AddComponent<PhaseController>();           break;
            case Enums.DataType.Quest:          dataController = dropdown.gameObject.AddComponent<QuestController>();           break;
            case Enums.DataType.Objective:      dataController = dropdown.gameObject.AddComponent<ObjectiveController>();       break;
            case Enums.DataType.TerrainElement: dataController = dropdown.gameObject.AddComponent<TerrainElementController>();  break;
            case Enums.DataType.Task:           dataController = dropdown.gameObject.AddComponent<TaskController>();            break;
            case Enums.DataType.Region:         dataController = dropdown.gameObject.AddComponent<RegionController>();          break;
            default:                            dataController = null;                                                          break;
        }

        dataController.InitializeController();

        if (dataController.DataList == null)
            dataController.DataList = componentData.data.DataController.DataList.Where(x => componentData.idList.Contains(x.Id)).Distinct().ToList();

        return dataController;
    }

    private void GetData(IDataController dataController)
    {
        SearchParameters searchParameters;

        switch (dataController.DataType)
        {
            case Enums.DataType.Chapter:        searchParameters = GetChapterData();        break;
            case Enums.DataType.Phase:          searchParameters = GetPhaseData();          break;
            case Enums.DataType.Quest:          searchParameters = GetQuestData();          break;
            case Enums.DataType.Objective:      searchParameters = GetObjectiveData();      break;
            case Enums.DataType.TerrainElement: searchParameters = GetTerrainElementData(); break;
            case Enums.DataType.Task:           searchParameters = GetTaskData();           break;
            case Enums.DataType.Region:         searchParameters = GetRegionData();         break;
            default:                            searchParameters = null;                    break;
        }

        searchParameters.id = FindComponentByDataType(dataController.DataType).idList;
        
        dataController.GetData(new[] { searchParameters });

        var componentData = FindComponentByDataType(dataController.DataType);

        Data data = new Data(dataController, dataController.DataList.FirstOrDefault());

        componentData.data = data;

        pathController.route.path.ReplaceAllRoutes(data);
    }

    #region Data

    private SearchParameters GetChapterData()
    {
        var searchParameters = new Search.Chapter();
        return searchParameters;
    }

    private SearchParameters GetPhaseData()
    {
        var searchParameters = new Search.Phase();

        searchParameters.chapterId = new List<int>() { pathController.route.path.FindFirstRoute(Enums.DataType.Chapter).GeneralData().id };

        return searchParameters;
    }

    private SearchParameters GetQuestData()
    {
        var searchParameters = new Search.Quest();

        searchParameters.phaseId = new List<int>() { pathController.route.path.FindFirstRoute(Enums.DataType.Phase).GeneralData().id };

        return searchParameters;
    }

    private SearchParameters GetObjectiveData()
    {
        var searchParameters = new Search.Objective();

        searchParameters.questId = new List<int>() { pathController.route.path.FindFirstRoute(Enums.DataType.Quest).GeneralData().id };

        return searchParameters;
    }

    private SearchParameters GetTerrainElementData()
    {
        var searchParameters = new Search.TerrainElement();

        searchParameters.requestType = Search.TerrainElement.RequestType.GetQuestAndObjectiveElements;

        searchParameters.questId     = new List<int>() { pathController.route.path.FindFirstRoute(Enums.DataType.Quest).GeneralData().id };
        searchParameters.objectiveId = new List<int>() { pathController.route.path.FindFirstRoute(Enums.DataType.Objective).GeneralData().id };

        return searchParameters;
    }

    private SearchParameters GetTaskData()
    {
        var searchParameters = new Search.Task();

        searchParameters.terrainElementId = new List<int>() { pathController.route.path.FindFirstRoute(Enums.DataType.TerrainElement).GeneralData().id };

        if(pathController.route.path.FindFirstRoute(Enums.DataType.Objective) != null)
            searchParameters.objectiveId      = new List<int>() { pathController.route.path.FindFirstRoute(Enums.DataType.Objective).GeneralData().id };

        return searchParameters;
    }

    private SearchParameters GetRegionData()
    {
        var searchParameters = new Search.Region();

        var phaseRoute = pathController.route.path.FindFirstRoute(Enums.DataType.Phase);

        int phaseId = phaseRoute != null ? phaseRoute.GeneralData().id : 0;

        searchParameters.phaseId = new List<int>() { phaseId };

        return searchParameters;
    }

    #endregion

    private void InitializePath(Data data, int index)
    {
        pathController.route.path.ReplaceAllRoutes(data);

        for(int i = index; i < componentDataList.Count; i++)
            GetData(componentDataList[i].data.DataController);

        EditorManager.editorManager.InitializePath(pathController.route.path);
    }

    private void SetStructureOptions(Dropdown dropdown, IDataController dataController)
    {
        switch (dataController.DataType)
        {
            case Enums.DataType.Chapter:        SetChapterOptions       (dropdown, dataController); break;
            case Enums.DataType.Phase:          SetPhaseOptions         (dropdown, dataController); break;
            case Enums.DataType.Quest:          SetQuestOptions         (dropdown, dataController); break;
            case Enums.DataType.Objective:      SetObjectiveOptions     (dropdown, dataController); break;
            case Enums.DataType.TerrainElement: SetTerrainElementOptions(dropdown, dataController); break;
            case Enums.DataType.Task:           SetTaskOptions          (dropdown, dataController); break;
            case Enums.DataType.Region:         SetRegionOptions        (dropdown, dataController); break;
        }

        Data data = pathController.route.path.FindLastRoute(dataController.DataType).data;

        int selectedIndex = dataController.DataList.Cast<GeneralData>().ToList().FindIndex(x => x.id == ((GeneralData)data.DataElement).id);

        dropdown.value = selectedIndex;

        dropdown.captionText.text = dropdown.options[dropdown.value].text; 
    }

    #region Dropdown options

    private void SetChapterOptions(Dropdown dropdown, IDataController dataController)
    {
        var dataElements = dataController.DataList.Cast<ChapterDataElement>().ToList();

        dataElements.ForEach(x => dropdown.options.Add(new Dropdown.OptionData(x.Name)));
    }

    private void SetPhaseOptions(Dropdown dropdown, IDataController dataController)
    {
        var dataElements = dataController.DataList.Cast<PhaseDataElement>().ToList();

        dataElements.ForEach(x => dropdown.options.Add(new Dropdown.OptionData(x.Name)));
    }

    private void SetQuestOptions(Dropdown dropdown, IDataController dataController)
    {
        var dataElements = dataController.DataList.Cast<QuestDataElement>().ToList();

        dataElements.ForEach(x => dropdown.options.Add(new Dropdown.OptionData(x.Name)));
    }

    private void SetObjectiveOptions(Dropdown dropdown, IDataController dataController)
    {
        var dataElements = dataController.DataList.Cast<ObjectiveDataElement>().ToList();

        dataElements.ForEach(x => dropdown.options.Add(new Dropdown.OptionData(x.Name)));
    }

    private void SetTerrainElementOptions(Dropdown dropdown, IDataController dataController)
    {
        var dataElements = dataController.DataList.Cast<TerrainElementDataElement>().ToList();

        dataElements.ForEach(x => dropdown.options.Add(new Dropdown.OptionData(x.elementName)));
    }

    private void SetTaskOptions(Dropdown dropdown, IDataController dataController)
    {
        var dataElements = dataController.DataList.Cast<TaskDataElement>().ToList();

        dataElements.ForEach(x => dropdown.options.Add(new Dropdown.OptionData(x.Description)));
    }

    private void SetRegionOptions(Dropdown dropdown, IDataController dataController)
    {
        var dataElements = dataController.DataList.Cast<RegionDataElement>().ToList();

        dataElements.ForEach(x => dropdown.options.Add(new Dropdown.OptionData(x.Name)));
    }

    #endregion

    private ComponentData FindComponentByDataType(Enums.DataType dataType)
    {
        return componentDataList.Where(x => x.data.DataController.DataType == dataType).FirstOrDefault();
    }

    public void CloseComponent() { }
}
