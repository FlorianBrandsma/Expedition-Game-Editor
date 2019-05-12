using UnityEngine;
using System.Collections;

public class ChapterElementCore : GeneralData
{
    private int elementId;

    public int originalElementId;

    public bool changed;
    private bool changedElementId;

    #region Properties

    public int ElementId
    {
        get { return elementId; }
        set
        {
            if (value == elementId) return;

            changed = true;
            changedElementId = true;

            elementId = value;
        }
    }

    #endregion

    #region Methods

    public void Create()
    {

    }

    public virtual void Update()
    {
        //if (!changed) return;

        //Debug.Log("Updated " + name);

        //if (changed_id)             return;
        //if (changed_table)          return;
        //if (changed_type)           return;
        //if (changed_index)          return;
        //if (changed_name)           return;
        //if (changed_description)    return;

        //SetOriginalValues();
    }

    public void UpdateIndex() { }

    public virtual void SetOriginalValues()
    {
        originalElementId = ElementId;

        //ClearChanges();
    }

    public void GetOriginalValues()
    {
        ElementId = originalElementId;
    }

    public virtual void ClearChanges()
    {
        GetOriginalValues();

        changed = false;
        changedElementId = false;
    }

    public void Delete()
    {

    }

    #endregion
}
