﻿using UnityEngine;
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
        public Data data;
        public IElementData elementData;
        public bool reset;
        public List<int> idFilter = new List<int>();

        public ActionData(Data data)
        {
            //this.data = new Data(data.dataController);
            //this.elementData = data.elementData.Clone();
        }
    }
    
    public ActionProperties actionProperties;

    private List<ActionData> actionDataList = new List<ActionData>();

    private List<Enums.DataType> structureList;

    private Route regionRoute;

    public PathController PathController { get { return GetComponent<PathController>(); } }

    public void InitializeAction(Path path)
    {
        structureList = path.routeList.Where(x => x.data.dataController != null && x.data.dataController.DataCategory == Enums.DataCategory.Navigation)
                                  .Select(x => x.data.dataController.DataType).Distinct().ToList();
        
        regionRoute = PathController.route.path.FindLastRoute(Enums.DataType.Region).Clone();

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
                //Must copy route or the source will have its values changed
                var interactionDestinationRoute = PathController.route.path.FindLastRoute(Enums.DataType.InteractionDestination).Clone();

                interactionDestinationRoute.controllerIndex = (int)Enums.WorldSelectionType.InteractionDestination;
                interactionDestinationRoute.selectionStatus = Enums.SelectionStatus.Main;

                path.Add(interactionDestinationRoute);

            } else if (RegionManager.regionType == Enums.RegionType.Party) {

                var phaseRoute = PathController.route.path.FindLastRoute(Enums.DataType.Phase).Clone();

                phaseRoute.controllerIndex = (int)Enums.WorldSelectionType.Party;
                phaseRoute.selectionStatus = Enums.SelectionStatus.Main;

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

            if (route.ElementData.Id != mainRoute.ElementData.Id)
                actionData.elementData = mainRoute.ElementData;

            if (actionData.data.dataController.DataType == Enums.DataType.Interaction)
                CheckTime((InteractionElementData)actionData.elementData);

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
        return actionData.data.dataList.Select(y => y.Id).ToList().Contains(actionData.elementData.Id);
    }
    
    private void CheckTime(IElementData elementData)
    {
        var interactionData = (InteractionElementData)elementData;

        var startTime   = interactionData.Default ? TimeManager.instance.DefaultTime(interactionData.DefaultTimes) : interactionData.StartTime;
        var endTime     = interactionData.Default ? TimeManager.instance.DefaultTime(interactionData.DefaultTimes) : interactionData.EndTime;

        if (!TimeManager.TimeInFrame(TimeManager.instance.ActiveTime, startTime, endTime))
            TimeManager.instance.SetEditorTime(startTime);
    }
    
    private void ResetData(ActionData actionData)
    {
        if (actionData.data.dataController.DataType == Enums.DataType.InteractionDestination)
            GetInteractionDestinationDependencies(actionData.elementData);

        GetData(actionData.data.dataController);
    }

    private void GetInteractionDestinationDependencies(IElementData elementData)
    {
        var interactionDestinationData = (InteractionDestinationElementData)elementData;

        var interactionAction = FindActionByDataType(Enums.DataType.Interaction);
        interactionAction.elementData.Id = interactionDestinationData.InteractionId;

        var taskAction = FindActionByDataType(Enums.DataType.Task);
        taskAction.elementData.Id = interactionDestinationData.TaskId;

        var worldInteractableAction = FindActionByDataType(Enums.DataType.WorldInteractable);
        worldInteractableAction.elementData.Id = interactionDestinationData.WorldInteractableId;

        var questAction = FindActionByDataType(Enums.DataType.Quest);

        if (questAction != null)
            questAction.elementData.Id = interactionDestinationData.QuestId;

        var objectiveAction = FindActionByDataType(Enums.DataType.Objective);

        if (objectiveAction != null)
            objectiveAction.elementData.Id = interactionDestinationData.ObjectiveId;
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

    private void InitializePath(IElementData elementData, int index)
    {
        actionDataList[index].elementData = elementData;

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
        
        var actionData = FindActionByDataType(dataController.DataType);
        
        dataController.GetData(searchProperties);

        //DataManager.GetData(dataController, searchProperties);

        //actionData.data = dataController.Data;

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
        searchParameters.chapterId = new List<int>() { FindActionByDataType(Enums.DataType.Chapter).elementData.Id };
    }

    private void GetQuestData(SearchProperties searchProperties)
    {
        var searchParameters = searchProperties.searchParameters.Cast<Search.Quest>().First();
        searchParameters.phaseId = new List<int>() { FindActionByDataType(Enums.DataType.Phase).elementData.Id };
    }

    private void GetObjectiveData(SearchProperties searchProperties)
    {
        var searchParameters = searchProperties.searchParameters.Cast<Search.Objective>().First();
        searchParameters.questId = new List<int>() { FindActionByDataType(Enums.DataType.Quest).elementData.Id };
    }

    private void GetWorldInteractableData(SearchProperties searchProperties)
    {
        var searchParameters = searchProperties.searchParameters.Cast<Search.WorldInteractable>().First();

        var questAction = FindActionByDataType(Enums.DataType.Quest);
        var objectiveAction = FindActionByDataType(Enums.DataType.Objective);

        if (questAction != null)
        {
            searchParameters.requestType = Search.WorldInteractable.RequestType.GetQuestAndObjectiveWorldInteractables;

            searchParameters.questId        = new List<int>() { questAction.elementData.Id };
            searchParameters.objectiveId    = new List<int>() { objectiveAction.elementData.Id };

        } else {

            searchParameters.requestType = Search.WorldInteractable.RequestType.GetRegionWorldInteractables;

            var regionAction = FindActionByDataType(Enums.DataType.Region);

            searchParameters.regionId = new List<int>() { regionAction.elementData.Id };

            searchParameters.objectiveId = new List<int>() { 0 };
        }
    }

    private void GetTaskData(SearchProperties searchProperties)
    {
        var searchParameters = searchProperties.searchParameters.Cast<Search.Task>().First();

        var objectiveAction = FindActionByDataType(Enums.DataType.Objective);

        if(objectiveAction != null)
            searchParameters.objectiveId = new List<int>() { objectiveAction.elementData.Id };

        searchParameters.worldInteractableId = new List<int>() { FindActionByDataType(Enums.DataType.WorldInteractable).elementData.Id };
    }

    private void GetInteractionData(SearchProperties searchProperties)
    {
        var searchParameters = searchProperties.searchParameters.Cast<Search.Interaction>().First();
        searchParameters.taskId = new List<int>() { FindActionByDataType(Enums.DataType.Task).elementData.Id };
    }

    private void GetInteractionDestinationData(SearchProperties searchProperties)
    {
        var searchParameters = searchProperties.searchParameters.Cast<Search.InteractionDestination>().First();
        searchParameters.interactionId = new List<int>() { FindActionByDataType(Enums.DataType.Interaction).elementData.Id };
    }

    private void GetRegionData(SearchProperties searchProperties)
    {
        var searchParameters = searchProperties.searchParameters.Cast<Search.Region>().First();
        
        int phaseId = 0;

        var phaseAction = FindActionByDataType(Enums.DataType.Phase);

        if (phaseAction != null)
        {
            var phaseData = (PhaseElementData)phaseAction.elementData;
            phaseId = phaseData.Id;
        }

        var phaseSaveAction = FindActionByDataType(Enums.DataType.PhaseSave);

        if (phaseSaveAction != null)
        {
            var phaseSaveData = (PhaseSaveElementData)phaseSaveAction.elementData;
            phaseId = phaseSaveData.PhaseId;
        }

        searchParameters.phaseId = new List<int>() { phaseId };
    }

    private void GetPhaseSaveData(SearchProperties searchProperties)
    {
        var searchParameters = searchProperties.searchParameters.Cast<Search.PhaseSave>().First();
        searchParameters.requestType = Search.PhaseSave.RequestType.GetPhaseSaveByChapter;

        var chapterSaveData = (ChapterSaveElementData)FindActionByDataType(Enums.DataType.ChapterSave).elementData;
        searchParameters.chapterId = new List<int>() { chapterSaveData.ChapterId };
    }
    #endregion

    private void SetOptions(ExDropdown dropdown, ActionData actionData)
    {
        var dataType = actionData.data.dataController.DataType;

        switch (dataType)
        {
            case Enums.DataType.Chapter:                SetChapterOptions(dropdown, actionData);                break;
            case Enums.DataType.Phase:                  SetPhaseOptions(dropdown, actionData);                  break;
            case Enums.DataType.Quest:                  SetQuestOptions(dropdown, actionData);                  break;
            case Enums.DataType.Objective:              SetObjectiveOptions(dropdown, actionData);              break;
            case Enums.DataType.WorldInteractable:      SetWorldInteractableOptions(dropdown, actionData);      break;
            case Enums.DataType.Task:                   SetTaskOptions(dropdown, actionData);                   break;
            case Enums.DataType.Interaction:            SetInteractionOptions(dropdown, actionData);            break;
            case Enums.DataType.InteractionDestination: SetInteractionDestinationOptions(dropdown, actionData); break;
            case Enums.DataType.Region:                 SetRegionOptions(dropdown, actionData);                 break;

            case Enums.DataType.ChapterSave:            SetChapterSaveOptions(dropdown, actionData);            break;
            case Enums.DataType.PhaseSave:              SetPhaseSaveOptions(dropdown, actionData);              break;
            
            default: Debug.Log("CASE MISSING: " + dataType); break;
        }

        var action = FindActionByDataType(dataType);

        int selectedIndex = actionData.data.dataList.FindIndex(x => x.Id == action.elementData.Id);

        dropdown.Dropdown.value = selectedIndex;

        dropdown.Dropdown.captionText.text = dropdown.Dropdown.options[dropdown.Dropdown.value].text;
    }

    #region Dropdown options
    private void SetChapterOptions(ExDropdown dropdown, ActionData actionData)
    {
        var elementDataList = actionData.data.dataList.Cast<ChapterElementData>().ToList();
        elementDataList.ForEach(x => dropdown.Dropdown.options.Add(new Dropdown.OptionData(x.Name)));
    }

    private void SetPhaseOptions(ExDropdown dropdown, ActionData actionData)
    {
        var elementDataList = actionData.data.dataList.Cast<PhaseElementData>().ToList();
        elementDataList.ForEach(x => dropdown.Dropdown.options.Add(new Dropdown.OptionData(x.Name)));
    }

    private void SetQuestOptions(ExDropdown dropdown, ActionData actionData)
    {
        var elementDataList = actionData.data.dataList.Cast<QuestElementData>().ToList();
        elementDataList.ForEach(x => dropdown.Dropdown.options.Add(new Dropdown.OptionData(x.Name)));
    }

    private void SetObjectiveOptions(ExDropdown dropdown, ActionData actionData)
    {
        var elementDataList = actionData.data.dataList.Cast<ObjectiveElementData>().ToList();
        elementDataList.ForEach(x => dropdown.Dropdown.options.Add(new Dropdown.OptionData(x.Name)));
    }

    private void SetWorldInteractableOptions(ExDropdown dropdown, ActionData actionData)
    {
        var elementDataList = actionData.data.dataList.Cast<WorldInteractableElementData>().ToList();
        elementDataList.ForEach(x => dropdown.Dropdown.options.Add(new Dropdown.OptionData(x.InteractableName)));
    }

    private void SetTaskOptions(ExDropdown dropdown, ActionData actionData)
    {
        var elementDataList = actionData.data.dataList.Cast<TaskElementData>().ToList();
        elementDataList.ForEach(x => dropdown.Dropdown.options.Add(new Dropdown.OptionData(x.Name)));
    }

    private void SetInteractionOptions(ExDropdown dropdown, ActionData actionData)
    {
        var elementDataList = actionData.data.dataList.Cast<InteractionElementData>().ToList();
        elementDataList.ForEach(x => dropdown.Dropdown.options.Add(new Dropdown.OptionData(x.Default ? "Default" : TimeManager.FormatTime(x.StartTime) + " - " + TimeManager.FormatTime(x.EndTime))));
    }

    private void SetInteractionDestinationOptions(ExDropdown dropdown, ActionData actionData)
    {
        var elementDataList = actionData.data.dataList.Cast<InteractionDestinationElementData>().ToList();
        elementDataList.ForEach(x => dropdown.Dropdown.options.Add(new Dropdown.OptionData("Destination " + x.Id)));
    }

    private void SetRegionOptions(ExDropdown dropdown, ActionData actionData)
    {
        var elementDataList = actionData.data.dataList.Cast<RegionElementData>().ToList();
        elementDataList.ForEach(x => dropdown.Dropdown.options.Add(new Dropdown.OptionData(x.Name)));
    }

    private void SetChapterSaveOptions(ExDropdown dropdown, ActionData actionData)
    {
        var elementDataList = actionData.data.dataList.Cast<ChapterSaveElementData>().ToList();
        elementDataList.ForEach(x => dropdown.Dropdown.options.Add(new Dropdown.OptionData(x.Name)));
    }

    private void SetPhaseSaveOptions(ExDropdown dropdown, ActionData actionData)
    {
        var elementDataList = actionData.data.dataList.Cast<PhaseSaveElementData>().ToList();
        elementDataList.ForEach(x => dropdown.Dropdown.options.Add(new Dropdown.OptionData(x.Name)));
    }
    #endregion

    public void SelectOption(Enums.DataType dataType)
    {
        var actionData = FindActionByDataType(dataType);

        if (dataType == Enums.DataType.Interaction)
        {
            SelectValidInteraction(actionData);

            PathController.layoutSection.EditorForm.activePath.ReplaceAllData(actionData.data);

        } else {

            if (!actionData.data.dataList.Select(y => y.Id).ToList().Contains(actionData.elementData.Id))
                actionData.elementData = actionData.data.dataList.First();

            //Assigns an element from the data list as the main element so that it may be editted instead of the clone
            actionData.elementData = actionData.data.dataList.First(x => x.Id == actionData.elementData.Id);

            PathController.route.path.ReplaceAllData(actionData.data);
        }
    }

    public void SelectInteraction()
    {
        var actionData = FindActionByDataType(Enums.DataType.Interaction);

        SelectValidInteraction(actionData);
        
        InitializePath(actionData.elementData, actionDataList.IndexOf(actionData));
    }

    private void SelectValidInteraction(ActionData actionData)
    {
        var interactionDataList = actionData.data.dataList.Cast<InteractionElementData>().ToList();
        var validInteraction = interactionDataList.Where(x => TimeManager.TimeInFrame(TimeManager.instance.ActiveTime, x.StartTime, x.EndTime) || x.Default).OrderBy(x => x.Default).First();

        actionData.elementData = validInteraction;
    }

    private ActionData FindActionByDataType(Enums.DataType dataType)
    {
        return actionDataList.Where(x => x.data.dataController.DataType == dataType).FirstOrDefault();
    }

    public void CloseAction() { }
}
