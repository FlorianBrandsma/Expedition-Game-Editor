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

    public Enums.HistoryGroup historyGroup;

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
            route = mainPath.routeList[step - 1];
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
                controller.historyGroup = historyGroup;
        }

        InitializeActions(mainPath);

        if (EditorController != null)
            layoutSection.TargetController = EditorController;

        if (step < mainPath.routeList.Count)
        {
            controllers[mainPath.routeList[step].controllerIndex].OpenPath(mainPath, step + 1, this);
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
    
    public void FinalizePath(Path path)
    {
        route.path.type = Path.Type.Loaded;

        if (step < path.routeList.Count)
        {
            controllers[path.routeList[step].controllerIndex].FinalizePath(path);
        } else {

            SetPreviousEditor();
            SetHistory(path);

            layoutSection.TargetController.FinalizeController();
        }
    }
    
    private void SetPreviousEditor()
    {
        if (layoutSection.dataEditor == null) return;

        layoutSection.previousEditor = layoutSection.dataEditor;
        layoutSection.previousDataSource = layoutSection.dataEditor.ElementData;
        layoutSection.previousElementDataList = layoutSection.dataEditor.ElementDataList.ToList();
    }

    private void SetHistory(Path path)
    {
        path.historyGroup = historyGroup;
    }

    public bool GetComponents(Path path)
    {
        if (step < path.routeList.Count)
            controllers[path.routeList[step].controllerIndex].GetComponents(path);

        return GetComponents<IAction>().Count() > 0;
    }

    public void SetSubControllers(Path path)
    {
        if (subControllerManager != null)
            subControllerManager.SetTabs(this, path);

        if (step < path.routeList.Count)
            controllers[path.routeList[step].controllerIndex].SetSubControllers(path);
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

        if (step < path.routeList.Count)
            controllers[path.routeList[step].controllerIndex].SetActions(path);
    }

    void InitializeTabs(Path path)
    {
        if (step == path.routeList.Count)
            path.Add();      
    }

    public void GetTargetLayout(Path path, int layoutStep)
    {
        this.layoutStep = step;

        if (GetComponent<LayoutDependency>() != null)
            layoutSection.TargetView = GetComponent<LayoutDependency>();

        if (step < path.routeList.Count)
        {
            controllers[path.routeList[step].controllerIndex].GetTargetLayout(path, layoutStep);
        }
    }

    public void ClosePath(Path path)
    {
        foreach (IAction action in GetComponents<IAction>())
            action.CloseAction();

        if (step < path.routeList.Count)
            controllers[path.routeList[step].controllerIndex].ClosePath(path);
    }

    public void CloseTabs(Path path)
    {
        if (subControllerManager != null)
            subControllerManager.CloseTabs();

        if (layoutStep < path.routeList.Count)
            controllers[path.routeList[layoutStep].controllerIndex].CloseTabs(path);
    }
}
