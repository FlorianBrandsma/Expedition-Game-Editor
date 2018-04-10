using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class SubEditor : MonoBehaviour
{
    public string table;
    public int id;

    public GameObject[] editor_parent;

    public GameObject tab_manager;
    private List<Button> tabs = new List<Button>();

    public GameObject[] editor;
    
    public GameObject[] editor_options;
    public bool lock_position;

    private float option_width;

    public RectTransform preview_window;

    public void OpenEditor(Path path, int editor_depth)
    {
        id = path.id[editor_depth];

        editor_depth++;

        gameObject.GetComponent<IEditor>().OpenEditor();

        for (int i = 0; i < editor_parent.Length; i++)
            editor_parent[i].SetActive(true);

        if (preview_window != null)
            preview_window.gameObject.SetActive(true);

        for (int i = 0; i < editor_options.Length; i++)
            SetOptions(i);

        //SubEditor > ListOptions > RowManager > ListManager > Organizer
        if (GetComponent<ListOptions>() != null && editor_depth == path.editor.Count) //Kan dit weg?
            GetComponent<ListOptions>().SetList();

        if (tab_manager != null)
        {
            SetTabs(path, editor_depth);       
        } else {
            if (editor_depth < path.editor.Count)
            {
                editor[path.editor[editor_depth]].GetComponent<SubEditor>().OpenEditor(path, editor_depth);
            }
        }
    }

    void SelectEditor(Path path, int editor_depth)
    {
        editor[path.editor[editor_depth]].GetComponent<SubEditor>().OpenEditor(path, editor_depth);
    }

    void SetOptions(int option_index)
    {
        if (!lock_position)
        {
            option_width = editor_options[option_index].GetComponent<RectTransform>().rect.width -
                            editor_options[option_index].GetComponent<RectTransform>().sizeDelta.x;

            editor_options[option_index].transform.localPosition = new Vector2((option_width * (option_index - 2)), 0);
        }

        editor_options[option_index].SetActive(true);
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
            editor[path.editor[editor_depth]].GetComponent<SubEditor>().OpenEditor(path, editor_depth);    
    }

    void SelectTab(int selected_tab, int index)
    {
        if (selected_tab == index)
            tab_manager.GetComponent<TabManager>().tab_list[index].GetComponent<Image>().sprite = Resources.Load<Sprite>("Textures/Buttons/Tab_A");
        else 
            tab_manager.GetComponent<TabManager>().tab_list[index].GetComponent<Image>().sprite = Resources.Load<Sprite>("Textures/Buttons/Tab_O");
    }
    //update history (?)

    public void CloseEditor(List<int> path, int editor_index)
    {
        NavigationManager.get_id = false;

        editor_index++;

        for (int i = 0; i < editor_options.Length; i++)
            CloseOptions(i);

        if (tab_manager != null)
            tab_manager.GetComponent<TabManager>().ResetTabs();

        if (editor_index < path.Count)
            editor[path[editor_index]].GetComponent<SubEditor>().CloseEditor(path, editor_index);

        if (preview_window != null)
            preview_window.gameObject.SetActive(false);

        for (int i = 0; i < editor_parent.Length; i++)
            editor_parent[i].SetActive(false);

        gameObject.GetComponent<IEditor>().CloseEditor();
    }

    void CloseOptions(int option_index)
    {
        editor_options[option_index].SetActive(false);
    }
}
