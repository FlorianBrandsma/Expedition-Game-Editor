using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using System.Linq;

public class RegionNavigationAction : MonoBehaviour, IAction
{
    //On startup, get the data of all relevant routes in the path, based on the data types
    //Load the rest of the data "manually", to filter out dead ends

    //When changing selections, load data from every datacontroller AFTER the index of the selected tab

    private List<ChapterBaseData> chapterDataFilter;
    private List<PhaseBaseData> phaseDataFilter;
    private List<QuestBaseData> questDataFilter;
    private List<ObjectiveBaseData> objectiveDataFilter;
    private List<WorldInteractableBaseData> worldInteractableDataFilter;
    private List<TaskBaseData> taskDataFilter;
    private List<InteractionBaseData> interactionDataFilter;
    private List<InteractionDestinationBaseData> interactionDestinationDataFilter;
    private List<OutcomeBaseData> outcomeDataFilter;
    private List<SceneBaseData> sceneDataFilter;

    public ActionProperties actionProperties;

    private List<Enums.DataType> dataTypeList;
    private List<Route> routeList = new List<Route>();

    public PathController PathController { get { return GetComponent<PathController>(); } }

    public void InitializeAction(Path path)
    {
        dataTypeList = path.routeList.Where(x => x.data != null && x.data.dataController.DataCategory == Enums.DataCategory.Navigation)
                                     .Select(x => x.data.dataController.DataType).Distinct().ToList();

        var regionRouteSource = path.FindLastRoute(Enums.DataType.Region);

        var regionRoute = new Route()
        {
            controllerIndex = regionRouteSource.controllerIndex,
            id = regionRouteSource.id,
            data = regionRouteSource.data,
            path = path,

            selectionStatus = regionRouteSource.selectionStatus,
            uniqueSelection = regionRouteSource.uniqueSelection
        };
        
        InitializeData(regionRoute);

        //The region route only gets added at the end when the action is initialized
        if (path.routeList.Count < (PathController.route.path.routeList.Count + 1))
        {
            int index = (int)RegionManager.activeDisplay;

            regionRoute.controllerIndex = index;

            if (RegionManager.activeDisplay == RegionManager.Display.World)
            {
                path.Add(regionRoute);
            }

            if (RegionManager.regionType == Enums.RegionType.InteractionDestination)
            {
                var interactionDestinationRouteSource = path.FindLastRoute(Enums.DataType.InteractionDestination);

                //DataController data is used so that the action data is also updated
                var interactionDestinationRoute = new Route()
                {
                    controllerIndex = (int)Enums.WorldSelectionType.InteractionDestination,
                    id = interactionDestinationRouteSource.id,
                    data = interactionDestinationRouteSource.data.dataController.Data,
                    path = interactionDestinationRouteSource.path,
                    selectionStatus = Enums.SelectionStatus.Main,
                    uniqueSelection = false
                };

                path.Add(interactionDestinationRoute);

            } else if (RegionManager.regionType == Enums.RegionType.Controllable) {

                var phaseRouteSource = path.FindLastRoute(Enums.DataType.Phase);

                var phaseRoute = new Route()
                {
                    controllerIndex = (int)Enums.WorldSelectionType.Controllable,
                    id = phaseRouteSource.id,
                    data = phaseRouteSource.data,
                    path = phaseRouteSource.path,
                    selectionStatus = Enums.SelectionStatus.Main,
                    uniqueSelection = false
                };
                
                path.Add(phaseRoute);

            } else {

                path.Add(regionRoute);
            }
        }
        
        InitializeStructureData(path);
    }

    private void InitializeData(Route regionRoute)
    {
        var regionData = (RegionElementData)regionRoute.ElementData;
        RegionManager.regionType = regionData.Type;

        if (PathController.route.path.type == Path.Type.New)
        {
            if (RegionManager.regionType == Enums.RegionType.InteractionDestination || 
                RegionManager.regionType == Enums.RegionType.Controllable           || 
                RegionManager.regionType == Enums.RegionType.Scene)
            {
                RegionManager.activeDisplay = RegionManager.Display.World;
                SceneShotManager.activeShotType = Enums.SceneShotType.Base;
            } else {
                RegionManager.activeDisplay = RegionManager.Display.Tiles;
            }
        }
    }

