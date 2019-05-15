﻿using UnityEngine;
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
    public EditorController displayTargetController { get; set; }
    public EditorController targetController    { get; set; }
    public LayoutDependency targetLayout        { get; set; }
    public Path previousControllerPath          { get; set; }
    
    public IEditor dataEditor;

    public ButtonActionManager buttonActionManager;

    public void InitializeSection(EditorForm new_form)
    {
        //Remove this later
        editorForm = new_form;
    }

    public void InitializeController()
    {
        if (targetController == null) return;

        if (buttonActionManager != null)
            buttonActionManager.InitializeButtons(this);

        targetController.InitializeController();

        dataEditor = targetController.pathController.dataEditor;
    }

    public void SetPreviousPath()
    {
        if (targetController != null)
            previousControllerPath = targetController.pathController.route.path;
        else
            previousControllerPath = null;
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
        if (dataEditor != null)

        if (buttonActionManager != null && dataEditor != null)
            buttonActionManager.SetButtons(dataEditor.Changed());
    }

    public void ClosePath()
    {
        if (targetController == null) return;

        if (buttonActionManager != null)
            buttonActionManager.CloseButtons();

        targetController = null;

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
