using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

static public class SelectionElementManager
{
    static public List<SelectionElement> elementPool = new List<SelectionElement>();
    
    static public void InitializeElement(SelectionElement element, Transform parent, IDisplayManager displayManager, SelectionManager.Type selectionType, SelectionManager.Property selectionProperty)
    {
        element.InitializeElement(displayManager, selectionType, selectionProperty);

        element.transform.SetParent(parent, false);
    }

    static public void Add(SelectionElement selectionElement)
    {
        elementPool.Add(selectionElement);

        if (selectionElement.child != null)
            Add(selectionElement.child);
    }

    //Reload the display of active elements that match the argument
    static public void UpdateElements(IDataElement dataElement)
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
            element.CloseElement();

        elementList.Clear();
    }
}
