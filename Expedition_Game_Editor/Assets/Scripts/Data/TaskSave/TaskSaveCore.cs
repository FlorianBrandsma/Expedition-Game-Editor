using UnityEngine;
using System.Linq;

public class TaskSaveCore : GeneralData
{
    private int saveId;
    private int worldInteractableId;
    private int objectiveSaveId;
    private int taskId;

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

    public int WorldInteractableId
    {
        get { return worldInteractableId; }
        set { worldInteractableId = value; }
    }

    public int ObjectiveSaveId
    {
        get { return objectiveSaveId; }
        set { objectiveSaveId = value; }
    }

    public int TaskId
    {
        get { return taskId; }
        set { taskId = value; }
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
        var taskSaveData = Fixtures.taskSaveList.Where(x => x.id == Id).FirstOrDefault();

        if (changedComplete)
            taskSaveData.complete = complete;
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

    new public virtual void Copy(IElementData dataSource)
    {
        var taskSaveDataSource = (TaskSaveElementData)dataSource;

        saveId = taskSaveDataSource.saveId;
        worldInteractableId = taskSaveDataSource.worldInteractableId;
        objectiveSaveId = taskSaveDataSource.objectiveSaveId;
        taskId = taskSaveDataSource.taskId;

        complete = taskSaveDataSource.complete;
    }
}
