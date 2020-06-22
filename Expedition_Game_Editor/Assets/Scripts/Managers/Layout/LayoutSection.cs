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

    public GeneralData PreviousTargetControllerData     { get; set; }
    
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

            if (PreviousTargetControllerData != null)
                return TargetController.PathController.route.GeneralData.Equals(PreviousTargetControllerData);

            return false;
        }
    }

    public IEditor dataEditor;

    //Previous data editor
    public IEditor previousEditor;
    public IDataElement previousDataSource;
    public List<IDataElement> previousDataElements;
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
        PreviousTargetControllerData = (GeneralData)TargetController.PathController.route.data.dataElement;
        
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
        PreviousTargetControllerData = (GeneralData)DisplayTargetController.PathController.route.data.dataElement;

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
        
        if (previousDataElements != null)
        {
            previousDataElements.ForEach(x => x.ClearChanges());

            previousDataElements.Where(x => x.DataElement != null && x.DataElement.gameObject.activeInHierarchy).ToList()
                                .ForEach(x => x.DataElement.UpdateElement());
        }
        
        if (!Active) dataEditor = null;
    }

    public void CloseEditor()
    {
        RenderManager.PreviousPath();
    }
}
