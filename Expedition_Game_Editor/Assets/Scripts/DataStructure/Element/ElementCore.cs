using UnityEngine;
using System.Collections;

public class ElementCore : GeneralData
{
    private int _index;
    private int _object_graphic_id;
    private string _name;
    
    public int original_index;
    public int original_object_graphic_id;
    public string original_name;

    public bool changed;
    private bool changed_id;
    private bool changed_table;
    private bool changed_type;
    private bool changed_index;
    private bool changed_object_graphic_id;
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

    public int objectGraphicId
    {
        get { return _object_graphic_id; }
        set
        {
            if (value == _object_graphic_id) return;

            changed = true;
            changed_object_graphic_id = true;

            _object_graphic_id = value;
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
        original_object_graphic_id = objectGraphicId;

        ClearChanges();
    }

    public void GetOriginalValues()
    {
        name = original_name;
        objectGraphicId = original_object_graphic_id;
    }

    public void ClearChanges()
    {
        GetOriginalValues();

        changed = false;
        changed_id = false;
        changed_table = false;
        changed_type = false;
        changed_index = false;
        changed_object_graphic_id = false;
        changed_name = false;
    }

    public void Delete()
    {

    }

    #endregion
}
