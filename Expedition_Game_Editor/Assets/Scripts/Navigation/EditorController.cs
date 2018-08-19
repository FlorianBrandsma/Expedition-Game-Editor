using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class EditorController : MonoBehaviour, IController
{
    public bool active      { get; set; }

    public ElementData      data;

    public EditorField      editorField;

    public HistoryManager   historyManager;

    public EditorLayout     editorLayout;

    public TabManager       tabManager;
    public EditorController[] controllers;

    public ActionManager    actionManager;

    public int depth        { get; set; }

    //Necessary steps to set up the correct path for the controller
    public void InitializeController(Path path, int new_depth)
    {
        editorField.target_controller = this;
 
        depth = new_depth;

        if (depth > 0 && path.data.Count > 0)
            data = path.data[depth - 1];

        data.path = path.Trim(depth);

        if (tabManager != null)
            InitializeTabs(path);

        if (GetComponent<ListData>() != null)
            GetComponent<ListData>().InitializeRows();

        if (depth < path.structure.Count)
            controllers[path.structure[depth]].InitializeController(path, depth + 1);
    }

    //Create separate function for obtaining path and initializing rows
    //Only initialize rows if the controller is inactive
    public void InitializeController(Path path)
    {
        if (depth < path.structure.Count)
            controllers[path.structure[depth]].InitializeController(path);
    }

    public void InitializeLayout()
    {
        if(editorLayout != null)
            editorLayout.InitializeLayout();
    }

    public void SetPath(Path path)
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
            tabManager.SetEditorTabs(this, path);

        if (depth < path.structure.Count)
            controllers[path.structure[depth]].SetPath(path);
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

    public void SetEditor()
    {
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

        if (historyManager != null)
            historyManager.AddHistory(data.path);
    }

    void InitializeTabs(Path path)
    {
        if (depth == path.structure.Count)
        {
            path.structure.Add(0);
            path.data.Add(new ElementData());
        }
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

    public void ClosePath(Path path)
    {
        if (actionManager != null)
            actionManager.CloseActions();

        if (tabManager != null)
            tabManager.CloseTabs();

        if (depth < path.structure.Count)
            controllers[path.structure[depth]].ClosePath(path);
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
