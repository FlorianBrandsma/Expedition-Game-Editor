using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PathController : MonoBehaviour
{
    public Route route { get; set; }

    public int step     { get; set; }

    public bool loaded  { get; set; }

    public HistoryElement   history;

    public EditorSection    editorSection;

    public IDataController  dataController  { get; set; }
    public IEditor          dataEditor      { get; set; }

    public SubControllerManager subControllerManager;
    public PathController[] controllers;

    private PathController parent_controller;

    //Necessary steps to set up the correct path for the controller
    public void InitializePath(Path main_path, int step, bool reload, PathController parent_controller)
    {
        this.parent_controller = parent_controller;

        if (GetComponent<EditorController>() != null)
            editorSection.target_controller = GetComponent<EditorController>();

        this.step = step;

        Path path = main_path.Trim(step);

        if (path.route.Count > 0)
            route = path.route.Last().Copy();
        else        
            route = new Route(path);

        route.path = path;
        
        if (step > 0)
        {
            //Don't check this if force is true. Must load!
            if(!reload)
                loaded = IsLoaded();

            //If this hasn't loaded, force load the next one
            if (!loaded || reload)
            {
                reload = true;
            } 
        }

        GetDataController();
        GetDataEditor();

        if (subControllerManager != null)
        {
            InitializeTabs(main_path);
        }
            
        InitializeComponents(main_path);

        if (step < main_path.route.Count)
        {
            controllers[main_path.route[step].controller].InitializePath(main_path, step + 1, reload, this);
        }
    }

    public void GetDataController()
    {
        if (GetComponent<IDataController>() != null)
        {
            dataController = GetComponent<IDataController>();
        } else {
            if (parent_controller != null && parent_controller.dataController != null)
                dataController = parent_controller.dataController;
        }
    }

    public void GetDataEditor()
    {
        if (GetComponent<IEditor>() != null)
        {
            dataEditor = GetComponent<IEditor>();
        } else {
            if (parent_controller != null && parent_controller.dataEditor != null)
                dataEditor = parent_controller.dataEditor;
        }
    }

    private void SetHistory()
    {
        if (history.group != HistoryManager.Group.None)
            history.AddHistory(route.path);   
    }

    public void FinalizePath(Path new_path)
    {
        route.path.type = Path.Type.Loaded;

        if (step < new_path.route.Count)
        {
            controllers[new_path.route[step].controller].FinalizePath(new_path);
        } else {
            SetHistory();
        }
    }

    public bool IsLoaded()
    {
        //if (GetComponent<ListProperties>() != null && GetComponent<ListProperties>().flexible_type)
        //    return false;

        //If there is no previous controller then it definitely hasn't loaded yet
        if (editorSection.previous_controller_path == null)
            return false;

        //If current step is longer than the previous route length, then it definitely hasn't been loaded yet
        if (step > editorSection.previous_controller_path.route.Count)
            return false;

        //If false then everything afterwards must be false as well
        return route.path.Equals(editorSection.previous_controller_path);
    }

    public bool GetComponents(Path new_path)
    {
        if (step < new_path.route.Count)
            controllers[new_path.route[step].controller].GetComponents(new_path);

        return GetComponents<IComponent>().Count() > 0;
    }

    public void SetTabs(Path new_path)
    {
        if (subControllerManager != null)
            subControllerManager.SetTabs(this, new_path);

        if (step < new_path.route.Count)
            controllers[new_path.route[step].controller].SetTabs(new_path);
    }

    private void InitializeComponents(Path new_path)
    {
        foreach (IComponent component in GetComponents<IComponent>())
            component.InitializeComponent(new_path);
    }

    public bool SetComponents(Path new_path)
    {
        foreach (IComponent component in GetComponents<IComponent>())
            component.SetComponent(new_path);

        if (step < new_path.route.Count)
            controllers[new_path.route[step].controller].SetComponents(new_path);

        return GetComponents<IComponent>().Count() > 0;
    }

    void InitializeTabs(Path new_path)
    {
        if (step == new_path.route.Count)
            new_path.Add();      
    }

    public void FilterRows(List<GeneralData> list) { }

    public void GetTargetLayout(Path path)
    {
        if (GetComponent<LayoutDependency>() != null)
            editorSection.target_layout = GetComponent<LayoutDependency>();

        if (step < path.route.Count)
            controllers[path.route[step].controller].GetTargetLayout(path);
    }

    public void ClosePath(Path path)
    {
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

        if (step < path.route.Count)
            controllers[path.route[step].controller].CloseTabs(path);
    }
}