    private void InitializeStructureData(Path path)
    {
        routeList.Clear();
        
        dataTypeList.ForEach(dataType =>
        {
            var route = PathController.route.path.FindLastRoute(dataType);
            routeList.Add(route);
        });

        var sceneRoute = FindRouteByDataType(Enums.DataType.Scene);

        if(sceneRoute != null)
        {
            var sceneActorRoute = path.FindLastRoute(Enums.DataType.SceneActor);
            var scenePropRoute = path.FindLastRoute(Enums.DataType.SceneProp);

            if (sceneActorRoute != null)
            {
                sceneRoute.id = ((SceneActorElementData)(sceneActorRoute.ElementData)).SceneId;

            } else if (scenePropRoute != null) {

                sceneRoute.id = ((ScenePropElementData)(scenePropRoute.ElementData)).SceneId;
            }
        }
        
        for (int i = routeList.Count - 1; i >= 0; i--)
        {
            var route = routeList[i];
            var dataType = route.data.dataController.DataType;

            var mainRoute = path.FindLastRoute(dataType);

            //World interactable doesn't always reset as the id can be the same in multiple objectives
            if (!ListContainsElement(route, mainRoute.id))
            {
                route.data.dataList = mainRoute.data.dataList;
                route.id = mainRoute.id;

                ResetData(route.data);

            } else {

                //Route data should be constant throughout the controllers, but adding routes in initialization
                //seems to mess with that. Below assigns the data of the main path to that saved by the controller
                route.data = mainRoute.data;

                //There is currently no reliable way to only load filter data on specific conditions,
                //so filter data must be loaded every time the action is initialized
                FilterData(route.data);
                
                if (route.data.dataController.DataType == Enums.DataType.Interaction)
                    CheckTime((InteractionElementData)route.ElementData);

                SelectOption(dataType);
            }
        }
    }

    private void FilterData(Data data)
    {
        switch(data.dataController.DataType)
        {
            case Enums.DataType.Region:                 GetRegionFilterData(data);                  break;
            case Enums.DataType.Chapter:                GetChapterFilterData(data);                 break;
            case Enums.DataType.Phase:                  GetPhaseFilterData(data);                   break;
            case Enums.DataType.Quest:                  GetQuestFilterData(data);                   break;
            case Enums.DataType.Objective:              GetObjectiveFilterData(data);               break;
            case Enums.DataType.WorldInteractable:      GetWorldInteractableFilterData(data);       break;
            case Enums.DataType.Task:                   GetTaskFilterData(data);                    break;
            case Enums.DataType.Interaction:            GetInteractionFilterData(data);             break;
            case Enums.DataType.InteractionDestination: GetInteractionDestinationFilterData(data);  break;
            case Enums.DataType.Outcome:                GetOutcomeFilterData(data);                 break;
            case Enums.DataType.Scene:                  GetSceneFilterData(data);                   break;

            default: Debug.Log("CASE MISSING: " + data.dataController.DataType); break;
        }
    }

    private void GetRegionFilterData(Data data)
    {
        data.dataList = data.dataList.Where(x => x.ExecuteType != Enums.ExecuteType.Add).ToList();
    }

    private void GetChapterFilterData(Data data)
    {
        var chapterSearchParameters = new Search.Chapter();
        chapterSearchParameters.id = phaseDataFilter.Select(x => x.ChapterId).ToList();

        chapterDataFilter = DataManager.GetChapterData(chapterSearchParameters);

        data.dataList = data.dataList.Where(x => chapterDataFilter.Select(y => y.Id).Contains(x.Id)).ToList();
    }

    private void GetPhaseFilterData(Data data)
    {
        var phaseSearchParameters = new Search.Phase();

        if (RegionManager.regionType == Enums.RegionType.Phase ||
            RegionManager.regionType == Enums.RegionType.Controllable)
        {
            //Region
            var regionDataList = DataManager.GetRegionData(new Search.Region());

            phaseSearchParameters.id = regionDataList.Select(x => x.Id).ToList();

        } else {

            phaseSearchParameters.id = questDataFilter.Select(x => x.PhaseId).ToList();
        }

        phaseDataFilter = DataManager.GetPhaseData(phaseSearchParameters);

        data.dataList = data.dataList.Where(x => phaseDataFilter.Select(y => y.Id).Contains(x.Id)).ToList();
    }

