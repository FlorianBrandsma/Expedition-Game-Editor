using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class EditorForm : MonoBehaviour
{
    [HideInInspector]
    public bool active;

    [HideInInspector]
    public FormComponent formComponent;

    [HideInInspector]
    public EditorController mainController;

    public PathController baseController;

    public bool mainForm;
    public EditorForm siblingForm;
    public EditorSection[] editorSections;

    public Path activePath = new Path();
    public Path previousPath;

    private bool closed;

    public void InitializeForm()
    {
        foreach (EditorSection section in editorSections)
            section.InitializeSection(this);

        baseController.InitializeDependencies();
    }

    #region Path
    public void InitializePath(Path path, bool reload = false)
    {
        OpenPath(path, reload);
        OpenLayout(path);

        previousPath = activePath;
        activePath = path;
        active = true;
        closed = false;
        //Auto select element
        mainController.FinalizeController();

        if (formComponent != null)
            ResetSiblingForm();
    }

    public void OpenPath(Path path, bool reload)
    {
        //Close the initialization of previous path
        ClosePath();

        //Flesh out the path and determine the target controller
        baseController.InitializePath(path, path.start, reload);

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
        
        baseController.ClosePath(activePath);

        foreach (EditorSection section in editorSections)
            section.ClosePath();

        active = false;
    }
    #endregion

    public void OpenLayout(Path path)
    {
        //Close previous active layout
        CloseLayout(mainForm);

        //Get the controller that must be visualized
        baseController.GetTargetLayout(path, path.start);

        //Activate necessary components to visualize the target editor
        InitializeLayout();

        //Follows path and activates tabs where indicated
        baseController.SetSubControllers(path);

        //Activate dependencies and set content layout based on header and footer
        SetLayout();

        //Open the target editors
        OpenEditor();

        //Activate all components along the path and sort them
        if (baseController.SetComponents(path))
            ComponentManager.componentManager.SortComponents();  
    }

    private void CloseLayout(bool close_components)
    {
        if (closed) return;

        //Tries to close with new target controller. Should be old target
        CloseLayout();

        baseController.CloseTabs(activePath);

        if (close_components)
            ComponentManager.componentManager.CloseComponents();

        GetComponent<LayoutManager>().ResetSiblingLayout();
    }

    private void SetPreviousPath()
    {
        foreach (EditorSection section in editorSections)
            section.SetPreviousPath();      
    }

    public void ResetPath()
    {
        if (active)
            InitializePath(activePath, true);
    }

    #region Controller
    private void InitializeController()
    {
        foreach (EditorSection section in editorSections)
            section.InitializeController();
    }
    #endregion

    #region Layout
    private void InitializeLayout()
    {
        foreach (EditorSection section in editorSections)
            section.InitializeLayout();
    }

    private void SetLayout()
    {
        foreach (EditorSection section in editorSections)
            section.SetLayout();
    }

    private void CloseLayout()
    {
        foreach (EditorSection section in editorSections)
            section.CloseLayout();
    }

    public void ResetLayout()
    {
        OpenLayout(activePath);
        SelectionManager.SelectElements();
    }

    public void ResetSiblingForm()
    {
        if (siblingForm != null)
            siblingForm.ResetLayout();
    }
    #endregion

    #region Editor
    private void OpenEditor()
    {
        foreach (EditorSection section in editorSections)
            section.OpenEditor();
    }
    #endregion

    public void CloseForm(bool close_components)
    {
        if (!active) return;
        
        ClosePath();
        CloseLayout(close_components);

        closed = true;

        ResetSiblingForm();
    }
}
