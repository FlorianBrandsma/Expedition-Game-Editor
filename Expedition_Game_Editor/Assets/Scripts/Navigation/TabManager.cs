using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class TabManager : MonoBehaviour
{
    private EditorController controller;

    public List<Button> tab_list = new List<Button>();

    public bool tab_parent;

    private EditorController[] tabs;

    public void SetEditorTabs(EditorController new_controller, Path main_path)
    {
        controller = new_controller;

        tabs = controller.controllers;

        if (tabs.Length > 0)
            gameObject.SetActive(true);

        for (int i = 0; i < tabs.Length; i++)
        {
            Button new_tab = SpawnTab();

            //Messes with paths -> Use this later 
            //>Take path from data (maybe) and into controller
            //>>
            //controllers[tab].data = controller.data;

            //Path is required in data so that it can add onto it for structure
            //Think about it. Rather remove path from data.

            /*
            controllers[tab].data.table = controller.data.table;
            controllers[tab].data.id = controller.data.id;
            controllers[tab].data.type = controller.data.type;
            */
            //FIX TAB PNG; THEN REMOVE THIS
            SetAnchors(i, tabs.Length);

            new_tab.GetComponentInChildren<Text>().text = tabs[i].name;

            int index = i;

            new_tab.onClick.AddListener(delegate
            {
                OpenPath(index);
            });

            new_tab.gameObject.SetActive(true);
        }

        SelectTab(main_path.Trim(controller.depth + 1).structure[controller.depth]);
    }
    
    void SetAnchors(int index, int tabs)
    {
        RectTransform new_tab = tab_list[index].GetComponent<RectTransform>();

        new_tab.anchorMin = new Vector2(index * (1f / tabs), 0);
        new_tab.anchorMax = new Vector2((index + 1) * (1f / tabs), 1);

        new_tab.offsetMin = new Vector2(-1, new_tab.offsetMin.y);
        new_tab.offsetMax = new Vector2(1, new_tab.offsetMax.y);      
    }

    void OpenPath(int selected_tab)
    {
        controller.data.path.structure.Add(selected_tab);
        controller.data.path.data.Add(new ElementData());

        EditorManager.editorManager.OpenPath(controller.data.path);
    }

    public void SelectTab(int selected_tab)
    {
        for (int tab = 0; tab < tab_list.Count; tab++) 
        {
            if (tab == selected_tab)
                tab_list[tab].GetComponent<Image>().sprite = Resources.Load<Sprite>("Textures/Buttons/Tab_A");
            else
                tab_list[tab].GetComponent<Image>().sprite = Resources.Load<Sprite>("Textures/Buttons/Tab_O");
        }
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
        for (int i = 0; i < tabs.Length; i++)
        {
            tab_list[i].onClick.RemoveAllListeners();
            tab_list[i].gameObject.SetActive(false);
            tabs[i].gameObject.SetActive(false);
        }

        gameObject.SetActive(false);
    }
}
