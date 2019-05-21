using UnityEngine;
using System.Collections;

public class ChapterRegionCore : GeneralData
{
    private int chapterId;
    private int regionId;

    public int originalIndex;
    public int originalRegionId;

    public bool changed;
    private bool changedIndex;
    private bool changedRegionId;

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

    public int ChapterId
    {
        get { return chapterId; }
        set { chapterId = value; }
    }

    public int RegionId
    {
        get { return regionId; }
        set
        {
            if (value == regionId) return;

            changedRegionId = true;

            regionId = value;
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
        originalRegionId = regionId;

        ClearChanges();
    }

    public void GetOriginalValues()
    {
        regionId = originalRegionId;
    }

    public void ClearChanges()
    {
        GetOriginalValues();

        changed = false;
        changedIndex = false;
        changedRegionId = false;
    }

    public void Delete()
    {

    }

    #endregion
}
