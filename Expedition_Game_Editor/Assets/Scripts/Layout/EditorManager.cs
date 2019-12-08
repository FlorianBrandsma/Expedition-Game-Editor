using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

//    [UnityEngine.Serialization.FormerlySerializedAs("previous_name")]

public class EditorManager : MonoBehaviour
{
    static public EditorManager editorManager;

    static public RectTransform UI;

    public List<EditorForm> forms;
    public List<EditorLayer> layers;
    
    static public HistoryManager historyManager = new HistoryManager();

    static public Enums.LoadType loadType;

    public TimeManager TimeManager { get { return GetComponent<TimeManager>(); } }

    private void Awake()
    {
        Fixtures.LoadFixtures();

        editorManager = this;

        UI = GetComponent<RectTransform>();
        
        forms.ForEach(x => x.InitializeForm());
        layers.ForEach(x => x.InitializeAnchors());    
    }

    private void Start()
    {
        LanguageManager.GetLanguage();

        InitializePath(new PathManager.Main().Initialize());
    }

    private void Update()
    {
        //Escape button shares a built in function of the dropdown that closes it
        if (GameObject.Find("Dropdown List") != null) return;
        
        if (Input.GetKeyUp(KeyCode.Escape))
            PreviousEditor();
    }

    public void InitializePath(Path path)
    {
        SelectionManager.CancelGetSelection();
        
        //Set up data along the path
        path.form.OpenPath(path);

        //Get target routes for selecting elements
        SelectionManager.GetRouteList();

        //Deload inactive forms
        DeloadForms();

        //Open visible elements along the path
        OpenView(path);

        //Opening view only needs to initialize editors, so lists can be set
        ResetView();
        
        //Performed at the end so it doesn't interfere with the current (de)activation process
        InitializeSecondaryPaths(path);
    }
    
    private void InitializeSecondaryPaths(Path path)
    {
        //Follows path, activates form components and adds last route to history
        path.form.FinalizePath();

        //Set forms based on their component's state
        SetForms();
    }

    private void DeloadForms()
    {
        forms.ForEach(x => x.loaded = x.activeInPath);
    }

    private void SetForms()
    {
        forms.Where(x => x.formComponent != null).Select(x => x.formComponent).ToList().ForEach(formComponent =>
        {
            if (formComponent.activeInPath)
                formComponent.SetForm();
            else
                formComponent.editorForm.CloseForm();                
        });
    }
    
    private void OpenView(Path path)
    {
        //Initialize the layout of this form
        path.form.OpenView();
    }

    public void ResetView()
    {
        //Activate layers, set anchors based on initialized values
        layers.ForEach(x => x.SetLayout());

        forms.ForEach(x => x.CloseEditor());
        forms.ForEach(x => x.OpenEditor());
    }

    public void PreviousEditor()
    {
        historyManager.PreviousEditor();

        loadType = Enums.LoadType.Normal;
    }

    public void ResetEditor()
    {
        foreach (EditorForm form in forms)
            form.ResetPath();

        loadType = Enums.LoadType.Normal;
    }

    public void ResetEditor(Path path)
    {
        InitializePath(path);

        forms.Where(form => form != path.form && form.activeInPath).ToList().ForEach(form => InitializePath(form.activePath));

        loadType = Enums.LoadType.Normal;
    }

    static public List<IDataElement> GetData(IDataController dataController, IEnumerable searchParameters)
    {
        var dataList = dataController.DataManager.GetDataElements(searchParameters);

        var pathController = dataController.SegmentController.editorController.PathController;

        var mainForm = pathController.editorSection.editorForm;
        mainForm.activePath.ReplaceDataLists(pathController.step, dataController.DataType, dataList);

        //editorManager.forms.Where(x => x != mainForm).ToList().ForEach(x => x.activePath.ReplaceDataLists(0, dataController.DataType, dataList));

        return dataList;
    }

    static public string PathString(Path path)
    {
        string str = "route: ";

        for (int i = 0; i < path.route.Count; i++)
            str += path.route[i].controller + "/";

        str += "\n";

        for (int i = 0; i < path.route.Count; i++)
            str += path.route[i].GeneralData.DataType + "-" + path.route[i].GeneralData.Id + "/";

        return str;
    }
}



