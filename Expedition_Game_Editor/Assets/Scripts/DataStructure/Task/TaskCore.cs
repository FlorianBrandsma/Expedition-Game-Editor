using UnityEngine;
using System.Collections;

public class TaskCore : GeneralData
{
    private string description;

    public int originalIndex;
    public string originalDescription;

    public bool changed;
    private bool changedIndex;
    private bool changedDescription;

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

    public string Description
    {
        get { return description; }
        set
        {
            if (value == description) return;

            changed = true;
            changedDescription = true;

            description = value;
        }
    }

    #endregion

    #region Methods

    public void Create()
    {

    }

    public void Update()
    {
        if (!changed) return;

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
        originalDescription = description;

        ClearChanges();
    }

    public void GetOriginalValues()
    {
        description = originalDescription;
    }

    public void ClearChanges()
    {
        GetOriginalValues();

        changed = false;
        changedIndex = false;
        changedDescription = false;
    }

    public void Delete()
    {

    }

    #endregion
}
