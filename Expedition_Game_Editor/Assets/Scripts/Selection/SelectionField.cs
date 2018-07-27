using UnityEngine;
using System.Collections;

public class SelectionField : MonoBehaviour
{
    public SelectionGroup selectionGroup;

    public SelectionElement selection { get; set; }

    public void SelectElement(SelectionElement new_selection)
    {
        if (selectionGroup != null)
            selectionGroup.SelectElement(new_selection);

        selection = new_selection;
    }

    public void DeactivateSelection()
    {
        if(selection != null)
        {
            if (selectionGroup.selection == selection)
                selectionGroup.DeactivateSelection();
            else
                selection.DeactivateElement();

            selection = null;
        }    
    }
}
