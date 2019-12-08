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
    public EditorController previousTargetController;

    [HideInInspector]
    public GeneralData previousTargetControllerData;

    [HideInInspector]
    public LayoutDependency targetView;

    public bool Loaded
    {
        get
        {
            if (!editorForm.loaded)
                return false;
            
            if (EditorManager.loadType == Enums.LoadType.Reload || EditorManager.loadType == Enums.LoadType.Return)
                return false;

            if (targetController != previousTargetController)
                return false;
            
            if(previousTargetControllerData != null)
                return targetController.PathController.route.GeneralData.Equals(previousTargetControllerData);
            
            return false;
        }
    }

    public IEditor dataEditor;

    //Previous data editor
    public IDataElement previousDataSource;
    public List<IDataElement> previousDataElements;
    //
    
    public ButtonActionManager buttonActionManager;

    public void InitializeSection(EditorForm editorForm)
    {
        this.editorForm = editorForm;

        if (buttonActionManager != null)
            buttonActionManager.InitializeButtons(this);
    }

    public void InitializeLayout()
    {
        if (targetView == null) return;

        targetView.InitializeDependency();       
    }

    public void SetLayout()
    {
        if (targetView == null) return;

        //Adjust size of dependency content based on active headers and footers
        targetView.SetDependency(); 
    }

    public void Activate()
    {
        active = true;
    }

    public void OpenEditor()
    {
        if (targetController == null) return;

        displayTargetController = targetController;

        displayTargetController.OpenSegments();

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
        if (targetController == null) return;

        if (buttonActionManager != null)
            buttonActionManager.CloseButtons();

        previousTargetController = targetController;
        previousTargetControllerData = (GeneralData)targetController.PathController.route.data.dataElement;//.Copy();
        
        targetController = null;

        active = false;
    }

    public void CloseLayoutDependencies()
    {
        if (targetView == null) return;
        
        targetView.CloseDependency();
        
        targetView = null;
    }

    public void CloseEditorSegments()
    {
        if (displayTargetController == null) return;

        previousTargetController = displayTargetController;
        previousTargetControllerData = (GeneralData)displayTargetController.PathController.route.data.dataElement;//.Copy();

        displayTargetController.CloseSegments();
    }

    public void ApplyChanges()
    {
        if(dataEditor != null)
            dataEditor.ApplyChanges();
    }

    public void CancelEdit()
    {
        if(dataEditor.Loaded)
            dataEditor.CancelEdit();
        
        if (previousDataElements != null)
        {
            previousDataElements.ForEach(x => x.ClearChanges());
            previousDataElements.Where(x => x.SelectionElement != null && x.SelectionElement.gameObject.activeInHierarchy).ToList()
                                .ForEach(x => x.SelectionElement.UpdateElement());
        }
        
        if (!active) dataEditor = null;
    }

    public void CloseEditor()
    {
        EditorManager.editorManager.PreviousEditor();
    }
}
