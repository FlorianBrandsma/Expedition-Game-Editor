using UnityEngine;
using System.Collections;

public class EditorField : MonoBehaviour
{  
    public EditorController target_editor { get; set; }

    public WindowManager windowManager { get; set; }

    public RectTransform field_rect { get; set; }

    public void InitializeField(WindowManager new_windowManager)
    {
        field_rect = GetComponent<RectTransform>();

        windowManager = new_windowManager;
    }

    public void ActivateDependency()
    {
        if (target_editor != null)
            target_editor.GetComponent<EditorDependency>().Activate();
    }

    public void SetDependency()
    {
        if (target_editor != null)
            target_editor.GetComponent<EditorDependency>().SetDependency();
    }

    public void DeactivateDependency()
    {
        if (target_editor != null)
            target_editor.GetComponent<EditorDependency>().Deactivate();
    }

    public void CloseDependency()
    {
        if (target_editor != null)
            target_editor.GetComponent<EditorDependency>().CloseDependency();
    }

    public void InitializeLayout()
    {
        if (target_editor != null)
            target_editor.InitializeLayout();
    }

    public void CloseLayout()
    {
        if (target_editor != null)
            target_editor.CloseLayout();
    }

    public void SetEditor()
    {
        if (target_editor != null)
            target_editor.SetEditor();
    }

    public void OpenEditor()
    {
        if (target_editor != null)
            target_editor.OpenEditor();
    }

    public void CloseEditor()
    {
        if (target_editor != null)
            target_editor.CloseEditor();

        target_editor = null;
    }
}
