using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class EditorForm : MonoBehaviour
{
    public bool active { get; set; }

    public Path active_path = new Path();

    public EditorController baseController;
    public EditorController main_controller { get; set; }

    public EditorSection[]    editor_sections;

    public void InitializeForm()
    {
        foreach (EditorSection section in editor_sections)
            section.InitializeSection(this);
    }

    public void InitializePath(Path path)
    {
        //Close the initialization of previous path
        CloseForm();
        
        //Determine the target controller
        baseController.InitializePath(path, 0, false);

        //Save previous target to compare data with
        SetPreviousTarget();

        //Activate target dependencies
        ActivateDependencies();

        //Follow the same path to activate anything along its way
        baseController.SetComponents(path);

        //Set layout of dependencies
        SetDependencies(path);

        //Set visual components of editor (list/preview)
        InitializeController();

        //Load specific editor (and add to history)
        OpenEditor();

        active_path = path;
        active = true;

        FinalizeController();
    }

    public void OpenPath(Path path)
    {
        InitializePath(path);

        //Does something with the width
        
        //hard reset
        //if (sibling_form != null)
        //    sibling_form.ResetPath(); 
    }

    public void ResetPath()
    {
        if(active)
            InitializePath(active_path);
    }

    public void CloseForm()
    {
        if(active)
        {
            foreach (EditorSection section in editor_sections)
            {
                if (section.target_controller != null)
                    section.ClosePath(active_path);
            }

            active = false;
        } 
    }

    private void SetPreviousTarget()
    {
        foreach (EditorSection section in editor_sections)
        {
            if (section.target_controller != null)
                section.SetPreviousTarget();
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

        if (main_controller != null)
            main_controller.FinalizeMainController();
    }
}
