using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class SubEditor : MonoBehaviour
{
    public Path path;

    //Always activate specific editors across a path
    public bool force_activation;

    public string table;
    public int id;

    //Activate parent objects that aren't editors
    public GameObject[] editor_parent;

    public GameObject button_manager;

    public TabManager tabManager;

    private List<Button> tabs = new List<Button>();

    public GameObject[] editor;
    
    public bool lock_position;

    public RectTransform editor_rect;

    public Vector2 min_anchor;
    public Vector2 max_anchor;

    public RectTransform preview_window;

    public OptionManager optionManager;

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

        if (GetComponent<LanguageManager>() != null)
            GetComponent<LanguageManager>().SortLanguages();
           
        if (GetComponent<StructureManager>() != null)
            GetComponent<StructureManager>().SortStructure(path, editor_depth, table, id);

        if (editor_rect != null)
            editor_rect.gameObject.SetActive(true);

        if (preview_window != null)
            preview_window.gameObject.SetActive(true);

        if (button_manager != null)
            button_manager.GetComponent<ButtonActionManager>().SetButtons(this);

        if (tabManager != null)
            SetTabs(path, editor_depth);

        if (editor_depth == path.editor.Count)
            OpenChildEditor();

        //Open next editor in line
        if (editor_depth < path.editor.Count)
            editor[path.editor[editor_depth]].GetComponent<SubEditor>().OpenEditor(path, editor_depth + 1);    
    }

    void OpenChildEditor()
    {
        for (int i = 0; i < editor_parent.Length; i++)
            editor_parent[i].SetActive(true);

        GetComponent<IEditor>().OpenEditor();

        if (editor_rect != null)
            SetAnchors();

        if (GetComponent<ListProperties>() != null)
            GetComponent<ListProperties>().SetList();
    }

    void SetTabs(Path path, int editor_depth)
    {
        if (editor_depth == path.editor.Count)
        {
            path.editor.Add(0);
            path.id.Add(0);
        }

        tabManager.SetEditorTabs(path, editor, editor_depth);
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

        if (optionManager != null)
            optionManager.CloseOptions();

        if (editor_rect != null)
            editor_rect.gameObject.SetActive(false);

        if (tabManager != null)
            tabManager.GetComponent<TabManager>().CloseTabs();

        if (button_manager != null)
            button_manager.GetComponent<ButtonActionManager>().CloseButtons();

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
}
