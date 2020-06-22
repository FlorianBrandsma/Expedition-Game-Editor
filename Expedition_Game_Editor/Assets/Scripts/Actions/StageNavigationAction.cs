using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class StageNavigationAction : MonoBehaviour, IAction
{
    private Path path;

    public ActionProperties actionProperties;

    private Route.Data data;

    private ExDropdown dropdown;

    private PathController PathController { get { return GetComponent<PathController>(); } }

    public void InitializeAction(Path path)
    {
        data = PathController.route.data;
    }

    public void SetAction(Path path)
    {
        if (data == null) return;

        this.path = path;

        dropdown = ActionManager.instance.AddDropdown(actionProperties);

        switch (data.dataController.DataType)
        {
            case Enums.DataType.Chapter:            SetChapterOptions();            break;
            case Enums.DataType.Phase:              SetPhaseOptions();              break;
            case Enums.DataType.Quest:              SetQuestOptions();              break;
            case Enums.DataType.Objective:          SetObjectiveOptions();          break;
            case Enums.DataType.WorldInteractable:  SetWorldInteractableOptions();  break;
            case Enums.DataType.Task:               SetTaskOptions();               break;
            case Enums.DataType.Interaction:        SetInteractionOptions();        break;

            case Enums.DataType.Terrain:            SetTerrainOptions();            break;

            case Enums.DataType.ChapterSave:        SetChapterSaveOptions();        break;
            case Enums.DataType.PhaseSave:          SetPhaseSaveOptions();          break;
            case Enums.DataType.QuestSave:          SetQuestSaveOptions();          break;
            case Enums.DataType.ObjectiveSave:      SetObjectiveSaveOptions();      break;
            case Enums.DataType.TaskSave:           SetTaskSaveOptions();           break;

            default: Debug.Log("CASE MISSING: " + data.dataController.DataType); break;
        }

        int selectedIndex = data.dataList.Cast<GeneralData>().ToList().FindIndex(x => x.Id == PathController.route.GeneralData.Id);

        dropdown.Dropdown.value = selectedIndex;
        dropdown.Dropdown.captionText.text = dropdown.Dropdown.options[selectedIndex].text;

        dropdown.Dropdown.onValueChanged.AddListener(delegate { InitializePath(PathController.route.path, new Route.Data(data, data.dataList[dropdown.Dropdown.value])); });
    }

    private void SetChapterOptions()
    {
        var elementDataList = data.dataList.Cast<ChapterElementData>().ToList();
        elementDataList.ForEach(x => dropdown.Dropdown.options.Add(new Dropdown.OptionData(x.Name)));
    }

    private void SetPhaseOptions()
    {
        var elementDataList = data.dataList.Cast<PhaseElementData>().ToList();
        elementDataList.ForEach(x => dropdown.Dropdown.options.Add(new Dropdown.OptionData(x.Name)));
    }

    private void SetQuestOptions()
    {
        var elementDataList = data.dataList.Cast<QuestElementData>().ToList();
        elementDataList.ForEach(x => dropdown.Dropdown.options.Add(new Dropdown.OptionData(x.Name)));
    }

    private void SetObjectiveOptions()
    {
        var elementDataList = data.dataList.Cast<ObjectiveElementData>().ToList();
        elementDataList.ForEach(x => dropdown.Dropdown.options.Add(new Dropdown.OptionData(x.Name)));
    }

    private void SetWorldInteractableOptions()
    {
        var elementDataList = data.dataList.Cast<WorldInteractableElementData>().ToList();      
        elementDataList.ForEach(x => dropdown.Dropdown.options.Add(new Dropdown.OptionData(x.interactableName)));
    }

    private void SetTaskOptions()
    {
        var elementDataList = data.dataList.Cast<TaskElementData>().ToList();
        elementDataList.ForEach(x => dropdown.Dropdown.options.Add(new Dropdown.OptionData(x.Name)));
    }

    private void SetInteractionOptions()
    {
        var elementDataList = data.dataList.Cast<InteractionElementData>().ToList();
        elementDataList.ForEach(x => dropdown.Dropdown.options.Add(new Dropdown.OptionData(x.Default ? "Default" : TimeManager.FormatTime(x.StartTime, true) + " - " + TimeManager.FormatTime(x.EndTime))));
    }

    private void SetTerrainOptions()
    {
        var elementDataList = data.dataList.Cast<TerrainElementData>().ToList();
        elementDataList.ForEach(x => dropdown.Dropdown.options.Add(new Dropdown.OptionData(x.Name)));
    }

    private void SetChapterSaveOptions()
    {
        var elementDataList = data.dataList.Cast<ChapterSaveElementData>().ToList();
        elementDataList.ForEach(x => dropdown.Dropdown.options.Add(new Dropdown.OptionData(x.name)));
    }

    private void SetPhaseSaveOptions()
    {
        var elementDataList = data.dataList.Cast<PhaseSaveElementData>().ToList();
        elementDataList.ForEach(x => dropdown.Dropdown.options.Add(new Dropdown.OptionData(x.name)));
    }

    private void SetQuestSaveOptions()
    {
        var elementDataList = data.dataList.Cast<QuestSaveElementData>().ToList();
        elementDataList.ForEach(x => dropdown.Dropdown.options.Add(new Dropdown.OptionData(x.name)));
    }

    private void SetObjectiveSaveOptions()
    {
        var elementDataList = data.dataList.Cast<ObjectiveSaveElementData>().ToList();
        elementDataList.ForEach(x => dropdown.Dropdown.options.Add(new Dropdown.OptionData(x.name)));
    }

    private void SetTaskSaveOptions()
    {
        var elementDataList = data.dataList.Cast<TaskSaveElementData>().ToList();
        elementDataList.ForEach(x => dropdown.Dropdown.options.Add(new Dropdown.OptionData(x.name)));
    }

    public void InitializePath(Path path, Route.Data data)
    {
        RenderManager.Render(PathManager.ReloadPath(path, data));
    }

    public void CloseAction() { }
}
