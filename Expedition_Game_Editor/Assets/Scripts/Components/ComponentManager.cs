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

    private List<Dropdown> dropdownPool = new List<Dropdown>();
    private List<Button> buttonPool = new List<Button>();
    private List<Button> formButtonPool = new List<Button>();

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

        List<ComponentElement> leftElements = new List<ComponentElement>();
        List<ComponentElement> rightElements = new List<ComponentElement>();
        List<ComponentElement> mainElements = new List<ComponentElement>();

        float leftOffset = 0;
        float rightOffset = 0;
        float mainSize = 0;

        //Sort element by anchor
        foreach (RectTransform element in elements)
        {
            ComponentElement componentElement = element.GetComponent<ComponentElement>();

            element.sizeDelta = new Vector2(componentElement.component.width + 5, element.sizeDelta.y);

            switch (componentElement.component.anchor)
            {
                case EditorComponent.Anchor.Left:

                    element.transform.SetParent(element_parent, false);
                    leftOffset += componentElement.component.width;

                    leftElements.Add(componentElement);
                    
                    break;

                case EditorComponent.Anchor.Right:

                    element.transform.SetParent(element_parent, false);
                    rightOffset += componentElement.component.width;

                    rightElements.Add(componentElement);
                    
                    break;

                case EditorComponent.Anchor.Main:

                    element.transform.SetParent(main_parent, false);
                    mainSize += componentElement.component.width;

                    mainElements.Add(componentElement);

                    break;
            }
        }

        SetLeftElements(leftElements);
        SetRightElements(rightElements);
        
        //Set main parent size according to the combined sizes of the left and right elements
        SetMainSize(leftOffset, rightOffset, mainSize);

        SetMainElements(mainElements);
    }

    private void SetLeftElements(List<ComponentElement> elements)
    {
        float previousPosition = -(element_parent.rect.width / 2);

        foreach(ComponentElement element in elements)
        {
            float elementWidth = element.component.width;

            element.transform.localPosition = new Vector2(previousPosition + (elementWidth / 2), 0);

            previousPosition += elementWidth;
        }
    }

    private void SetRightElements(List<ComponentElement> elements)
    {
        float previousPosition = (element_parent.rect.width / 2);

        foreach(ComponentElement element in elements)
        {
            float elementWidth = element.component.width;

            element.transform.localPosition = new Vector2(previousPosition - (elementWidth / 2), 0);

            previousPosition -= elementWidth;
        }
    }

    private void SetMainElements(List<ComponentElement> elements)
    {
        float previousPosition = -(main_parent.rect.width / 2);

        foreach(ComponentElement element in elements)
        {
            float elementWidth = element.component.width;

            element.transform.localPosition = new Vector2(previousPosition + (elementWidth / 2), 0);

            previousPosition += elementWidth;
        }
    }

    private void SetMainSize(float leftOffset, float rightOffset, float mainSize)
    {
        main_content.offsetMin = new Vector2(element_parent.offsetMin.x + leftOffset, main_content.offsetMin.y);
        main_content.offsetMax = new Vector2(element_parent.offsetMax.x - rightOffset, main_content.offsetMax.y);

        float parentWidth = main_parent.parent.GetComponent<RectTransform>().rect.width;

        main_parent.offsetMax = new Vector2(-parentWidth + mainSize, 0);

        if (mainSize > parentWidth)
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
        float sliderValue = Mathf.Clamp(scrollRect.horizontalNormalizedPosition, 0, 1);

        if(sliderValue > 0)
            SetArrow(left_arrow, true);
         else
            SetArrow(left_arrow, false);
        
        if (sliderValue < 1)
            SetArrow(right_arrow, true);
        else
            SetArrow(right_arrow, false);
    }

    public Dropdown AddDropdown(EditorComponent component)
    {
        Dropdown dropdown = SpawnDropdown();

        if (dropdown.GetComponent<IDataController>() != null)
            DestroyImmediate((UnityEngine.Object)dropdown.GetComponent<IDataController>());

        dropdown.options.Clear();
        dropdown.value = 0;
        dropdown.onValueChanged.RemoveAllListeners();

        AddComponents(dropdown.GetComponent<RectTransform>(), component);

        return dropdown;
    }

    public Button AddButton(EditorComponent component)
    {
        Button button = SpawnButton();

        button.onClick.RemoveAllListeners();

        AddComponents(button.GetComponent<RectTransform>(), component);

        return button;
    }

    public Button AddFormButton(EditorComponent component)
    {
        Button button = SpawnFormButton();

        button.onClick.RemoveAllListeners();

        AddComponents(button.GetComponent<RectTransform>(), component);

        return button;
    }

    private void AddComponents(RectTransform element, EditorComponent component)
    {
        if (element.GetComponent<ComponentElement>() == null)
            element.gameObject.AddComponent<ComponentElement>();

        ComponentElement componentElement = element.GetComponent<ComponentElement>();
        componentElement.SetElement(component);

        elements.Add(element);
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
        foreach(Dropdown dropdown in dropdownPool)
        {
            if (!dropdown.gameObject.activeInHierarchy)
            {
                dropdown.gameObject.SetActive(true);

                return dropdown;
            }
        }

        Dropdown newComponent = Instantiate(Resources.Load<Dropdown>("UI/Dropdown"));

        dropdownPool.Add(newComponent);

        return newComponent;
    }

    private Button SpawnButton()
    {
        foreach(Button button in buttonPool)
        {
            if (!button.gameObject.activeInHierarchy)
            {
                button.onClick.RemoveAllListeners();

                button.gameObject.SetActive(true);

                return button;
            }
        }

        Button newComponent = Instantiate(Resources.Load<Button>("UI/ButtonComponent"));

        buttonPool.Add(newComponent);

        return newComponent;
    }

    private Button SpawnFormButton()
    {
        foreach(Button button in formButtonPool)
        {
            if (!button.gameObject.activeInHierarchy)
            {
                button.GetComponent<Button>().onClick.RemoveAllListeners();

                button.gameObject.SetActive(true);

                return button;
            }
        }

        Button newComponent = Instantiate(Resources.Load<Button>("UI/FormButton"));

        formButtonPool.Add(newComponent);

        return newComponent;
    }
    #endregion
}
