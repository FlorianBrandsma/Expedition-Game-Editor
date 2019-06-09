using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

static public class SelectionElementManager
{
    static public List<SelectionElement> elementPool = new List<SelectionElement>();

    static public SelectionElement SpawnElement(SelectionElement elementPrefab, Enums.ElementType elementType, 
                                                ListManager listManager, SelectionManager.Type selectionType, SelectionManager.Property selectionProperty, Transform parent)
    {
        foreach (SelectionElement element in elementPool)
        {
            if (!element.disableSpawn && !element.gameObject.activeInHierarchy && element.elementType == elementType)
            {
                InitializeElement(element, listManager, selectionType, selectionProperty, parent);
                return element;
            }
        }

        SelectionElement newElement = Object.Instantiate(elementPrefab);
        newElement.elementType = elementType;

        InitializeElement(newElement, listManager, selectionType, selectionProperty, parent);

        elementPool.Add(newElement);

        if (newElement.child != null)
            elementPool.Add(newElement.child);

        return newElement;
    }

    static public void InitializeElement(SelectionElement element, ListManager listManager, SelectionManager.Type selectionType, SelectionManager.Property selectionProperty, Transform parent)
    {
        element.InitializeElement(listManager, selectionType, selectionProperty);

        element.transform.SetParent(parent, false);
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
