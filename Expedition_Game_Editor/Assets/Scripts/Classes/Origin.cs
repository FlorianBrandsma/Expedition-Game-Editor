using UnityEngine;
using System.Collections;

public class Origin
{
    public SelectionManager.Property property;
    public ListManager listManager;

    public Origin()
    {
        property = SelectionManager.Property.None;
        listManager = null;
    }

    public Origin(SelectionElement element)
    {
        property = element.selectionProperty;
        listManager = element.listManager;
    }

    public Origin(Origin selection)
    {
        property = selection.property;
        listManager = selection.listManager;
    }

    public bool Equals(SelectionElement element)
    {
        if (element.selectionProperty != property)
            return false;

        return true;
    }

    public Origin Copy()
    {
        return new Origin(this);
    }
}
