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
    public EditorController main_controller { get; set; }

    public EditorSection[]    editor_sections;

    public EditorForm sibling_form;

    public void InitializeForm()
    {
        foreach (EditorSection section in editor_sections)
            section.InitializeSection(this);
    }

    #region Path
    public void InitializePath(Path path, bool reload = false)
    {
        //Debug.Log(EditorManager.PathString(path));
        
        OpenPath(path, reload);
        OpenLayout(path);

        active_path = path;
        active = true;

        //Auto select element
        main_controller.FinalizeController();
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

        //Follows path and adds last route to history
        baseController.FinalizePath(path);

        path.type = Path.Type.Loaded;
    }

    public void ClosePath()
    {
        if (!active) return;

        baseController.ClosePath(active_path);

        foreach (EditorSection section in editor_sections)
            section.ClosePath();

        active = false;
    }
    #endregion

    public void OpenLayout(Path path)
    {
        //Close previous active layout
        CloseLayout(main_form);

        //Get the controller that must be visualized
        baseController.GetTargetLayout(path);

        //Activate necessary components to visualize the target editor
        InitializeLayout();

        //Follows path and activates tabs where indicated
        baseController.SetSubControllers(path);

        //Activate dependencies and set content layout based on header and footer
        SetLayout();

        //Set up the organizers
        InitializeDisplay();

        //Set the visual component (list/camera)
        SetDisplay();

        //Activate all components along the path and sort them
        if (baseController.SetComponents(path))
            ComponentManager.componentManager.SortComponents();  
    }

    private void CloseLayout(bool close_components)
    {
        //Tries to close with new target controller. Should be old target
        CloseLayout();

        baseController.CloseTabs(active_path);

        CloseDisplay();

        if (close_components)
            ComponentManager.componentManager.CloseComponents();

        GetComponent<LayoutManager>().ResetSiblingLayout();
    }

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
        SelectionManager.SelectElements();
    }

    public void ResetSiblingForm()
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
        if (!active) return;

        ClosePath();
        CloseLayout(close_components);
        CloseDisplay();

        ResetSiblingForm();
    }
}
