﻿using UnityEngine;
using System.Linq;

public class ObjectiveSaveCore : GeneralData
{
    private int questSaveId;
    private int objectiveId;

    private bool complete;

    //Original
    public bool originalComplete;

    //Changed
    private bool changedComplete;

    public bool Changed
    {
        get
        {
            return changedComplete;
        }
    }

    #region Properties
    public int QuestSaveId
    {
        get { return questSaveId; }
        set { questSaveId = value; }
    }

    public int ObjectiveId
    {
        get { return objectiveId; }
        set { objectiveId = value; }
    }

    public bool Complete
    {
        get { return complete; }
        set
        {
            if (value == complete) return;

            changedComplete = (value != originalComplete);

            complete = value;
        }
    }
    #endregion

    #region Methods
    public void Create() { }

    public virtual void Update()
    {
        var objectiveSaveData = Fixtures.objectiveSaveList.Where(x => x.Id == Id).FirstOrDefault();

        if (changedComplete)
            objectiveSaveData.complete = complete;
    }

    public void UpdateSearch() { }

    public void UpdateIndex() { }

    public virtual void SetOriginalValues()
    {
        originalComplete = complete;
    }

    public void GetOriginalValues()
    {
        complete = originalComplete;
    }

    public virtual void ClearChanges()
    {
        GetOriginalValues();

        changedComplete = false;
    }

    public void Delete() { }
    #endregion

    new public virtual void Copy(IDataElement dataSource)
    {
        var objectiveSaveDataSource = (ObjectiveSaveDataElement)dataSource;

        questSaveId = objectiveSaveDataSource.questSaveId;
        objectiveId = objectiveSaveDataSource.objectiveId;

        complete = objectiveSaveDataSource.complete;
    }
}
