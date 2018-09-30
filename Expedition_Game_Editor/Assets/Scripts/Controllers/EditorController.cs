using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class EditorController : MonoBehaviour, IController
{
    public Path path    { get; set; }

    public Route route  { get; set; }

    public int step     { get; set; }

    public bool loaded  { get; set; }

    public HistoryElement   history;

    public EditorField      editorField;

    public EditorLayout     editorLayout;

    public SubControllerManager subControllerManager;
    public EditorController[]   controllers;

    public ButtonActionManager  buttonActionManager;

    public ActionManager        actionManager;

    //Necessary steps to set up the correct path for the controller
    public void InitializePath(Path new_path, int new_step, bool reload)
    {
        editorField.target_controller = this;
        editorField.sectionManager.main_controller = this;

        step = new_step;

        path = new_path.Trim(step);

        if (path.route.Count > 0)
            route = path.route.Last().Copy();

        if (step > 0)
        {
            //Don't check this if force is true. Must load!
            if(!reload)
                loaded = IsLoaded();

            //If this hasn't loaded, force load the next one
            if (!loaded || reload)
            {
                if (GetComponent<ListData>() != null)
                    GetComponent<ListData>().GetData(route);

                reload = true;
            } 
        }

        if (subControllerManager != null)
            InitializeTabs(new_path);

        if (step < new_path.route.Count)
        {
            controllers[new_path.route[step].controller].InitializePath(new_path, new_step + 1, reload);

        } else {

            FinalizePath();
        }
    }

    private void FinalizePath()
    {
        if (history.group != HistoryManager.Group.None)
            history.AddHistory(path);
    }

    public bool IsLoaded()
    {
        //If there is no previous controller then it definitely hasn't loaded yet
        if (editorField.previous_controller_path == null)
            return false;

        //If current step is longer than the previous route length, then it definitely hasn't been loaded yet
        if (step > editorField.previous_controller_path.route.Count)
            return false;

        //If false then everything afterwards must be false as well
        return path.Equals(editorField.previous_controller_path);
    }

    public void InitializeLayout()
    {
        if(editorLayout != null)
            editorLayout.InitializeLayout();
    }

    public void SetComponents(Path new_path)
    {
        if (GetComponent<MiniButtonManager>() != null)
            GetComponent<MiniButtonManager>().SetButtons();

        if (GetComponent<DisplayManager>() != null)
            GetComponent<DisplayManager>().InitializeDisplay();

        if (GetComponent<LanguageManager>() != null)
            GetComponent<LanguageManager>().SetLanguages();

        if (GetComponent<TimeManager>() != null)
            GetComponent<TimeManager>().SetTimes();

        if (GetComponent<StructureManager>() != null)
            GetComponent<StructureManager>().SetStructure();

        if (subControllerManager != null)
            subControllerManager.SetTabs(this, new_path);

        if (step < new_path.route.Count)
            controllers[new_path.route[step].controller].SetComponents(new_path);
    }

    public void SetLayout()
    {
        editorLayout.SetLayout();
    }

    public void CloseLayout()
    {
        if(editorLayout != null)
            editorLayout.CloseLayout();
    }

    public void InitializeController()
    {
        if (buttonActionManager != null)
            buttonActionManager.SetButtons(this);

        if (GetComponent<ListData>() != null)
            GetComponent<ListData>().SetRows();

        if (GetComponent<PreviewProperties>() != null)
            GetComponent<PreviewProperties>().SetPreview();
    }

    public void OpenEditor()
    {
        if (GetComponent<IEditor>() != null)
            GetComponent<IEditor>().OpenEditor(); 
    }

    public void FinalizeController()
    {
        if (route.origin.listManager != null)
            SelectionManager.SelectEdit(route);

        if (GetComponent<ListProperties>() != null)
        {
            if (GetComponent<ListProperties>().selectionType == SelectionManager.Type.Automatic)
                GetComponent<ListProperties>().AutoSelectElement();
        }  
    }

    public void FinalizeMainController()
    {
        if(route.origin.listManager != null)
        {
            if (route.origin.listManager.selected_element == null)
                route.origin.listManager.ResetListPosition();
        }
    }

    void InitializeTabs(Path new_path)
    {
        if (step == new_path.route.Count)
            new_path.Add();   
    }

    public void FilterRows(List<ElementData> list) { }

    public void SaveEdit()
    {
        GetComponent<IEditor>().SaveEdit();

        EditorManager.editorManager.PreviousEditor();
    }

    public void ApplyEdit()
    {
        GetComponent<IEditor>().ApplyEdit();
    }

    public void CloseEdit()
    {
        GetComponent<IEditor>().CancelEdit();

        EditorManager.editorManager.PreviousEditor();
    }

    public void ClosePath(Path new_path)
    {
        if (actionManager != null)
            actionManager.CloseActions();

        if (subControllerManager != null)
            subControllerManager.CloseTabs();

        if (step < new_path.route.Count)
            controllers[new_path.route[step].controller].ClosePath(new_path);
    }

    public void CloseEditor()
    {
        if (buttonActionManager != null)
            buttonActionManager.CloseButtons();
        
        if (route.origin.listManager != null)
            SelectionManager.CancelSelection(route);
            
        if (GetComponent<ListData>() != null)
            GetComponent<ListData>().CloseRows();

        if (GetComponent<PreviewProperties>() != null)
            GetComponent<PreviewProperties>().ClosePreview();

        if (GetComponent<IEditor>() != null)
            GetComponent<IEditor>().CloseEditor();

        loaded = false;
    }

    #region IController

    ElementData IController.data
    {
        get { return route.data; }
        set { }
    }

    EditorField IController.field
    {
        get { return editorField; }
        set { }
    }
    #endregion
}
