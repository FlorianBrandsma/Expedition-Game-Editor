using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using System.Linq;

public class StageNavigationAction : MonoBehaviour, IAction
{
    public ActionProperties actionProperties;
    
    private ExDropdown dropdown;

    private PathController PathController   { get { return GetComponent<PathController>(); } }
    private Data Data                       { get { return PathController.route.data; } }

    private List<IElementData> dataList;

    public void InitializeAction(Path path) { }

    public void SetAction(Path path)
    {
        if (Data == null) return;
        
        dropdown = ActionManager.instance.AddDropdown(actionProperties);
        dataList = Data.dataList.Where(x => x.Id > 0).ToList();

        switch (Data.dataController.DataType)
        {
            case Enums.DataType.Chapter:            SetChapterOptions();            break;
            case Enums.DataType.Phase:              SetPhaseOptions();              break;
            case Enums.DataType.Quest:              SetQuestOptions();              break;
            case Enums.DataType.Objective:          SetObjectiveOptions();          break;
            case Enums.DataType.WorldInteractable:  SetWorldInteractableOptions();  break;
            case Enums.DataType.Task:               SetTaskOptions();               break;
            case Enums.DataType.Interaction:        SetInteractionOptions();        break;
            case Enums.DataType.Outcome:            SetOutcomeOptions();            break;

            case Enums.DataType.Terrain:            SetTerrainOptions();            break;

            case Enums.DataType.ChapterSave:        SetChapterSaveOptions();        break;
            case Enums.DataType.PhaseSave:          SetPhaseSaveOptions();          break;
            case Enums.DataType.QuestSave:          SetQuestSaveOptions();          break;
            case Enums.DataType.ObjectiveSave:      SetObjectiveSaveOptions();      break;
            case Enums.DataType.TaskSave:           SetTaskSaveOptions();           break;

            default: Debug.Log("CASE MISSING: " + Data.dataController.DataType); break;
        }

        int selectedIndex = dataList.FindIndex(x => x.Id == PathController.route.ElementData.Id);

        dropdown.Dropdown.value = selectedIndex;
        dropdown.Dropdown.captionText.text = dropdown.Dropdown.options[selectedIndex].text;

        dropdown.Dropdown.onValueChanged.AddListener(delegate { InitializePath(PathController.route.path, dataList[dropdown.Dropdown.value]); });
    }

    private void SetChapterOptions()
    {
        var elementDataList = dataList.Cast<ChapterElementData>().ToList();
        elementDataList.ForEach(x => dropdown.Dropdown.options.Add(new Dropdown.OptionData(x.Name)));
    }

    private void SetPhaseOptions()
    {
        var elementDataList = dataList.Cast<PhaseElementData>().ToList();
        elementDataList.ForEach(x => dropdown.Dropdown.options.Add(new Dropdown.OptionData(x.Name)));
    }

    private void SetQuestOptions()
    {
        var elementDataList = dataList.Cast<QuestElementData>().ToList();
        elementDataList.ForEach(x => dropdown.Dropdown.options.Add(new Dropdown.OptionData(x.Name)));
    }

    private void SetObjectiveOptions()
    {
        var elementDataList = dataList.Cast<ObjectiveElementData>().ToList();
        elementDataList.ForEach(x => dropdown.Dropdown.options.Add(new Dropdown.OptionData(x.Name)));
    }

    private void SetWorldInteractableOptions()
    {
        var elementDataList = dataList.Cast<WorldInteractableElementData>().ToList();      
        elementDataList.ForEach(x => dropdown.Dropdown.options.Add(new Dropdown.OptionData(x.InteractableName)));
    }

    private void SetTaskOptions()
    {
        var elementDataList = dataList.Cast<TaskElementData>().ToList();
        elementDataList.ForEach(x => dropdown.Dropdown.options.Add(new Dropdown.OptionData(x.Name)));
    }

    private void SetInteractionOptions()
    {
        var elementDataList = dataList.Cast<InteractionElementData>().ToList();
        elementDataList.ForEach(x => dropdown.Dropdown.options.Add(new Dropdown.OptionData(x.Default ? "Default" : TimeManager.FormatTime(x.StartTime) + " - " + TimeManager.FormatTime(x.EndTime))));
    }

    private void SetOutcomeOptions()
    {
        var elementDataList = dataList.Cast<OutcomeElementData>().ToList();
        elementDataList.ForEach(x => dropdown.Dropdown.options.Add(new Dropdown.OptionData(Enum.GetName(typeof(Enums.OutcomeType), x.Type))));
    }

    private void SetTerrainOptions()
    {
        var elementDataList = dataList.Cast<TerrainElementData>().ToList();
        elementDataList.ForEach(x => dropdown.Dropdown.options.Add(new Dropdown.OptionData(x.Name)));
    }

    private void SetChapterSaveOptions()
    {
        var elementDataList = dataList.Cast<ChapterSaveElementData>().ToList();
        elementDataList.ForEach(x => dropdown.Dropdown.options.Add(new Dropdown.OptionData(x.Name)));
    }

    private void SetPhaseSaveOptions()
    {
        var elementDataList = dataList.Cast<PhaseSaveElementData>().ToList();
        elementDataList.ForEach(x => dropdown.Dropdown.options.Add(new Dropdown.OptionData(x.Name)));
    }

    private void SetQuestSaveOptions()
    {
        var elementDataList = dataList.Cast<QuestSaveElementData>().ToList();
        elementDataList.ForEach(x => dropdown.Dropdown.options.Add(new Dropdown.OptionData(x.Name)));
    }

    private void SetObjectiveSaveOptions()
    {
        var elementDataList = dataList.Cast<ObjectiveSaveElementData>().ToList();
        elementDataList.ForEach(x => dropdown.Dropdown.options.Add(new Dropdown.OptionData(x.Name)));
    }

    private void SetTaskSaveOptions()
    {
        var elementDataList = dataList.Cast<TaskSaveElementData>().ToList();
        elementDataList.ForEach(x => dropdown.Dropdown.options.Add(new Dropdown.OptionData(x.Name)));
    }

    public void InitializePath(Path path, IElementData elementData)
    {
        Path newPath = path.Clone();

        newPath.routeList.Last().id = elementData.Id;

        RenderManager.Render(newPath);
    }

    public void CloseAction() { }
}
