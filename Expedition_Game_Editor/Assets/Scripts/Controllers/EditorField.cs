﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class EditorField : MonoBehaviour
{
    public SectionManager   sectionManager      { get; set; }
    public EditorController target_controller   { get; set; }
    public Path previous_controller_path        { get; set; }

    public void InitializeField(SectionManager new_section)
    {
        //Remove this later
        sectionManager = new_section;
    }

    public void SetPreviousTarget()
    {
        previous_controller_path = target_controller.path.Copy();
    }

    public void ActivateDependencies()
    {
        //Activate necessary components to visualize the target editor
        if (target_controller.GetComponent<EditorDependency>() != null)
            target_controller.GetComponent<EditorDependency>().Activate();       
    }

    public void InitializeLayout(Path path)
    {
        //First wave layout: Adjust size of fields and windows
        target_controller.InitializeLayout();  
    }

    public void SetDependencies(Path path)
    {
        //Adjust size of dependency content based on active headers and footers
        if (target_controller.GetComponent<EditorDependency>() != null)
            target_controller.GetComponent<EditorDependency>().SetDependency(); 
    }

    public void InitializeController()
    {
        target_controller.InitializeController();
    }

    public void OpenEditor()
    {
        target_controller.OpenEditor();  
    }

    public void FinalizeController()
    {
        target_controller.FinalizeController();
    }

    public void ClosePath(Path active_path)
    {
        if (target_controller.GetComponent<EditorDependency>() != null)
            target_controller.GetComponent<EditorDependency>().CloseDependency();

        sectionManager.baseController.ClosePath(active_path);

        target_controller.CloseLayout();

        if (target_controller.GetComponent<EditorDependency>() != null)
            target_controller.GetComponent<EditorDependency>().Deactivate();

        target_controller.CloseEditor();

        target_controller = null;   
    }
}