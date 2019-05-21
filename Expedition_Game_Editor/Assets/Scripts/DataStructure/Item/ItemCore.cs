using UnityEngine;
using System.Collections;

public class ItemCore : GeneralData
{
    private int type;
    private int objectGraphicId;
    private string name;

    public int originalIndex;
    public int originalObjectGraphicId;
    public string originalName;

    public bool changed;
    private bool changedId;
    private bool changedTable;
    private bool changedType;
    private bool changedIndex;
    private bool changedObjectGraphicId;
    private bool changedName;

    #region Properties

    public int Type
    {
        get { return type; }
        set { type = value; }
    }

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

            changed = true;
            changedName = true;

            name = value;
        }
    }

    public int ObjectGraphicId
    {
        get { return objectGraphicId; }
        set
        {
            if (value == objectGraphicId) return;

            changed = true;
            changedObjectGraphicId = true;
            
            objectGraphicId = value;
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

    public void UpdateIndex()
    {
        if (changedIndex)
        {
            //Debug.Log("Update index " + index);
            changedIndex = false;
        }
    }

    public virtual void SetOriginalValues()
    {
        originalName = Name;
        originalObjectGraphicId = ObjectGraphicId;

        //ClearChanges();
    }

    public void GetOriginalValues()
    {
        Name = originalName;
        ObjectGraphicId = originalObjectGraphicId;
    }

    public virtual void ClearChanges()
    {
        GetOriginalValues();

        changed = false;
        changedId = false;
        changedTable = false;
        changedType = false;
        changedIndex = false;
        changedObjectGraphicId = false;
        changedName = false;
    }

    public void Delete()
    {

    }

    #endregion
}