    private void GetQuestFilterData(Data data)
    {
        var questSearchParameters = new Search.Quest();
        questSearchParameters.id = objectiveDataFilter.Select(x => x.QuestId).ToList();

        questDataFilter = DataManager.GetQuestData(questSearchParameters);

        data.dataList = data.dataList.Where(x => questDataFilter.Select(y => y.Id).Contains(x.Id)).ToList();
    }

    private void GetObjectiveFilterData(Data data)
    {
        var objectiveSearchParameters = new Search.Objective();
        objectiveSearchParameters.id = worldInteractableDataFilter.Select(x => x.ObjectiveId).ToList();

        objectiveDataFilter = DataManager.GetObjectiveData(objectiveSearchParameters);

        data.dataList = data.dataList.Where(x => objectiveDataFilter.Select(y => y.Id).Contains(x.Id)).ToList();
    }

    private void GetWorldInteractableFilterData(Data data)
    {
        var worldInteractableSearchParameters = new Search.WorldInteractable();
        worldInteractableSearchParameters.id = taskDataFilter.Select(x => x.WorldInteractableId).ToList();

        worldInteractableDataFilter = DataManager.GetWorldInteractableData(worldInteractableSearchParameters);

        data.dataList = data.dataList.Where(x => worldInteractableDataFilter.Select(y => y.Id).Contains(x.Id)).ToList();
    }

    private void GetTaskFilterData(Data data)
    {
        var taskSearchParameters = new Search.Task();
        taskSearchParameters.id = interactionDataFilter.Select(x => x.TaskId).ToList();

        taskDataFilter = DataManager.GetTaskData(taskSearchParameters);

        data.dataList = data.dataList.Where(x => taskDataFilter.Select(y => y.Id).Contains(x.Id)).ToList();
    }

    private void GetInteractionFilterData(Data data)
    {
        var interactionSearchParameters = new Search.Interaction();

        if(RegionManager.regionType == Enums.RegionType.Scene)
            interactionSearchParameters.id = outcomeDataFilter.Select(x => x.InteractionId).ToList();

        if (RegionManager.regionType == Enums.RegionType.InteractionDestination)
            interactionSearchParameters.id = interactionDestinationDataFilter.Select(x => x.InteractionId).ToList();

        interactionDataFilter = DataManager.GetInteractionData(interactionSearchParameters);

        data.dataList = data.dataList.Where(x => interactionDataFilter.Select(y => y.Id).Contains(x.Id)).ToList();
    }

    private void GetInteractionDestinationFilterData(Data data)
    {
        //Region
        var regionDataList = DataManager.GetRegionData(new Search.Region());

        var interactionDestinationSearchParameters = new Search.InteractionDestination();
        interactionDestinationSearchParameters.regionId = regionDataList.Select(x => x.Id).ToList();

        interactionDestinationDataFilter = DataManager.GetInteractionDestinationData(interactionDestinationSearchParameters);

        var interactionDestinationRoute = FindRouteByDataType(Enums.DataType.InteractionDestination);

        if(interactionDestinationRoute.ElementData.ExecuteType != Enums.ExecuteType.Add)
            data.dataList = data.dataList.Where(x => interactionDestinationDataFilter.Select(y => y.Id).Contains(x.Id)).ToList();
    }

    private void GetOutcomeFilterData(Data data)
    {
        var outcomeSearchParameters = new Search.Outcome();
        outcomeSearchParameters.id = sceneDataFilter.Select(x => x.OutcomeId).ToList();

        outcomeDataFilter = DataManager.GetOutcomeData(outcomeSearchParameters);

        data.dataList = data.dataList.Where(x => outcomeDataFilter.Select(y => y.Id).Contains(x.Id)).ToList();
    }

    private void GetSceneFilterData(Data data)
    {
        //Region
        var regionDataList = DataManager.GetRegionData(new Search.Region());

        var sceneSearchParameters = new Search.Scene();
        sceneSearchParameters.regionId = regionDataList.Select(x => x.Id).ToList();

        sceneDataFilter = DataManager.GetSceneData(sceneSearchParameters);

        data.dataList = data.dataList.Where(x => sceneDataFilter.Select(y => y.Id).Contains(x.Id)).ToList();
    }

