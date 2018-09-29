public class Origin
{
    public SelectionManager.Property selectionProperty;
    public ListProperties.Type listType;
    public ListManager listManager;

    public Origin(){ }

    public Origin(SelectionElement element)
    {
        selectionProperty = element.selectionProperty;
        listType = element.listType;
        listManager = element.listManager;
    }

    public Origin(Origin selection)
    {
        selectionProperty = selection.selectionProperty;
        listType = selection.listType;
        listManager = selection.listManager;
    }

    public bool Equals(SelectionElement element)
    {
        if (element.selectionProperty != selectionProperty)
            return false;

        return true;
    }

    public Origin Copy()
    {
        return new Origin(this);
    }
}
