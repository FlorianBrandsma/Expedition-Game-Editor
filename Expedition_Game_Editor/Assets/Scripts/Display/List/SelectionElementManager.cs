using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

static public class SelectionElementManager
{
    static public List<SelectionElement> elementPool = new List<SelectionElement>();
    
    static public SelectionElement SpawnElement(SelectionElement elementPrefab, Transform parent,
                                                Enums.ElementType elementType, IDisplayManager displayManager, 
                                                SelectionManager.Type selectionType, SelectionManager.Property selectionProperty)
    {
        foreach (SelectionElement element in elementPool.Where(x => x.elementType == elementType))
        {
            if (element.disableSpawn) continue;
            if (element.gameObject.activeInHierarchy) continue;

            if (!element.disableSpawn && !element.gameObject.activeInHierarchy)
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

    static public void UpdateElements(IDataElement dataElement, bool updateList = false)
    {
        var activeElements = elementPool.Where(x => x.gameObject.activeInHierarchy).ToList();

        var elementList = FindSelectionElements(activeElements, (GeneralData)dataElement);

        var managerList = elementList.Select(x => x.DisplayManager).Distinct().ToList();

        managerList.ForEach(x =>
        {
            x.UpdateData();
            x.CorrectPosition(dataElement);
        });
    }
    
    static public List<IDataElement> FindDataElements(GeneralData generalData)
    {
        var dataElementList = FindSelectionElements(elementPool.Where(x => x.gameObject.activeInHierarchy).ToList(), generalData)
                                                   .Select(x => x.data.dataElement).Distinct().ToList();

        return dataElementList;
    }

    static public List<SelectionElement> FindSelectionElements(GeneralData generalData)
    {
        var elementList = elementPool.Where(x => x.gameObject.activeInHierarchy).ToList();

        return FindSelectionElements(elementList, generalData);
    }

    static public List<SelectionElement> FindSelectionElements(List<SelectionElement> selectionElements, GeneralData generalData)
    {
        return selectionElements.Where(x => x.selectionStatus == Enums.SelectionStatus.Main &&
                                                        x.GeneralData != null)
                                            .Where(x => x.GeneralData.Equals(generalData)).ToList();
    }

    static public void CloseElement(List<SelectionElement> elementList)
    {
        foreach (SelectionElement element in elementList)
        {
            element.CloseElement();
            element.GetComponent<IElement>().CloseElement();
            element.OnSelection.RemoveAllListeners();

            if (element.elementStatus != Enums.ElementStatus.Enabled)
            {
                element.elementStatus = Enums.ElementStatus.Enabled;
                element.lockIcon.SetActive(false);
            }

            if (element.child != null)
            {
                element.child.gameObject.SetActive(false);
                element.child.OnSelection.RemoveAllListeners();
            }

            element.gameObject.SetActive(false);
        }

        elementList.Clear();
    }
}
