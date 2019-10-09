using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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
    private bool hasComponents;

    public void InitializeForm()
    {
        foreach (EditorSection section in editorSections)
            section.InitializeSection(this);

        baseController.InitializeDependencies();
    }

    #region Path
    public void InitializePath(Path path)
    {
        OpenPath(path);
        
        OpenLayout(path);
        
        SetComponents(path);

        previousPath = activePath;
        activePath = path;

        active = true;
        closed = false;

        //Follows path, activates form components and adds last route to history
        baseController.FinalizePath(path);

        ResetSiblingForm();
    }

    public void OpenPath(Path path)
    {
        //Close the initialization of previous path
        ClosePath();
        
        //Flesh out the path and determine the target controller
        baseController.OpenPath(path, path.start);

        //Activate sections with a target controller
        ActivateSections();
        
        //Close editors of inactive sections
        CloseSections();

        //Initialize editors
        InitializeSections();
        
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
        CloseFormLayout();

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
    }

    private void InitializeSections()
    {
        foreach(EditorSection editorSection in editorSections)
        {
            if (editorSection.targetController == null) continue;

            editorSection.targetController.InitializeController();
        }
    }

    private void ActivateSections()
    {
        foreach (EditorSection section in editorSections)
        {
            if (section.targetController == null) continue;

            section.ActivateEditor();
        }
    }

    private void CloseSections()
    {
        foreach (EditorSection editorSection in editorSections)
        {
            if (editorSection.dataEditor == null) continue;

            //If section is inactive or the section's editor doesn't match the previous editor
            if (!editorSection.active || !MatchPrevious(editorSection))
                editorSection.CancelEdit();
        }
    }

    private bool MatchPrevious(EditorSection editorSection)
    {
        if (editorSection.previousDataSource == null) return true;

        return ((GeneralData)editorSection.previousDataSource).Equals((GeneralData)editorSection.dataEditor.Data.dataElement);
    }

    private void CloseFormLayout()
    {
        if (closed) return;

        CloseLayout();

        baseController.CloseTabs(activePath);

        CloseComponents();

        GetComponent<LayoutManager>().ResetSiblingLayout();
    }

    private void SetComponents(Path path)
    {
        //Activate all components along the path and sort them
        hasComponents = baseController.SetComponents(path);

        if (hasComponents)
            ComponentManager.componentManager.SortComponents();

        if (formComponent != null)
            formComponent.SetIcon(true);
    }

    private void CloseComponents()
    {
        if (hasComponents)
        {
            ComponentManager.componentManager.CloseComponents();
            hasComponents = false;
        }
    }

    public void ResetPath()
    {
        if (active)
            InitializePath(activePath);
    }

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
        if (!gameObject.activeInHierarchy) return;

        OpenLayout(activePath);

        SetComponents(activePath);

        SelectionManager.SelectElements();

        ResetSiblingForm();
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

    public void CloseForm()
    {
        if (!active) return;
        
        ClosePath();
        CloseFormLayout();

        closed = true;

        ResetSiblingForm();

        if (formComponent != null)
            formComponent.SetIcon(false);
    }
}
