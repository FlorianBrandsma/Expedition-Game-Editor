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

    public void SetAnchors(int index, int tabs)
    {
        RectTransform new_tab = tab_list[index].GetComponent<RectTransform>();

        new_tab.anchorMin = new Vector2(index * (1f / tabs), 1);
        new_tab.anchorMax = new Vector2((index + 1) * (1f / tabs), 1);

        new_tab.offsetMin = new Vector2(-1, new_tab.offsetMin.y);
        new_tab.offsetMax = new Vector2(1, new_tab.offsetMax.y);    
    }

    public void SetTab(Path path, GameObject[] editor, int index, int editor_depth)
    {
        Button new_tab = SpawnTab();

        //FIX TAB PNG; THEN REMOVE THIS
        SetAnchors(index, editor.Length);
        //When removed, change gameobject[] editor to string editor_name
        new_tab.GetComponentInChildren<Text>().text = editor[index].name;

        if (source)
        {
            new_tab.onClick.AddListener(delegate {
                OpenSource(path, index, editor_depth);
            });
        } else {
            new_tab.onClick.AddListener(delegate {
                OpenEditor(path, index, editor_depth);
            });
        }
            
        new_tab.gameObject.SetActive(true);
    }

    void OpenEditor(Path path, int index, int editor_depth)
    {
        NavigationManager.navigation_manager.OpenEditor(NewEditor(path, index, editor_depth), true, false);
    }

    void OpenSource(Path path, int index, int editor_depth)
    {
        NavigationManager.navigation_manager.OpenSource(NewEditor(path, index, editor_depth));
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

    public void ResetTabs()
    {
        for (int i = 0; i < tab_list.Count; i++)
        {
            tab_list[i].onClick.RemoveAllListeners();
            tab_list[i].gameObject.SetActive(false);
        }
    }
}
