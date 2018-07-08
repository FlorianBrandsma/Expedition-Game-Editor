using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class WindowManager : MonoBehaviour
{
    private Path active_path = new Path(new List<int>(), new List<int>());

    public List<Path> history = new List<Path>();

    //0: unlimited
    public int history_min;
    public int history_max;

    public EditorController base_controller;

    public EditorField[] editor_fields;

    //Not here
    public LayoutManager layoutManager { get; set; }

    public void InitializeWindow()
    {
        layoutManager = GetComponent<LayoutManager>();

        foreach (EditorField field in editor_fields)
            field.InitializeField(this);
    }

    public void PreviousEditor()
    {
        //Open the last path in the last history
        if(history.Count > history_min)
        {
            history.RemoveAt(history.Count - 1);

            InitializePath(history[history.Count - 1], false);  
        }
    }

    public void InitializePath(Path path, bool add_history)
    {
        if(active_path.editor.Count > 0)
            ClosePath(active_path);

        active_path = path;

        if (add_history)
        {
            if(history_max == 0 || history.Count < history_max)
                history.Add(path);
        }
        
        //Determine the target editor
        base_controller.InitializePath(path, 0);

        //Activate necessary components to visualize the target editor
        ActivateDependency();

        //First wave layout: Adjust size of fields and windows
        InitializeLayout();

        //Follow the same path to activate anything along its way
        base_controller.SetPath(path, 0);

        //Activate target specific elements before second layout wave
        SetEditor();

        //Adjust size of dependency content based on active headers and footers
        SetDependencies();

        //Open the editor
        OpenEditor();
    }

    void ActivateDependency()
    {
        foreach (EditorField field in editor_fields)
            field.ActivateDependency();    
    }

    void SetDependencies()
    {
        foreach (EditorField field in editor_fields)
            field.SetDependency();
    }

    void InitializeLayout()
    {
        foreach (EditorField field in editor_fields)
            field.InitializeLayout();
    }

    void SetEditor()
    {
        foreach (EditorField field in editor_fields)
            field.SetEditor();
    }

    void OpenEditor()
    {
        foreach (EditorField field in editor_fields)
            field.OpenEditor();
    }

    public void ResetPath()
    {
        InitializePath(active_path, false);
    }

    void ClosePath(Path path)
    {
        CloseDependency();

        base_controller.ClosePath(path, 0);

        CloseLayout();

        DeactivateDependency();

        CloseEditor();
    }

    void DeactivateDependency()
    {
        foreach (EditorField field in editor_fields)
            field.DeactivateDependency();
    }

    void CloseLayout()
    {
        foreach (EditorField field in editor_fields)
            field.CloseLayout();
    }

    void CloseDependency()
    {
        foreach (EditorField field in editor_fields)
            field.CloseDependency();
    }

    void CloseEditor()
    {
        foreach (EditorField field in editor_fields)
            field.CloseEditor();      
    }
}
