using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class NavigationComponent : MonoBehaviour, IComponent
{
    private Path path;

    public EditorComponent component;

    private IDataController dataController;

    private Dropdown dropdown;

    private PathController PathController { get { return GetComponent<PathController>(); } }

    public void InitializeComponent(Path path)
    {
        dataController = PathController.route.data.DataController;
    }

    public void SetComponent(Path path)
    {
        if (dataController == null) return;

        this.path = path;

        dropdown = ComponentManager.componentManager.AddDropdown(component);

        switch (dataController.DataType)
        {
            case Enums.DataType.Chapter:            SetChapterOptions();            break;
            case Enums.DataType.Phase:              SetPhaseOptions();              break;
            case Enums.DataType.Quest:              SetQuestOptions();              break;
            case Enums.DataType.Objective:          SetObjectiveOptions();          break;
            case Enums.DataType.TerrainInteractable:SetTerrainInteractableOptions();break;

            default: Debug.Log("CASE MISSING: " + dataController.DataType); break;
        }

        int selectedIndex = dataController.DataList.Cast<GeneralData>().ToList().FindIndex(x => x.id == PathController.route.GeneralData().id);

        dropdown.value = selectedIndex;
        dropdown.captionText.text = dropdown.options[selectedIndex].text;

        dropdown.onValueChanged.AddListener(delegate { InitializePath(PathController.route.path, new Data(dataController, dataController.DataList[dropdown.value])); });
    }

    private void SetChapterOptions()
    {
        var dataElements = dataController.DataList.Cast<ChapterDataElement>().ToList();

        dataElements.ForEach(x => dropdown.options.Add(new Dropdown.OptionData(x.Name)));
    }

    private void SetPhaseOptions()
    {
        var dataElements = dataController.DataList.Cast<PhaseDataElement>().ToList();

        dataElements.ForEach(x => dropdown.options.Add(new Dropdown.OptionData(x.Name)));
    }

    private void SetQuestOptions()
    {
        var dataElements = dataController.DataList.Cast<QuestDataElement>().ToList();

        dataElements.ForEach(x => dropdown.options.Add(new Dropdown.OptionData(x.Name)));
    }

    private void SetObjectiveOptions()
    {
        var dataElements = dataController.DataList.Cast<ObjectiveDataElement>().ToList();

        dataElements.ForEach(x => dropdown.options.Add(new Dropdown.OptionData(x.Name)));
    }

    private void SetTerrainInteractableOptions()
    {
        var dataElements = dataController.DataList.Cast<TerrainInteractableDataElement>().ToList();

        dataElements.ForEach(x => dropdown.options.Add(new Dropdown.OptionData(x.interactableName)));
    }

    public void InitializePath(Path path, Data data)
    {
        EditorManager.editorManager.InitializePath(PathManager.ReloadPath(path, data));
    }

    public void CloseComponent() { }
}
