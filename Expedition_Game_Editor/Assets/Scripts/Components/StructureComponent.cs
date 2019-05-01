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
            case Enums.DataType.Step:           SetStepOptions();           break;
            case Enums.DataType.StepElement:    SetStepElementOptions();    break;
        }

        int selected_index = dataController.DataList.Cast<GeneralData>().ToList().FindIndex(x => x.id == controller.route.GeneralData().id);

        dropdown.value = selected_index;
        dropdown.captionText.text = dropdown.options[selected_index].text;

        dropdown.onValueChanged.AddListener(delegate { InitializePath(controller.route.path, new Data(dataController, GetEnumerable(dataController.DataList))); });
    }

    private void SetChapterOptions()
    {
        List<ChapterDataElement> dataElements = dataController.DataList.Cast<ChapterDataElement>().ToList();

        foreach(ChapterDataElement dataElement in dataElements)
            dropdown.options.Add(new Dropdown.OptionData(dataElement.name)); 
    }

    private void SetPhaseOptions()
    {
        List<PhaseDataElement> dataElements = dataController.DataList.Cast<PhaseDataElement>().ToList();

        foreach (PhaseDataElement dataElement in dataElements)
            dropdown.options.Add(new Dropdown.OptionData(dataElement.name));
    }

    private void SetQuestOptions()
    {
        List<QuestDataElement> dataElements = dataController.DataList.Cast<QuestDataElement>().ToList();

        foreach (QuestDataElement dataElement in dataElements)
            dropdown.options.Add(new Dropdown.OptionData(dataElement.name));
    }

    private void SetStepOptions()
    {
        List<StepDataElement> dataElements = dataController.DataList.Cast<StepDataElement>().ToList();

        foreach (StepDataElement dataElement in dataElements)
            dropdown.options.Add(new Dropdown.OptionData(dataElement.name));
    }

    private void SetStepElementOptions()
    {
        List<StepElementDataElement> dataElements = dataController.DataList.Cast<StepElementDataElement>().ToList();

        foreach (StepElementDataElement dataElement in dataElements)
            dropdown.options.Add(new Dropdown.OptionData(dataElement.name));
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
