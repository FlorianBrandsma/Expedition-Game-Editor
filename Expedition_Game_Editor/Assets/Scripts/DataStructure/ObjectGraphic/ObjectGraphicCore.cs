using UnityEngine;
using System.Collections;

public class ObjectGraphicCore : GeneralData
{
    private string _name;
    private string _icon;

    public string original_name;
    public string original_icon;

    public bool changed;
    private bool changed_name;
    private bool changed_icon;

    #region Properties

    public string name
    {
        get { return _name; }
        set
        {
            if (value == _name) return;

            changed = true;
            changed_name = true;

            _name = value;
        }
    }

    public string icon
    {
        get { return _icon; }
        set
        {
            if (value == _icon) return;

            changed = true;
            changed_icon = true;

            _icon = value;
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
        original_name = name;
        original_icon = icon;

        ClearChanges();
    }

    public void GetOriginalValues()
    {
        name = original_name;
        icon = original_icon;
    }

    public void ClearChanges()
    {
        GetOriginalValues();

        changed = false;
        changed_name = false;
        changed_icon = false;
    }

    public void Delete()
    {

    }

    #endregion
}
