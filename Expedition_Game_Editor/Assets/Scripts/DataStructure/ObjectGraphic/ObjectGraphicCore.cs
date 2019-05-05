using UnityEngine;
using System.Collections;

public class ObjectGraphicCore : GeneralData
{
    private string name;
    private string icon;

    public string originalName;
    public string originalIcon;

    public bool changed;
    private bool changedName;
    private bool changedIcon;

    #region Properties

    public string Name
    {
        get { return name; }
        set
        {
            if (value == name) return;

            changed = true;
            changedName = true;

            name = value;
        }
    }

    public string Icon
    {
        get { return icon; }
        set
        {
            if (value == icon) return;

            changed = true;
            changedIcon = true;

            icon = value;
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

    public void SetOriginalValues()
    {
        originalName = name;
        originalIcon = icon;

        ClearChanges();
    }

    public void GetOriginalValues()
    {
        name = originalName;
        icon = originalIcon;
    }

    public void ClearChanges()
    {
        GetOriginalValues();

        changed = false;
        changedName = false;
        changedIcon = false;
    }

    public void Delete()
    {

    }

    #endregion
}
