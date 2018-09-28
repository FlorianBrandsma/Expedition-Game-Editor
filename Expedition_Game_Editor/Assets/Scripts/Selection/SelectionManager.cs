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
        Open,
    }

    static public List<Origin> edit_data = new List<Origin>();

    static public void SelectEdit(Route route)
    {
        if(route.origin.listManager != null)
            route.origin.listManager.SelectElement(route.data);
    }

    static public void SelectGet()
    {

    }

    static public void SelectSet()
    {

    }

    static public void CancelSelection(Route route)
    {
        if (route.origin.listManager != null)
            route.origin.listManager.CancelSelection(route.data);
    }
}
