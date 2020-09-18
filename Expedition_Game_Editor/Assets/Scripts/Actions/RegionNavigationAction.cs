using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class RegionNavigationAction : MonoBehaviour, IAction
{
    //On startup, get the data of all relevant routes in the path, based on the data types
    //Load the rest of the data "manually", to filter out dead ends

    //When changing selections, load data from every datacontroller AFTER the index of the selected tab

    public ActionProperties actionProperties;

    private List<Route> routeList = new List<Route>();

    private List<Enums.DataType> structureList;

    private Route regionRoute;

    public PathController PathController { get { return GetComponent<PathController>(); } }

    public void InitializeAction(Path path)
    {
        structureList = path.routeList.Where(x => x.data != null && x.data.dataController.DataCategory == Enums.DataCategory.Navigation)
                                      .Select(x => x.data.dataController.DataType).Distinct().ToList();

        var regionRouteSource = PathController.route.path.FindLastRoute(Enums.DataType.Region);

        regionRoute = new Route()
        {
            controllerIndex = regionRouteSource.controllerIndex,
            id = regionRouteSource.id,
            data = regionRouteSource.data,
            path = path,

            selectionStatus = regionRouteSource.selectionStatus
        };
        
        InitializeData();

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
                var interactionDestinationRouteSource = PathController.route.path.FindLastRoute(Enums.DataType.InteractionDestination);

                var interactionDestinationRoute = new Route()
                {
                    controllerIndex = (int)Enums.WorldSelectionType.InteractionDestination,
                    id = interactionDestinationRouteSource.id,
                    data = interactionDestinationRouteSource.data,
                    path = interactionDestinationRouteSource.path,
                    selectionStatus = Enums.SelectionStatus.Main
                };

                path.Add(interactionDestinationRoute);

            } else if (RegionManager.regionType == Enums.RegionType.Party) {

                var phaseRouteSource = PathController.route.path.FindLastRoute(Enums.DataType.Phase);

                var phaseRoute = new Route()
                {
                    controllerIndex = (int)Enums.WorldSelectionType.Party,
                    id = phaseRouteSource.id,
                    data = phaseRouteSource.data,
                    path = phaseRouteSource.path,
                    selectionStatus = Enums.SelectionStatus.Main
                };
                
                path.Add(phaseRoute);

            } else {

                path.Add(regionRoute);
            }
        }
    }

    private void InitializeData()
    {
        InitializeStructureData();

        var regionData = (RegionElementData)regionRoute.ElementData;
        RegionManager.regionType = regionData.Type;

        if (PathController.route.path.type == Path.Type.New)
        {
            if (RegionManager.regionType == Enums.RegionType.InteractionDestination || RegionManager.regionType == Enums.RegionType.Party)
                RegionManager.activeDisplay = RegionManager.Display.World;
            else
                RegionManager.activeDisplay = RegionManager.Display.Tiles;
        }
    }

    private void InitializeStructureData()
    {
        routeList.Clear();

        structureList.ForEach(x =>
        {
            var route = PathController.route.path.FindLastRoute(x);
            routeList.Add(route);
        });

        //Not relevant at the moment: fix when data can be added!
        //FilterData();

        for (int i = routeList.Count - 1; i >= 0; i--)
        {
            var route = routeList[i];
            var dataType = route.data.dataController.DataType;

            //var route = PathController.route.path.FindLastRoute(dataType);
            var mainRoute = PathController.layoutSection.EditorForm.activePath.FindLastRoute(dataType);

            Debug.Log(mainRoute.data.dataController);
            //route.data = mainRoute.data;
            //route.id = mainRoute.id;

            //if (route.ElementData.Id != mainRoute.ElementData.Id)
            //    actionData.id = mainRoute.ElementData.Id;

            if (route.data.dataController.DataType == Enums.DataType.Interaction)
                CheckTime((InteractionElementData)route.ElementData);
            
            //World interactable doesn't always reset as the id can be the same in multiple objectives
            if (!ListContainsElement(route, mainRoute.id))
            {
                route.data = mainRoute.data;
                ResetData(route);
            }
            else
            {
                SelectOption(dataType);
            }
                


        }
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
    
    private void ResetData(Route route)
    {
        if (route.data.dataController.DataType == Enums.DataType.InteractionDestination)
            GetInteractionDestinationDependencies(route.ElementData);

        GetData(route.data.dataController);
    }

    private void GetInteractionDestinationDependencies(IElementData elementData)
    {
        var interactionDestinationData = (InteractionDestinationElementData)elementData;

        var interactionAction = FindRouteByDataType(Enums.DataType.Interaction);
        interactionAction.id = interactionDestinationData.InteractionId;

        var taskAction = FindRouteByDataType(Enums.DataType.Task);
        taskAction.id = interactionDestinationData.TaskId;

        var worldInteractableAction = FindRouteByDataType(Enums.DataType.WorldInteractable);
        worldInteractableAction.id = interactionDestinationData.WorldInteractableId;

        var questAction = FindRouteByDataType(Enums.DataType.Quest);

        if (questAction != null)
            questAction.id = interactionDestinationData.QuestId;

        var objectiveAction = FindRouteByDataType(Enums.DataType.Objective);

        if (objectiveAction != null)
            objectiveAction.id = interactionDestinationData.ObjectiveId;
    }

    #region Data Filter
    //Remove all dead ends from data
    /*
    private void FilterData()
    {
        if (structureList.Contains(Enums.DataType.WorldInteractable))
        {
            var worldInteractableData = Fixtures.worldInteractableList;
            var worldInteractableAction = FindActionByDataType(Enums.DataType.WorldInteractable);
            worldInteractableAction.idFilter = worldInteractableData.Select(x => x.Id).Distinct().ToList();
        }

        if (structureList.Contains(Enums.DataType.Task))
        {
            if (structureList.Contains(Enums.DataType.WorldInteractable))
            {
                var worldInteractableData = FindActionByDataType(Enums.DataType.WorldInteractable).idFilter;

                var taskData = Fixtures.taskList.Where(x => worldInteractableData.Contains(x.worldInteractableId)).Distinct().ToList();
                var taskAction = FindActionByDataType(Enums.DataType.Task);
                taskAction.idFilter = taskData.Select(x => x.Id).Distinct().ToList();
            }
        }

        if (structureList.Contains(Enums.DataType.Interaction))
        {
            //if (structureList.Contains(Enums.DataType.Interaction))
            //{
            //    var idList = FindActionByDataType(Enums.DataType.Interaction).idFilter;
            //    var interactionData = Fixtures.interactionList.Where(x => idList.Contains(x.Id)).Distinct().ToList();

            //    var objectiveData = Fixtures.objectiveList.Where(x => interactionData.Select(y => y.objectiveId).Contains(x.Id)).Distinct().ToList();
            //    var objectiveAction = FindActionByDataType(Enums.DataType.Objective);
            //    objectiveAction.idFilter = objectiveData.Select(x => x.Id).Distinct().ToList();
            //}
        }

        if (structureList.Contains(Enums.DataType.Objective))
        {
            if (structureList.Contains(Enums.DataType.Task))
            {
                var idList = FindActionByDataType(Enums.DataType.Task).idFilter;
                var taskData = Fixtures.taskList.Where(x => idList.Contains(x.Id)).Distinct().ToList();

                var objectiveData = Fixtures.objectiveList.Where(x => taskData.Select(y => y.objectiveId).Contains(x.Id)).Distinct().ToList();
                var objectiveAction = FindActionByDataType(Enums.DataType.Objective);
                objectiveAction.idFilter = objectiveData.Select(x => x.Id).Distinct().ToList();
            }
        }

        if (structureList.Contains(Enums.DataType.Quest))
        {
            if (structureList.Contains(Enums.DataType.Objective))
            {
                var idList = FindActionByDataType(Enums.DataType.Objective).idFilter;
                var objectiveData = Fixtures.objectiveList.Where(x => idList.Contains(x.Id)).Distinct().ToList();

                var questData = Fixtures.questList.Where(x => objectiveData.Select(y => y.questId).Contains(x.Id)).Distinct().ToList();
                var questAction = FindActionByDataType(Enums.DataType.Quest);
                questAction.idFilter = questData.Select(x => x.Id).Distinct().ToList();
            }
        }

        if (structureList.Contains(Enums.DataType.Phase))
        {
            if (structureList.Contains(Enums.DataType.Quest))
            {
                var idList = FindActionByDataType(Enums.DataType.Quest).idFilter;
                var questData = Fixtures.questList.Where(x => idList.Contains(x.Id)).Distinct().ToList();

                var phaseData = Fixtures.phaseList.Where(x => questData.Select(y => y.phaseId).Contains(x.Id)).Distinct().ToList();
                var phaseAction = FindActionByDataType(Enums.DataType.Phase);
                phaseAction.idFilter = phaseData.Select(x => x.Id).Distinct().ToList();

            } else {

                var phaseRegionIds = Fixtures.regionList.Where(x => x.phaseId != 0).Distinct().ToList();

                var phaseData = Fixtures.phaseList.Where(x => phaseRegionIds.Select(y => y.phaseId).Contains(x.Id)).Distinct().ToList();
                var phaseAction = FindActionByDataType(Enums.DataType.Phase);
                phaseAction.idFilter = phaseData.Select(x => x.Id).Distinct().ToList();
            }
        }

        if (structureList.Contains(Enums.DataType.Chapter))
        {
            if (structureList.Contains(Enums.DataType.Phase))
            {
                var idList = FindActionByDataType(Enums.DataType.Phase).idFilter;
                var phaseData = Fixtures.phaseList.Where(x => idList.Contains(x.Id)).Distinct().ToList();

                var chapterData = Fixtures.phaseList.Where(x => phaseData.Select(y => y.chapterId).Contains(x.Id)).Distinct().ToList();
                var chapterAction = FindActionByDataType(Enums.DataType.Chapter);
                chapterAction.idFilter = chapterData.Select(x => x.Id).Distinct().ToList();
            }
        }

        if (structureList.Contains(Enums.DataType.Region))
        {
            if (structureList.Contains(Enums.DataType.Phase))
            {
                var phaseData = FindActionByDataType(Enums.DataType.Phase).idFilter;

                var regionData = Fixtures.regionList.Where(x => phaseData.Contains(x.phaseId)).Distinct().ToList();
                var regionAction = FindActionByDataType(Enums.DataType.Region);
                regionAction.idFilter = regionData.Select(x => x.Id).Distinct().ToList();

            } else {

                var regionAction = FindActionByDataType(Enums.DataType.Region);
                var regionData = Fixtures.regionList.Where(x => x.phaseId == 0).Distinct().ToList();
                regionAction.idFilter = regionData.Select(x => x.Id).Distinct().ToList();
            }
        }
    }
    */
    #endregion

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

        var path = PathController.route.path;

        RenderManager.loadType = Enums.LoadType.Reload;

        for (int i = (index + 1); i < routeList.Count; i++)
            GetData(routeList[i].data.dataController);

        RenderManager.ResetPath(PathController.route.path);
    }

    private void GetData(IDataController dataController)
    {
        SearchProperties searchProperties = new SearchProperties(dataController.DataType);

        switch (dataController.DataType)
        {
            case Enums.DataType.Chapter:                GetChapterData(searchProperties);               break;
            case Enums.DataType.Phase:                  GetPhaseData(searchProperties);                 break;
            case Enums.DataType.Quest:                  GetQuestData(searchProperties);                 break;
            case Enums.DataType.Objective:              GetObjectiveData(searchProperties);             break;
            case Enums.DataType.WorldInteractable:      GetWorldInteractableData(searchProperties);     break;
            case Enums.DataType.Task:                   GetTaskData(searchProperties);                  break;
            case Enums.DataType.Interaction:            GetInteractionData(searchProperties);           break;
            case Enums.DataType.InteractionDestination: GetInteractionDestinationData(searchProperties);break;
            case Enums.DataType.Region:                 GetRegionData(searchProperties);                break;

            case Enums.DataType.PhaseSave:              GetPhaseSaveData(searchProperties);             break;

            default: Debug.Log("CASE MISSING: " + dataController.DataType); break;
        }
        
        var actionData = FindRouteByDataType(dataController.DataType);
        
        dataController.GetData(searchProperties);

        Debug.Log(dataController);
        Debug.Log(dataController.Data.dataList.Count);
        //DataManager.GetData(dataController, searchProperties);

        //actionData.data = dataController.Data;

        SelectOption(dataController.DataType);
    }

    #region Get Data
    private void GetChapterData(SearchProperties searchProperties)
    {
        //var searchParameters = searchProperties.searchParameters.Cast<Search.Chapter>().First();
        //searchProperties.searchParameters = FindRouteByDataType(searchProperties.dataType).idFilter;
    }

    private void GetPhaseData(SearchProperties searchProperties)
    {
        var searchParameters = searchProperties.searchParameters.Cast<Search.Phase>().First();
        searchParameters.chapterId = new List<int>() { FindRouteByDataType(Enums.DataType.Chapter).ElementData.Id };
    }

    private void GetQuestData(SearchProperties searchProperties)
    {
        var searchParameters = searchProperties.searchParameters.Cast<Search.Quest>().First();
        searchParameters.phaseId = new List<int>() { FindRouteByDataType(Enums.DataType.Phase).ElementData.Id };
    }

    private void GetObjectiveData(SearchProperties searchProperties)
    {
        var searchParameters = searchProperties.searchParameters.Cast<Search.Objective>().First();
        searchParameters.questId = new List<int>() { FindRouteByDataType(Enums.DataType.Quest).ElementData.Id };
    }

    private void GetWorldInteractableData(SearchProperties searchProperties)
    {
        var searchParameters = searchProperties.searchParameters.Cast<Search.WorldInteractable>().First();

        var questAction = FindRouteByDataType(Enums.DataType.Quest);
        var objectiveAction = FindRouteByDataType(Enums.DataType.Objective);

        if (questAction != null)
        {
            searchParameters.requestType = Search.WorldInteractable.RequestType.GetQuestAndObjectiveWorldInteractables;

            searchParameters.questId        = new List<int>() { questAction.ElementData.Id };
            searchParameters.objectiveId    = new List<int>() { objectiveAction.ElementData.Id };

        } else {

            searchParameters.requestType = Search.WorldInteractable.RequestType.GetRegionWorldInteractables;

            var regionAction = FindRouteByDataType(Enums.DataType.Region);

            searchParameters.regionId = new List<int>() { regionAction.ElementData.Id };

            searchParameters.objectiveId = new List<int>() { 0 };
        }
    }

    private void GetTaskData(SearchProperties searchProperties)
    {
        var searchParameters = searchProperties.searchParameters.Cast<Search.Task>().First();

        var objectiveAction = FindRouteByDataType(Enums.DataType.Objective);

        if(objectiveAction != null)
            searchParameters.objectiveId = new List<int>() { objectiveAction.ElementData.Id };

        searchParameters.worldInteractableId = new List<int>() { FindRouteByDataType(Enums.DataType.WorldInteractable).ElementData.Id };
    }

    private void GetInteractionData(SearchProperties searchProperties)
    {
        var searchParameters = searchProperties.searchParameters.Cast<Search.Interaction>().First();
        searchParameters.taskId = new List<int>() { FindRouteByDataType(Enums.DataType.Task).ElementData.Id };
    }

    private void GetInteractionDestinationData(SearchProperties searchProperties)
    {
        var searchParameters = searchProperties.searchParameters.Cast<Search.InteractionDestination>().First();
        searchParameters.interactionId = new List<int>() { FindRouteByDataType(Enums.DataType.Interaction).ElementData.Id };
    }

    private void GetRegionData(SearchProperties searchProperties)
    {
        var searchParameters = searchProperties.searchParameters.Cast<Search.Region>().First();
        
        int phaseId = 0;

        var phaseAction = FindRouteByDataType(Enums.DataType.Phase);

        if (phaseAction != null)
        {
            var phaseData = (PhaseElementData)phaseAction.ElementData;
            phaseId = phaseData.Id;
        }

        var phaseSaveAction = FindRouteByDataType(Enums.DataType.PhaseSave);

        if (phaseSaveAction != null)
        {
            var phaseSaveData = (PhaseSaveElementData)phaseSaveAction.ElementData;
            phaseId = phaseSaveData.PhaseId;
        }

        searchParameters.phaseId = new List<int>() { phaseId };
    }

    private void GetPhaseSaveData(SearchProperties searchProperties)
    {
        var searchParameters = searchProperties.searchParameters.Cast<Search.PhaseSave>().First();
        searchParameters.requestType = Search.PhaseSave.RequestType.GetPhaseSaveByChapter;

        var chapterSaveData = (ChapterSaveElementData)FindRouteByDataType(Enums.DataType.ChapterSave).ElementData;
        searchParameters.chapterId = new List<int>() { chapterSaveData.ChapterId };
    }
    #endregion

    private void SetOptions(ExDropdown dropdown, Data data)
    {
        var dataType = data.dataController.DataType;

        switch (dataType)
        {
            case Enums.DataType.Chapter:                SetChapterOptions(dropdown, data);                break;
            case Enums.DataType.Phase:                  SetPhaseOptions(dropdown, data);                  break;
            case Enums.DataType.Quest:                  SetQuestOptions(dropdown, data);                  break;
            case Enums.DataType.Objective:              SetObjectiveOptions(dropdown, data);              break;
            case Enums.DataType.WorldInteractable:      SetWorldInteractableOptions(dropdown, data);      break;
            case Enums.DataType.Task:                   SetTaskOptions(dropdown, data);                   break;
            case Enums.DataType.Interaction:            SetInteractionOptions(dropdown, data);            break;
            case Enums.DataType.InteractionDestination: SetInteractionDestinationOptions(dropdown, data); break;
            case Enums.DataType.Region:                 SetRegionOptions(dropdown, data);                 break;

            case Enums.DataType.ChapterSave:            SetChapterSaveOptions(dropdown, data);            break;
            case Enums.DataType.PhaseSave:              SetPhaseSaveOptions(dropdown, data);              break;
            
            default: Debug.Log("CASE MISSING: " + dataType); break;
        }

        var action = FindRouteByDataType(dataType);

        int selectedIndex = data.dataList.FindIndex(x => x.Id == action.ElementData.Id);

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
        elementDataList.ForEach(x => dropdown.Dropdown.options.Add(new Dropdown.OptionData("Destination " + x.Id)));
    }

    private void SetRegionOptions(ExDropdown dropdown, Data data)
    {
        var elementDataList = data.dataList.Cast<RegionElementData>().ToList();
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
        var actionData = FindRouteByDataType(dataType);

        if (dataType == Enums.DataType.Interaction)
        {
            SelectValidInteraction(actionData);

            PathController.layoutSection.EditorForm.activePath.ReplaceAllData(actionData.data);

        } else {

            if (!actionData.data.dataList.Select(y => y.Id).ToList().Contains(actionData.ElementData.Id))
                actionData.id = actionData.data.dataList.First().Id;

            //Assigns an element from the data list as the main element so that it may be editted instead of the clone
            actionData.id = actionData.data.dataList.First(x => x.Id == actionData.ElementData.Id).Id;

            PathController.route.path.ReplaceAllData(actionData.data);
        }
    }

    public void SelectInteraction()
    {
        var actionData = FindRouteByDataType(Enums.DataType.Interaction);

        SelectValidInteraction(actionData);
        
        InitializePath(actionData.ElementData, routeList.IndexOf(actionData));
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
