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

    internal class ActionData
    {
        public Route.Data data;
        public bool reset;
        public List<int> idFilter = new List<int>();

        public ActionData(Route.Data data)
        {
            this.data = new Route.Data(data);
            this.data.dataElement = data.dataElement.Clone();
        }
    }

    public bool active;

    private Enums.RegionType regionType;

    public RegionDisplayManager defaultDisplay;

    public PathController PathController { get { return GetComponent<PathController>(); } }

    public ActionProperties actionProperties;

    private List<ActionData> actionDataList = new List<ActionData>();

    private List<Enums.DataType> structureList;

    private Route regionRoute;

    public void InitializeAction(Path path)
    {
        active = true;

        structureList = path.route.Where(x => x.data.dataController != null && x.data.dataController.DataCategory == Enums.DataCategory.Navigation)
                                  .Select(x => x.data.dataController.DataType).Distinct().ToList();

        regionRoute = PathController.route.path.FindLastRoute(Enums.DataType.Region).Copy();

        InitializeData();

        //The region route only gets added at the end when the action is initialized
        if (path.route.Count < (PathController.route.path.route.Count + 1))
        {
            int index = (int)RegionDisplayManager.activeDisplay;

            regionRoute.controller = index;

            if (RegionDisplayManager.activeDisplay == RegionDisplayManager.Display.World)
                path.Add(regionRoute);

            if (regionType != Enums.RegionType.Interaction)
            {
                path.Add(regionRoute);

            } else {

                //Adds the selected interaction to the path
                var interactionRoute = PathController.route.path.FindFirstRoute(Enums.DataType.Interaction);
                interactionRoute.controller = 0;

                path.Add(interactionRoute);
            }
        }
    }

    private void InitializeData()
    {
        InitializeStructureData();

        if (PathController.route.path.type == Path.Type.New)
        {
            var regionDataElement = (RegionDataElement)regionRoute.data.dataList.FirstOrDefault();
            regionType = regionDataElement.type;

            if (regionType == Enums.RegionType.Interaction)
                RegionDisplayManager.activeDisplay = RegionDisplayManager.Display.World;
            else
                RegionDisplayManager.activeDisplay = RegionDisplayManager.Display.Tiles;
        }
    }

    private void InitializeStructureData()
    {
        actionDataList.Clear();

        structureList.ForEach(x =>
        {
            var data = PathController.route.path.FindLastRoute(x).data;

            actionDataList.Add(new ActionData(data));
        });

        //Not relevant at the moment: fix when data can be added!
        //FilterData();

        for (int i = actionDataList.Count - 1; i >= 0; i--)
        {
            var actionData = actionDataList[i];
            var dataType = actionData.data.dataController.DataType;

            var route = PathController.route.path.FindLastRoute(dataType);
            var mainRoute = PathController.layoutSection.EditorForm.activePath.FindLastRoute(dataType);

            if (route.GeneralData.Id != mainRoute.GeneralData.Id)
                actionData.data.dataElement = mainRoute.data.dataElement;

            if (actionData.data.dataController.DataType == Enums.DataType.Interaction)
                CheckTime((InteractionDataElement)actionData.data.dataElement);

            //World interactable doesn't always reset as the id can be the same in multiple objectives
            if (!ListContainsElement(actionData))
                ResetData(actionData);
            else
                SelectOption(dataType);
        }

        for (int i = actionDataList.Count - 1; i >= 0; i--)
        {
            var actionData = actionDataList[i];

            actionData.reset = false;
        }
    }
    
    private bool ListContainsElement(ActionData actionData)
    {
        return actionData.data.dataList.Select(y => y.Id).ToList().Contains(actionData.data.dataElement.Id);
    }
    
    private void CheckTime(IDataElement dataElement)
    {
        var interactionData = (InteractionDataElement)dataElement;

        var startTime   = interactionData.Default ? TimeManager.DefaultTime(interactionData.defaultTimes) : interactionData.StartTime;
        var endTime     = interactionData.Default ? TimeManager.DefaultTime(interactionData.defaultTimes) : interactionData.EndTime;
        
        if (!TimeManager.TimeInFrame(TimeManager.activeTime, startTime, endTime))
            TimeManager.instance.SetTime(startTime);  
    }
    
    private void ResetData(ActionData actionData)
    {
        if (actionData.data.dataController.DataType == Enums.DataType.Interaction)
            GetInteractionDependencies(actionData.data.dataElement);

        GetData(actionData.data.dataController);
    }

    private void GetInteractionDependencies(IDataElement dataElement)
    {
        var interactionData = (InteractionDataElement)dataElement;

        var taskAction = FindActionByDataType(Enums.DataType.Task);
        taskAction.data.dataElement.Id = interactionData.TaskId;

        var worldInteractableAction = FindActionByDataType(Enums.DataType.WorldInteractable);
        worldInteractableAction.data.dataElement.Id = interactionData.worldInteractableId;

        var questAction = FindActionByDataType(Enums.DataType.Quest);

        if (questAction != null)
            questAction.data.dataElement.Id = interactionData.questId;

        var objectiveAction = FindActionByDataType(Enums.DataType.Objective);

        if (objectiveAction != null)
            objectiveAction.data.dataElement.Id = interactionData.objectiveId;
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
        actionDataList.ForEach(x => SetDropdown(x));
    }

    private void SetDropdown(ActionData actionData)
    {
        var dropdown = ActionManager.instance.AddDropdown(actionProperties);

        SetOptions(dropdown, actionData);

        dropdown.Dropdown.onValueChanged.AddListener(delegate
        {
            InitializePath(actionData.data.dataList[dropdown.Dropdown.value], actionDataList.IndexOf(actionData));
        });
    }

    private void InitializePath(IDataElement dataElement, int index)
    {
        actionDataList[index].data.dataElement = dataElement;

        var path = PathController.route.path;

        RenderManager.loadType = Enums.LoadType.Reload;

        for (int i = (index + 1); i < actionDataList.Count; i++)
            GetData(actionDataList[i].data.dataController);

        RenderManager.ResetPath(PathController.route.path);
    }

    private void GetData(IDataController dataController)
    {
        SearchProperties searchProperties = new SearchProperties(dataController.DataType);

        switch (dataController.DataType)
        {
            case Enums.DataType.Chapter:            GetChapterData(searchProperties);            break;
            case Enums.DataType.Phase:              GetPhaseData(searchProperties);              break;
            case Enums.DataType.Quest:              GetQuestData(searchProperties);              break;
            case Enums.DataType.Objective:          GetObjectiveData(searchProperties);          break;
            case Enums.DataType.WorldInteractable:  GetWorldInteractableData(searchProperties);  break;
            case Enums.DataType.Task:               GetTaskData(searchProperties);               break;
            case Enums.DataType.Interaction:        GetInteractionData(searchProperties);        break;
            case Enums.DataType.Region:             GetRegionData(searchProperties);             break;
        }
        
        var actionData = FindActionByDataType(dataController.DataType);

        actionData.data.dataList = RenderManager.GetData(dataController, searchProperties);

        SelectOption(dataController.DataType);
    }

    #region Get Data
    private void GetChapterData(SearchProperties searchProperties)
    {
        var searchParameters = searchProperties.searchParameters.Cast<Search.Chapter>().First();

        searchProperties.searchParameters = FindActionByDataType(searchProperties.dataType).idFilter;
    }

    private void GetPhaseData(SearchProperties searchProperties)
    {
        var searchParameters = searchProperties.searchParameters.Cast<Search.Phase>().First();

        searchParameters.chapterId = new List<int>() { FindActionByDataType(Enums.DataType.Chapter).data.dataElement.Id };
    }

    private void GetQuestData(SearchProperties searchProperties)
    {
        var searchParameters = searchProperties.searchParameters.Cast<Search.Quest>().First();

        searchParameters.phaseId = new List<int>() { FindActionByDataType(Enums.DataType.Phase).data.dataElement.Id };
    }

    private void GetObjectiveData(SearchProperties searchProperties)
    {
        var searchParameters = searchProperties.searchParameters.Cast<Search.Objective>().First();

        searchParameters.questId = new List<int>() { FindActionByDataType(Enums.DataType.Quest).data.dataElement.Id };
    }

    private void GetWorldInteractableData(SearchProperties searchProperties)
    {
        var searchParameters = searchProperties.searchParameters.Cast<Search.WorldInteractable>().First();

        var questAction = FindActionByDataType(Enums.DataType.Quest);
        var objectiveAction = FindActionByDataType(Enums.DataType.Objective);

        if (questAction != null)
        {
            searchParameters.requestType = Search.WorldInteractable.RequestType.GetQuestAndObjectiveWorldInteractables;

            searchParameters.questId        = new List<int>() { questAction.data.dataElement.Id };
            searchParameters.objectiveId    = new List<int>() { objectiveAction.data.dataElement.Id };

        } else {

            searchParameters.requestType = Search.WorldInteractable.RequestType.GetRegionWorldInteractables;

            var regionRoute = FindActionByDataType(Enums.DataType.Region);

            searchParameters.regionId = new List<int>() { regionRoute.data.dataElement.Id };

            searchParameters.objectiveId = new List<int>() { 0 };
        }
    }

    private void GetTaskData(SearchProperties searchProperties)
    {
        var searchParameters = searchProperties.searchParameters.Cast<Search.Task>().First();

        var objectiveAction = FindActionByDataType(Enums.DataType.Objective);

        if(objectiveAction != null)
            searchParameters.objectiveId = new List<int>() { objectiveAction.data.dataElement.Id };

        searchParameters.worldInteractableId = new List<int>() { FindActionByDataType(Enums.DataType.WorldInteractable).data.dataElement.Id };
    }

    private void GetInteractionData(SearchProperties searchProperties)
    {
        var searchParameters = searchProperties.searchParameters.Cast<Search.Interaction>().First();

        searchParameters.taskId = new List<int>() { FindActionByDataType(Enums.DataType.Task).data.dataElement.Id };
    }

    private void GetRegionData(SearchProperties searchProperties)
    {
        var searchParameters = searchProperties.searchParameters.Cast<Search.Region>().First();

        var phaseAction = FindActionByDataType(Enums.DataType.Phase);
        int phaseId = phaseAction != null ? phaseAction.data.dataElement.Id : 0;

        searchParameters.phaseId = new List<int>() { phaseId };
    }
    #endregion

    private void SetOptions(ExDropdown dropdown, ActionData actionData)
    {
        var dataType = actionData.data.dataController.DataType;

        switch (actionData.data.dataController.DataType)
        {
            case Enums.DataType.Chapter:            SetChapterOptions(dropdown, actionData);            break;
            case Enums.DataType.Phase:              SetPhaseOptions(dropdown, actionData);              break;
            case Enums.DataType.Quest:              SetQuestOptions(dropdown, actionData);              break;
            case Enums.DataType.Objective:          SetObjectiveOptions(dropdown, actionData);          break;
            case Enums.DataType.WorldInteractable:  SetWorldInteractableOptions(dropdown, actionData);  break;
            case Enums.DataType.Task:               SetTaskOptions(dropdown, actionData);               break;
            case Enums.DataType.Interaction:        SetInteractionOptions(dropdown, actionData);        break;
            case Enums.DataType.Region:             SetRegionOptions(dropdown, actionData);             break;
        }

        var data = FindActionByDataType(actionData.data.dataController.DataType).data;

        int selectedIndex = actionData.data.dataList.Cast<GeneralData>().ToList().FindIndex(x => x.Id == data.dataElement.Id);

        dropdown.Dropdown.value = selectedIndex;

        dropdown.Dropdown.captionText.text = dropdown.Dropdown.options[dropdown.Dropdown.value].text;
    }

    #region Dropdown options
    private void SetChapterOptions(ExDropdown dropdown, ActionData actionData)
    {
        var dataElements = actionData.data.dataList.Cast<ChapterDataElement>().ToList();
        dataElements.ForEach(x => dropdown.Dropdown.options.Add(new Dropdown.OptionData(x.Name)));
    }

    private void SetPhaseOptions(ExDropdown dropdown, ActionData actionData)
    {
        var dataElements = actionData.data.dataList.Cast<PhaseDataElement>().ToList();
        dataElements.ForEach(x => dropdown.Dropdown.options.Add(new Dropdown.OptionData(x.Name)));
    }

    private void SetQuestOptions(ExDropdown dropdown, ActionData actionData)
    {
        var dataElements = actionData.data.dataList.Cast<QuestDataElement>().ToList();
        dataElements.ForEach(x => dropdown.Dropdown.options.Add(new Dropdown.OptionData(x.Name)));
    }

    private void SetObjectiveOptions(ExDropdown dropdown, ActionData actionData)
    {
        var dataElements = actionData.data.dataList.Cast<ObjectiveDataElement>().ToList();
        dataElements.ForEach(x => dropdown.Dropdown.options.Add(new Dropdown.OptionData(x.Name)));
    }

    private void SetWorldInteractableOptions(ExDropdown dropdown, ActionData actionData)
    {
        var dataElements = actionData.data.dataList.Cast<WorldInteractableDataElement>().ToList();
        dataElements.ForEach(x => dropdown.Dropdown.options.Add(new Dropdown.OptionData(x.interactableName)));
    }

    private void SetTaskOptions(ExDropdown dropdown, ActionData actionData)
    {
        var dataElements = actionData.data.dataList.Cast<TaskDataElement>().ToList();
        dataElements.ForEach(x => dropdown.Dropdown.options.Add(new Dropdown.OptionData(x.Name)));
    }

    private void SetInteractionOptions(ExDropdown dropdown, ActionData actionData)
    {
        var dataElements = actionData.data.dataList.Cast<InteractionDataElement>().ToList();
        dataElements.ForEach(x => dropdown.Dropdown.options.Add(new Dropdown.OptionData(x.Default ? "Default" : TimeManager.FormatTime(x.StartTime, true) + " - " + TimeManager.FormatTime(x.EndTime))));
    }

    private void SetRegionOptions(ExDropdown dropdown, ActionData actionData)
    {
        var dataElements = actionData.data.dataList.Cast<RegionDataElement>().ToList();
        dataElements.ForEach(x => dropdown.Dropdown.options.Add(new Dropdown.OptionData(x.Name)));
    }
    #endregion

    public void SelectOption(Enums.DataType dataType)
    {
        var actionData = FindActionByDataType(dataType);

        if(dataType == Enums.DataType.Interaction)
        {
            SelectValidInteraction(actionData);

            PathController.layoutSection.EditorForm.activePath.ReplaceAllData(actionData.data);

        } else {

            if (!actionData.data.dataList.Select(y => y.Id).ToList().Contains(actionData.data.dataElement.Id))
                actionData.data.dataElement = actionData.data.dataList.First();

            PathController.route.path.ReplaceAllData(actionData.data);
        } 
    }

    private void SelectValidInteraction(ActionData actionData)
    {
        var interactionDataList = actionData.data.dataList.Cast<InteractionDataElement>().ToList();
        var validInteraction = interactionDataList.Where(x => TimeManager.TimeInFrame(TimeManager.activeTime, x.StartTime, x.EndTime) || x.Default).OrderBy(x => x.Default).First();

        actionData.data.dataElement = validInteraction;
    }

    private ActionData FindActionByDataType(Enums.DataType dataType)
    {
        return actionDataList.Where(x => x.data.dataController.DataType == dataType).FirstOrDefault();
    }

    public void CloseAction()
    {
        active = false;
    }
}
