using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class DefaultNavigationAction : MonoBehaviour, IAction
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

            default: Debug.Log("CASE MISSING: " + data.dataController.DataType); break;
        }

        int selectedIndex = data.dataList.Cast<GeneralData>().ToList().FindIndex(x => x.Id == PathController.route.GeneralData.Id);

        dropdown.Dropdown.value = selectedIndex;
        dropdown.Dropdown.captionText.text = dropdown.Dropdown.options[selectedIndex].text;

        dropdown.Dropdown.onValueChanged.AddListener(delegate { InitializePath(PathController.route.path, new Route.Data(data, data.dataList[dropdown.Dropdown.value])); });
    }

    private void SetChapterOptions()
    {
        var dataElements = data.dataList.Cast<ChapterDataElement>().ToList();
        dataElements.ForEach(x => dropdown.Dropdown.options.Add(new Dropdown.OptionData(x.Name)));
    }

    private void SetPhaseOptions()
    {
        var dataElements = data.dataList.Cast<PhaseDataElement>().ToList();
        dataElements.ForEach(x => dropdown.Dropdown.options.Add(new Dropdown.OptionData(x.Name)));
    }

    private void SetQuestOptions()
    {
        var dataElements = data.dataList.Cast<QuestDataElement>().ToList();
        dataElements.ForEach(x => dropdown.Dropdown.options.Add(new Dropdown.OptionData(x.Name)));
    }

    private void SetObjectiveOptions()
    {
        var dataElements = data.dataList.Cast<ObjectiveDataElement>().ToList();
        dataElements.ForEach(x => dropdown.Dropdown.options.Add(new Dropdown.OptionData(x.Name)));
    }

    private void SetWorldInteractableOptions()
    {
        var dataElements = data.dataList.Cast<WorldInteractableDataElement>().ToList();      
        dataElements.ForEach(x => dropdown.Dropdown.options.Add(new Dropdown.OptionData(x.interactableName)));
    }

    private void SetTaskOptions()
    {
        var dataElements = data.dataList.Cast<TaskDataElement>().ToList();
        dataElements.ForEach(x => dropdown.Dropdown.options.Add(new Dropdown.OptionData(x.Name)));
    }

    private void SetInteractionOptions()
    {
        var dataElements = data.dataList.Cast<InteractionDataElement>().ToList();
        dataElements.ForEach(x => dropdown.Dropdown.options.Add(new Dropdown.OptionData(x.Default ? "Default" : TimeManager.FormatTime(x.StartTime, true) + " - " + TimeManager.FormatTime(x.EndTime))));
    }

    private void SetTerrainOptions()
    {
        var dataElements = data.dataList.Cast<TerrainDataElement>().ToList();

        dataElements.ForEach(x => dropdown.Dropdown.options.Add(new Dropdown.OptionData(x.Name)));
    }

    public void InitializePath(Path path, Route.Data data)
    {
        RenderManager.Render(PathManager.ReloadPath(path, data));
    }

    public void CloseAction() { }
}
