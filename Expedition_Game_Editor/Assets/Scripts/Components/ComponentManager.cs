using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class ComponentManager : MonoBehaviour
{
    static List<Dropdown> dropdown_pool = new List<Dropdown>();
    static List<Button> button_pool = new List<Button>();
    static List<Button> form_button_pool = new List<Button>();

    private List<RectTransform> components = new List<RectTransform>();

    public RectTransform footer;

    public int wrap_limit;

    public void SortComponents()
    {
        for (int i = 0; i < components.Count; i++)
        {
            RectTransform new_option = components[i];

            if (components.Count <= wrap_limit)
            {
                new_option.anchorMin = new Vector2(      i * (1f / wrap_limit), 1);
                new_option.anchorMax = new Vector2((i + 1) * (1f / wrap_limit), 1);
            } else {
                new_option.anchorMin = new Vector2(      i * (1f / components.Count), 1);
                new_option.anchorMax = new Vector2((i + 1) * (1f / components.Count), 1);
            }

            new_option.anchoredPosition = new Vector2(new_option.anchoredPosition.x, -(new_option.sizeDelta.y / 2));

            new_option.offsetMin = new Vector2(-2.5f, new_option.offsetMin.y);
            new_option.offsetMax = new Vector2( 2.5f, new_option.offsetMax.y);
        }

        if(components.Count > 0 && footer != null)
            footer.gameObject.SetActive(true);
    }

    public Dropdown AddDropdown()
    {
        Dropdown new_component = SpawnDropdown();

        AddComponents(new_component.GetComponent<RectTransform>());

        return new_component;
    }

    public Button AddButton()
    {
        Button new_component = SpawnButton();

        AddComponents(new_component.GetComponent<RectTransform>());

        return new_component;
    }

    public Button AddFormButton()
    {
        Button new_component = SpawnFormButton();

        AddComponents(new_component.GetComponent<RectTransform>());

        return new_component;
    }

    public void AddComponents(RectTransform component)
    {
        components.Add(component);

        SortComponents();
    }

    public void RemoveComponent(RectTransform component, bool reset)
    {
        if(component.gameObject.activeInHierarchy)
        {
            components.RemoveAt(components.IndexOf(component));
            component.gameObject.SetActive(false);
        }
        
        if (reset)
            SortComponents();
    }

    public void CloseComponents()
    {
        foreach (RectTransform component in components)
            component.gameObject.SetActive(false);

        if(footer != null)
            footer.gameObject.SetActive(false);

        components.Clear();
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

        Dropdown new_component = Instantiate(Resources.Load<Dropdown>("UI/Dropdown"));

        new_component.transform.SetParent(transform, false);

        dropdown_pool.Add(new_component);

        return new_component;
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

        Button new_component = Instantiate(Resources.Load<Button>("UI/Button"));

        new_component.transform.SetParent(transform, false);

        button_pool.Add(new_component);

        return new_component;
    }

    Button SpawnFormButton()
    {
        foreach(Button button in form_button_pool)
        {
            if (!button.gameObject.activeInHierarchy)
            {
                button.GetComponent<Button>().onClick.RemoveAllListeners();

                button.transform.SetParent(transform, false);

                button.gameObject.SetActive(true);

                return button;
            }
        }

        Button new_component = Instantiate(Resources.Load<Button>("UI/FormButton"));

        new_component.transform.SetParent(transform, false);

        form_button_pool.Add(new_component);

        return new_component;
    }
    #endregion
}
