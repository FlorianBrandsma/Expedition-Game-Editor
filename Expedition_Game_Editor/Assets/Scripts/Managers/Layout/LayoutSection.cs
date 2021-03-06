﻿using UnityEngine;

public class LayoutSection : MonoBehaviour
{
    public EditorForm editorForm;

    public bool Active                                  { get; set; }
    
    public EditorController TargetController            { get; set; }
    public EditorController PreviousTargetController    { get; set; }
    public EditorController DisplayTargetController     { get; set; }

    public IElementData PreviousTargetControllerData    { get; set; }
    
    public LayoutDependency TargetView                  { get; set; }

    private Enums.ExecuteType originalExecuteType;

    public bool Loaded
    {
        get
        {
            if (!editorForm.loaded)
                return false;

            if (RenderManager.loadType == Enums.LoadType.Reload || RenderManager.loadType == Enums.LoadType.Return)
                return false;

            //Empty editor controllers are added to enforce loading (e.g. items, interactables)
            if (TargetController != PreviousTargetController)
                return false;

            if (PreviousTargetControllerData == null && TargetController.PathController.route.data != null)
                return false;

            if (PreviousTargetControllerData != null && !DataManager.Equals(TargetController.PathController.route.ElementData, PreviousTargetControllerData))
                return false;

            return true;
        }
    }

    public IEditor dataEditor;

    //Previous data editor
    public IEditor previousEditor;
    public IElementData previousDataSource;
    //
    
    public ButtonActionManager buttonActionManager;

    public void InitializeLayout()
    {
        if (TargetView == null) return;

        TargetView.InitializeDependency();       
    }

    public void SetLayout()
    {
        if (TargetView == null) return;

        //Adjust size of dependency content based on active headers and footers
        TargetView.SetDependency(); 
    }

    public void Activate()
    {
        Active = true;
    }

    public void OpenSegments()
    {
        if (TargetController == null) return;

        DisplayTargetController = TargetController;

        DisplayTargetController.OpenSegments();
    }

    public void SetActionButtons()
    {
        if (dataEditor == null) return;

        if (buttonActionManager != null && dataEditor != null)
        {
            buttonActionManager.SetButtons(dataEditor.EditData.ExecuteType, dataEditor);
        }
    }

    public void ResetEditor()
    {
        if (DisplayTargetController == null) return;

        DisplayTargetController.ResetEditor();

        SetActionButtons();
    }

    public void ClosePath()
    {
        if (TargetController == null) return;

        if (buttonActionManager != null)
            buttonActionManager.CloseButtons();

        PreviousTargetController = TargetController;
        PreviousTargetControllerData = TargetController.PathController.route.ElementData;
        
        TargetController = null;

        Active = false;
    }

    public void CloseLayoutDependencies()
    {
        if (TargetView == null) return;
        
        TargetView.CloseDependency();
        
        TargetView = null;
    }

    public void CloseEditorSegments()
    {
        if (DisplayTargetController == null) return;

        PreviousTargetController = DisplayTargetController;
        PreviousTargetControllerData = DisplayTargetController.PathController.route.ElementData;

        DisplayTargetController.CloseSegments();
    }

    public void ApplyChanges()
    {
        DataRequestManager.CreateDataRequest(dataEditor, Enums.RequestType.Validate);
    }

    private void ResetExecutionType()
    {
        dataEditor.ElementDataList.ForEach(x => x.ExecuteType = Enums.ExecuteType.Update);
    }

    public void CancelEdit()
    {
        if (dataEditor.Loaded)
            dataEditor.CancelEdit();

        if (!Active) dataEditor = null;
    }

    public void ToggleRemoval(bool toggled)
    {
        if (toggled)
        {
            originalExecuteType = dataEditor.EditData.ExecuteType;

            dataEditor.EditData.ExecuteType = Enums.ExecuteType.Remove;

        } else {

            dataEditor.EditData.ExecuteType = originalExecuteType;
        }

        SetActionButtons();
    }

    public void CloseEditor()
    {
        RenderManager.PreviousPath();
    }
}
