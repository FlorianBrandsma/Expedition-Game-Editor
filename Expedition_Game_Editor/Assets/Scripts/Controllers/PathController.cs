using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PathController : MonoBehaviour
{
    public Route route      { get; set; }
    private Route previousRoute;
    public int step         { get; set; }
    public int layoutStep   { get; set; }

    public HistoryElement   history;

    public EditorSection    editorSection;

    private EditorController EditorController   { get { return GetComponent<EditorController>(); } }

    public IDataController  dataController      { get; set; }

    public IEditor DataEditor
    {
        get { return editorSection.dataEditor; }
        set { editorSection.dataEditor = value; }
    }

    public SubControllerManager subControllerManager;
    public PathController[] controllers;

    private PathController parentController;

    public void InitializeDependencies(PathController parentController = null)
    {
        this.parentController = parentController;

        GetDataController();
        GetDataEditor();

        foreach (PathController pathController in controllers)
            pathController.InitializeDependencies(this);

        if (EditorController != null)
            EditorController.InitializeDependencies();
    }

    //Necessary steps to set up the correct path for the controller
    public void OpenPath(Path mainPath, int step, PathController parentController = null)
    {
        //This is an exception to ensure shared controllers have the correct values
        //One way to circumvent this is to simply get rid of shared controllers
        this.parentController = parentController;

        this.step = step;

        Path path = mainPath.Trim(step);

        if(step > 0)
        {
            route = mainPath.route[step - 1];
        } else {
            route = new Route(path);
        }

        route.path = path;

        previousRoute = route;
        GetDataController();
        GetDataEditor();
        
        if (subControllerManager != null)
        {
            InitializeTabs(mainPath);

            foreach (PathController controller in controllers)
                controller.history.group = history.group;
        }

        InitializeComponents(mainPath);

        if (EditorController != null)
            editorSection.targetController = EditorController;

        if (step < mainPath.route.Count)
        {
            controllers[mainPath.route[step].controller].OpenPath(mainPath, step + 1, this);
        }
    }

    public void GetDataController()
    {
        if (GetComponent<IDataController>() != null)
        {
            dataController = GetComponent<IDataController>();
        } else {
            if (parentController != null && parentController.dataController != null)
                dataController = parentController.dataController;
        }
    }

    public void GetDataEditor()
    {
        if (GetComponent<IEditor>() != null)
        {
            DataEditor = GetComponent<IEditor>();
        } else {
            if (parentController != null && parentController.DataEditor != null)
                DataEditor = parentController.DataEditor;
        }
    }

    private void SetHistory()
    {
        if (history.group != HistoryManager.Group.None)
            history.AddHistory(route.path);   
    }

    public void FinalizePath(Path path)
    {
        route.path.type = Path.Type.Loaded;

        if (step < path.route.Count)
        {
            controllers[path.route[step].controller].FinalizePath(path);
        } else {
            
            SetPreviousEditor();
            SetHistory();

            editorSection.targetController.FinalizeController();
        }
    }

    private void SetPreviousEditor()
    {
        if (editorSection.dataEditor == null) return;

        editorSection.previousDataSource = editorSection.dataEditor.Data.dataElement;
        editorSection.previousDataElements = editorSection.dataEditor.DataElements.ToList();
    }

    public bool GetComponents(Path path)
    {
        if (step < path.route.Count)
            controllers[path.route[step].controller].GetComponents(path);

        return GetComponents<IComponent>().Count() > 0;
    }

    public void SetSubControllers(Path path)
    {
        if (subControllerManager != null)
            subControllerManager.SetTabs(this, path);

        if (step < path.route.Count)
            controllers[path.route[step].controller].SetSubControllers(path);
    }

    private void InitializeComponents(Path path)
    {
        foreach (IComponent component in GetComponents<IComponent>())
            component.InitializeComponent(path);
    }

    public bool SetComponents(Path path)
    {
        foreach (IComponent component in GetComponents<IComponent>())
            component.SetComponent(path);

        if (step < path.route.Count)
            controllers[path.route[step].controller].SetComponents(path);

        return GetComponents<IComponent>().Count() > 0;
    }

    void InitializeTabs(Path path)
    {
        if (step == path.route.Count)
            path.Add();      
    }

    public void FilterRows(List<GeneralData> list) { }

    public void GetTargetLayout(Path path, int layoutStep)
    {
        this.layoutStep = step;

        if (GetComponent<LayoutDependency>() != null)
            editorSection.targetView = GetComponent<LayoutDependency>();

        if (step < path.route.Count)
            controllers[path.route[step].controller].GetTargetLayout(path, layoutStep);
    }

    public void ClosePath(Path path)
    {
        foreach (IComponent component in GetComponents<IComponent>())
            component.CloseComponent();

        if (step < path.route.Count)
            controllers[path.route[step].controller].ClosePath(path);
    }

    public void CloseTabs(Path path)
    {
        if (subControllerManager != null)
            subControllerManager.CloseTabs();

        if (layoutStep < path.route.Count)
            controllers[path.route[layoutStep].controller].CloseTabs(path);
    }
}
