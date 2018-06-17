using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class TabManager : MonoBehaviour
{
    public List<Button> tab_list = new List<Button>();

    public bool source;

    private GameObject[] editor;

    private int active_tab = -1;

    public void SetEditorTabs(Path path, GameObject[] new_editor, int editor_depth)
    {
        editor = new_editor;

        for (int tab = 0; tab < editor.Length; tab++)
        {
            Button new_tab = SpawnTab();

            //FIX TAB PNG; THEN REMOVE THIS
            SetAnchors(tab, editor.Length);

            new_tab.GetComponentInChildren<Text>().text = editor[tab].name;

            int temp_int = tab;

            if (source)
            {
                new_tab.onClick.AddListener(delegate
                {
                    OpenSource(path, temp_int, editor_depth);
                });
            }
            else
            {
                new_tab.onClick.AddListener(delegate
                {
                    OpenEditor(path, temp_int, editor_depth);
                });
            }

            new_tab.gameObject.SetActive(true);
        }

        SelectTab(path.editor[editor_depth]);
    }

    public void SetFieldTabs(GameObject[] new_editor)
    {
        editor = new_editor;

        for (int tab = 0; tab < editor.Length; tab++)
        {
            Button new_tab = SpawnTab();

            //FIX TAB PNG; THEN REMOVE THIS
            SetAnchors(tab, editor.Length);

            new_tab.GetComponentInChildren<Text>().text = editor[tab].name;

            int temp_int = tab;

            new_tab.onClick.AddListener(delegate { OpenFieldEditor(temp_int); });

            new_tab.gameObject.SetActive(true);
        }

        OpenFieldEditor(0);
    }

    public void OpenFieldEditor(int selected_tab)
    {
        CloseFieldEditor();

        SelectTab(selected_tab);

        editor[selected_tab].GetComponent<EditorField>().OpenField();

        active_tab = selected_tab;
    }

    void CloseFieldEditor()
    {
        if(active_tab >= 0)
            editor[active_tab].GetComponent<EditorField>().CloseField();      
    }

    public void SetAnchors(int index, int tabs)
    {
        RectTransform new_tab = tab_list[index].GetComponent<RectTransform>();

        new_tab.anchorMin = new Vector2(index * (1f / tabs), 1);
        new_tab.anchorMax = new Vector2((index + 1) * (1f / tabs), 1);

        new_tab.offsetMin = new Vector2(-1, new_tab.offsetMin.y);
        new_tab.offsetMax = new Vector2(1, new_tab.offsetMax.y);    
    }

    void OpenEditor(Path path, int selected_tab, int editor_depth)
    {
        NavigationManager.navigation_manager.OpenStructure(NewEditor(path, selected_tab, editor_depth), true, false);
    }

    void OpenSource(Path path, int selected_tab, int editor_depth)
    {
        NavigationManager.navigation_manager.OpenSource(NewEditor(path, selected_tab, editor_depth));
    }

    void SelectTab(int selected_tab)
    {
        for (int tab = 0; tab < tab_list.Count; tab++) 
        {
            if (tab == selected_tab)
                tab_list[tab].GetComponent<Image>().sprite = Resources.Load<Sprite>("Textures/Buttons/Tab_A");
            else
                tab_list[tab].GetComponent<Image>().sprite = Resources.Load<Sprite>("Textures/Buttons/Tab_O");
        }
    }

    Path NewEditor(Path path, int index, int editor_depth)
    {
        //Copy the old editor. Any changes made to "path" are visible throughout the entire code
        Path new_path = new Path(new List<int>(), new List<int>());

        for (int i = 0; i < path.editor.Count; i++)
        {
            new_path.editor.Add(path.editor[i]);
            new_path.id.Add(path.id[i]);
        }

        //Set the editor option based on the tab index
        new_path.editor[editor_depth] = index;

        //In case there are more options than the current editor depth
        //Make it 0 so it always opens the first tab
        if (new_path.editor.Count > (editor_depth + 1))
            new_path.editor[new_path.editor.Count - 1] = 0;

        return new_path;
    }

    public Button SpawnTab()
    {
        for (int i = 0; i < tab_list.Count; i++)
        {
            if (!tab_list[i].gameObject.activeInHierarchy)
                return tab_list[i];
        }

        Button new_tab = Instantiate(Resources.Load<Button>("Editor/Tab"));
        new_tab.transform.SetParent(transform, false);
        tab_list.Add(new_tab);

        return new_tab;
    }

    public void CloseTabs()
    {
        //In case of problems: use tab_list[i].Count
        //and put an if statement before closing editor
        for (int i = 0; i < editor.Length; i++)
        {
            tab_list[i].onClick.RemoveAllListeners();
            tab_list[i].gameObject.SetActive(false);
            editor[i].gameObject.SetActive(false);
        }

        active_tab = -1;
    }
}
