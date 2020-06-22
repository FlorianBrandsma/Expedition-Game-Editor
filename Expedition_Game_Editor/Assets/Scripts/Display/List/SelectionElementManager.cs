using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

static public class SelectionElementManager
{
    static public List<EditorElement> elementPool = new List<EditorElement>();
    
    static public void InitializeElement(DataElement dataElement, Transform parent, IDisplayManager displayManager, SelectionManager.Type selectionType, SelectionManager.Property selectionProperty)
    {
        dataElement.InitializeElement(displayManager, selectionType, selectionProperty);

        dataElement.transform.SetParent(parent, false);
    }

    static public void Add(EditorElement element)
    {
        elementPool.Add(element);

        if (element.child != null)
            Add(element.child);
    }

    //Reload the display of active elements that match the argument
    static public void UpdateElements(IDataElement dataElement)
    {
        var activeElements = elementPool.Where(x => x.gameObject.activeInHierarchy).ToList();

        var elementList = FindSelectionElements(activeElements, (GeneralData)dataElement);

        var managerList = elementList.Select(x => x.DataElement.DisplayManager).Distinct().ToList();

        managerList.ForEach(x =>
        {
            x.UpdateData();
            x.CorrectPosition(dataElement);
        });
    }
    
    static public List<IDataElement> FindDataElements(GeneralData generalData)
    {
        var dataElementList = FindSelectionElements(elementPool.Where(x => x.gameObject.activeInHierarchy).ToList(), generalData)
                                                   .Select(x => x.DataElement.data.dataElement).Distinct().ToList();

        return dataElementList;
    }

    static public List<EditorElement> FindSelectionElements(GeneralData generalData)
    {
        var elementList = elementPool.Where(x => x.gameObject.activeInHierarchy).ToList();

        return FindSelectionElements(elementList, generalData);
    }

    static public List<EditorElement> FindSelectionElements(List<EditorElement> elementList, GeneralData generalData)
    {
        return elementList.Where(x => x.selectionStatus == Enums.SelectionStatus.Main &&
                                                        x.DataElement.GeneralData != null)
                                            .Where(x => x.DataElement.GeneralData.Equals(generalData)).ToList();
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
            if (element.child != null)
                CloseElement(element.child);

            element.CloseElement();
            //elementPool.Remove(element);
        }

        elementList.Clear();
    }

    static public bool SelectionActive(DataElement selectionElement)
    {
        return (selectionElement != null && selectionElement.gameObject.activeInHierarchy);
    }
}
