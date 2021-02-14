using UnityEngine;
using System.Collections.Generic;

public class EditorForm : MonoBehaviour
{
    public bool pauseTime;

    public EditorLayer editorLayer;

    [HideInInspector]
    public bool loaded;

    [HideInInspector]
    public bool activeInPath;

    [HideInInspector]
    public FormAction formAction;

    public PathController baseController;

    public LayoutSection[] editorSections;

    public Path activePath = new Path();
    public Path previousPath;

    public Path activeViewPath = new Path();

    public List<EditorForm> siblingFormList;

    public void InitializeForm()
    {
        baseController.InitializeDependencies();

        activePath.form = this;
        activeViewPath.form = this;
    }

    #region Path
    public void InitializePath(Path path)
    {
        OpenPath(path);
    }

    public void OpenPath(Path path)
    {
        //Close the initialization of previous path
        ClosePath();

        previousPath = activePath;
        activePath = path;

        if (activePath.routeList.Count == 0) return;

        activeInPath = true;

        TimeManager.instance.PauseTime(pauseTime);

        //Flesh out the path and determine the target controller
        baseController.OpenPath(path, path.start);

        //Activate sections with a target controller
        ActivateSections();

        //Cancel edit of inactive sections
        CancelEdit();

        //Initialize editors
        InitializeSections();

        path.type = Path.Type.Loaded;
    }

    public void OpenView()
    {
        //Close previous active layout (dependencies, tabs and reset layout values and content size)
        CloseView();
        
        activeViewPath = activePath;

        if (activeViewPath.routeList.Count == 0) return;

        //Get the controller that must be visualized
        baseController.GetTargetLayout(activeViewPath, activeViewPath.start);

        //Activate dependencies and initialize layout anchors
        InitializeSectionLayout();

        //Follows path and activates tabs where indicated
        baseController.SetSubControllers(activeViewPath);
        
        //Activate dependencies and set content layout based on header and footer
        SetSectionLayout();
    }

    public void OpenSegments()
    {
        if (!gameObject.activeInHierarchy) return;

        //Open the target editors
        OpenSectionSegments();
        
        //Activate and sort actions
        SetActions(activeViewPath);
    }

    public void ResetEditor()
    {
        if (!gameObject.activeInHierarchy) return;

        foreach (LayoutSection section in editorSections)
            section.ResetEditor();
    }

    public void CloseSegments()
    {
        CloseSectionEditorSegments();

        if (GetComponent<ActionManager>() != null)
            GetComponent<ActionManager>().CloseActions();
    }

    public void FinalizePath()
    {
        if (activePath.routeList.Count == 0) return;

        baseController.FinalizePath(activePath);
    }

    public void ClosePath()
    {
        if (!activeInPath) return;
        
        baseController.ClosePath(activePath);

        foreach (LayoutSection section in editorSections)
            section.ClosePath();

        activeInPath = false;
    }

    public void ResetPath()
    {
        if (activeInPath)
            RenderManager.Render(activePath);
    }
    #endregion

    private void CancelEdit()
    {
        foreach (LayoutSection editorSection in editorSections)
        {
            if (editorSection.dataEditor == null) continue;

            //If section is inactive or the section's editor doesn't match the previous editor
            if (!editorSection.Active || !MatchPrevious(editorSection))
                editorSection.CancelEdit();
        }
    }

    private bool MatchPrevious(LayoutSection editorSection)
    {
        if (editorSection.previousEditor == null) return true;

        if (editorSection.previousEditor != editorSection.dataEditor)
            return false;

        return DataManager.Equals(editorSection.previousDataSource, editorSection.dataEditor.ElementData);
    }

    private void CloseView()
    {
        CloseSectionDependencies();
        
        baseController.CloseTabs(activeViewPath);
    }

    public void SetActions(Path path)
    {
        //Activate all components along the path and sort them
        baseController.SetActions(path);

        if (GetComponent<ActionManager>() != null)
            GetComponent<ActionManager>().SortActions();
    }
    
    #region Layout
    private void InitializeSectionLayout()
    {
        foreach (LayoutSection section in editorSections)
            section.InitializeLayout();
    }

    private void SetSectionLayout()
    {
        foreach (LayoutSection section in editorSections)
            section.SetLayout();
    }

    private void CloseSectionDependencies()
    {
        foreach (LayoutSection section in editorSections)
            section.CloseLayoutDependencies();
    }
    #endregion

    #region Sections
    private void InitializeSections()
    {
        foreach (LayoutSection editorSection in editorSections)
        {
            if (editorSection.TargetController != null)
                editorSection.TargetController.InitializeController();
            else
                editorSection.PreviousTargetController = null;
        }
    }

    private void ActivateSections()
    {
        foreach (LayoutSection section in editorSections)
        {
            if (section.TargetController == null) continue;

            section.Activate();
        }
    }

    public void OpenSectionSegments()
    {
        foreach (LayoutSection section in editorSections)
            section.OpenSegments();
    }

    private void CloseSectionEditorSegments()
    {
        foreach (LayoutSection section in editorSections)
            section.CloseEditorSegments();
    }
    #endregion

    #region Form
    public void CloseForm()
    {
        if (!activeInPath) return;

        if(pauseTime)
            TimeManager.instance.PauseTime(false);

        Path path = new Path()
        {
            form = this
        };

        RenderManager.Render(path);
    }
    #endregion
}