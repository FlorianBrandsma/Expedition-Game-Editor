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
        Toggle,
        OpenOutcomeEditor,
        LoadGameSave
    }

    static public List<Route> routeList = new List<Route>();

    static public IDataElement getDataElement;

    static public void GetRouteList()
    {
        routeList.Clear();

        foreach (EditorForm form in RenderManager.layoutManager.forms)
        {
            foreach (LayoutSection section in form.editorSections)
            {
                if (!section.Active) continue;

                //For each section, add the target controller's route to a list so
                //that it can be used for finding the elements that should be selected
                routeList.Add(section.TargetController.PathController.route);
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
        
        //First set the result to all relevant elements
        dataElementList.ForEach(x => x.SelectionElement.SetResult(setDataElement));

        //Cancelling selection will re-render the editor and visualize the selection
        CancelGetSelection();
    }
    
    static public void CancelGetSelection()
    {
        if (getDataElement == null) return;

        CancelSelection(new List<IDataElement>() { getDataElement });

        getDataElement = null;

        //Return to previous path in form
        RenderManager.Render(RenderManager.layoutManager.forms[2].previousPath);  
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
