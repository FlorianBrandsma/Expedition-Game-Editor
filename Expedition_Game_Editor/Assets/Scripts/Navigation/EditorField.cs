using UnityEngine;
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
    public EditorController previous_controller { get; set; }
    public SelectionGroup   selectionGroup      { get; set; }

    public void InitializeField(SectionManager new_section)
    {
        selectionGroup = GetComponent<SelectionGroup>();

        //Remove this later
        sectionManager = new_section;
    }

    public void SetPreviousTarget()
    {
        previous_controller = target_controller;
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

    public void ClosePath(Path active_path, Path new_path)
    {
        if (GetComponent<SelectionGroup>() != null)
            GetComponent<SelectionGroup>().Deactivate();

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
