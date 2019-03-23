using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;

public class SubControllerManager : MonoBehaviour
{
    public enum Axis
    {
        Horizontal,
        Vertical,
    }

    private PathController pathController;

    private List<EditorTab> tab_list = new List<EditorTab>();
    private List<EditorTab> local_tab_list = new List<EditorTab>();

    private PathController[] controllers;

    private RectTransform header;

    public Axis axis;
    private string axis_name;

    public void SetTabs(PathController pathController, Path main_path)
    {
        this.pathController = pathController;

        controllers = pathController.controllers;

        if (controllers.Length > 0)
            gameObject.SetActive(true);

        //Spawn tabs if more than 1 controller is to be displayed
        //Otherwise only show a basic header

        if(controllers.Length > 1)
        {
            axis_name = Enum.GetName(typeof(Axis), axis);

            for (int i = 0; i < controllers.Length; i++)
            {
                EditorTab new_tab = SpawnTab();
                local_tab_list.Add(new_tab);

                SetAnchors(i, controllers.Length);

                controllers[i].route = pathController.route;

                new_tab.label.text = controllers[i].name;
                 
                int index = i;
                new_tab.GetComponent<Button>().onClick.AddListener(delegate { OpenPath(index); });

                new_tab.gameObject.SetActive(true);
            }

            SelectTab(main_path.Trim(pathController.step + 1).route[pathController.step].controller);

        } else if(controllers.Length == 1) { 

            //Only optimized for horizontal
            //No cases where it's required vertically as of 01/01/2019

            if (header == null)
                header = SpawnHeader();

            header.GetComponentInChildren<Text>().text = controllers[0].name;

            header.localPosition = Vector2.zero;

            header.gameObject.SetActive(true);   
        } 
    }
    
    private void ScaleLabel(RectTransform tab_rect, RectTransform label_rect)
    {
        label_rect.sizeDelta = new Vector2(tab_rect.rect.height - 10, tab_rect.rect.width);
    }

    private void SetAnchors(int index, int tabs)
    {
        EditorTab new_tab = local_tab_list[index].GetComponent<EditorTab>();
        RectTransform rect = new_tab.GetComponent<RectTransform>();

        if(axis == Axis.Horizontal)
        {
            rect.anchorMin = new Vector2(index * (1f / tabs), 0);
            rect.anchorMax = new Vector2((index + 1) * (1f / tabs), 1);

            rect.offsetMin = new Vector2(-1, rect.offsetMin.y);
            rect.offsetMax = new Vector2(1, rect.offsetMax.y);
        }     

        if(axis == Axis.Vertical)
        {
            rect.anchorMin = new Vector2(0, (tabs - (index + 1)) * (1f / tabs));
            rect.anchorMax = new Vector2(1, (tabs - (index)) * (1f / tabs));

            rect.offsetMin = new Vector2(rect.offsetMin.x, -1);
            rect.offsetMax = new Vector2(rect.offsetMax.x, 1);

            ScaleLabel(rect, new_tab.label_rect);
        }
    }

    private void OpenPath(int selected_tab)
    {
        pathController.route.path.Add(selected_tab);

        EditorManager.editorManager.OpenPath(pathController.route.path);
    }

    private void SelectTab(int selected_tab)
    {
        for (int tab = 0; tab < local_tab_list.Count; tab++) 
        {
            if (tab == selected_tab)
                local_tab_list[tab].GetComponent<Image>().sprite = Resources.Load<Sprite>("Textures/UI/" + axis_name + "_Tab_A");
            else
                local_tab_list[tab].GetComponent<Image>().sprite = Resources.Load<Sprite>("Textures/UI/" + axis_name + "_Tab_O");
        }
    }

    private EditorTab SpawnTab()
    {
        foreach(EditorTab tab in tab_list)
        {
            if (!tab.gameObject.activeInHierarchy)
                return tab;
        }

        EditorTab new_tab = Instantiate(Resources.Load<EditorTab>("UI/Tab_" + axis_name));
        new_tab.transform.SetParent(transform, false);
        tab_list.Add(new_tab);

        return new_tab;
    }

    private RectTransform SpawnHeader()
    {
        RectTransform new_header = Instantiate(Resources.Load<RectTransform>("UI/Header"));
        new_header.transform.SetParent(transform, false);

        return new_header;
    }

    public void CloseTabs()
    {
        for (int i = 0; i < local_tab_list.Count; i++)
        {
            local_tab_list[i].GetComponent<Button>().onClick.RemoveAllListeners();
            local_tab_list[i].gameObject.SetActive(false);

            controllers[i].gameObject.SetActive(false);
        }

        local_tab_list.Clear();

        if(header != null)
            header.gameObject.SetActive(false);

        gameObject.SetActive(false);
    }
}
