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
        Open,
    }

    static public List<Origin> edit_data = new List<Origin>();

    static public SelectionElement get;

    static public void SelectEdit(Route route)
    {
        route.origin.listManager.SelectElement(route);
    }

    static public void SelectGet(SelectionElement new_get)
    {
        get = new_get;
    }

    static public void SelectSet() { }

    static public void CancelSelection(Route route)
    {
        route.origin.listManager.CancelSelection(route);
    }

    static public void CancelGetSelection()
    {
        if(get != null)
        {
            get.CancelSelection();
            get = null;
        } 
    }
}
