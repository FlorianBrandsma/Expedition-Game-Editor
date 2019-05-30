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

    static public List<ListManager> lists = new List<ListManager>();

    static public IDataElement getDataElement;

    static public void SelectElements()
    {
        foreach(EditorForm form in EditorManager.editorManager.forms)
        {
            if (!form.active) continue;
            
            foreach (EditorSection section in form.editorSections)
            {
                if (!section.active) continue;
                
                SelectElement(section.targetController.pathController.route);
            }       
        }
    }

    static public void ResetSelection(ListManager listManager)
    {
        listManager.ResetSelection();
    }

    static public void SelectElement(Route route)
    {
        foreach (ListManager list in lists)
        {
            Property property = list.listProperties.selectionProperty;

            list.SelectElement(route);
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
        foreach (ListManager list in lists)
            list.CancelSelection(route);                 
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
