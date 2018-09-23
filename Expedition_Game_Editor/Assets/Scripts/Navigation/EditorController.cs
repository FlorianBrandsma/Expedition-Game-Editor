using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class EditorController : MonoBehaviour, IController
{
    public Path path        { get; set; }

    public bool loaded      { get; set; }

    public ElementData      data;

    public EditorField      editorField;

    public EditorLayout     editorLayout;

    public SubControllerManager subControllerManager;
    public EditorController[] controllers;

    public ButtonActionManager buttonActionManager;

    public ActionManager    actionManager;

    //Determine which element is selected based on controller data
    //Also check property, so that if "edit" is selected, you can still "open"
    //List through which the controller is opened

    public Selection        origin;

    public int step        { get; set; }

    //Necessary steps to set up the correct path for the controller
    public void InitializePath(Path new_path, int new_step, bool force_load)
    {
        editorField.target_controller = this;
 
        step = new_step;
        
        path = new_path.Trim(step);

        if (path.data.Count > 0)
            data = path.data[path.data.Count - 1];

        if (step > 0)
        {
            //Don't check this if force is true. Must load!
            if(!force_load)
                loaded = IsLoaded();

            //If this hasn't loaded, force load the next one
            if (!loaded || force_load)
            {
                if (GetComponent<ListData>() != null)
                    GetComponent<ListData>().GetData();

                force_load = true;
            } 
        }

        if (subControllerManager != null)
            InitializeTabs(new_path);

        if (step < new_path.route.Count)
        {
            controllers[new_path.route[step]].InitializePath(new_path, new_step + 1, force_load);

        } else {

            FinalizePath();
        }
    }

    private void FinalizePath()
    {
        origin = path.origin;

        if (path.origin != null)
        {
            //Debug.Log(origin.data.table);

            //path.origin.data.table = "Test1";

            //Debug.Log(path.origin.data.table);
            //Debug.Log(origin.data.table);
        }
            

        if (GetComponent<HistoryElement>() != null)
            GetComponent<HistoryElement>().AddHistory();
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
        return path.Equals(editorField.previous_controller_path, step - 1);
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
            controllers[new_path.route[step]].SetComponents(new_path);
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
        //Works only because it's called before rows (temporary)
        if (origin != null)
            SelectionManager.SelectEdit(origin);
        
        if (GetComponent<ListData>() != null)
            GetComponent<ListData>().SetRows();

        if (GetComponent<EditManager>() != null)
            GetComponent<EditManager>().SetEdit();

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
        //PROBLEM:  When assigning origin to path, ALL PATH ORIGINS GET OVERWRITTEN
        //WHY:      Origin gets passed from path to path. Probably always the same instance?
        //          Or a REALLY dumb copying mistake?

        //Must work here
        /*
        if (origin != null)
            SelectionManager.SelectEdit(origin);
        */
        if (GetComponent<ListProperties>() != null)
        {
            if (GetComponent<ListProperties>().selectionType == SelectionManager.Type.Automatic)
                GetComponent<ListProperties>().AutoSelectElement();
        }  
    }

    void InitializeTabs(Path new_path)
    {
        if (step == new_path.route.Count)
            new_path.Add();   
    }

    public void FilterRows(List<ElementData> list)
    {

    }

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
        if (origin != null)
            SelectionManager.CancelSelection(origin);

        if (actionManager != null)
            actionManager.CloseActions();

        if (subControllerManager != null)
            subControllerManager.CloseTabs();

        if (step < new_path.route.Count)
            controllers[new_path.route[step]].ClosePath(new_path);
    }

    public void CloseEditor()
    {
        if (buttonActionManager != null)
            buttonActionManager.CloseButtons();

        if (GetComponent<ListData>() != null)
            GetComponent<ListData>().CloseRows();

        if (GetComponent<PreviewProperties>() != null)
            GetComponent<PreviewProperties>().ClosePreview();

        if (GetComponent<IEditor>() != null)
            GetComponent<IEditor>().CloseEditor();
    }

    #region IController

    ElementData IController.data
    {
        get { return data; }
        set { }
    }

    EditorField IController.field
    {
        get { return editorField; }
        set { }
    }
    #endregion
}
