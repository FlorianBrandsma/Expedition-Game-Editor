using UnityEngine;
using System.Collections;

public class ChapterElementCore : GeneralData
{
    private int chapterId;
    private int elementId;

    public int originalChapterId;
    public int originalElementId;

    public bool changed;
    private bool changedChapterId;
    private bool changedElementId;

    #region Properties

    public int ChapterId
    {
        get { return chapterId; }
        set
        {
            if (value == chapterId) return;

            changed = true;
            changedChapterId = true;

            chapterId = value;
        }
    }

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

    public bool Changed { get { return changed; } }

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
        originalChapterId = ChapterId;
        originalElementId = ElementId;

        //ClearChanges();
    }

    public void GetOriginalValues()
    {
        ChapterId = originalChapterId;
        ElementId = originalElementId;
    }

    public virtual void ClearChanges()
    {
        GetOriginalValues();

        changed = false;
        changedChapterId = false;
        changedElementId = false;
    }

    public void Delete()
    {

    }

    #endregion
}
