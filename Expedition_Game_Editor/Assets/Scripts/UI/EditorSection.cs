using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class EditorSection : MonoBehaviour
{
    public bool active { get; set; }

    public EditorForm formManager      { get; set; }
    public EditorController target_controller   { get; set; }
    public Path previous_controller_path        { get; set; }

    public void InitializeSection(EditorForm new_form)
    {
        //Remove this later
        formManager = new_form;
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

        active = true;
    }

    public void FinalizeController()
    {
        target_controller.FinalizeController();
    }

    public void ClosePath(Path active_path)
    {
        if (target_controller.GetComponent<EditorDependency>() != null)
            target_controller.GetComponent<EditorDependency>().Deactivate();

        if (target_controller.GetComponent<EditorDependency>() != null)
            target_controller.GetComponent<EditorDependency>().CloseDependency();

        formManager.baseController.ClosePath(active_path);

        target_controller.CloseEditor();

        target_controller = null;

        active = false;
    }
}
