using UnityEngine;
using System.Collections;

public class SelectionGroup : MonoBehaviour
{
	public SelectionManager selectionManager;

	public SelectionElement selection { get; set; }

    public void SelectElement(SelectionElement new_selection)
    {
        if (new_selection.selectionType != Enums.SelectionType.None)
        {
            if (selection != new_selection)
            {
                ActivateSelection(new_selection);
            } else {
                DeactivateSelection();
            }
        } else {
            ActivateSelection(new_selection);
        }
    }
    
    public void ActivateSelection(SelectionElement new_selection)
    {
        DeactivateSelection();

        new_selection.ActivateElement();

        selection = new_selection;
    }

    public void DeactivateSelection()
    {
        if(selection != null)
        {
            selection.DeactivateElement();

            selection = null;
        }  
    }   
}
