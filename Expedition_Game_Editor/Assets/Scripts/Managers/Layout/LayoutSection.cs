using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class LayoutSection : MonoBehaviour
{
    public bool Active                                  { get; set; }
    
    public EditorForm EditorForm                        { get; set; }

    public EditorController TargetController            { get; set; }
    public EditorController PreviousTargetController    { get; set; }
    public EditorController DisplayTargetController     { get; set; }

    public IElementData PreviousTargetControllerData    { get; set; }
    
    public LayoutDependency TargetView                  { get; set; }

    public bool Loaded
    {
        get
        {
            if (!EditorForm.loaded)
                return false;

            if (RenderManager.loadType == Enums.LoadType.Reload)
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
    public List<IElementData> previousElementDataList;
    //
    
    public ButtonActionManager buttonActionManager;

    public void InitializeSection(EditorForm editorForm)
    {
        this.EditorForm = editorForm;

        if (buttonActionManager != null)
            buttonActionManager.InitializeButtons(this);
    }

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

    public void OpenEditor()
    {
        if (TargetController == null) return;

        DisplayTargetController = TargetController;

        DisplayTargetController.OpenSegments();

        SetActionButtons();
    }

    public void SetActionButtons()
    {
        if (dataEditor == null) return;

        if (buttonActionManager != null && dataEditor != null)
            buttonActionManager.SetButtons(dataEditor.Changed());
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
        if(dataEditor != null)
            dataEditor.ApplyChanges();
    }

    public void CancelEdit()
    {
        if (dataEditor.Loaded)
            dataEditor.CancelEdit();

        if (previousElementDataList != null)
        {
            previousElementDataList.ForEach(x => x.ClearChanges());

            previousElementDataList.Where(x => x.DataElement != null && x.DataElement.gameObject.activeInHierarchy).ToList()
                                .ForEach(x => x.DataElement.UpdateElement());
        }

        if (!Active) dataEditor = null;
    }

    public void CloseEditor()
    {
        RenderManager.PreviousPath();
    }
}
