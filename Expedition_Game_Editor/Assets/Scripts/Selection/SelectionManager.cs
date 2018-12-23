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

    static public void SelectSet(SelectionElement new_set)
    {
        get.data = new_set.data.Copy();

        CancelGetSelection();
    }

    static public void CancelSelection(Route route)
    {
        route.origin.listManager.CancelSelection(route);
    }

    static public void CancelGetSelection()
    {
        if (get == null) return;

        get.CancelSelection();
        get = null;

        //Reset this though
        //EditorManager.editorManager.OpenPath(new PathManager.Secondary().Initialize());  
    }
}
