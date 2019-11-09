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

    static public IDataElement getDataElement;

    static public void SelectElements()
    {
        List<Route> routeList = new List<Route>();

        foreach (EditorForm form in EditorManager.editorManager.forms)
        {
            if (!form.activeInPath) continue;

            foreach (EditorSection section in form.editorSections)
            {
                if (!section.active) continue;

                //For each section, add the target controller's route to a list so
                //that it can be used for finding the elements that should be selected
                routeList.Add(section.targetController.PathController.route);
            }
        }

        //CORRECT POSITION BEFORE SELECTING ELEMENT

        //SelectElement(routeList);
    }

    //static public List<Route> GetRouteList()
    //{
    //    List<Route> routeList = new List<Route>();

    //    foreach (EditorForm form in EditorManager.editorManager.forms)
    //    {
    //        foreach (EditorSection section in form.editorSections)
    //        {
    //            if (!section.active) continue;

    //            //For each section, add the target controller's route to a list so
    //            //that it can be used for finding the elements that should be selected
    //            routeList.Add(section.targetController.PathController.route);
    //        }
    //    }

    //    return routeList;
    //}
    
    static public void SelectElement(List<Route> routeList)
    {
        //Debug.Log(routeList.Count);

        //foreach (IDataElement dataElement in SelectionElementManager.dataElementPool)
        //{
        //    var selectionElement = dataElement.SelectionElement;

        //    foreach (Route route in routeList)
        //    {
        //        //Debug.Log(route.GeneralData.DataType + ", " + route.GeneralData.Id + ", " + route.selectionGroup);

        //        if (((GeneralData)dataElement).Equals(route.GeneralData) /*&&
        //            selectionElement.selectionGroup == route.selectionGroup*/ )
        //        {
        //            if (selectionElement.DisplayManager != null)
        //            {
        //                selectionElement.DisplayManager.SelectedElement = selectionElement;

        //                if (selectionElement.parent == null)
        //                    selectionElement.DisplayManager.CorrectPosition(selectionElement);
        //                else
        //                    selectionElement.DisplayManager.CorrectPosition(selectionElement.parent);
        //            }
        //            Debug.Log(route.GeneralData.DataType + ", " + route.GeneralData.Id + ", " + route.selectionGroup);
        //            //Debug.Log(dataElement.DataType + ", " + dataElement.Id  + ", " + selectionElement.selectionGroup);

        //            if (route.selectionGroup == Enums.SelectionGroup.Main)
        //                selectionElement.ActivateSelection();
        //            else
        //                selectionElement.child.ActivateSelection();

        //            break;
        //        }
        //    }
        //}
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

    static public void CancelSelection(Route route)
    {
        foreach (SelectionElement selectionElement in SelectionElementManager.elementPool.Where(x => x.gameObject.activeInHierarchy).Where(x => x.selected))
        {
            if (selectionElement.GeneralData.Equals(route.GeneralData))
            {
                if (selectionElement.DisplayManager != null)
                    selectionElement.DisplayManager.SelectedElement = null;

                selectionElement.CancelSelection();

                return;
            }          
        }              
    }

    static public void CancelGetSelection()
    {
        if (getDataElement == null) return;

        getDataElement.SelectionElement.CancelSelection();
        getDataElement = null;

        //Return to previous path in form
        EditorManager.editorManager.InitializePath(EditorManager.editorManager.forms[2].previousPath);  
    }
}
