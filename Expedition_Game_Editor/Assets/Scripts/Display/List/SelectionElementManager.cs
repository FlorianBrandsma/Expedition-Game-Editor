using UnityEngine;
using System.Collections.Generic;
using System.Linq;

static public class SelectionElementManager
{
    static public List<EditorElement> elementPool = new List<EditorElement>();
    
    static public void InitializeElement(DataElement dataElement, Transform parent, IDisplayManager displayManager, 
                                         SelectionManager.Type selectionType, SelectionManager.Property selectionProperty, SelectionManager.Property addProperty, bool uniqueSelection)
    {
        dataElement.InitializeElement(displayManager, selectionType, selectionProperty, addProperty, uniqueSelection);

        dataElement.transform.SetParent(parent, false);
    }

    static public void Add(EditorElement element)
    {
        elementPool.Add(element);

        if (element.child != null)
            Add(element.child);
    }

    static public void Remove(EditorElement element)
    {
        elementPool.Remove(element);

        if (element.child != null)
            Remove(element.child);
    }

    //Reload the display of active elements that match the argument. Exclusively used by index switching
    static public void UpdateElements(IElementData elementData)
    {
        var activeElements = elementPool.Where(x => x.gameObject.activeInHierarchy).ToList();

        var elementList = FindSelectionElements(activeElements, elementData);

        var managerList = elementList.Select(x => x.DataElement.DisplayManager).Distinct().ToList();

        managerList.ForEach(x =>
        {
            x.UpdateData();
            x.CorrectPosition(elementData, x.Display.DataController.Data.dataList);
        });
    }
    
    static public List<IElementData> FindElementData(IElementData elementData)
    {
        if (elementData == null) return new List<IElementData>();

        var activeSelectionElements = FindSelectionElements(elementPool.Where(x => x.gameObject.activeInHierarchy).ToList(), elementData);
        var elementDataList = activeSelectionElements.Select(x => x.DataElement.ElementData).Distinct().ToList();

        return elementDataList;
    }

    static public List<EditorElement> FindSelectionElements(IElementData elementData)
    {
        var elementList = elementPool.Where(x => x.gameObject.activeInHierarchy).ToList();

        return FindSelectionElements(elementList, elementData);
    }

    static public List<EditorElement> FindSelectionElements(List<EditorElement> elementList, IElementData elementData)
    {
        var resultList = elementList.Where(x => x.selectionProperty != SelectionManager.Property.Set &&
                                                x.selectionStatus == Enums.SelectionStatus.Main &&
                                                x.DataElement.ElementData != null &&
                                                DataManager.Equals(x.DataElement.ElementData, elementData)).ToList();

        //Only return the source's element 
        if (elementData.UniqueSelection)
            resultList = resultList.Where(x => x.DataElement == elementData.DataElement).ToList();

        return resultList;
    }

    static public void CloseElement(DataElement dataElement)
    {
        var editorElement = (EditorElement)dataElement.SelectionElement;

        CloseElement(editorElement);
    }

    static public void CloseElement(EditorElement editorElement)
    {
        CloseElement(new List<EditorElement>() { editorElement });
    }

    static public void CloseElement(List<EditorElement> elementList)
    {
        foreach (EditorElement element in elementList)
        {
            if (element.child != null && element.child.isActiveAndEnabled)
                CloseElement(element.child);

            element.CloseElement();
        }

        elementList.Clear();
    }

    static public bool SelectionActive(DataElement dataElement)
    {
        return (dataElement != null && dataElement.gameObject.activeInHierarchy);
    }
}
