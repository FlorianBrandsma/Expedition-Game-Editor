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
    public LayoutDependency targetLayout;

    public bool Loaded
    {
        get
        {
            if (targetController.PathController.route.path.type == Path.Type.Reload)
                return false;

            if (targetController != previousTargetController)
                return false;
            
            if(previousTargetController != null)
                return targetController.PathController.route.GeneralData.Equals(previousTargetController.PathController.route.GeneralData);
            
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
        if (targetLayout == null) return;

        targetLayout.InitializeLayout();       
    }

    public void SetLayout()
    {
        if (targetLayout == null) return;

        //Adjust size of dependency content based on active headers and footers
        targetLayout.SetLayout(); 
    }

    public void ActivateEditor()
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

        targetController = null;

        active = false;
    }

    public void CloseLayout()
    {
        if (targetLayout == null) return;

        previousTargetController = displayTargetController;

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
