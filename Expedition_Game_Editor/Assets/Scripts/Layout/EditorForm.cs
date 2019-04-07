using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class EditorForm : MonoBehaviour
{
    public bool main_form;

    public bool active { get; set; }

    public Path active_path = new Path();

    public PathController baseController;
    //public PathController main_controller { get; set; }

    public EditorSection[]    editor_sections;

    public EditorForm sibling_form;

    public void InitializeForm()
    {
        foreach (EditorSection section in editor_sections)
            section.InitializeSection(this);
    }

    public void InitializePath(Path path, bool reload = false)
    {
        //Debug.Log(EditorManager.PathString(path));
        
        OpenPath(path, reload);
        OpenLayout(path);

        //ResetSiblingLayout();

        active = true;
    }

    public void OpenPath(Path path, bool reload)
    {
        //Close the initialization of previous path
        ClosePath();

        //Flesh out the path and determine the target controller
        baseController.InitializePath(path, path.start, reload, null);

        //Save previous target to compare data with
        SetPreviousPath();

        //Set visual components of editor (list/preview)
        InitializeController();

        FinalizeController();

        baseController.FinalizePath(path);

        path.type = Path.Type.Loaded;
        active_path = path;
    }

    public void ClosePath()
    {
        if (active)
        {
            baseController.ClosePath(active_path);

            foreach (EditorSection section in editor_sections)
                section.ClosePath();

            active = false;
        }
    }

    public void OpenLayout(Path path)
    {
        //Close Layout
        CloseLayout(main_form);

        baseController.GetTargetLayout(path);

        //Activate target dependencies
        InitializeLayout();

        baseController.SetTabs(path);

        //Set layout of dependencies (tabs must be activated to serve as header)
        SetLayout();

        //Follow the same path to activate anything along its way
        //If the path contains any component, sort them afterwards

        if (baseController.SetComponents(path))
            ComponentManager.componentManager.SortComponents();

        //Set up the organizers
        InitializeDisplay();

        //Set the organizers
        SetDisplay();

        SelectionManager.SelectElements();
    }

    private void CloseLayout(bool close_components)
    {
        CloseLayout();

        //Also closes tabs
        baseController.CloseLayout(active_path);

        if (close_components)
            ComponentManager.componentManager.CloseComponents();

        //Affects sibling form
        GetComponent<LayoutManager>().ResetLayout();

        CloseDisplay();
    }

    //public void OpenPath(Path path)
    //{
    //    InitializePath(path);

    //    
    //}

    private void SetPreviousPath()
    {
        foreach (EditorSection section in editor_sections)
            section.SetPreviousPath();      
    }

    public void ResetPath()
    {
        if (active)
            InitializePath(active_path, true);
    }

    #region Controller
    private void InitializeController()
    {
        foreach (EditorSection section in editor_sections)
            section.InitializeController();
    }

    private void FinalizeController()
    {
        foreach (EditorSection section in editor_sections)
            section.FinalizeController();
    }
    #endregion

    #region Layout
    private void InitializeLayout()
    {
        foreach (EditorSection section in editor_sections)
            section.InitializeLayout();
    }

    private void SetLayout()
    {
        foreach (EditorSection section in editor_sections)
            section.SetLayout();
    }

    private void CloseLayout()
    {
        foreach (EditorSection section in editor_sections)
            section.CloseLayout();
    }

    public void ResetLayout()
    {
        OpenLayout(active_path);  
    }

    public void ResetSiblingLayout()
    {
        if (sibling_form != null)
            sibling_form.ResetLayout();
    }
    #endregion

    #region Display
    private void InitializeDisplay()
    {
        foreach (EditorSection section in editor_sections)
            section.InitializeDisplay();
    }

    private void SetDisplay()
    {
        foreach (EditorSection section in editor_sections)
            section.SetDisplay();
    }

    private void CloseDisplay()
    {
        foreach (EditorSection section in editor_sections)
            section.CloseDisplay();
    }
    #endregion

    public void CloseForm(bool close_components)
    {
        ClosePath();
        CloseLayout(close_components);
        CloseDisplay();
    }
}
