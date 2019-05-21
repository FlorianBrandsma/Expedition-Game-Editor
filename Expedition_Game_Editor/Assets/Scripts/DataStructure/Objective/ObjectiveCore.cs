using UnityEngine;
using System.Collections;

public class ObjectiveCore : GeneralData
{
    private string name;
    private string journal;
    private string notes;

    public int originalIndex;
    public string originalName;
    public string originalJournal;
    public string originalNotes;

    public bool changed;
    private bool changedIndex;
    private bool changedName;
    private bool changedJournal;
    private bool changedNotes;

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

            changed = true;
            changedName = true;

            name = value;
        }
    }

    public string Journal
    {
        get { return journal; }
        set
        {
            if (value == journal) return;

            changed = true;
            changedJournal = true;

            journal = value;
        }
    }

    public string Notes
    {
        get { return notes; }
        set
        {
            if (value == notes) return;

            changed = true;
            changedNotes = true;

            notes = value;
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
        originalName = name;
        originalJournal = journal;
        originalNotes = notes;

        ClearChanges();
    }

    public void GetOriginalValues()
    {
        name = originalName;
        journal = originalJournal;
        notes = originalNotes;
    }

    public void ClearChanges()
    {
        GetOriginalValues();

        changed = false;
        changedIndex = false;
        changedName = false;
        changedJournal = false;
        changedNotes = false;
    }

    public void Delete()
    {

    }

    #endregion
}
