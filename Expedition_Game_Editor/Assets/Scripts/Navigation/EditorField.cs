using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class EditorField : MonoBehaviour
{
    public WindowManager    windowManager       { get; set; }
    public EditorController target_controller   { get; set; }
    public SelectionGroup   selectionGroup      { get; set; }

    public void InitializeField(WindowManager new_windowManager)
    {
        selectionGroup = GetComponent<SelectionGroup>();

        //Remove this later
        windowManager = new_windowManager;
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

    public void OpenController(Path path)
    {
        if (target_controller.path.Equals(path))
            target_controller.OpenController();
    }

    public void ClosePath(Path active_path, Path new_path)
    {
        if (GetComponent<SelectionGroup>() != null)
            GetComponent<SelectionGroup>().Deactivate();

        if (target_controller.GetComponent<EditorDependency>() != null)
            target_controller.GetComponent<EditorDependency>().CloseDependency();

        windowManager.baseController.ClosePath(active_path);

        target_controller.CloseLayout();

        if (target_controller.GetComponent<EditorDependency>() != null)
            target_controller.GetComponent<EditorDependency>().Deactivate();

        target_controller.CloseEditor();

        target_controller = null;   
    }
}
