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

        foreach (EditorField field in editor_fields)
        {
            if(field.target_controller != null)
                field.ClosePath(active_path, path);
        }

        //Determine the target editor
        baseController.InitializeController(path, 0);
        
        //Initialize the path
        foreach (EditorField field in editor_fields)
        {
            if(field.target_controller != null)
                field.InitializePath(path);
        }
        
        //Follow the same path to activate anything along its way
        baseController.SetPath(path);

        //Set everything along the path
        foreach (EditorField field in editor_fields)
        {
            if(field.target_controller != null)
                field.SetPath(path);
        }

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
}
