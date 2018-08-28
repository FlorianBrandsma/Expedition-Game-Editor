using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class EditorController : MonoBehaviour, IController
{
    public Path path        { get; set; }

    public bool active      { get; set; }

    public ElementData      data;

    public EditorField      editorField;

    public HistoryManager   historyManager;

    public EditorLayout     editorLayout;

    public TabManager       tabManager;
    public EditorController[] controllers;

    public ActionManager    actionManager;

    public int step        { get; set; }

    //Necessary steps to set up the correct path for the controller
    public void InitializePath(Path new_path, int new_step)
    {
        editorField.target_controller = this;
 
        step = new_step;

        if (step > 0 && new_path.data.Count > 0)
            data = new_path.data[step - 1];

        path = new_path.Trim(step);

        if (tabManager != null)
            InitializeTabs(new_path);

        if (step < new_path.route.Count)
            controllers[new_path.route[step]].InitializePath(new_path, step + 1);       
    }

    //Create separate function for obtaining path and initializing rows
    //Only initialize rows if the controller is inactive
    public void GetData(Path new_path)
    {
        if (GetComponent<ListData>() != null)
            GetComponent<ListData>().GetData();

        if (step < new_path.route.Count)
            controllers[new_path.route[step]].GetData(new_path);
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

        if (tabManager != null)
            tabManager.SetEditorTabs(this, new_path);

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
        if (GetComponent<ListData>() != null)
            GetComponent<ListData>().SetRows();

        if (GetComponent<EditManager>() != null)
            GetComponent<EditManager>().SetEdit();

        if (GetComponent<PreviewProperties>() != null)
            GetComponent<PreviewProperties>().SetPreview();
    }

    public void OpenController()
    {
        if (GetComponent<IEditor>() != null)
            GetComponent<IEditor>().OpenEditor();

        if (historyManager != null)
            historyManager.AddHistory(path);
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

    public void CancelEdit()
    {
        GetComponent<IEditor>().CancelEdit();

        EditorManager.editorManager.PreviousEditor();
    }

    public void ClosePath(Path new_path)
    {
        if (actionManager != null)
            actionManager.CloseActions();

        if (tabManager != null)
            tabManager.CloseTabs();

        if (step < new_path.route.Count)
            controllers[new_path.route[step]].ClosePath(new_path);
    }

    public void CloseEditor()
    {  
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
