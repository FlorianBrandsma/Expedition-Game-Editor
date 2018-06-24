using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class SubEditor : MonoBehaviour, IEditorData
{
    public Path path;

    public string table;
    public int id;

    //Activate parent objects that aren't editors
    public GameObject[] editor_parent;

    public ButtonActionManager buttonActionManager;

    public TabManager tabManager;

    private List<Button> tabs = new List<Button>();

    public GameObject[] editor;
    
    public RectTransform editor_rect;

    public Vector2 min_anchor;
    public Vector2 max_anchor;

    public RectTransform preview_window;

    public ActionManager actionManager;

    public void OpenEditor(Path new_path, int editor_depth)
    {
        path = new_path;

        if (editor_depth > 0)
        {
            if (path.id.Count > 0)
                id = path.id[editor_depth - 1]; 
        }

        if (GetComponent<RowManager>() != null)
            GetComponent<RowManager>().GetRows();

        if (GetComponent<ButtonManager>() != null)
            GetComponent<ButtonManager>().SortButtons();

        if (GetComponent<LanguageManager>() != null)
            GetComponent<LanguageManager>().SortLanguages();

        if (GetComponent<TimeManager>() != null)
            GetComponent<TimeManager>().SortTimes();
           
        if (GetComponent<StructureManager>() != null)
            GetComponent<StructureManager>().SortStructure(path, editor_depth, table, id);

        if (editor_rect != null)
            editor_rect.gameObject.SetActive(true);

        if (preview_window != null)
            preview_window.gameObject.SetActive(true);

        if (buttonActionManager != null)
            buttonActionManager.SetButtons(this);

        if (tabManager != null)
            SetTabs(path, editor_depth);
        else if (editor_depth == path.editor.Count)
            OpenChildEditor();

        //Open next editor in line
        if (editor_depth < path.editor.Count)
            editor[path.editor[editor_depth]].GetComponent<SubEditor>().OpenEditor(path, editor_depth + 1);    
    }

    public void FilterRows(List<int> list)
    {
        
    }

    void SetTabs(Path path, int editor_depth)
    {
        if (editor_depth == path.editor.Count)
        {
            path.editor.Add(0);
            path.id.Add(0);
        }

        tabManager.SetEditorTabs(path, editor, editor_depth);

        OpenChildEditor();
    }

    void OpenChildEditor()
    {
        for (int i = 0; i < editor_parent.Length; i++)
            editor_parent[i].SetActive(true);

        if (GetComponent<ListProperties>() != null)
            GetComponent<ListProperties>().SetList();

        if (editor_rect != null)
            SetAnchors();

        GetComponent<IEditor>().OpenEditor();   
    }

    void SetAnchors()
    {
        editor_rect.anchorMin = min_anchor;
        editor_rect.anchorMax = max_anchor;
    }

    public void SaveEdit()
    {
        GetComponent<IEditor>().SaveEdit();
    }

    public void ApplyEdit()
    {
        GetComponent<IEditor>().ApplyEdit();
    }

    public void CancelEdit()
    {
        GetComponent<IEditor>().CancelEdit();
    }

    public void CloseEditor(List<int> path, int editor_index)
    {
        NavigationManager.get_id = false;

        editor_index++;

        if (GetComponent<RowManager>() != null)
            GetComponent<RowManager>().CloseList();

        if (actionManager != null)
            actionManager.CloseOptions();

        if (editor_rect != null)
            editor_rect.gameObject.SetActive(false);

        if (tabManager != null)
            tabManager.CloseTabs();

        if (buttonActionManager != null)
            buttonActionManager.CloseButtons();

        if (preview_window != null)
            preview_window.gameObject.SetActive(false);

        for (int i = 0; i < editor_parent.Length; i++)
            editor_parent[i].SetActive(false);

        gameObject.GetComponent<IEditor>().CloseEditor();

        if (editor_index <= path.Count)
            editor[path[editor_index - 1]].GetComponent<SubEditor>().CloseEditor(path, editor_index);
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

    #region ISubEditor

    public Path GetPath()
    {
        return path;
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
