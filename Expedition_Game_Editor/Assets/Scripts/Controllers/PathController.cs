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

    public LayoutSection    layoutSection;

    private EditorController EditorController   { get { return GetComponent<EditorController>(); } }

    public IDataController  dataController      { get; set; }

    public IEditor DataEditor
    {
        get { return layoutSection.dataEditor; }
        set { layoutSection.dataEditor = value; }
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

        InitializeActions(mainPath);

        if (EditorController != null)
            layoutSection.TargetController = EditorController;

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

            layoutSection.TargetController.FinalizeController();
        }
    }

    private void SetPreviousEditor()
    {
        if (layoutSection.dataEditor == null) return;

        layoutSection.previousEditor = layoutSection.dataEditor;
        layoutSection.previousDataSource = layoutSection.dataEditor.Data.elementData;
        layoutSection.previousElementDataList = layoutSection.dataEditor.ElementDataList.ToList();
    }

    public bool GetComponents(Path path)
    {
        if (step < path.route.Count)
            controllers[path.route[step].controller].GetComponents(path);

        return GetComponents<IAction>().Count() > 0;
    }

    public void SetSubControllers(Path path)
    {
        if (subControllerManager != null)
        {
            subControllerManager.SetTabs(this, path);

        //    if(GetComponent<RectTransform>() != null)
        //    {
        //        GetComponent<RectTransform>().offsetMin = new Vector2(GetComponent<RectTransform>().offsetMin.x, 0);
        //        GetComponent<RectTransform>().offsetMax = new Vector2(GetComponent<RectTransform>().offsetMax.x, 30);
        //    }
                
        //} else
        //{
        //    if (GetComponent<RectTransform>() != null)
        //    {
        //        GetComponent<RectTransform>().offsetMin = new Vector2(GetComponent<RectTransform>().offsetMin.x, 0);
        //        GetComponent<RectTransform>().offsetMax = new Vector2(GetComponent<RectTransform>().offsetMax.x, 0);
        //    }
        }
            

        if (step < path.route.Count)
            controllers[path.route[step].controller].SetSubControllers(path);
    }

    private void InitializeActions(Path path)
    {
        foreach (IAction action in GetComponents<IAction>())
            action.InitializeAction(path);
    }

    public void SetActions(Path path)
    {
        foreach (IAction action in GetComponents<IAction>())
            action.SetAction(path);

        if (step < path.route.Count)
            controllers[path.route[step].controller].SetActions(path);
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
            layoutSection.TargetView = GetComponent<LayoutDependency>();

        if (step < path.route.Count)
            controllers[path.route[step].controller].GetTargetLayout(path, layoutStep);
    }

    public void ClosePath(Path path)
    {
        foreach (IAction action in GetComponents<IAction>())
            action.CloseAction();

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
