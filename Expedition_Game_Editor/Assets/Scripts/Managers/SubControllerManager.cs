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

    private List<EditorTab> tabList = new List<EditorTab>();
    private List<EditorTab> localTabList = new List<EditorTab>();

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
                EditorTab newTab = SpawnTab();
                localTabList.Add(newTab);

                SetAnchors(i, controllers.Length);

                //controllers[i].route = pathController.route;

                newTab.label.text = controllers[i].name;
                 
                int index = i;
                newTab.GetComponent<Button>().onClick.AddListener(delegate { InitializePath(index); });

                newTab.gameObject.SetActive(true);
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
    
    private void ScaleLabel(RectTransform tabRect, RectTransform labelRect)
    {
        labelRect.sizeDelta = new Vector2(tabRect.rect.height - 10, tabRect.rect.width);
    }

    private void SetAnchors(int index, int tabs)
    {
        EditorTab new_tab = localTabList[index].GetComponent<EditorTab>();
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

    private void InitializePath(int selectedTab)
    {
        pathController.route.path.Add(selectedTab);

        EditorManager.editorManager.Render(pathController.route.path);
    }

    private void SelectTab(int selectedTab)
    {
        for (int tab = 0; tab < localTabList.Count; tab++) 
        {
            if (tab == selectedTab)
                localTabList[tab].GetComponent<Image>().sprite = Resources.Load<Sprite>("Textures/UI/" + axis_name + "_Tab_A");
            else
                localTabList[tab].GetComponent<Image>().sprite = Resources.Load<Sprite>("Textures/UI/" + axis_name + "_Tab_O");
        }
    }

    private EditorTab SpawnTab()
    {
        foreach(EditorTab tab in tabList)
        {
            if (!tab.gameObject.activeInHierarchy)
                return tab;
        }

        EditorTab newTab = Instantiate(Resources.Load<EditorTab>("UI/Tab_" + axis_name));
        newTab.transform.SetParent(transform, false);
        tabList.Add(newTab);

        return newTab;
    }

    private RectTransform SpawnHeader()
    {
        RectTransform newHeader = Instantiate(Resources.Load<RectTransform>("UI/EditorHeader"));
        newHeader.transform.SetParent(transform, false);

        return newHeader;
    }

    public void CloseTabs()
    {
        for (int i = 0; i < localTabList.Count; i++)
        {
            localTabList[i].GetComponent<Button>().onClick.RemoveAllListeners();
            localTabList[i].gameObject.SetActive(false);

            controllers[i].gameObject.SetActive(false);
        }

        localTabList.Clear();

        if(header != null)
            header.gameObject.SetActive(false);

        gameObject.SetActive(false);
    }
}
