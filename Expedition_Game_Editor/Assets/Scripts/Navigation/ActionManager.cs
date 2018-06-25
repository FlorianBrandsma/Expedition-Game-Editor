﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class ActionManager : MonoBehaviour
{
    static List<Dropdown> dropdown_pool = new List<Dropdown>();
    static List<Button> button_pool = new List<Button>();
    static List<Button> mini_button_pool = new List<Button>();

    private List<RectTransform> actions = new List<RectTransform>();

    public RectTransform main_editor_parent;

    public int wrap_limit;

    public void SortActions()
    {
        for (int i = 0; i < actions.Count; i++)
        {
            RectTransform new_option = actions[i];

            if (actions.Count <= wrap_limit)
            {
                new_option.anchorMin = new Vector2(      i * (1f / wrap_limit), 1);
                new_option.anchorMax = new Vector2((i + 1) * (1f / wrap_limit), 1);
            } else {
                new_option.anchorMin = new Vector2(      i * (1f / actions.Count), 1);
                new_option.anchorMax = new Vector2((i + 1) * (1f / actions.Count), 1);
            }

            new_option.offsetMin = new Vector2(-2.5f, new_option.offsetMin.y);
            new_option.offsetMax = new Vector2( 2.5f, new_option.offsetMax.y);
        }

        if(actions.Count > 0)
            SetEditorSize(true);
    }

    public void SetEditorSize(bool collapse)
    {
        if(collapse)
        {
            main_editor_parent.anchorMin = new Vector2(GetComponent<RectTransform>().anchorMin.x, GetComponent<RectTransform>().anchorMax.y);
        } else {
            main_editor_parent.anchorMin = Vector2.zero;
        }  
    }

    public Dropdown AddDropdown()
    {
        Dropdown new_action = SpawnDropdown();

        AddActions(new_action.GetComponent<RectTransform>());

        return new_action;
    }

    public Button AddButton()
    {
        Button new_action = SpawnButton();

        AddActions(new_action.GetComponent<RectTransform>());

        return new_action;
    }

    public Button AddMiniButton()
    {
        Button new_action = SpawnMiniButton();

        AddActions(new_action.GetComponent<RectTransform>());

        return new_action;
    }

    public void AddActions(RectTransform new_action)
    {
        actions.Add(new_action);

        SortActions();
    }

    public void CloseActions()
    {
        foreach (RectTransform action in actions)
            action.gameObject.SetActive(false);

        SetEditorSize(false);

        actions.Clear();
    }

    #region Spawners

    Dropdown SpawnDropdown()
    {
        foreach(Dropdown dropdown in dropdown_pool)
        {
            if (!dropdown.gameObject.activeInHierarchy)
            {
                dropdown.transform.SetParent(transform, false);

                dropdown.gameObject.SetActive(true);

                return dropdown;
            }
        }

        Dropdown new_action = Instantiate(Resources.Load<Dropdown>("Editor/Actions/Dropdown"));

        new_action.transform.SetParent(transform, false);

        dropdown_pool.Add(new_action);

        return new_action;
    }

    Button SpawnButton()
    {
        foreach(Button button in button_pool)
        {
            if (!button.gameObject.activeInHierarchy)
            {
                button.onClick.RemoveAllListeners();

                button.transform.SetParent(transform, false);

                button.gameObject.SetActive(true);

                return button;
            }
        }

        Button new_action = Instantiate(Resources.Load<Button>("Editor/Actions/Button"));

        new_action.transform.SetParent(transform, false);

        button_pool.Add(new_action);

        return new_action;
    }

    Button SpawnMiniButton()
    {
        foreach(Button button in mini_button_pool)
        {
            if (!button.gameObject.activeInHierarchy)
            {
                button.onClick.RemoveAllListeners();

                button.transform.SetParent(transform, false);

                button.gameObject.SetActive(true);

                return button;
            }
        }

        Button new_action = Instantiate(Resources.Load<Button>("Editor/Actions/Mini_Button"));

        new_action.transform.SetParent(transform, false);

        mini_button_pool.Add(new_action);

        return new_action;
    }
    #endregion
}