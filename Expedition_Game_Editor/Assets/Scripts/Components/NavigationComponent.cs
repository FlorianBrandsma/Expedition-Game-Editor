using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class NavigationComponent : MonoBehaviour, IComponent
{
    private Path path;

    public EditorComponent component;

    private Route.Data data;

    private Dropdown dropdown;

    private PathController PathController { get { return GetComponent<PathController>(); } }

    public void InitializeComponent(Path path)
    {
        data = PathController.route.data;
    }

    public void SetComponent(Path path)
    {
        if (data == null) return;

        this.path = path;

        dropdown = ComponentManager.componentManager.AddDropdown(component);

        switch (data.dataController.DataType)
        {
            case Enums.DataType.Chapter:            SetChapterOptions();            break;
            case Enums.DataType.Phase:              SetPhaseOptions();              break;
            case Enums.DataType.Quest:              SetQuestOptions();              break;
            case Enums.DataType.Objective:          SetObjectiveOptions();          break;
            case Enums.DataType.SceneInteractable:  SetSceneInteractableOptions();  break;

            default: Debug.Log("CASE MISSING: " + data.dataController.DataType); break;
        }

        int selectedIndex = data.dataList.Cast<GeneralData>().ToList().FindIndex(x => x.Id == PathController.route.GeneralData.Id);

        dropdown.value = selectedIndex;
        dropdown.captionText.text = dropdown.options[selectedIndex].text;

        dropdown.onValueChanged.AddListener(delegate { InitializePath(PathController.route.path, new Route.Data(data, data.dataList[dropdown.value])); });
    }

    private void SetChapterOptions()
    {
        var dataElements = data.dataList.Cast<ChapterDataElement>().ToList();

        dataElements.ForEach(x => dropdown.options.Add(new Dropdown.OptionData(x.Name)));
    }

    private void SetPhaseOptions()
    {
        var dataElements = data.dataList.Cast<PhaseDataElement>().ToList();

        dataElements.ForEach(x => dropdown.options.Add(new Dropdown.OptionData(x.Name)));
    }

    private void SetQuestOptions()
    {
        var dataElements = data.dataList.Cast<QuestDataElement>().ToList();

        dataElements.ForEach(x => dropdown.options.Add(new Dropdown.OptionData(x.Name)));
    }

    private void SetObjectiveOptions()
    {
        var dataElements = data.dataList.Cast<ObjectiveDataElement>().ToList();

        dataElements.ForEach(x => dropdown.options.Add(new Dropdown.OptionData(x.Name)));
    }

    private void SetSceneInteractableOptions()
    {
        var dataElements = data.dataList.Cast<SceneInteractableDataElement>().ToList();
        
        dataElements.ForEach(x => dropdown.options.Add(new Dropdown.OptionData(x.interactableName)));
    }

    public void InitializePath(Path path, Route.Data data)
    {
        EditorManager.editorManager.InitializePath(PathManager.ReloadPath(path, data));
    }

    public void CloseComponent() { }
}
