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
        public Route.Data data;
        public List<int> idFilter = new List<int>();

        public ComponentData(Route.Data data)
        {
            this.data = data;
        }   
    }

    private Enums.RegionType regionType;

    public RegionDisplayManager defaultDisplay;

    private PathController PathController { get { return GetComponent<PathController>(); } }

    public EditorComponent component;
    
    private List<ComponentData> componentDataList = new List<ComponentData>();

    private List<Enums.DataType> structureList;

    private Route regionRoute;

    public void InitializeComponent(Path path)
    {
        structureList = path.route.Where(x => x.data.dataController != null && x.data.dataController.DataCategory == Enums.DataCategory.Navigation).Select(x => x.data.dataController.DataType).Distinct().ToList();

        regionRoute = PathController.route.path.FindLastRoute(Enums.DataType.Region).Copy();
        
        InitializeData();

        if (path.route.Count < (PathController.route.path.route.Count + 1))
        {
            //The region route gets added at the end when the component is initialized.
            //It tries to add another route every time it gets opened, causing the selection to appear.
            //This attempt gets blocked by using the max_length variable

            int index = (int)RegionDisplayManager.activeDisplay;

            regionRoute.controller = index;

            if (RegionDisplayManager.activeDisplay == RegionDisplayManager.Display.Scene)
                path.Add(regionRoute);

            if (regionType != Enums.RegionType.Interaction)
            {
                path.Add(regionRoute);

            } else /*TEMPORARY*/ {

                //Temporary solution currently results in a couple of bugs as a result of the scene interaction editor not loading.
                //This will be fixed when the temporary solution is replaced by automatically selecting the selection element
                //where the data matches the current selection data. Whenever navigation selection is changed, the selection element
                //must be selected from the same starting point, thereby triggering the editor to load.
                
                var interactionRoute = PathController.route.path.FindLastRoute(Enums.DataType.Interaction);
                interactionRoute.controller = 0;

                path.Add(interactionRoute);
            }
        }  
    }

    private void InitializeData()
    {
        if (PathController.route.path.type == Path.Type.New)
        {
            var regionDataElement = (RegionDataElement)regionRoute.data.dataElement;
            regionType = regionDataElement.type;

            if (regionType == Enums.RegionType.Interaction)
                RegionDisplayManager.activeDisplay = RegionDisplayManager.Display.Scene;
            else
                RegionDisplayManager.activeDisplay = RegionDisplayManager.Display.Tiles;
        }

        InitializeStructureData();
    }

    //Remove all dead ends from data
    #region Data Filter

    private void FilterData()
    {
        if (structureList.Contains(Enums.DataType.SceneInteractable))
        {
            var sceneInteractableData = Fixtures.sceneInteractableList;//.Where(x => x.objectiveId != 0 || x.chapterId != 0).Distinct().ToList();
            var sceneInteractableComponent = FindComponentByDataType(Enums.DataType.SceneInteractable);
            sceneInteractableComponent.idFilter = sceneInteractableData.Select(x => x.Id).Distinct().ToList();
        }

        if (structureList.Contains(Enums.DataType.Interaction))
        {
            if (structureList.Contains(Enums.DataType.SceneInteractable))
            {
                var sceneInteractableData = FindComponentByDataType(Enums.DataType.SceneInteractable).idFilter;

                var interactionData = Fixtures.interactionList.Where(x => sceneInteractableData.Contains(x.sceneInteractableId)).Distinct().ToList();
                var interactionComponent = FindComponentByDataType(Enums.DataType.Interaction);
                interactionComponent.idFilter = interactionData.Select(x => x.Id).Distinct().ToList();
            }  
        }

        if (structureList.Contains(Enums.DataType.Objective))
        {
            if (structureList.Contains(Enums.DataType.Interaction))
            {
                var idList = FindComponentByDataType(Enums.DataType.Interaction).idFilter;
                var interactionData = Fixtures.interactionList.Where(x => idList.Contains(x.Id)).Distinct().ToList();

                var objectiveData = Fixtures.objectiveList.Where(x => interactionData.Select(y => y.objectiveId).Contains(x.Id)).Distinct().ToList();
                var objectiveComponent = FindComponentByDataType(Enums.DataType.Objective);
                objectiveComponent.idFilter = objectiveData.Select(x => x.Id).Distinct().ToList();
            }   
        }

        if (structureList.Contains(Enums.DataType.Quest))
        {
            if (structureList.Contains(Enums.DataType.Objective))
            {
                var idList = FindComponentByDataType(Enums.DataType.Objective).idFilter;
                var objectiveData = Fixtures.objectiveList.Where(x => idList.Contains(x.Id)).Distinct().ToList();

                var questData = Fixtures.questList.Where(x => objectiveData.Select(y => y.questId).Contains(x.Id)).Distinct().ToList();
                var questComponent = FindComponentByDataType(Enums.DataType.Quest);
                questComponent.idFilter = questData.Select(x => x.Id).Distinct().ToList();
            }   
        }

        if (structureList.Contains(Enums.DataType.Phase))
        {
            if (structureList.Contains(Enums.DataType.Quest))
            {
                var idList = FindComponentByDataType(Enums.DataType.Quest).idFilter;
                var questData = Fixtures.questList.Where(x => idList.Contains(x.Id)).Distinct().ToList();

                var phaseData = Fixtures.phaseList.Where(x => questData.Select(y => y.phaseId).Contains(x.Id)).Distinct().ToList();
                var phaseComponent = FindComponentByDataType(Enums.DataType.Phase);
                phaseComponent.idFilter = phaseData.Select(x => x.Id).Distinct().ToList();

            } else {

                var phaseRegionIds = Fixtures.regionList.Where(x => x.phaseId != 0).Distinct().ToList();

                var phaseData = Fixtures.phaseList.Where(x => phaseRegionIds.Select(y => y.phaseId).Contains(x.Id)).Distinct().ToList();
                var phaseComponent = FindComponentByDataType(Enums.DataType.Phase);
                phaseComponent.idFilter = phaseData.Select(x => x.Id).Distinct().ToList();
            }
        }

        if(structureList.Contains(Enums.DataType.Chapter))
        {
            if(structureList.Contains(Enums.DataType.Phase))
            {
                var idList = FindComponentByDataType(Enums.DataType.Phase).idFilter;
                var phaseData = Fixtures.phaseList.Where(x => idList.Contains(x.Id)).Distinct().ToList();

                var chapterData = Fixtures.phaseList.Where(x => phaseData.Select(y => y.chapterId).Contains(x.Id)).Distinct().ToList();
                var chapterComponent = FindComponentByDataType(Enums.DataType.Chapter);
                chapterComponent.idFilter = chapterData.Select(x => x.Id).Distinct().ToList();
            }
        }

        if(structureList.Contains(Enums.DataType.Region))
        {
            if (structureList.Contains(Enums.DataType.Phase))
            {
                var phaseData = FindComponentByDataType(Enums.DataType.Phase).idFilter;

                var regionData = Fixtures.regionList.Where(x => phaseData.Contains(x.phaseId)).Distinct().ToList();
                var regionComponent = FindComponentByDataType(Enums.DataType.Region);
                regionComponent.idFilter = regionData.Select(x => x.Id).Distinct().ToList();

            } else {

                var regionComponent = FindComponentByDataType(Enums.DataType.Region);
                var regionData = Fixtures.regionList.Where(x => x.phaseId == 0).Distinct().ToList();
                regionComponent.idFilter = regionData.Select(x => x.Id).Distinct().ToList();
            }  
        }
    }

    #endregion

    private void InitializeStructureData()
    {
        componentDataList.Clear();

        foreach (Enums.DataType structure in structureList)
        {
            var data = PathController.route.path.FindLastRoute(structure).data;
            componentDataList.Add(new ComponentData(data));
        }

        FilterData();
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

        SetOptions(dropdown, componentData);
        
        dropdown.onValueChanged.AddListener(delegate
        {
            InitializePath(componentData.data.dataList[dropdown.value], componentDataList.IndexOf(componentData));
        });
    }

    private void InitializePath(IDataElement dataElement, int index)
    {
        componentDataList[index].data.dataElement = dataElement;

        var path = PathController.route.path;

        //Replace routes as multiple routes of type "Region" exist outside of the componentData
        path.ReplaceAllRoutes(componentDataList[index].data);
        EditorManager.loadType = Enums.LoadType.Reload;

        for (int i = (index+1); i < componentDataList.Count; i++)
            GetData(componentDataList[i].data.dataController);

        EditorManager.editorManager.ResetEditor(PathController.route.path);
    }

    private void GetData(IDataController dataController)
    {
        SearchParameters searchParameters = null;

        switch (dataController.DataType)
        {
            case Enums.DataType.Chapter:            searchParameters = GetChapterData();            break;
            case Enums.DataType.Phase:              searchParameters = GetPhaseData();              break;
            case Enums.DataType.Quest:              searchParameters = GetQuestData();              break;
            case Enums.DataType.Objective:          searchParameters = GetObjectiveData();          break;
            case Enums.DataType.SceneInteractable:  searchParameters = GetSceneInteractableData();  break;
            case Enums.DataType.Interaction:        searchParameters = GetInteractionData();        break;
            case Enums.DataType.Region:             searchParameters = GetRegionData();             break;
        }

        searchParameters.id = FindComponentByDataType(dataController.DataType).idFilter;

        var componentData = FindComponentByDataType(dataController.DataType);
        
        componentData.data.dataList = dataController.GetData(new[] { searchParameters });
        componentData.data.dataElement = componentData.data.dataList.FirstOrDefault();

        PathController.route.path.ReplaceAllRoutes(componentData.data);
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

        searchParameters.chapterId = new List<int>() { PathController.route.path.FindFirstRoute(Enums.DataType.Chapter).GeneralData.Id };

        return searchParameters;
    }

    private SearchParameters GetQuestData()
    {
        var searchParameters = new Search.Quest();

        searchParameters.phaseId = new List<int>() { PathController.route.path.FindFirstRoute(Enums.DataType.Phase).GeneralData.Id };

        return searchParameters;
    }

    private SearchParameters GetObjectiveData()
    {
        var searchParameters = new Search.Objective();

        searchParameters.questId = new List<int>() { PathController.route.path.FindFirstRoute(Enums.DataType.Quest).GeneralData.Id };

        return searchParameters;
    }

    private SearchParameters GetSceneInteractableData()
    {
        var searchParameters = new Search.SceneInteractable();

        var questRoute = PathController.route.path.FindFirstRoute(Enums.DataType.Quest);
        var objectiveRoute = PathController.route.path.FindFirstRoute(Enums.DataType.Objective);

        if(questRoute != null && objectiveRoute != null)
        {
            searchParameters.requestType = Search.SceneInteractable.RequestType.GetQuestAndObjectiveInteractables;

            searchParameters.questId = new List<int>() { questRoute.GeneralData.Id };
            searchParameters.objectiveId = new List<int>() { objectiveRoute.GeneralData.Id };

        } else {

            searchParameters.requestType = Search.SceneInteractable.RequestType.GetInteractablesFromInteractionRegion;

            var regionRoute = PathController.route.path.FindFirstRoute(Enums.DataType.Region);

            searchParameters.regionId = new List<int>() { regionRoute.GeneralData.Id };

            searchParameters.questId = new List<int>() { 0 };
            searchParameters.objectiveId = new List<int>() { 0 };
        }

        return searchParameters;
    }

    private SearchParameters GetInteractionData()
    {
        var searchParameters = new Search.Interaction();

        searchParameters.sceneInteractableId = new List<int>() { PathController.route.path.FindFirstRoute(Enums.DataType.SceneInteractable).GeneralData.Id };

        if(PathController.route.path.FindFirstRoute(Enums.DataType.Objective) != null)
            searchParameters.objectiveId = new List<int>() { PathController.route.path.FindFirstRoute(Enums.DataType.Objective).GeneralData.Id };

        return searchParameters;
    }

    private SearchParameters GetRegionData()
    {
        var searchParameters = new Search.Region();

        var phaseRoute = PathController.route.path.FindFirstRoute(Enums.DataType.Phase);

        int phaseId = phaseRoute != null ? phaseRoute.GeneralData.Id : 0;

        searchParameters.phaseId = new List<int>() { phaseId };

        return searchParameters;
    }

    #endregion
    
    private void SetOptions(Dropdown dropdown, ComponentData componentData)
    {
        switch (componentData.data.dataController.DataType)
        {
            case Enums.DataType.Chapter:            SetChapterOptions           (dropdown, componentData); break;
            case Enums.DataType.Phase:              SetPhaseOptions             (dropdown, componentData); break;
            case Enums.DataType.Quest:              SetQuestOptions             (dropdown, componentData); break;
            case Enums.DataType.Objective:          SetObjectiveOptions         (dropdown, componentData); break;
            case Enums.DataType.SceneInteractable:  SetSceneInteractableOptions (dropdown, componentData); break;
            case Enums.DataType.Interaction:        SetInteractionOptions       (dropdown, componentData); break;
            case Enums.DataType.Region:             SetRegionOptions            (dropdown, componentData); break;
        }

        var data = PathController.route.path.FindLastRoute(componentData.data.dataController.DataType).data;

        int selectedIndex = componentData.data.dataList.Cast<GeneralData>().ToList().FindIndex(x => x.Id == ((GeneralData)data.dataElement).Id);

        dropdown.value = selectedIndex;

        dropdown.captionText.text = dropdown.options[dropdown.value].text; 
    }

    #region Dropdown options

    private void SetChapterOptions(Dropdown dropdown, ComponentData componentData)
    {
        var dataElements = componentData.data.dataList.Cast<ChapterDataElement>().ToList();

        dataElements.ForEach(x => dropdown.options.Add(new Dropdown.OptionData(x.Name)));
    }

    private void SetPhaseOptions(Dropdown dropdown, ComponentData componentData)
    {
        var dataElements = componentData.data.dataList.Cast<PhaseDataElement>().ToList();

        dataElements.ForEach(x => dropdown.options.Add(new Dropdown.OptionData(x.Name)));
    }

    private void SetQuestOptions(Dropdown dropdown, ComponentData componentData)
    {
        var dataElements = componentData.data.dataList.Cast<QuestDataElement>().ToList();

        dataElements.ForEach(x => dropdown.options.Add(new Dropdown.OptionData(x.Name)));
    }

    private void SetObjectiveOptions(Dropdown dropdown, ComponentData componentData)
    {
        var dataElements = componentData.data.dataList.Cast<ObjectiveDataElement>().ToList();

        dataElements.ForEach(x => dropdown.options.Add(new Dropdown.OptionData(x.Name)));
    }

    private void SetSceneInteractableOptions(Dropdown dropdown, ComponentData componentData)
    {
        var dataElements = componentData.data.dataList.Cast<SceneInteractableDataElement>().ToList();

        dataElements.ForEach(x => dropdown.options.Add(new Dropdown.OptionData(x.interactableName)));
    }

    private void SetInteractionOptions(Dropdown dropdown, ComponentData componentData)
    {
        var dataElements = componentData.data.dataList.Cast<InteractionDataElement>().ToList();

        dataElements.ForEach(x => dropdown.options.Add(new Dropdown.OptionData(x.Description)));
    }

    private void SetRegionOptions(Dropdown dropdown, ComponentData componentData)
    {
        var dataElements = componentData.data.dataList.Cast<RegionDataElement>().ToList();

        dataElements.ForEach(x => {

            dropdown.options.Add(new Dropdown.OptionData(x.Name));
            x.type = regionType;
        });
    }

    #endregion

    private ComponentData FindComponentByDataType(Enums.DataType dataType)
    {
        return componentDataList.Where(x => x.data.dataController.DataType == dataType).FirstOrDefault();
    }

    public void CloseComponent() { }
}
