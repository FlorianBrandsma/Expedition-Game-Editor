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
            case Enums.DataType.Chapter:        SetChapterOptions();        break;
            case Enums.DataType.Phase:          SetPhaseOptions();          break;
            case Enums.DataType.Quest:          SetQuestOptions();          break;
            case Enums.DataType.Objective:      SetObjectiveOptions();      break;
            case Enums.DataType.TerrainElement: SetTerrainElementOptions(); break;
            default:                            Debug.Log("CASE MISSING");  break;
        }

        int selectedIndex = dataController.DataList.Cast<GeneralData>().ToList().FindIndex(x => x.id == PathController.route.GeneralData().id);

        dropdown.value = selectedIndex;
        dropdown.captionText.text = dropdown.options[selectedIndex].text;

        dropdown.onValueChanged.AddListener(delegate { InitializePath(PathController.route.path, new Data(dataController, dataController.DataList[dropdown.value])); });
    }

    private void SetChapterOptions()
    {
        List<ChapterDataElement> dataElements = dataController.DataList.Cast<ChapterDataElement>().ToList();

        foreach(ChapterDataElement dataElement in dataElements)
            dropdown.options.Add(new Dropdown.OptionData(dataElement.Name)); 
    }

    private void SetPhaseOptions()
    {
        List<PhaseDataElement> dataElements = dataController.DataList.Cast<PhaseDataElement>().ToList();

        foreach (PhaseDataElement dataElement in dataElements)
            dropdown.options.Add(new Dropdown.OptionData(dataElement.Name));
    }

    private void SetQuestOptions()
    {
        List<QuestDataElement> dataElements = dataController.DataList.Cast<QuestDataElement>().ToList();

        foreach (QuestDataElement dataElement in dataElements)
            dropdown.options.Add(new Dropdown.OptionData(dataElement.Name));
    }

    private void SetObjectiveOptions()
    {
        List<ObjectiveDataElement> dataElements = dataController.DataList.Cast<ObjectiveDataElement>().ToList();

        foreach (ObjectiveDataElement dataElement in dataElements)
            dropdown.options.Add(new Dropdown.OptionData(dataElement.Name));
    }

    private void SetTerrainElementOptions()
    {
        List<TerrainElementDataElement> dataElements = dataController.DataList.Cast<TerrainElementDataElement>().ToList();
        
        foreach (TerrainElementDataElement dataElement in dataElements)
            dropdown.options.Add(new Dropdown.OptionData(dataElement.elementName));
    }

    public void InitializePath(Path path, Data data)
    {
        EditorManager.editorManager.InitializePath(PathManager.ReloadPath(path, data));
    }

    public IEnumerable GetEnumerable(ICollection list)
    {
        int index = 0;

        foreach(var data in list)
        {
            if (index == dropdown.value)
                return new[] { data };

            index++;
        }

        return null;
    }

    public void CloseComponent() { }
}
