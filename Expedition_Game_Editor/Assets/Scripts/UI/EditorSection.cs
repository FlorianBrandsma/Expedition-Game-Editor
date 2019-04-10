using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class EditorSection : MonoBehaviour
{
    public bool active { get; set; }

    public EditorForm editorForm                { get; set; }
    public EditorController target_controller   { get; set; }
    public LayoutDependency target_layout       { get; set; }
    public Path previous_controller_path        { get; set; }
    public EditorController previous_target_controller     { get; set; }

    public IEditor dataEditor;

    public ButtonActionManager buttonActionManager;

    public void InitializeSection(EditorForm new_form)
    {
        //Remove this later
        editorForm = new_form;
    }

    public void InitializeController()
    {
        if (target_controller == null) return;

        if (buttonActionManager != null)
            buttonActionManager.InitializeButtons(this);

        target_controller.InitializeController();

        dataEditor = target_controller.pathController.dataEditor;
    }

    public void SetPreviousPath()
    {
        if (target_controller != null)
            previous_controller_path = target_controller.pathController.route.path;
        else
            previous_controller_path = null;
    }

    public void InitializeLayout()
    {
        if (target_layout == null) return;

        target_layout.InitializeLayout();       
    }

    public void SetLayout()
    {
        if (target_layout == null) return;

        //Adjust size of dependency content based on active headers and footers
        target_layout.SetLayout(); 
    }

    public void InitializeDisplay()
    {
        if (target_controller == null) return;

        target_controller.InitializeDisplay();
    }

    public void SetDisplay()
    {
        if (target_controller == null) return;

        target_controller.SetDisplay();

        SetActionButtons();

        previous_target_controller = target_controller;

        active = true;
    }

    public void CloseDisplay()
    {
        if (previous_target_controller == null) return;

        previous_target_controller.CloseDisplay();

        previous_target_controller = null;
    }

    public void SetActionButtons()
    {
        if (buttonActionManager != null && dataEditor != null)
            buttonActionManager.SetButtons(dataEditor.Changed());
    }

    public void FinalizeController()
    {
        if (target_controller == null) return;

        target_controller.FinalizeController();
    }

    public void ClosePath()
    {
        if (target_controller == null) return;

        if (buttonActionManager != null)
            buttonActionManager.CloseButtons();

        target_controller = null;

        active = false;
    }

    public void CloseLayout()
    {
        if (target_layout == null) return;

        target_layout.CloseLayout();

        target_layout = null;
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
