using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class ActionManager : MonoBehaviour
{
    static List<Dropdown> dropdown_pool = new List<Dropdown>();
    static List<Button> button_pool = new List<Button>();

    private List<RectTransform> options = new List<RectTransform>();

    public RectTransform main_editor_parent;

    public int wrap_limit;

    public void SortOptions()
    {
        for (int i = 0; i < options.Count; i++)
        {
            RectTransform new_option = options[i];

            if (options.Count <= wrap_limit)
            {
                new_option.anchorMin = new Vector2(      i * (1f / wrap_limit), 1);
                new_option.anchorMax = new Vector2((i + 1) * (1f / wrap_limit), 1);
            } else {
                new_option.anchorMin = new Vector2(      i * (1f / options.Count), 1);
                new_option.anchorMax = new Vector2((i + 1) * (1f / options.Count), 1);
            }

            new_option.offsetMin = new Vector2(-2.5f, new_option.offsetMin.y);
            new_option.offsetMax = new Vector2( 2.5f, new_option.offsetMax.y);
        }

        if(options.Count > 0)
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
        Dropdown new_option = SpawnDropdown();

        AddOptions(new_option.GetComponent<RectTransform>());

        return new_option;
    }

    public Button AddButton()
    {
        Button new_option = SpawnButton();

        AddOptions(new_option.GetComponent<RectTransform>());

        return new_option;
    }

    public void AddOptions(RectTransform new_option)
    {
        options.Add(new_option);

        SortOptions();
    }

    public void CloseOptions()
    {
        foreach (RectTransform option in options)
            option.gameObject.SetActive(false);

        SetEditorSize(false);

        options.Clear();
    }

    #region Spawners

    Dropdown SpawnDropdown()
    {
        for (int i = 0; i < dropdown_pool.Count; i++)
        {
            if (!dropdown_pool[i].gameObject.activeInHierarchy)
            {
                dropdown_pool[i].transform.SetParent(transform, false);

                dropdown_pool[i].gameObject.SetActive(true);

                return dropdown_pool[i];
            }
        }

        Dropdown new_option = Instantiate(Resources.Load<Dropdown>("Editor/Actions/Dropdown"));

        new_option.transform.SetParent(transform, false);

        dropdown_pool.Add(new_option);

        return new_option;
    }

    Button SpawnButton()
    {
        for (int i = 0; i < button_pool.Count; i++)
        {
            if (!button_pool[i].gameObject.activeInHierarchy)
            {
                button_pool[i].onClick.RemoveAllListeners();

                button_pool[i].transform.SetParent(transform, false);

                button_pool[i].gameObject.SetActive(true);

                return button_pool[i];
            }
        }

        Button new_option = Instantiate(Resources.Load<Button>("Editor/Actions/Button"));

        new_option.transform.SetParent(transform, false);

        button_pool.Add(new_option);

        return new_option;
    }
    #endregion
}
