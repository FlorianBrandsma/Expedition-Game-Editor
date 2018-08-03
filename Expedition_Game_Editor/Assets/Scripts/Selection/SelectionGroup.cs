using UnityEngine;
using System.Collections;

public class SelectionGroup : MonoBehaviour
{
    public SelectionGroup parentGroup;
    public SelectionElement selection { get; set; }

    //Field: Close element when closing field
    //Group: Only one selection available per group
    //Manager: Only pass on get/set elements
    //Open other elements here

    private bool selection_set;

    public void SelectElement(SelectionElement new_selection)
    {
        selection_set = false;

        if (parentGroup != null)
        {
            if (new_selection.selectionProperty != Enums.SelectionProperty.None)
                parentGroup.SelectElement(new_selection);
              else
                SetSelection(new_selection);         
        } else {
            SetSelection(new_selection);    
        }

        if(!selection_set)
            selection = new_selection;
    }

    public void SetSelection(SelectionElement new_selection)
    {
        //If new selection different from last
        if (selection != new_selection)
        {
            //If selected type is get and new selection type is set: give set values to get. Else activate new selection
            if (selection != null && selection.selectionProperty == Enums.SelectionProperty.Get && new_selection.selectionProperty == Enums.SelectionProperty.Set)
                SetElementValue(new_selection);
            else
                ActivateSelection(new_selection);
        } else {
            //Deactivate if selection is same as last
            Deactivate();
        }

        selection_set = true;
    }

    public void SetElementValue(SelectionElement new_selection)
    {
        if (selection != null)
        {
            selection.SetElement(new_selection);

            Deactivate();
        }
    }

    public void ActivateSelection(SelectionElement new_selection)
    {
        Deactivate();

        new_selection.ActivateElement();

        selection = new_selection;
    }

    public void Deactivate()
    {
        if (selection != null)
        {
            if(parentGroup != null)
                DeactivateGroup();
             else 
                DeactivateSelection();

            selection = null;
        }
    }

    public void DeactivateGroup()
    {
        if (parentGroup.selection == selection)
            parentGroup.Deactivate();
        else
            selection.DeactivateElement();
    }

    public void DeactivateSelection()
    {
        selection.DeactivateElement();
    }
}
