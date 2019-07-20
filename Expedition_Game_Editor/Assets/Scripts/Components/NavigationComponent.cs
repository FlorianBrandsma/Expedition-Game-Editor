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

    private Data data;

    private PathController PathController { get { return GetComponent<PathController>(); } }

    public void InitializeComponent(Path path)
    {
        if(path.route.Count == PathController.step)
        {
            var test = path.route.Where(x => x.data.DataController != null && x.data.DataController.DataCategory == Enums.DataCategory.Navigation).Select(x => x.data.DataController.DataType).Distinct().ToList();

            //test.ForEach(x => { Debug.Log(x); });
        }
        //InitializeData();
    }

    private void InitializeData()
    {
        Enums.DataType dataType = PathController.route.data.DataController.DataType;

        data = PathController.route.path.FindFirstRoute(dataType).data;
    }

    public void SetComponent(Path path)
    {
        //this.path = path;

        //Dropdown dropdown = ComponentManager.componentManager.AddDropdown(component);

        //data.DataController = InitializeDataController(dropdown);

        //SetOptions(dropdown, data.DataController);

        //dropdown.onValueChanged.AddListener(delegate { InitializePath(PathController.route.path, new Data(dataController, dataController.DataList[dropdown.value])); });
    }

    private IDataController InitializeDataController(Dropdown dropdown)
    {
        IDataController dataController;

        Enums.DataType dataType = data.DataController.DataType;

        switch (dataType)
        {
            case Enums.DataType.Chapter:            dataController = dropdown.gameObject.AddComponent<ChapterController>();             break;
            case Enums.DataType.Phase:              dataController = dropdown.gameObject.AddComponent<PhaseController>();               break;
            case Enums.DataType.Quest:              dataController = dropdown.gameObject.AddComponent<QuestController>();               break;
            case Enums.DataType.Objective:          dataController = dropdown.gameObject.AddComponent<ObjectiveController>();           break;
            case Enums.DataType.TerrainInteractable:dataController = dropdown.gameObject.AddComponent<TerrainInteractableController>(); break;
            default:                                dataController = null;                                                              break;
        }

        dataController.InitializeController();

        if (dataController.DataList == null)
            dataController.DataList = data.DataController.DataList.ToList();

        return dataController;
    }

    private void SetOptions(Dropdown dropdown, IDataController dataController)
    {
        switch (dataController.DataType)
        {
            case Enums.DataType.Chapter:            SetChapterOptions(dropdown, dataController); break;
            case Enums.DataType.Phase:              SetPhaseOptions(dropdown, dataController); break;
            case Enums.DataType.Quest:              SetQuestOptions(dropdown, dataController); break;
            case Enums.DataType.Objective:          SetObjectiveOptions(dropdown, dataController); break;
            case Enums.DataType.TerrainInteractable:SetTerrainInteractableOptions(dropdown, dataController); break;
        }

        Data data = PathController.route.path.FindLastRoute(dataController.DataType).data;

        int selectedIndex = dataController.DataList.Cast<GeneralData>().ToList().FindIndex(x => x.id == ((GeneralData)data.DataElement).id);

        dropdown.value = selectedIndex;

        dropdown.captionText.text = dropdown.options[dropdown.value].text;
    }

    #region Dropdown options

    private void SetChapterOptions(Dropdown dropdown, IDataController dataController)
    {
        var dataElements = dataController.DataList.Cast<ChapterDataElement>().ToList();

        dataElements.ForEach(x => dropdown.options.Add(new Dropdown.OptionData(x.Name)));
    }

    private void SetPhaseOptions(Dropdown dropdown, IDataController dataController)
    {
        var dataElements = dataController.DataList.Cast<PhaseDataElement>().ToList();

        dataElements.ForEach(x => dropdown.options.Add(new Dropdown.OptionData(x.Name)));
    }

    private void SetQuestOptions(Dropdown dropdown, IDataController dataController)
    {
        var dataElements = dataController.DataList.Cast<QuestDataElement>().ToList();

        dataElements.ForEach(x => dropdown.options.Add(new Dropdown.OptionData(x.Name)));
    }

    private void SetObjectiveOptions(Dropdown dropdown, IDataController dataController)
    {
        var dataElements = dataController.DataList.Cast<ObjectiveDataElement>().ToList();

        dataElements.ForEach(x => dropdown.options.Add(new Dropdown.OptionData(x.Name)));
    }

    private void SetTerrainInteractableOptions(Dropdown dropdown, IDataController dataController)
    {
        var dataElements = dataController.DataList.Cast<TerrainInteractableDataElement>().ToList();

        dataElements.ForEach(x => dropdown.options.Add(new Dropdown.OptionData(x.interactableName)));
    }

    #endregion

    public void InitializePath(Path path, Data data)
    {
        EditorManager.editorManager.InitializePath(PathManager.ReloadPath(path, data));
    }

    //public IEnumerable GetEnumerable(ICollection list)
    //{
    //    int index = 0;

    //    foreach(var data in list)
    //    {
    //        if (index == dropdown.value)
    //            return new[] { data };

    //        index++;
    //    }

    //    return null;
    //}

    public void CloseComponent() { }
}
