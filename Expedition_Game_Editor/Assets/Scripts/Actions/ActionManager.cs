using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class ActionManager : MonoBehaviour
{
    static public ActionManager instance;

    private List<ActionElement> elementList = new List<ActionElement>();

    public RectTransform element_parent;
    public RectTransform main_content;
    public ScrollRect scrollRect;
    public RectTransform main_parent;

    public RawImage left_arrow, right_arrow;
    public Texture arrow_active, arrow_inactive;

    private void Awake()
    {
        instance = this;

        CloseSlider();
    }

    public void SortActions()
    {
        if (elementList.Count == 0) return;

        List<ActionElement> leftElements = new List<ActionElement>();
        List<ActionElement> rightElements = new List<ActionElement>();
        List<ActionElement> mainElements = new List<ActionElement>();

        float leftOffset = 0;
        float rightOffset = 0;
        float mainSize = 0;

        //Sort element by anchor
        foreach (ActionElement element in elementList)
        {
            var rectTransform = element.GetComponent<RectTransform>();

            rectTransform.sizeDelta = new Vector2(element.ActionProperties.width + 5, rectTransform.sizeDelta.y);

            switch (element.ActionProperties.anchor)
            {
                case ActionProperties.Anchor.Left:

                    element.transform.SetParent(element_parent, false);
                    leftOffset += element.ActionProperties.width;

                    leftElements.Add(element);
                    
                    break;

                case ActionProperties.Anchor.Right:

                    element.transform.SetParent(element_parent, false);
                    rightOffset += element.ActionProperties.width;

                    rightElements.Add(element);
                    
                    break;

                case ActionProperties.Anchor.Main:

                    element.transform.SetParent(main_parent, false);
                    mainSize += element.ActionProperties.width;

                    mainElements.Add(element);

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
        main_content.gameObject.SetActive(elements.Count > 0);
        
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

    public ExDropdown AddDropdown(ActionProperties actionProperties)
    {
        var dropdown = SpawnDropdown();

        if (dropdown.GetComponent<IDataController>() != null)
            DestroyImmediate((UnityEngine.Object)dropdown.GetComponent<IDataController>());

        dropdown.Dropdown.options.Clear();
        dropdown.Dropdown.value = 0;
        dropdown.Dropdown.onValueChanged.RemoveAllListeners();

        AddAction(dropdown.GetComponent<RectTransform>(), actionProperties);

        return dropdown;
    }

    public ExButton AddButton(ActionProperties actionProperties)
    {
        var button = SpawnButton();

        button.Button.onClick.RemoveAllListeners();

        AddAction(button.GetComponent<RectTransform>(), actionProperties);

        return button;
    }

    public ExButton AddFormButton(ActionProperties actionProperties)
    {
        var button = SpawnFormButton();

        button.Button.onClick.RemoveAllListeners();

        AddAction(button.GetComponent<RectTransform>(), actionProperties);

        return button;
    }

    public ExInputNumber AddInputNumber(ActionProperties actionProperties)
    {
        var inputNumber = SpawnInputNumber();

        inputNumber.minusButton.onClick.RemoveAllListeners();
        inputNumber.plusButton.onClick.RemoveAllListeners();
        inputNumber.inputField.onEndEdit.RemoveAllListeners();

        AddAction(inputNumber.GetComponent<RectTransform>(), actionProperties);

        return inputNumber;
    }

    private void AddAction(RectTransform element, ActionProperties actionProperties)
    {
        if (element.GetComponent<ActionElement>() == null)
            element.gameObject.AddComponent<ActionElement>();

        ActionElement actionElement = element.GetComponent<ActionElement>();
        actionElement.SetElement(actionProperties);

        elementList.Add(actionElement);
    }

    public void CloseActions()
    {
        CloseSlider();

        main_content.offsetMin = new Vector2(0, main_content.offsetMin.y);
        main_content.offsetMax = new Vector2(0, main_content.offsetMax.y);

        main_parent.offsetMin = new Vector2(0, main_parent.offsetMin.y);
        main_parent.offsetMax = new Vector2(0, main_parent.offsetMax.y);

        elementList.ForEach(x => PoolManager.ClosePoolObject(x.GetComponent<IPoolable>()));

        elementList.Clear();
    }

    private void CloseSlider()
    {
        scrollRect.horizontalNormalizedPosition = 1f;

        scrollRect.enabled = false;

        SetArrow(left_arrow, false);
        SetArrow(right_arrow, false);
    }

    private void SetArrow(RawImage arrow, bool active)
    {
        arrow.texture = active ? arrow_active : arrow_inactive;
    }

    #region Spawners
    private ExDropdown SpawnDropdown()
    {
        var prefab = Resources.Load<ExDropdown>("Elements/UI/Dropdown");
        var dropdown = (ExDropdown)PoolManager.SpawnObject(prefab);

        dropdown.gameObject.SetActive(true);

        return dropdown;
    }

    private ExButton SpawnButton()
    {
        var prefab = Resources.Load<ExButton>("Elements/UI/ActionButton");
        var button = (ExButton)PoolManager.SpawnObject(prefab);

        button.gameObject.SetActive(true);

        return button;
    }

    private ExButton SpawnFormButton()
    {
        var prefab = Resources.Load<ExButton>("Elements/UI/FormButton");
        var button = (ExButton)PoolManager.SpawnObject(prefab);

        button.gameObject.SetActive(true);

        return button;
    }

    private ExInputNumber SpawnInputNumber()
    {
        var prefab = Resources.Load<ExInputNumber>("Elements/UI/InputNumber");
        var inputNumber = (ExInputNumber)PoolManager.SpawnObject(prefab);

        inputNumber.gameObject.SetActive(true);

        return inputNumber;
    }
    #endregion
}
