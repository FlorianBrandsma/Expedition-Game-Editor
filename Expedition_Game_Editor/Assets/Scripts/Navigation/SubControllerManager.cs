using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class SubControllerManager : MonoBehaviour
{
    private EditorController controller;

    private List<Button> tab_list = new List<Button>();
    private List<Button> local_tab_list = new List<Button>();

    private EditorController[] controllers;

    private RectTransform header;

    public void SetTabs(EditorController new_controller, Path main_path)
    {
        controller = new_controller;

        controllers = controller.controllers;

        if (controllers.Length > 0)
            gameObject.SetActive(true);

        //Spawn tabs if more than 1 controller is to be displayed
        //Otherwise only show a basic header

        if(controllers.Length > 1)
        {
            for (int i = 0; i < controllers.Length; i++)
            {
                Button new_tab = SpawnTab();
                local_tab_list.Add(new_tab);

                SetAnchors(i, controllers.Length);

                controllers[i].data = controller.data;
                new_tab.GetComponentInChildren<Text>().text = controllers[i].name;

                int index = i;

                new_tab.onClick.AddListener(delegate { OpenPath(index); });

                new_tab.gameObject.SetActive(true);
            }

            SelectTab(main_path.Trim(controller.step + 1).route[controller.step]);

        } else if(controllers.Length == 1) { 

            if (header == null)
                header = SpawnHeader();

            header.GetComponentInChildren<Text>().text = controllers[0].name;

            header.localPosition = Vector2.zero;

            header.gameObject.SetActive(true);   
        } 
    }
    
    private void SetAnchors(int index, int tabs)
    {
        RectTransform new_tab = local_tab_list[index].GetComponent<RectTransform>();

        new_tab.anchorMin = new Vector2(index * (1f / tabs), 0);
        new_tab.anchorMax = new Vector2((index + 1) * (1f / tabs), 1);

        new_tab.offsetMin = new Vector2(-1, new_tab.offsetMin.y);
        new_tab.offsetMax = new Vector2(1, new_tab.offsetMax.y);      
    }

    private void OpenPath(int selected_tab)
    {
        controller.path.Add(selected_tab);

        EditorManager.editorManager.OpenPath(controller.path);
    }

    private void SelectTab(int selected_tab)
    {
        for (int tab = 0; tab < local_tab_list.Count; tab++) 
        {
            if (tab == selected_tab)
                local_tab_list[tab].GetComponent<Image>().sprite = Resources.Load<Sprite>("Textures/Buttons/Tab_A");
            else
                local_tab_list[tab].GetComponent<Image>().sprite = Resources.Load<Sprite>("Textures/Buttons/Tab_O");
        }
    }

    private Button SpawnTab()
    {
        foreach(Button tab in tab_list)
        {
            if (!tab.gameObject.activeInHierarchy)
                return tab;
        }

        Button new_tab = Instantiate(Resources.Load<Button>("Editor/Tab"));
        new_tab.transform.SetParent(transform, false);
        tab_list.Add(new_tab);

        return new_tab;
    }

    private RectTransform SpawnHeader()
    {
        RectTransform new_header = Instantiate(Resources.Load<RectTransform>("Editor/Header"));
        new_header.transform.SetParent(transform, false);

        return new_header;
    }

    public void CloseTabs()
    {
        for (int i = 0; i < local_tab_list.Count; i++)
        {
            local_tab_list[i].onClick.RemoveAllListeners();
            local_tab_list[i].gameObject.SetActive(false);

            controllers[i].gameObject.SetActive(false);
        }

        local_tab_list.Clear();

        if(header != null && header.gameObject.activeInHierarchy)
            header.gameObject.SetActive(false);

        gameObject.SetActive(false);
    }
}
