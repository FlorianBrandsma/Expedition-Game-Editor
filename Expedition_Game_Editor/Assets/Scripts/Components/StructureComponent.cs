using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

//Attached dataController parent is required to have a DataController component
public class StructureComponent : MonoBehaviour, IComponent
{
    public EditorComponent component;

    public GameObject dataController_parent;
    private IDataController dataController;

    private Dropdown dropdown;

    public void InitializeComponent(Path path)
    {
        if (dataController_parent == null) return;

        dataController = dataController_parent.GetComponent<IDataController>();
    }

    public void SetComponent(Path path)
    {
        if (dataController == null) return;

        PathController controller = GetComponent<PathController>();
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

        int selected_index = dataController.DataList.Cast<GeneralData>().ToList().FindIndex(x => x.id == controller.route.GeneralData().id);

        dropdown.value = selected_index;
        dropdown.captionText.text = dropdown.options[selected_index].text;

        dropdown.onValueChanged.AddListener(delegate { InitializePath(controller.route.path, new Data(dataController, dataController.DataList[dropdown.value])); });
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
