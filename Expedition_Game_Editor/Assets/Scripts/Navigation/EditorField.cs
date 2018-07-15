using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class EditorField : MonoBehaviour
{
    public Path active_path = new Path(new List<int>(), new List<int>());

    public EditorController target_editor { get; set; }

    public EditorController active_target { get; set; }

    public WindowManager windowManager { get; set; }

    public RectTransform field_rect { get; set; }

    public void InitializeField(WindowManager new_windowManager)
    {
        field_rect = GetComponent<RectTransform>();

        windowManager = new_windowManager;
    }

    public void InitializePath(Path path)
    {
        if(target_editor != null)
        {
            //Activate necessary components to visualize the target editor
            target_editor.GetComponent<EditorDependency>().Activate();

            //First wave layout: Adjust size of fields and windows
            target_editor.InitializeLayout();
        }  
    }

    public void SetPath(Path path)
    {
        if (target_editor != null)
        {
            //Activate target specific elements before second layout wave
            target_editor.GetComponent<EditorDependency>().Activate();

            target_editor.SetEditor();

            //Adjust size of dependency content based on active headers and footers
            target_editor.GetComponent<EditorDependency>().SetDependency();

            //Open the editor
            if (target_editor.controller_path.Equals(path))
                target_editor.OpenEditor();
        }
    }

    public void ClosePath(Path active_path, Path new_path)
    {
        if (target_editor != null)
        {
            target_editor.GetComponent<EditorDependency>().CloseDependency();

            windowManager.baseController.ClosePath(active_path);

            target_editor.CloseLayout();

            target_editor.GetComponent<EditorDependency>().Deactivate();

            target_editor.CloseEditor();

            target_editor = null;
        }  
    }
}
