using UnityEngine;
using System.Collections;

public class QuestCore : GeneralData
{
    private int _index;
    private string _name;

    public int original_index;
    public string original_name;

    public bool changed;
    private bool changed_index;
    private bool changed_name;

    #region Properties

    public int index
    {
        get { return _index; }
        set
        {
            if (value == _index) return;

            changed_index = true;

            _index = value;
        }
    }

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
        if (changed_index)
        {
            //Debug.Log("Update index " + index);
            changed_index = false;
        }
    }

    public void SetOriginalValues()
    {
        original_name = name;

        ClearChanges();
    }

    public void GetOriginalValues()
    {
        name = original_name;
    }

    public void ClearChanges()
    {
        GetOriginalValues();

        changed = false;
        changed_index = false;
        changed_name = false;
    }

    public void Delete()
    {

    }

    #endregion
}
