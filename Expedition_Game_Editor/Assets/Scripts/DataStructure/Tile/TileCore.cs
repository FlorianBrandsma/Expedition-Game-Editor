using UnityEngine;
using System.Collections;

public class TileCore : GeneralData
{
    public int originalIndex;

    private bool changedIndex;

    public bool Changed
    {
        get
        {
            return false;
        }
    }

    #region Properties

    public int Index
    {
        get { return index; }
        set
        {
            if (value == index) return;

            changedIndex = true;

            index = value;
        }
    }

    #endregion

    #region Methods

    public void Create()
    {

    }

    public void Update()
    {
        if (!Changed) return;

        //Debug.Log("Updated " + name);

        //if (changed_id)             return;
        //if (changed_table)          return;
        //if (changed_type)           return;
        //if (changed_index)          return;
        //if (changed_name)           return;
        //if (changed_description)    return;

        SetOriginalValues();
    }

    public void UpdateIndex()
    {
        if (changedIndex)
        {
            //Debug.Log("Update index " + index);
            changedIndex = false;
        }
    }

    public void SetOriginalValues()
    {
        ClearChanges();
    }

    public void GetOriginalValues()
    {

    }

    public void ClearChanges()
    {
        GetOriginalValues();

        changedIndex = false;
    }

    public void Delete()
    {

    }

    #endregion
}