    private bool ListContainsElement(Route route, int id)
    {
        return route.data.dataList.Select(y => y.Id).ToList().Contains(id);
    }

    private void CheckTime(IElementData elementData)
    {
        var interactionData = (InteractionElementData)elementData;

        var startTime   = interactionData.Default ? TimeManager.instance.DefaultTime(interactionData.DefaultTimes) : interactionData.StartTime;
        var endTime     = interactionData.Default ? TimeManager.instance.DefaultTime(interactionData.DefaultTimes) : interactionData.EndTime;

        if (!TimeManager.TimeInFrame(TimeManager.instance.ActiveTime, startTime, endTime))
            TimeManager.instance.SetEditorTime(startTime);
    }
    
    private void ResetData(Data data)
    {
        var dataType = data.dataController.DataType;

        if(dataType == Enums.DataType.InteractionDestination || dataType == Enums.DataType.Scene)
            GetDependencies(dataType);

        GetData(data.dataController);
    }

    private void GetDependencies(Enums.DataType dataType)
    {
        var route = FindRouteByDataType(dataType);
        var searchProperties = new SearchProperties(dataType);

        var nextDataType = Enums.DataType.None;

        switch(dataType)
        {
            case Enums.DataType.Quest:                  nextDataType = GetQuestDependencies(searchProperties);                  break;
            case Enums.DataType.Objective:              nextDataType = GetObjectiveDependencies(searchProperties);              break;
            case Enums.DataType.WorldInteractable:      nextDataType = GetWorldInteractableDependencies(searchProperties);      break;
            case Enums.DataType.Task:                   nextDataType = GetTaskDependencies(searchProperties);                   break;
            case Enums.DataType.Interaction:            nextDataType = GetInteractionDependencies(searchProperties);            break;
            case Enums.DataType.InteractionDestination: nextDataType = GetInteractionDestinationDependencies(searchProperties); break;
            case Enums.DataType.Outcome:                nextDataType = GetOutcomeDependencies(searchProperties);                break;
            case Enums.DataType.Scene:                  nextDataType = GetSceneDependencies(searchProperties);                  break;
            
            default: Debug.Log("CASE MISSING: " + dataType); break;
        }

        route.data.dataList.ForEach(x => 
        {
            //Close active elements before overwriting data
            if (x.DataElement != null)
            {
                PoolManager.ClosePoolObject(x.DataElement.Poolable);
                SelectionElementManager.CloseElement(x.DataElement);
            }
        });

        route.data.dataController.GetData(searchProperties);

        if(route.data.dataList.Count > 0)
        {
            route.id = route.data.dataList.FirstOrDefault().Id;

            if (nextDataType != Enums.DataType.None)
                GetDependencies(nextDataType);

            route.data.dataList = new List<IElementData>();
        }
    }

    private Enums.DataType GetQuestDependencies(SearchProperties searchProperties)
    {
        var objectiveRoute = FindRouteByDataType(Enums.DataType.Objective);
        var objectiveElementData = (ObjectiveElementData)objectiveRoute.data.dataList.First();

        var searchParameters = searchProperties.searchParameters.Cast<Search.Quest>().First();
        searchParameters.id = new List<int>() { objectiveElementData.QuestId };
        
        return Enums.DataType.None;
    }

    private Enums.DataType GetObjectiveDependencies(SearchProperties searchProperties)
    {
        var taskRoute = FindRouteByDataType(Enums.DataType.Task);
        var taskElementData = (TaskElementData)taskRoute.data.dataList.First();
        
        var searchParameters = searchProperties.searchParameters.Cast<Search.Objective>().First();
        searchParameters.id = new List<int>() { taskElementData.ObjectiveId };
        
        return Enums.DataType.Quest;
    }

    private Enums.DataType GetWorldInteractableDependencies(SearchProperties searchProperties)
    {
        var taskRoute = FindRouteByDataType(Enums.DataType.Task);
        var taskElementData = (TaskElementData)taskRoute.data.dataList.First();

        var searchParameters = searchProperties.searchParameters.Cast<Search.WorldInteractable>().First();
        searchParameters.id = new List<int>() { taskElementData.WorldInteractableId };

        if (FindRouteByDataType(Enums.DataType.Objective) != null)
            return Enums.DataType.Objective;
        else
            return Enums.DataType.None;
    }

