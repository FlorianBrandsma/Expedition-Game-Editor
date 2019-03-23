using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
        Open
    }

    static public List<ListManager> lists = new List<ListManager>();

    static public SelectionElement get;

    static public void SelectElements()
    {
        foreach(EditorForm form in EditorManager.editorManager.forms)
        {
            if (!form.active) continue;

            foreach (EditorSection section in form.editor_sections)
            {
                if (!section.active) continue;

                SelectEdit(section.target_controller.pathController.route);
            }       
        }
    }

    static public void ResetSelection(ListManager listManager)
    {
        listManager.ResetSelection();
    }

    static public void SelectEdit(Route route)
    {
        foreach(ListManager list in lists)
        {
            Property property = list.listProperties.selectionProperty;

            if(property == Property.Edit || property == Property.Enter)
                list.SelectElement(route);
        }           
    }

    static public void SelectGet(SelectionElement new_get)
    {
        get = new_get;
    }

    static public void SelectSet(SelectionElement new_set)
    {
        get.data = new_set.data;//.Copy();

        CancelGetSelection();
    }

    static public void CancelSelection(Route route)
    {
        foreach (ListManager list in lists)
            list.CancelSelection(route);
    }

    static public void CancelGetSelection()
    {
        if (get == null) return;

        get.CancelSelection();
        get = null;

        //Reset
        EditorManager.editorManager.OpenPath(new PathManager.Form(EditorManager.editorManager.forms[2]).Initialize());  
    }
}
