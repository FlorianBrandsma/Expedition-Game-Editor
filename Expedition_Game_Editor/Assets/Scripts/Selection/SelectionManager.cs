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
            if (!form.active) continue;

            foreach (EditorSection section in form.editorSections)
            {
                if (!section.active) continue;

                routeList.Add(section.targetController.PathController.route);
            }
        }

        SelectElement(routeList);
    }

    static public void SelectElement(List<Route> routeList)
    {
        foreach (SelectionElement selectionElement in SelectionElementManager.elementPool.Where(x => x.gameObject.activeInHierarchy))
        {
            if (selectionElement.selectionProperty == Property.Set) continue;

            foreach (Route route in routeList)
            {
                //Should a selection rely on a type, rather than a property, to pick an element?
                //Mainly concerns elements with children that otherwise have the same data
                if (selectionElement.GeneralData().Equals(route.GeneralData()) &&
                    selectionElement.selectionGroup == route.selectionGroup)
                {
                    selectionElement.ActivateSelection();

                    if (selectionElement.ListManager != null)
                    {
                        selectionElement.ListManager.selectedElement = selectionElement;

                        if (selectionElement.parent == null)
                            selectionElement.ListManager.CorrectPosition(selectionElement);
                        else
                            selectionElement.ListManager.CorrectPosition(selectionElement.parent);
                    }

                    break;
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
        getDataElement.SelectionElement.SetResult(setDataElement.SelectionElement.route.data);

        CancelGetSelection();
    }

    static public void CancelSelection(Route route)
    {
        //Closing only one route may cause problems
        foreach (SelectionElement selectionElement in SelectionElementManager.elementPool.Where(x => x.gameObject.activeInHierarchy).Where(x => x.selected))
        {
            if (selectionElement.GeneralData().Equals(route.GeneralData()))
            {
                if (selectionElement.ListManager != null)
                    selectionElement.ListManager.selectedElement = null;

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
