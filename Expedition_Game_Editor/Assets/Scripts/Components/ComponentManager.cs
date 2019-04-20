using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class ComponentManager : MonoBehaviour
{
    static public ComponentManager componentManager;

    private List<Dropdown> dropdown_pool = new List<Dropdown>();
    private List<Button> button_pool = new List<Button>();
    private List<Button> form_button_pool = new List<Button>();

    private List<RectTransform> elements = new List<RectTransform>();

    public RectTransform element_parent;
    public RectTransform main_content;
    public ScrollRect scrollRect;
    public RectTransform main_parent;

    public RawImage left_arrow, right_arrow;
    public Texture arrow_active, arrow_inactive;

    private void Awake()
    {
        componentManager = this;

        CloseSlider();
    }

    public void SortComponents()
    {
        if (elements.Count == 0) return;

        List<ComponentElement> left_elements = new List<ComponentElement>();
        List<ComponentElement> right_elements = new List<ComponentElement>();
        List<ComponentElement> main_elements = new List<ComponentElement>();

        float left_offset = 0;
        float right_offset = 0;
        float main_size = 0;

        //Sort element by anchor
        foreach (RectTransform element in elements)
        {
            ComponentElement componentElement = element.GetComponent<ComponentElement>();

            element.sizeDelta = new Vector2(componentElement.component.width + 5, element.sizeDelta.y);

            switch (componentElement.component.anchor)
            {
                case EditorComponent.Anchor.Left:

                    element.transform.SetParent(element_parent, false);
                    left_offset += componentElement.component.width;

                    left_elements.Add(componentElement);
                    
                    break;

                case EditorComponent.Anchor.Right:

                    element.transform.SetParent(element_parent, false);
                    right_offset += componentElement.component.width;

                    right_elements.Add(componentElement);
                    
                    break;

                case EditorComponent.Anchor.Main:

                    element.transform.SetParent(main_parent, false);
                    main_size += componentElement.component.width;

                    main_elements.Add(componentElement);

                    break;
            }
        }

        SetLeftElements(left_elements);
        SetRightElements(right_elements);
        
        //Set main parent size according to the combined sizes of the left and right elements
        SetMainSize(left_offset, right_offset, main_size);

        SetMainElements(main_elements);
    }

    private void SetLeftElements(List<ComponentElement> elements)
    {
        float previous_position = -(element_parent.rect.width / 2);

        foreach(ComponentElement element in elements)
        {
            float element_width = element.component.width;

            element.transform.localPosition = new Vector2(previous_position + (element_width / 2), 0);

            previous_position += element_width;
        }
    }

    private void SetRightElements(List<ComponentElement> elements)
    {
        float previous_position = (element_parent.rect.width / 2);

        foreach(ComponentElement element in elements)
        {
            float element_width = element.component.width;

            element.transform.localPosition = new Vector2(previous_position - (element_width / 2), 0);

            previous_position -= element_width;
        }
    }

    private void SetMainElements(List<ComponentElement> elements)
    {
        float previous_position = -(main_parent.rect.width / 2);

        foreach(ComponentElement element in elements)
        {
            float element_width = element.component.width;

            element.transform.localPosition = new Vector2(previous_position + (element_width / 2), 0);

            previous_position += element_width;
        }
    }

    private void SetMainSize(float left_offset, float right_offset, float main_size)
    {
        main_content.offsetMin = new Vector2(element_parent.offsetMin.x + left_offset, main_content.offsetMin.y);
        main_content.offsetMax = new Vector2(element_parent.offsetMax.x - right_offset, main_content.offsetMax.y);

        float parent_width = main_parent.parent.GetComponent<RectTransform>().rect.width;

        main_parent.offsetMax = new Vector2(-parent_width + main_size, 0);

        if (main_size > parent_width)
            ActivateSlider();
    }

    public void ActivateSlider()
    {
        scrollRect.enabled = true;

        SetSlider();

        scrollRect.horizontalNormalizedPosition = 1f;
    }

    public void SetSlider()
    {
        float slider_value = Mathf.Clamp(scrollRect.horizontalNormalizedPosition, 0, 1);

        if(slider_value > 0)
            SetArrow(left_arrow, true);
         else
            SetArrow(left_arrow, false);
        
        if (slider_value < 1)
            SetArrow(right_arrow, true);
        else
            SetArrow(right_arrow, false);
    }

    public Dropdown AddDropdown(EditorComponent new_component)
    {
        Dropdown dropdown = SpawnDropdown();

        if (dropdown.GetComponent<IDataController>() != null)
            DestroyImmediate((UnityEngine.Object)dropdown.GetComponent<IDataController>());

        dropdown.options.Clear();
        dropdown.onValueChanged.RemoveAllListeners();

        AddComponents(dropdown.GetComponent<RectTransform>(), new_component);

        return dropdown;
    }

    public Button AddButton(EditorComponent new_component)
    {
        Button button = SpawnButton();

        button.onClick.RemoveAllListeners();

        AddComponents(button.GetComponent<RectTransform>(), new_component);

        return button;
    }

    public Button AddFormButton(EditorComponent new_component)
    {
        Button button = SpawnFormButton();

        button.onClick.RemoveAllListeners();

        AddComponents(button.GetComponent<RectTransform>(), new_component);

        return button;
    }

    private void AddComponents(RectTransform new_element, EditorComponent new_component)
    {
        if (new_element.GetComponent<ComponentElement>() == null)
            new_element.gameObject.AddComponent<ComponentElement>();

        ComponentElement componentElement = new_element.GetComponent<ComponentElement>();
        componentElement.SetElement(new_component);

        elements.Add(new_element);
    }

    public void CloseComponents()
    {
        CloseSlider();

        main_content.offsetMin = new Vector2(0, main_content.offsetMin.y);
        main_content.offsetMax = new Vector2(0, main_content.offsetMax.y);

        main_parent.offsetMin = new Vector2(0, main_parent.offsetMin.y);
        main_parent.offsetMax = new Vector2(0, main_parent.offsetMax.y);

        foreach (RectTransform element in elements)
            element.gameObject.SetActive(false);

        elements.Clear();
    }

    private void CloseSlider()
    {
        //scrollRect.horizontalNormalizedPosition = 1f;

        scrollRect.enabled = false;

        SetArrow(left_arrow, false);
        SetArrow(right_arrow, false);
    }

    private void SetArrow(RawImage arrow, bool active)
    {
        arrow.texture = active ? arrow_active : arrow_inactive;
    }

    #region Spawners

    private Dropdown SpawnDropdown()
    {
        foreach(Dropdown dropdown in dropdown_pool)
        {
            if (!dropdown.gameObject.activeInHierarchy)
            {
                dropdown.gameObject.SetActive(true);

                return dropdown;
            }
        }

        Dropdown new_component = Instantiate(Resources.Load<Dropdown>("UI/Dropdown"));

        dropdown_pool.Add(new_component);

        return new_component;
    }

    private Button SpawnButton()
    {
        foreach(Button button in button_pool)
        {
            if (!button.gameObject.activeInHierarchy)
            {
                button.onClick.RemoveAllListeners();

                button.gameObject.SetActive(true);

                return button;
            }
        }

        Button new_component = Instantiate(Resources.Load<Button>("UI/ButtonComponent"));

        button_pool.Add(new_component);

        return new_component;
    }

    private Button SpawnFormButton()
    {
        foreach(Button button in form_button_pool)
        {
            if (!button.gameObject.activeInHierarchy)
            {
                button.GetComponent<Button>().onClick.RemoveAllListeners();

                button.gameObject.SetActive(true);

                return button;
            }
        }

        Button new_component = Instantiate(Resources.Load<Button>("UI/FormButton"));

        form_button_pool.Add(new_component);

        return new_component;
    }
    #endregion
}
