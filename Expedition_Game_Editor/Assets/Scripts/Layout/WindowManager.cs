using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class WindowManager : MonoBehaviour
{
    public Path active_path = new Path();

    public EditorController baseController;

    public WindowManager    sibling_window;
    public EditorField[]    editor_fields;

    public void InitializeWindow()
    {
        foreach (EditorField field in editor_fields)
            field.InitializeField(this);
    }

    public void InitializePath(Path path)
    {
        //Close the initialization of previous path
        ClosePath(path);
        
        //Determine the target controller
        baseController.InitializePath(path, 0);

        //Load controller data
        baseController.GetData(path); //special rules

        //Activate target dependencies
        ActivateDependencies();

        //Set layout of target controllers
        InitializeLayout(path);

        //Follow the same path to activate anything along its way
        baseController.SetComponents(path);

        //Set layout of dependencies
        SetDependencies(path);

        //Set visual components of editor (list/preview)
        InitializeController();

        //Load specific editor (and add to history)
        OpenController(path); //Special rules

        active_path = path;
    }

    public void OpenPath(Path path)
    {
        InitializePath(path);

        if (sibling_window != null)
            sibling_window.ResetPath();
    }

    public void ResetPath()
    {
        InitializePath(active_path);
    }


    private void ClosePath(Path path)
    {
        foreach (EditorField field in editor_fields)
        {
            if (field.target_controller != null)
                field.ClosePath(active_path, path);
        }
    }

    private void ActivateDependencies()
    {
        foreach (EditorField field in editor_fields)
        {
            if (field.target_controller != null)
                field.ActivateDependencies();
        }
    }
    private void InitializeLayout(Path path)
    {
        foreach (EditorField field in editor_fields)
        {
            if (field.target_controller != null)
                field.InitializeLayout(path);
        }
    }

    private void SetDependencies(Path path)
    {
        foreach (EditorField field in editor_fields)
        {
            if (field.target_controller != null)
                field.SetDependencies(path);
        }
    }
    private void InitializeController()
    {
        foreach (EditorField field in editor_fields)
        {
            if (field.target_controller != null)
                field.InitializeController();
        }
    }
    private void OpenController(Path path)
    {
        foreach (EditorField field in editor_fields)
        {
            if (field.target_controller != null)
                field.OpenController(path);        
        }
    }
}