    private Enums.DataType GetTaskDependencies(SearchProperties searchProperties)
    {
        var interactionRoute = FindRouteByDataType(Enums.DataType.Interaction);
        var interactionElementData = (InteractionElementData)interactionRoute.data.dataList.First();

        var searchParameters = searchProperties.searchParameters.Cast<Search.Task>().First();
        searchParameters.id = new List<int>() { interactionElementData.TaskId };
        
        return Enums.DataType.WorldInteractable;
    }

    private Enums.DataType GetInteractionDependencies(SearchProperties searchProperties)
    {
        var searchParameters = searchProperties.searchParameters.Cast<Search.Interaction>().First();

        var interactionDestinationRoute = FindRouteByDataType(Enums.DataType.InteractionDestination);
        
        if (interactionDestinationRoute != null)
        {
            var interactionDestinationElementData = (InteractionDestinationElementData)interactionDestinationRoute.data.dataList.First();
            searchParameters.id = new List<int>() { interactionDestinationElementData.InteractionId };
        }

        var outcomeRoute = FindRouteByDataType(Enums.DataType.Outcome);

        if(outcomeRoute != null)
        {
            var outcomeElementData = (OutcomeElementData)outcomeRoute.data.dataList.First();
            searchParameters.id = new List<int>() { outcomeElementData.InteractionId };
        }
        
        return Enums.DataType.Task;
    }

    private Enums.DataType GetInteractionDestinationDependencies(SearchProperties searchProperties)
    {
        var searchParameters = searchProperties.searchParameters.Cast<Search.InteractionDestination>().First();
        searchParameters.id = new List<int>() { FindRouteByDataType(Enums.DataType.InteractionDestination).id };

        return Enums.DataType.Interaction;
    }

    private Enums.DataType GetOutcomeDependencies(SearchProperties searchProperties)
    {
        var sceneRoute = FindRouteByDataType(Enums.DataType.Scene);
        var sceneElementData = (SceneElementData)sceneRoute.data.dataList.First();

        var searchParameters = searchProperties.searchParameters.Cast<Search.Outcome>().First();
        searchParameters.id = new List<int>() { sceneElementData.OutcomeId };

        return Enums.DataType.Interaction;
    }

    private Enums.DataType GetSceneDependencies(SearchProperties searchProperties)
    {
        var searchParameters = searchProperties.searchParameters.Cast<Search.Scene>().First();
        searchParameters.id = new List<int>() { FindRouteByDataType(Enums.DataType.Scene).id };

        return Enums.DataType.Outcome;
    }

    public void SetAction(Path path)
    {
        routeList.ForEach(x => SetDropdown(x));
    }

    private void SetDropdown(Route route)
    {
        var dropdown = ActionManager.instance.AddDropdown(actionProperties);

        SetOptions(dropdown, route.data);

        dropdown.Dropdown.onValueChanged.AddListener(delegate
        {
            InitializePath(route.data.dataList[dropdown.Dropdown.value], routeList.IndexOf(route));
        });
    }

    private void InitializePath(IElementData elementData, int index)
    {
        routeList[index].id = elementData.Id;

        for (int i = (index + 1); i < routeList.Count; i++)
        {
            GetData(routeList[i].data.dataController);
        }

        RenderManager.loadType = Enums.LoadType.Reload;
        RenderManager.ResetPath(PathController.route.path);
    }

    private void GetData(IDataController dataController)
    {
        var searchProperties = new SearchProperties(dataController.DataType);

        switch (dataController.DataType)
        {
            case Enums.DataType.Phase:                  SetPhaseSearchParameters(searchProperties);                 break;
            case Enums.DataType.Quest:                  SetQuestSearchParameters(searchProperties);                 break;
            case Enums.DataType.Objective:              SetObjectiveSearchParameters(searchProperties);             break;
            case Enums.DataType.WorldInteractable:      SetWorldInteractableSearchParameters(searchProperties);     break;
            case Enums.DataType.Task:                   SetTaskSearchParameters(searchProperties);                  break;
            case Enums.DataType.Interaction:            SetInteractionSearchParameters(searchProperties);           break;
            case Enums.DataType.InteractionDestination: SetInteractionDestinationSearchParameters(searchProperties);break;
            case Enums.DataType.Outcome:                SetOutcomeSearchParameters(searchProperties);               break;
            case Enums.DataType.Scene:                  SetSceneSearchParameters(searchProperties);                 break;
            case Enums.DataType.Region:                 SetRegionSearchParameters(searchProperties);                break;

            case Enums.DataType.PhaseSave:              SetPhaseSaveSearchParameters(searchProperties);             break;

            default: Debug.Log("CASE MISSING: " + dataController.DataType); break;
        }

        dataController.GetData(searchProperties);

        SelectOption(dataController.DataType);
    }

