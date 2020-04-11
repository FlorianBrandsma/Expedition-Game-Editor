using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class ActionManager : MonoBehaviour
{
    static public ActionManager actionManager;

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
        actionManager = this;

        CloseSlider();
    }

    public void SortActions()
    {
        if (elements.Count == 0) return;

        List<ActionElement> leftElements = new List<ActionElement>();
        List<ActionElement> rightElements = new List<ActionElement>();
        List<ActionElement> mainElements = new List<ActionElement>();

        float leftOffset = 0;
        float rightOffset = 0;
        float mainSize = 0;

        //Sort element by anchor
        foreach (RectTransform element in elements)
        {
            ActionElement actionElement = element.GetComponent<ActionElement>();

            element.sizeDelta = new Vector2(actionElement.ActionProperties.width + 5, element.sizeDelta.y);

            switch (actionElement.ActionProperties.anchor)
            {
                case ActionProperties.Anchor.Left:

                    element.transform.SetParent(element_parent, false);
                    leftOffset += actionElement.ActionProperties.width;

                    leftElements.Add(actionElement);
                    
                    break;

                case ActionProperties.Anchor.Right:

                    element.transform.SetParent(element_parent, false);
                    rightOffset += actionElement.ActionProperties.width;

                    rightElements.Add(actionElement);
                    
                    break;

                case ActionProperties.Anchor.Main:

                    element.transform.SetParent(main_parent, false);
                    mainSize += actionElement.ActionProperties.width;

                    mainElements.Add(actionElement);

                    break;
            }
        }

        SetLeftElements(leftElements);
        SetRightElements(rightElements);
        
        //Set main parent size according to the combined sizes of the left and right elements
        SetMainSize(leftOffset, rightOffset, mainSize);

        SetMainElements(mainElements);
    }

    private void SetLeftElements(List<ActionElement> elements)
    {
        float previousPosition = -(element_parent.rect.width / 2);

        foreach(ActionElement element in elements)
        {
            float elementWidth = element.ActionProperties.width;

            element.transform.localPosition = new Vector2(previousPosition + (elementWidth / 2), 0);

            previousPosition += elementWidth;
        }
    }

    private void SetRightElements(List<ActionElement> elements)
    {
        float previousPosition = (element_parent.rect.width / 2);

        foreach(ActionElement element in elements)
        {
            float elementWidth = element.ActionProperties.width;

            element.transform.localPosition = new Vector2(previousPosition - (elementWidth / 2), 0);

            previousPosition -= elementWidth;
        }
    }

    private void SetMainElements(List<ActionElement> elements)
    {
        float previousPosition = -(main_parent.rect.width / 2);

        foreach(ActionElement element in elements)
        {
            float elementWidth = element.ActionProperties.width;

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

    public Dropdown AddDropdown(ActionProperties actionProperties)
    {
        Dropdown dropdown = SpawnDropdown();

        if (dropdown.GetComponent<IDataController>() != null)
            DestroyImmediate((UnityEngine.Object)dropdown.GetComponent<IDataController>());

        dropdown.options.Clear();
        dropdown.value = 0;
        dropdown.onValueChanged.RemoveAllListeners();

        AddAction(dropdown.GetComponent<RectTransform>(), actionProperties);

        return dropdown;
    }

    public Button AddButton(ActionProperties actionProperties)
    {
        Button button = SpawnButton();

        button.onClick.RemoveAllListeners();

        AddAction(button.GetComponent<RectTransform>(), actionProperties);

        return button;
    }

    public Button AddFormButton(ActionProperties actionProperties)
    {
        Button button = SpawnFormButton();

        button.onClick.RemoveAllListeners();

        AddAction(button.GetComponent<RectTransform>(), actionProperties);

        return button;
    }

    private void AddAction(RectTransform element, ActionProperties actionProperties)
    {
        if (element.GetComponent<ActionElement>() == null)
            element.gameObject.AddComponent<ActionElement>();

        ActionElement actionElement = element.GetComponent<ActionElement>();
        actionElement.SetElement(actionProperties);

        elements.Add(element);
    }

    public void CloseActions()
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

        Dropdown newComponent = Instantiate(Resources.Load<Dropdown>("UI/EditorDropdown"));

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

        Button newComponent = Instantiate(Resources.Load<Button>("UI/ActionButton"));

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

        Button newComponent = Instantiate(Resources.Load<Button>("UI/EditorFormButton"));

        formButtonPool.Add(newComponent);

        return newComponent;
    }
    #endregion
}
