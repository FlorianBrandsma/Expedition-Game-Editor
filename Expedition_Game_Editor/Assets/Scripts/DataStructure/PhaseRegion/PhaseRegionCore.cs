using UnityEngine;
using System.Collections;

public class PhaseRegionCore : GeneralData
{
    private string name;

    public int originalIndex;
    public string originalName;

    private bool changedIndex;
    private bool changedName;

    public bool Changed
    {
        get
        {
            return changedName;
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

    public string Name
    {
        get { return name; }
        set
        {
            if (value == name) return;

            changedName = (value != originalName);

            name = value;
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
        originalName = name;

        ClearChanges();
    }

    public void GetOriginalValues()
    {
        name = originalName;
    }

    public void ClearChanges()
    {
        GetOriginalValues();

        changedIndex = false;
        changedName = false;
    }

    public void Delete()
    {

    }

    #endregion
}
