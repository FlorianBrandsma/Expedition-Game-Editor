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

    private List<ExTab> tabList = new List<ExTab>();
    private ExHeader header;

    private PathController[] controllers;
    
    public Axis axis;

    public void SetTabs(PathController pathController, Path main_path)
    {
        this.pathController = pathController;

        controllers = pathController.controllers;

        if (controllers.Length > 0)
            gameObject.SetActive(true);

        //Spawn tabs if more than 1 controller is to be displayed
        //Otherwise only show a basic header

        if (controllers.Length > 1)
        {
            var prefab = Resources.Load<ExTab>("Elements/UI/Tab" + Enum.GetName(typeof(Axis), axis));

            for (int i = 0; i < controllers.Length; i++)
            {
                var newTab = (ExTab)PoolManager.SpawnObject(prefab);
                tabList.Add(newTab);

                newTab.transform.SetParent(transform, false);
                SetAnchors(i, controllers.Length);
                
                newTab.label.text = controllers[i].name;

                var tempIndex = i; //Required for the delegate
                newTab.GetComponent<Button>().onClick.AddListener(delegate { InitializePath(tempIndex); });

                newTab.gameObject.SetActive(true);
            }

            SelectTab(main_path.Trim(pathController.step + 1).routeList[pathController.step].controllerIndex);

        } else if(controllers.Length == 1) {

            //Only optimized for horizontal
            //No cases where it's required vertically as of 01/01/2019

            var prefab = Resources.Load<ExHeader>("Elements/UI/Header");
            header = (ExHeader)PoolManager.SpawnObject(prefab);

            header.transform.SetParent(transform, false);
            header.transform.localPosition = Vector2.zero;

            header.RectTransform.sizeDelta = new Vector2(-20, header.RectTransform.sizeDelta.y);

            header.label.text = controllers[0].name;
            
            header.gameObject.SetActive(true);   
        } 
    }
    
    private void ScaleLabel(RectTransform tabRect, RectTransform labelRect)
    {
        labelRect.sizeDelta = new Vector2(tabRect.rect.height - 10, tabRect.rect.width);
    }

    private void SetAnchors(int index, int tabs)
    {
        ExTab tab = tabList[index].GetComponent<ExTab>();
        RectTransform rect = tab.GetComponent<RectTransform>();
        
        if (axis == Axis.Horizontal)
        {
            rect.anchorMin = new Vector2(index * (1f / tabs), 0);
            rect.anchorMax = new Vector2((index + 1) * (1f / tabs), 1);

            rect.offsetMin = new Vector2(-1, 0);
            rect.offsetMax = new Vector2(1, 0);
        }     

        if(axis == Axis.Vertical)
        {
            rect.anchorMin = new Vector2(0, (tabs - (index + 1)) * (1f / tabs));
            rect.anchorMax = new Vector2(1, (tabs - (index)) * (1f / tabs));

            rect.offsetMin = new Vector2(rect.offsetMin.x, -1);
            rect.offsetMax = new Vector2(rect.offsetMax.x, 1);

            ScaleLabel(rect, tab.LabelRect);
        }
    }

    private void InitializePath(int selectedTab)
    {
        pathController.route.path.Add(selectedTab);

        RenderManager.Render(pathController.route.path);
    }

    private void SelectTab(int selectedTab)
    {
        for (int i = 0; i < tabList.Count; i++) 
        {
            var tab = tabList[i];
            tab.Image.sprite = i == selectedTab ? tab.tabActive : tab.tabInactive;
        }
    }

    public void CloseTabs()
    {
        for (int i = 0; i < tabList.Count; i++)
        {
            var tab = tabList[i];
            PoolManager.ClosePoolObject(tab);

            controllers[i].gameObject.SetActive(false);
        }

        tabList.Clear();

        if(header != null)
        {
            PoolManager.ClosePoolObject(header);
            header = null;
        }

        gameObject.SetActive(false);
    }
}
