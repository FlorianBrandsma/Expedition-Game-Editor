using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class OptionManager : MonoBehaviour
{
    static List<Dropdown> dropdown_pool = new List<Dropdown>();
    static List<Button> button_pool = new List<Button>();

    public GameObject option_parent;

    public OptionOrganizer optionOrganizer;


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
        optionOrganizer.options.Add(new_option);
    }

    Dropdown SpawnDropdown()
    {
        for (int i = 0; i < dropdown_pool.Count; i++)
        {
            if (!dropdown_pool[i].gameObject.activeInHierarchy)
            {
                dropdown_pool[i].transform.SetParent(option_parent.transform, false);

                dropdown_pool[i].gameObject.SetActive(true);

                return dropdown_pool[i];
            }
        }

        Dropdown new_option = Instantiate(Resources.Load<Dropdown>("Editor/Options/Dropdown"));

        new_option.transform.SetParent(option_parent.transform, false);

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

                button_pool[i].transform.SetParent(option_parent.transform, false);

                button_pool[i].gameObject.SetActive(true);

                return button_pool[i];
            }
        }

        Button new_option = Instantiate(Resources.Load<Button>("Editor/Options/Button"));

        new_option.transform.SetParent(option_parent.transform, false);

        button_pool.Add(new_option);

        return new_option;
    }
}