    #region Set search parameters
    private void SetPhaseSearchParameters(SearchProperties searchProperties)
    {
        var searchParameters = searchProperties.searchParameters.Cast<Search.Phase>().First();
        searchParameters.chapterId = new List<int>() { FindRouteByDataType(Enums.DataType.Chapter).id };
    }

    private void SetQuestSearchParameters(SearchProperties searchProperties)
    {
        var searchParameters = searchProperties.searchParameters.Cast<Search.Quest>().First();
        searchParameters.phaseId = new List<int>() { FindRouteByDataType(Enums.DataType.Phase).id };
    }

    private void SetObjectiveSearchParameters(SearchProperties searchProperties)
    {
        var searchParameters = searchProperties.searchParameters.Cast<Search.Objective>().First();
        searchParameters.questId = new List<int>() { FindRouteByDataType(Enums.DataType.Quest).id };
    }

    private void SetWorldInteractableSearchParameters(SearchProperties searchProperties)
    {
        var searchParameters = searchProperties.searchParameters.Cast<Search.WorldInteractable>().First();

        var questRoute      = FindRouteByDataType(Enums.DataType.Quest);
        var objectiveRoute  = FindRouteByDataType(Enums.DataType.Objective);
        
        if (questRoute != null)
        {
            searchParameters.requestType = Search.WorldInteractable.RequestType.GetQuestAndObjectiveWorldInteractables;

            searchParameters.questId        = new List<int>() { questRoute.id };
            searchParameters.objectiveId    = new List<int>() { objectiveRoute.id };

        } else {

            searchParameters.requestType = Search.WorldInteractable.RequestType.GetRegionWorldInteractables;

            var regionRoute = FindRouteByDataType(Enums.DataType.Region);

            searchParameters.regionId       = new List<int>() { regionRoute.id };
            searchParameters.objectiveId    = new List<int>() { 0 };
        }
    }

    private void SetTaskSearchParameters(SearchProperties searchProperties)
    {
        var searchParameters = searchProperties.searchParameters.Cast<Search.Task>().First();

        var objectiveRoute = FindRouteByDataType(Enums.DataType.Objective);

        if(objectiveRoute != null)
            searchParameters.objectiveId = new List<int>() { objectiveRoute.id };
        
        searchParameters.worldInteractableId = new List<int>() { FindRouteByDataType(Enums.DataType.WorldInteractable).id };
    }

    private void SetInteractionSearchParameters(SearchProperties searchProperties)
    {
        var searchParameters = searchProperties.searchParameters.Cast<Search.Interaction>().First();
        searchParameters.taskId = new List<int>() { FindRouteByDataType(Enums.DataType.Task).id };
    }

    private void SetInteractionDestinationSearchParameters(SearchProperties searchProperties)
    {
        var searchParameters = searchProperties.searchParameters.Cast<Search.InteractionDestination>().First();
        searchParameters.interactionId = new List<int>() { FindRouteByDataType(Enums.DataType.Interaction).id };
    }

    private void SetOutcomeSearchParameters(SearchProperties searchProperties)
    {
        var searchParameters = searchProperties.searchParameters.Cast<Search.Outcome>().First();
        searchParameters.interactionId = new List<int>() { FindRouteByDataType(Enums.DataType.Interaction).id };
    }

    private void SetSceneSearchParameters(SearchProperties searchProperties)
    {
        var searchParameters = searchProperties.searchParameters.Cast<Search.Scene>().First();
        searchParameters.outcomeId = new List<int>() { FindRouteByDataType(Enums.DataType.Outcome).id };
    }

