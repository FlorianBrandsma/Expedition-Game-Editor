using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

static public class SelectionManager
{
    public enum Type
    {
        None,
        Select,
        Automatic,
    }

    public enum Property
    {
        None,
        Get,
        Set,
        Edit,
        Enter,
        Open,
        Toggle
    }

    static public List<Route> routeList = new List<Route>();

    static public IDataElement getDataElement;

    static public void GetRouteList()
    {
        routeList.Clear();

        foreach (EditorForm form in EditorManager.editorManager.forms)
        {
            foreach (EditorSection section in form.editorSections)
            {
                if (!section.active) continue;

                //For each section, add the target controller's route to a list so
                //that it can be used for finding the elements that should be selected
                routeList.Add(section.targetController.PathController.route);
            }
        }
    }

    static public void SelectData(List<IDataElement> dataList, IDisplayManager displayManager = null)
    {
        if (dataList.Count == 0) return;

        foreach (IDataElement dataElement in dataList)
        {
            foreach (Route route in routeList)
            {
                if (((GeneralData)dataElement).Equals(route.GeneralData))
                {

                    if (dataElement.SelectionStatus == Enums.SelectionStatus.None)
                        dataElement.SelectionStatus = route.selectionStatus;
                    else
                        dataElement.SelectionStatus = Enums.SelectionStatus.Both;

                    if (displayManager != null)
                        displayManager.CorrectPosition(dataElement);                 
                }
            }
        }
    }

    static public void SelectSearch(IDataElement selectedDataElement)
    {
        getDataElement = selectedDataElement;
    }

    static public void SelectSet(IDataElement setDataElement)
    {
        var dataElementList = SelectionElementManager.FindDataElements((GeneralData)getDataElement);
        
        CancelGetSelection();

        dataElementList.ForEach(x => x.SelectionElement.SetResult(setDataElement));
    }
    
    static public void CancelGetSelection()
    {
        if (getDataElement == null) return;

        CancelSelection(new List<IDataElement>() { getDataElement });

        getDataElement = null;

        //Return to previous path in form
        EditorManager.editorManager.InitializePath(EditorManager.editorManager.forms[2].previousPath);  
    }

    static public void CancelSelection(List<IDataElement> dataList)
    {
        dataList.ForEach(x => 
        {
            if(x.SelectionElement != null)
                x.SelectionElement.CancelSelection();
            else
                x.SelectionStatus = Enums.SelectionStatus.None;
        });
    }
}
