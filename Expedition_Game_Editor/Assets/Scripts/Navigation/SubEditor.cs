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

    public Text header;

    public string table;
    public int id;

    //Activate parent objects that aren't editors
    public GameObject[] editor_parent;

    public GameObject tab_manager;
    private List<Button> tabs = new List<Button>();

    public GameObject[] editor;
    
    public bool lock_position;

    private float option_width;

    public RectTransform preview_window;

    public void OpenEditor(Path new_path, int editor_depth)
    {
        path = new_path;

        if (editor_depth > 0)
        {
            if (path.id.Count > 0)
                id = path.id[editor_depth - 1]; 
        }

        //Set rows before opening editors
        //Or not
        if (GetComponent<RowManager>() != null)
            GetComponent<RowManager>().SetRows();

        if (header != null)
            SetHeader(table + " " + id);

        if (GetComponent<LanguageManager>() != null)
            GetComponent<LanguageManager>().SortLanguages();
           
        if (GetComponent<StructureManager>() != null)
            GetComponent<StructureManager>().SortStructure(path, editor_depth, table, id);

        if (force_activation)
            gameObject.GetComponent<IEditor>().OpenEditor();

        if (editor_depth == path.editor.Count)
        {
            gameObject.GetComponent<IEditor>().OpenEditor();

            //SubEditor > ListOptions > RowManager > ListManager > Organizer
            if (GetComponent<ListProperties>() != null)
                GetComponent<ListProperties>().SetList();
        }

        for (int i = 0; i < editor_parent.Length; i++)
            editor_parent[i].SetActive(true);

        if (preview_window != null)
            preview_window.gameObject.SetActive(true);

        if (tab_manager != null)
            SetTabs(path, editor_depth);

        if (tab_manager == null && editor_depth < path.editor.Count)
            editor[path.editor[editor_depth]].GetComponent<SubEditor>().OpenEditor(path, editor_depth + 1);
    }

    public string PathString(Path path)
    {
        string path_string = "editor: ";

        for (int i = 0; i < path.editor.Count; i++)
        {
            path_string += path.editor[i] + ",";
        }

        path_string += "id: ";

        for (int i = 0; i < path.id.Count; i++)
        {
            path_string += path.id[i] + ",";
        }


        return path_string;
    }

    void SetTabs(Path path, int editor_depth)
    {
        if (editor_depth == path.editor.Count)
        {
            path.editor.Add(0);
            path.id.Add(0);
        }

        for (int i = 0; i < editor.Length; i++)
        {
            tab_manager.GetComponent<TabManager>().SetTab(path, editor, i, editor_depth);

            SelectTab(path.editor[editor_depth], i);
        }

        //Automatically open next editor if the depth is lower than the count
        if (editor_depth < path.editor.Count)
            editor[path.editor[editor_depth]].GetComponent<SubEditor>().OpenEditor(path, editor_depth + 1);  
    }

    void SelectTab(int selected_tab, int index)
    {
        if (selected_tab == index)
            tab_manager.GetComponent<TabManager>().tab_list[index].GetComponent<Image>().sprite = Resources.Load<Sprite>("Textures/Buttons/Tab_A");
        else 
            tab_manager.GetComponent<TabManager>().tab_list[index].GetComponent<Image>().sprite = Resources.Load<Sprite>("Textures/Buttons/Tab_O");
    }
    //update history (?)

    void SetHeader(string header_text)
    {
        header.text = header_text;
        header.gameObject.SetActive(true);
    }

    public void CloseEditor(List<int> path, int editor_index)
    {
        NavigationManager.get_id = false;

        editor_index++;

        if (header != null)
            header.gameObject.SetActive(false);

        if (tab_manager != null)
            tab_manager.GetComponent<TabManager>().CloseTabs();

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
