﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PathController : MonoBehaviour
{
    public Route route      { get; set; }

    private Route previousRoute;

    public int step         { get; set; }
    public int layoutStep   { get; set; }

    public bool loaded      { get; set; }

    public bool autoExtend;

    public HistoryElement   history;

    public EditorSection    editorSection;

    private EditorController EditorController   { get { return GetComponent<EditorController>(); } }

    public IDataController  dataController      { get; set; }
    public IEditor          dataEditor          { get; set; }

    public SubControllerManager subControllerManager;
    public PathController[] controllers;

    private PathController parentController;

    public SelectionElement Origin
    {
        get { return route.path.origin.selectedElement; }
    }

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
    public void InitializePath(Path mainPath, int step, bool reload, PathController parentController = null)
    {
        //This is an exception to ensure shared controllers have the correct values
        //One way to circumvent this is to simply get rid of shared controllers
        this.parentController = parentController;

        this.step = step;
        
        Path path = mainPath.Trim(step);
        
        if (path.route.Count > 0)
            route = path.route.Last().Copy();
        else        
            route = new Route(path);

        route.path = path;
        
        if (step > 0)
        {
            //Don't check this if force is true. Must load!
            if (!reload)
                loaded = IsLoaded();
            
            //If this hasn't loaded, force load the next one
            if (!loaded || reload)
            {
                reload = true;
            } 
        }

        //if(!loaded && autoExtend)
        //{
        //    Debug.Log("ADD MORE");
        //    mainPath.Add();
        //}

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
        InitializeForm();

        editorSection.targetPath = mainPath;

        if (EditorController != null)
        {
            editorSection.targetController = EditorController;
            editorSection.editorForm.mainController = EditorController;

            EditorController.InitializeController();
        }

        if (dataEditor != null)
            editorSection.dataEditor = dataEditor;
        
        if (step < mainPath.route.Count)
        {
            controllers[mainPath.route[step].controller].InitializePath(mainPath, step + 1, reload, this);
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
            dataEditor = GetComponent<IEditor>();
        } else {
            if (parentController != null && parentController.dataEditor != null)
                dataEditor = parentController.dataEditor;
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

        SetForm();

        if (step < path.route.Count)
        {
            controllers[path.route[step].controller].FinalizePath(path);
        } else {
            SetHistory();
        }
    }

    private void InitializeForm()
    {
        foreach (FormComponent form in GetComponents<FormComponent>())
            form.InitializeForm(this);
    }

    private void SetForm()
    {
        foreach (FormComponent form in GetComponents<FormComponent>())
            form.SetForm();
    }

    public void ForceLoadPath(Path path)
    {
        loaded = true;
        
        if (step < path.route.Count)
            controllers[path.route[step].controller].ForceLoadPath(path);    
    }

    public bool IsLoaded()
    {
        //If there is no previous controller then it definitely hasn't loaded yet
        if (editorSection.previousTargetPath == null)
            return false;

        //If current step is longer than the previous route length, then it definitely hasn't been loaded yet
        if (step > editorSection.previousTargetPath.route.Count)
            return false;

        if (previousRoute != null)
            return route.Equals(previousRoute);

        //If false then everything afterwards must be false as well
        return route.path.Equals(editorSection.previousTargetPath);
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
            editorSection.targetLayout = GetComponent<LayoutDependency>();

        if (step < path.route.Count)
            controllers[path.route[step].controller].GetTargetLayout(path, layoutStep);
    }

    public void ClosePath(Path path)
    {
        SelectionManager.CancelSelection(route);

        foreach (IComponent component in GetComponents<IComponent>())
            component.CloseComponent();

        if (GetComponent<IEditor>() != null)
            GetComponent<IEditor>().CloseEditor();

        if (step < path.route.Count)
            controllers[path.route[step].controller].ClosePath(path);

        loaded = false;
    }

    public void CloseTabs(Path path)
    {
        if (subControllerManager != null)
            subControllerManager.CloseTabs();

        if (layoutStep < path.route.Count)
            controllers[path.route[layoutStep].controller].CloseTabs(path);
    }
}
