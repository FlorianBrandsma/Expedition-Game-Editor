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

        //Close the initialization of previous path
        CloseForm(main_form);

        //Determine the target controller
        baseController.InitializePath(path, path.start, reload, null);
        
        //Save previous target to compare data with
        SetPreviousTarget();

        //Activate target dependencies
        ActivateDependencies();

        baseController.SetTabs(path);

        //Set layout of dependencies
        SetDependencies(path);

        //Set visual components of editor (list/preview)
        InitializeController();

        //Load specific editor (and add to history)
        OpenEditor();
 
        //Follow the same path to activate anything along its way
        //If the path contains any component, sort them afterwards
        //if (baseController.SetComponents(path))
        //    ComponentManager.componentManager.SortComponents();

        active_path = path;
        active = true;

        FinalizeController();

        baseController.FinalizePath(path);

        path.type = Path.Type.Loaded;
    }

    public void OpenPath(Path path)
    {
        InitializePath(path);

        ResetSibling();
    }

    private void SetPreviousTarget()
    {
        foreach (EditorSection section in editor_sections)
        {
            if (section.target_controller != null)
                section.SetPreviousTarget();
            else
                section.previous_controller_path = null;
        }
    }

    private void ActivateDependencies()
    {
        foreach (EditorSection section in editor_sections)
        {
            if (section.target_controller != null)
                section.ActivateDependencies();
        }
    }

    private void SetDependencies(Path path)
    {
        foreach (EditorSection section in editor_sections)
        {
            if (section.target_controller != null)
                section.SetDependencies(path);
        }
    }
    private void InitializeController()
    {
        foreach (EditorSection section in editor_sections)
        {
            if (section.target_controller != null)
                section.InitializeController();
        }
    }
    private void OpenEditor()
    {
        foreach (EditorSection section in editor_sections)
        {
            if (section.target_controller != null)
                section.OpenEditor();        
        }
    }

    private void FinalizeController()
    {
        foreach (EditorSection section in editor_sections)
        {
            if (section.target_controller != null)
                section.FinalizeController();
        }       
    }

    public void ResetPath()
    {
        if (active)
            InitializePath(active_path, true);
    }

    public void ResetSibling()
    {
        if (sibling_form != null)
            sibling_form.ResetPath();
    }

    public void CloseForm(bool close_components)
    {
        if (active)
        {
            if (close_components)
                CloseComponents();

            CloseSections();

            baseController.ClosePath(active_path);

            active = false;
        }
    }

    private void CloseComponents()
    {
        ComponentManager.componentManager.CloseComponents();
    }

    private void CloseSections()
    {
        foreach (EditorSection section in editor_sections)
        {
            if (section.target_controller != null)
                section.CloseSection();
        }
    }
}