    private void SetRegionSearchParameters(SearchProperties searchProperties)
    {
        var searchParameters = searchProperties.searchParameters.Cast<Search.Region>().First();
        
        int phaseId = 0;

        var phaseRoute = FindRouteByDataType(Enums.DataType.Phase);

        if (phaseRoute != null)
        {
            var phaseData = (PhaseElementData)phaseRoute.ElementData;
            phaseId = phaseData.Id;
        }

        var phaseSaveRoute = FindRouteByDataType(Enums.DataType.PhaseSave);

        if (phaseSaveRoute != null)
        {
            var phaseSaveData = (PhaseSaveElementData)phaseSaveRoute.ElementData;
            phaseId = phaseSaveData.PhaseId;
        }

        searchParameters.phaseId    = new List<int>() { phaseId };
        searchParameters.type       = RegionManager.regionType;
    }

    private void SetPhaseSaveSearchParameters(SearchProperties searchProperties)
    {
        var searchParameters = searchProperties.searchParameters.Cast<Search.PhaseSave>().First();

        var chapterSaveElementData = (ChapterSaveElementData)FindRouteByDataType(Enums.DataType.ChapterSave).ElementData;
        searchParameters.chapterId = new List<int>() { chapterSaveElementData.ChapterId };
    }
    #endregion

    private void SetOptions(ExDropdown dropdown, Data data)
    {
        var dataType = data.dataController.DataType;

        switch (dataType)
        {
            case Enums.DataType.Chapter:                SetChapterOptions(dropdown, data);                  break;
            case Enums.DataType.Phase:                  SetPhaseOptions(dropdown, data);                    break;
            case Enums.DataType.Quest:                  SetQuestOptions(dropdown, data);                    break;
            case Enums.DataType.Objective:              SetObjectiveOptions(dropdown, data);                break;
            case Enums.DataType.WorldInteractable:      SetWorldInteractableOptions(dropdown, data);        break;
            case Enums.DataType.Task:                   SetTaskOptions(dropdown, data);                     break;
            case Enums.DataType.Interaction:            SetInteractionOptions(dropdown, data);              break;
            case Enums.DataType.InteractionDestination: SetInteractionDestinationOptions(dropdown, data);   break;
            case Enums.DataType.Region:                 SetRegionOptions(dropdown, data);                   break;
            case Enums.DataType.Outcome:                SetOutcomeOptions(dropdown, data);                  break;
            case Enums.DataType.Scene:                  SetSceneOptions(dropdown, data);                    break;

            case Enums.DataType.ChapterSave:            SetChapterSaveOptions(dropdown, data);              break;
            case Enums.DataType.PhaseSave:              SetPhaseSaveOptions(dropdown, data);                break;
            
            default: Debug.Log("CASE MISSING: " + dataType); break;
        }

        var route = FindRouteByDataType(dataType);

        int selectedIndex = data.dataList.FindIndex(x => x.Id == route.id);

        dropdown.Dropdown.value = selectedIndex;

        dropdown.Dropdown.captionText.text = dropdown.Dropdown.options[dropdown.Dropdown.value].text;
    }

    #region Dropdown options
    private void SetChapterOptions(ExDropdown dropdown, Data data)
    {
        var elementDataList = data.dataList.Cast<ChapterElementData>().ToList();
        elementDataList.ForEach(x => dropdown.Dropdown.options.Add(new Dropdown.OptionData(x.Name)));
    }

    private void SetPhaseOptions(ExDropdown dropdown, Data data)
    {
        var elementDataList = data.dataList.Cast<PhaseElementData>().ToList();
        elementDataList.ForEach(x => dropdown.Dropdown.options.Add(new Dropdown.OptionData(x.Name)));
    }

    private void SetQuestOptions(ExDropdown dropdown, Data data)
    {
        var elementDataList = data.dataList.Cast<QuestElementData>().ToList();
        elementDataList.ForEach(x => dropdown.Dropdown.options.Add(new Dropdown.OptionData(x.Name)));
    }

    private void SetObjectiveOptions(ExDropdown dropdown, Data data)
    {
        var elementDataList = data.dataList.Cast<ObjectiveElementData>().ToList();
        elementDataList.ForEach(x => dropdown.Dropdown.options.Add(new Dropdown.OptionData(x.Name)));
    }

