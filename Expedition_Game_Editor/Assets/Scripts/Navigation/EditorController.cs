﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class EditorController : MonoBehaviour, IController
{
    public EditorLayout editorLayout;

    public EditorField editorField;

    public Path controller_path;

    public string table;
    public int id;

    //Header
    public TabManager tabManager;
    //Footer
    public ButtonActionManager buttonActionManager;

    private List<Button> tabs = new List<Button>();

    public GameObject[] editor;
 
    public ActionManager actionManager;

    public void InitializePath(Path new_path, int editor_depth)
    {
        editorField.target_editor = this;

        if (editor_depth > 0)
        {
            if (new_path.id.Count > 0)
                id = new_path.id[editor_depth - 1];
        }

        if (GetComponent<RowManager>() != null)
            GetComponent<RowManager>().InitializeRows();

        if (tabManager != null)
            InitializeTabs(new_path, editor_depth);

        if (editor_depth < new_path.editor.Count)
            editor[new_path.editor[editor_depth]].GetComponent<EditorController>().InitializePath(new_path, editor_depth + 1);

        controller_path = TrimPath(new_path, editor_depth);
    }

    public void InitializeLayout()
    {
        editorLayout.InitializeLayout();
    }

    public void SetPath(Path new_path, int editor_depth)
    {
        if (GetComponent<MiniButtonManager>() != null)
            GetComponent<MiniButtonManager>().SetButtons();

        if (GetComponent<LanguageManager>() != null)
            GetComponent<LanguageManager>().SetLanguages();

        if (GetComponent<TimeManager>() != null)
            GetComponent<TimeManager>().SetTimes();

        if (GetComponent<StructureManager>() != null)
            GetComponent<StructureManager>().SetStructure(this, new_path, editor_depth, table, id);

        if (buttonActionManager != null)
            buttonActionManager.SetButtons(this);

        if (tabManager != null)
            SetTabs(new_path, editor_depth);

        if (editor_depth < new_path.editor.Count)
            editor[new_path.editor[editor_depth]].GetComponent<EditorController>().SetPath(new_path, editor_depth + 1);
    }

    public void SetLayout()
    {
        editorLayout.SetLayout();
    }

    public void CloseLayout()
    {
        editorLayout.CloseLayout();
    }

    public void SetEditor()
    {
        if (GetComponent<EditManager>() != null)
            GetComponent<EditManager>().SetEdit();
    }

    public void OpenEditor()
    {
        if (GetComponent<ListProperties>() != null)
            GetComponent<ListProperties>().SetList();

        if (GetComponent<PreviewProperties>() != null)
            GetComponent<PreviewProperties>().SetPreview();

        if (GetComponent<IEditor>() != null)
            GetComponent<IEditor>().OpenEditor();
    }

    void InitializeTabs(Path new_path, int editor_depth)
    {
        if (editor_depth == new_path.editor.Count)
        {
            new_path.editor.Add(0);
            new_path.id.Add(0);
        }
    }

    void SetTabs(Path new_path, int editor_depth)
    {
        tabManager.SetEditorTabs(this, new_path, editor, editor_depth);
    }

    public void FilterRows(List<int> list)
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

    public void ClosePath(Path new_path, int editor_depth)
    {
        if (actionManager != null)
            actionManager.CloseActions();

        if (tabManager != null)
            tabManager.CloseTabs();

        if (buttonActionManager != null)
            buttonActionManager.CloseButtons();

        if (GetComponent<RowManager>() != null)
            GetComponent<RowManager>().CloseRows();

        if (GetComponent<PreviewProperties>() != null)
            GetComponent<PreviewProperties>().ClosePreview();

        if (editor_depth < new_path.editor.Count)
            editor[new_path.editor[editor_depth]].GetComponent<EditorController>().ClosePath(new_path, editor_depth + 1);
    }

    public void CloseEditor()
    {
        if (GetComponent<IEditor>() != null)
            GetComponent<IEditor>().CloseEditor();
    }

    Path TrimPath(Path path, int editor_depth)
    {
        Path new_path = new Path(new List<int>(), new List<int>());

        for (int i = 0; i < editor_depth; i++)
        {
            new_path.editor.Add(path.editor[i]);
            new_path.id.Add(path.id[i]);
        }

        return new_path;
    }

    public Path CopyPath(Path path, int depth)
    {
        Path new_path = new Path(new List<int>(), new List<int>());

        for (int i = 0; i < depth; i++)
            new_path.editor.Add(path.editor[i]);

        for (int i = 0; i < depth; i++)
            new_path.id.Add(path.id[i]);

        return new_path;
    }

    #region IController

    public EditorField GetField()
    {
        return editorField;
    }

    public Path GetPath()
    {
        return controller_path;
    }

    public string GetTable()
    {
        return table;
    }

    public int GetID()
    {
        return id;
    }

    #endregion
}
