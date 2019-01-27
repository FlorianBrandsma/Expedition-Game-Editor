public class Origin
{
    public SelectionManager.Property selectionProperty;
    public ListProperties.Type listType;
    public SelectionElement selectionElement;

    public Origin(){ }

    public Origin(SelectionElement element)
    {
        selectionProperty = element.selectionProperty;
        listType = element.listType;
        selectionElement = element;
    }

    public Origin(Origin selection)
    {
        selectionProperty = selection.selectionProperty;
        listType = selection.listType;
        selectionElement = selection.selectionElement;
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
