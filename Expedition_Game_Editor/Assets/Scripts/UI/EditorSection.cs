using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class EditorSection : MonoBehaviour
{
    [HideInInspector]
    public bool active;

    [HideInInspector]
    public EditorForm editorForm;

    [HideInInspector]
    public EditorController displayTargetController;

    [HideInInspector]
    public EditorController targetController;

    [HideInInspector]
    public LayoutDependency targetLayout;

    [HideInInspector]
    public Path targetPath;

    [HideInInspector]
    public Path previousTargetPath;
    
    public IEditor dataEditor;

    public ButtonActionManager buttonActionManager;

    public void InitializeSection(EditorForm editorForm)
    {
        this.editorForm = editorForm;

        if (buttonActionManager != null)
            buttonActionManager.InitializeButtons(this);
    }

    public void SetPreviousPath()
    {
        previousTargetPath = targetPath;
    }

    public void InitializeLayout()
    {
        if (targetLayout == null) return;

        targetLayout.InitializeLayout();       
    }

    public void SetLayout()
    {
        if (targetLayout == null) return;

        //Adjust size of dependency content based on active headers and footers
        targetLayout.SetLayout(); 
    }

    public void OpenEditor()
    {
        if (targetController == null) return;

        displayTargetController = targetController;

        displayTargetController.OpenSegments();

        SetActionButtons();

        active = true;
    }

    public void SetActionButtons()
    {
        if (dataEditor == null) return;

        if (buttonActionManager != null && dataEditor != null)
            buttonActionManager.SetButtons(dataEditor.Changed());
    }

    public void ClosePath()
    {
        if (targetController == null) return;

        if (buttonActionManager != null)
            buttonActionManager.CloseButtons();

        targetController = null;
        targetPath = null;

        active = false;
    }

    public void CloseLayout()
    {
        if (targetLayout == null) return;

        displayTargetController.CloseSegments();

        targetLayout.CloseLayout();

        targetLayout = null;
    }

    public void ApplyChanges()
    {
        if(dataEditor != null)
            dataEditor.ApplyChanges();
    }

    public void CancelEdit()
    {
        EditorManager.editorManager.PreviousEditor();
    }
}
