using UnityEngine;
using System.Linq;

public class TaskCore : GeneralData
{
    private int worldInteractableId;
    private int objectiveId;

    private string name;

    private bool completeObjective;
    private bool repeatable;

    private string publicNotes;
    private string privateNotes;

    //Original
    public string originalName;

    private bool originalCompleteObjective;
    private bool originalRepeatable;

    public string originalPublicNotes;
    public string originalPrivateNotes;

    //Changed
    private bool changedName;

    private bool changedCompleteObjective;
    private bool changedRepeatable;

    private bool changedPublicNotes;
    private bool changedPrivateNotes;

    public bool Changed
    {
        get
        {
            return changedName || changedCompleteObjective || changedRepeatable || changedPublicNotes || changedPrivateNotes;
        }
    }

    #region Properties
    public int WorldInteractableId
    {
        get { return worldInteractableId; }
        set { worldInteractableId = value; }
    }

    public int ObjectiveId
    {
        get { return objectiveId; }
        set { objectiveId = value; }
    }

    public string Name
    {
        get { return name; }
        set
        {
            if (value == name) return;

            changedName = (value != originalName);

            name = value;
        }
    }

    public bool CompleteObjective
    {
        get { return completeObjective; }
        set
        {
            if (value == completeObjective) return;

            changedCompleteObjective = (value != originalCompleteObjective);

            completeObjective = value;
        }
    }

    public bool Repeatable
    {
        get { return repeatable; }
        set
        {
            if (value == repeatable) return;

            changedRepeatable = (value != originalRepeatable);

            repeatable = value;
        }
    }

    public string PublicNotes
    {
        get { return publicNotes; }
        set
        {
            if (value == publicNotes) return;

            changedPublicNotes = (value != originalPublicNotes);

            publicNotes = value;
        }
    }

    public string PrivateNotes
    {
        get { return privateNotes; }
        set
        {
            if (value == privateNotes) return;

            changedPrivateNotes = (value != originalPrivateNotes);

            privateNotes = value;
        }
    }
    #endregion

    #region Methods
    public void Create() { }

    public virtual void Update()
    {
        var taskData = Fixtures.taskList.Where(x => x.id == Id).FirstOrDefault();

        if (changedName)
            taskData.name = name;

        if (changedCompleteObjective)
            taskData.completeObjective = completeObjective;

        if (changedRepeatable)
            taskData.repeatable = repeatable;

        if (changedPublicNotes)
            taskData.publicNotes = publicNotes;

        if (changedPrivateNotes)
            taskData.privateNotes = privateNotes;

        SetOriginalValues();
    }

    public void UpdateSearch() { }

    public void UpdateIndex()
    {
        if (!changedIndex) return;

        var taskData = Fixtures.taskList.Where(x => x.id == Id).FirstOrDefault();

        taskData.index = Index;

        changedIndex = false;
    }

    public virtual void SetOriginalValues()
    {
        originalName = name;

        originalCompleteObjective = completeObjective;
        originalRepeatable = repeatable;

        originalPublicNotes = publicNotes;
        originalPrivateNotes = privateNotes;
    }

    public void GetOriginalValues()
    {
        name = originalName;

        completeObjective = originalCompleteObjective;
        repeatable = originalRepeatable;

        publicNotes = originalPublicNotes;
        privateNotes = originalPrivateNotes;
    }

    public virtual void ClearChanges()
    {
        GetOriginalValues();

        changedName = false;

        changedCompleteObjective = false;
        changedRepeatable = false;

        changedPublicNotes = false;
        changedPrivateNotes = false;
    }

    public void Delete() { }
    #endregion

    new public virtual void Copy(IElementData dataSource)
    {
        var taskDataSource = (TaskElementData)dataSource;

        worldInteractableId = taskDataSource.worldInteractableId;
        objectiveId = taskDataSource.objectiveId;

        name = taskDataSource.name;

        completeObjective = taskDataSource.completeObjective;
        repeatable = taskDataSource.repeatable;

        publicNotes = taskDataSource.publicNotes;
        privateNotes = taskDataSource.privateNotes;
    }
}