    private void SetWorldInteractableOptions(ExDropdown dropdown, Data data)
    {
        var elementDataList = data.dataList.Cast<WorldInteractableElementData>().ToList();
        elementDataList.ForEach(x => dropdown.Dropdown.options.Add(new Dropdown.OptionData(x.InteractableName)));
    }

    private void SetTaskOptions(ExDropdown dropdown, Data data)
    {
        var elementDataList = data.dataList.Cast<TaskElementData>().ToList();
        elementDataList.ForEach(x => dropdown.Dropdown.options.Add(new Dropdown.OptionData(x.Name)));
    }

    private void SetInteractionOptions(ExDropdown dropdown, Data data)
    {
        var elementDataList = data.dataList.Cast<InteractionElementData>().ToList();
        elementDataList.ForEach(x => dropdown.Dropdown.options.Add(new Dropdown.OptionData(x.Default ? "Default" : TimeManager.FormatTime(x.StartTime) + " - " + TimeManager.FormatTime(x.EndTime))));
    }

    private void SetInteractionDestinationOptions(ExDropdown dropdown, Data data)
    {
        var elementDataList = data.dataList.Cast<InteractionDestinationElementData>().ToList();
        elementDataList.ForEach(x => dropdown.Dropdown.options.Add(new Dropdown.OptionData(x.ExecuteType == Enums.ExecuteType.Add ? "New" : "Destination " + x.Id)));
    }

    private void SetRegionOptions(ExDropdown dropdown, Data data)
    {
        var elementDataList = data.dataList.Cast<RegionElementData>().ToList();
        elementDataList.ForEach(x => dropdown.Dropdown.options.Add(new Dropdown.OptionData(x.Name)));
    }

    private void SetOutcomeOptions(ExDropdown dropdown, Data data)
    {
        var elementDataList = data.dataList.Cast<OutcomeElementData>().ToList();
        elementDataList.ForEach(x => dropdown.Dropdown.options.Add(new Dropdown.OptionData(Enum.GetName(typeof(Enums.OutcomeType), x.Type))));
    }

    private void SetSceneOptions(ExDropdown dropdown, Data data)
    {
        var elementDataList = data.dataList.Cast<SceneElementData>().ToList();
        elementDataList.ForEach(x => dropdown.Dropdown.options.Add(new Dropdown.OptionData(x.Name)));
    }

    private void SetChapterSaveOptions(ExDropdown dropdown, Data data)
    {
        var elementDataList = data.dataList.Cast<ChapterSaveElementData>().ToList();
        elementDataList.ForEach(x => dropdown.Dropdown.options.Add(new Dropdown.OptionData(x.Name)));
    }

    private void SetPhaseSaveOptions(ExDropdown dropdown, Data data)
    {
        var elementDataList = data.dataList.Cast<PhaseSaveElementData>().ToList();
        elementDataList.ForEach(x => dropdown.Dropdown.options.Add(new Dropdown.OptionData(x.Name)));
    }
    #endregion

    public void SelectOption(Enums.DataType dataType)
    {
        var route = FindRouteByDataType(dataType);

        if (dataType == Enums.DataType.Interaction)
        {
            SelectValidInteraction(route);

        } else {

            if (!route.data.dataList.Select(y => y.Id).ToList().Contains(route.id))
                route.id = route.data.dataList.Where(x => x.ExecuteType != Enums.ExecuteType.Add).First().Id;
        }
    }

    public void SelectInteraction()
    {
        var route = FindRouteByDataType(Enums.DataType.Interaction);

        SelectValidInteraction(route);
        
        InitializePath(route.ElementData, routeList.IndexOf(route));
    }

    private void SelectValidInteraction(Route route)
    {
        var interactionDataList = route.data.dataList.Cast<InteractionElementData>().ToList();
        var validInteraction = interactionDataList.Where(x => TimeManager.TimeInFrame(TimeManager.instance.ActiveTime, x.StartTime, x.EndTime) || x.Default).OrderBy(x => x.Default).First();

        route.id = validInteraction.Id;
    }

    private Route FindRouteByDataType(Enums.DataType dataType)
    {
        return routeList.Where(x => x.data.dataController.DataType == dataType).FirstOrDefault();
    }

    public void CloseAction() { }
}
