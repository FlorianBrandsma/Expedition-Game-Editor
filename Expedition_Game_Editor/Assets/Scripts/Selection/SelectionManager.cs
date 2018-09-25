using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Selection
{
    public ElementData data = new ElementData();
    public SelectionManager.Property property;
    public ListManager listManager;

    public Selection()
    {
        data        = new ElementData();
        property    = SelectionManager.Property.None;
        listManager = null;
    }

    public Selection(SelectionElement element)
    {
        data = element.data.Copy();
        property = element.selectionProperty;
        listManager = element.listManager;
    }

    public Selection(Selection selection)
    {
        data = selection.data.Copy();
        property = selection.property;
        listManager = selection.listManager;
    }

    public bool Equals(SelectionElement element)
    {
        if (!element.data.Equals(data))
            return false;

        if (element.selectionProperty != property)
            return false;

        return true;
    }

    public Selection Copy()
    {
        return new Selection(this);
    }
}

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

    static public List<Selection> edit_data = new List<Selection>();

    static public void SelectEdit(Selection selection)
    {
        if(selection.listManager != null)
            selection.listManager.SelectElement(selection);
    }

    static public void SelectGet()
    {

    }

    static public void SelectSet()
    {

    }

    static public void CancelSelection(Selection selection)
    {
        if (selection.listManager != null)
            selection.listManager.CancelSelection(selection);
    }
}
