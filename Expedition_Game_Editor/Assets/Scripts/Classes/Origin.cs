public class Origin
{
    public SelectionManager.Property selectionProperty;
    public DisplayManager.Type displayType;
    public SelectionElement selectionElement;

    public Origin(){ }

    public Origin(SelectionElement element)
    {
        selectionProperty = element.selectionProperty;
        displayType = element.displayType;
        selectionElement = element;
    }

    public Origin(Origin selection)
    {
        selectionProperty = selection.selectionProperty;
        displayType = selection.displayType;
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
