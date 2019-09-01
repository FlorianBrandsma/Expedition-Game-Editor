﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

static public class SelectionElementManager
{
    static public List<SelectionElement> elementPool = new List<SelectionElement>();

    static public SelectionElement SpawnElement(SelectionElement elementPrefab, Enums.ElementType elementType, 
                                                IDisplayManager displayManager, SelectionManager.Type selectionType, 
                                                SelectionManager.Property selectionProperty, Transform parent)
    {
        foreach (SelectionElement element in elementPool)
        {
            if (!element.disableSpawn && !element.gameObject.activeInHierarchy && element.elementType == elementType)
            {
                InitializeElement(element, displayManager, selectionType, selectionProperty, parent);
                return element;
            }
        }

        SelectionElement newElement = Object.Instantiate(elementPrefab);
        newElement.elementType = elementType;

        InitializeElement(newElement, displayManager, selectionType, selectionProperty, parent);

        Add(newElement);

        if (newElement.child != null)
            Add(newElement.child);

        return newElement;
    }

    static public void Add(SelectionElement selectionElement)
    {
        if(selectionElement.selectionType != SelectionManager.Type.None)
            elementPool.Add(selectionElement);
    }

    static public void InitializeElement(SelectionElement element, IDisplayManager displayManager, SelectionManager.Type selectionType, SelectionManager.Property selectionProperty, Transform parent)
    {
        element.InitializeElement(displayManager, selectionType, selectionProperty);

        element.transform.SetParent(parent, false);
    }

    static public void UpdateElements(GeneralData generalData, bool updateList = false)
    {
        var activeElements = elementPool.Where(x => x.gameObject.activeInHierarchy).ToList();

        var elementDataList = activeElements.Where(x => x.selectionGroup == Enums.SelectionGroup.Main &&                                                   
                                                        x.GeneralData() != null)
                                            .Where(x => x.GeneralData().Equals(generalData)).ToList();

        var managerList = elementDataList.Select(x => x.DisplayManager).Distinct().ToList();

        if (updateList)
            managerList.ForEach(x => x.UpdateData());
        else
            elementDataList.ForEach(x => x.UpdateElement());

        SelectionManager.SelectElements();
    }

    static public void CloseElement(List<SelectionElement> elementList)
    {
        foreach (SelectionElement element in elementList)
        {
            element.GetComponent<IElement>().CloseElement();
            element.GetComponent<Button>().onClick.RemoveAllListeners();

            if (element.elementStatus != Enums.ElementStatus.Enabled)
            {
                element.elementStatus = Enums.ElementStatus.Enabled;
                element.background.color = element.enabledColor;
                element.lockIcon.SetActive(false);
            }

            if (element.child != null)
            {
                element.child.gameObject.SetActive(false);
                element.child.GetComponent<Button>().onClick.RemoveAllListeners();
            }

            element.gameObject.SetActive(false);
        }

        elementList.Clear();
    }
}
