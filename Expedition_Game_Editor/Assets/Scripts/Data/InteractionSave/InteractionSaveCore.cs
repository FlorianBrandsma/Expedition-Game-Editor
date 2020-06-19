using UnityEngine;
using System.Linq;

public class InteractionSaveCore : GeneralData
{
    private int saveId;
    private int taskSaveId;
    private int interactionId;

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
    public int SaveId
    {
        get { return saveId; }
        set { saveId = value; }
    }

    public int TaskSaveId
    {
        get { return taskSaveId; }
        set { taskSaveId = value; }
    }

    public int InteractionId
    {
        get { return interactionId; }
        set { interactionId = value; }
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
        var interactionSaveData = Fixtures.interactionSaveList.Where(x => x.Id == Id).FirstOrDefault();

        if (changedComplete)
            interactionSaveData.complete = complete;
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
        var interactionSaveDataSource = (InteractionSaveDataElement)dataSource;

        saveId = interactionSaveDataSource.saveId;
        taskSaveId = interactionSaveDataSource.taskSaveId;
        interactionId = interactionSaveDataSource.interactionId;

        complete = interactionSaveDataSource.complete;
    }
}
