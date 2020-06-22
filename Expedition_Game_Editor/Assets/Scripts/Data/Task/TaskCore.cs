using UnityEngine;
using System.Linq;

public class TaskCore : GeneralData
{
    private int worldInteractableId;
    private int objectiveId;

    private string name;

    private string publicNotes;
    private string privateNotes;

    //Original
    public string originalName;

    public string originalPublicNotes;
    public string originalPrivateNotes;

    //Changed
    private bool changedName;

    private bool changedPublicNotes;
    private bool changedPrivateNotes;

    public bool Changed
    {
        get
        {
            return changedName || changedPublicNotes || changedPrivateNotes;
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
        var taskData = Fixtures.taskList.Where(x => x.Id == Id).FirstOrDefault();

        if (changedName)
            taskData.name = name;

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

        var taskData = Fixtures.taskList.Where(x => x.Id == Id).FirstOrDefault();

        taskData.Index = Index;

        changedIndex = false;
    }

    public virtual void SetOriginalValues()
    {
        originalName = name;

        originalPublicNotes = publicNotes;
        originalPrivateNotes = privateNotes;
    }

    public void GetOriginalValues()
    {
        name = originalName;

        publicNotes = originalPublicNotes;
        privateNotes = originalPrivateNotes;
    }

    public virtual void ClearChanges()
    {
        GetOriginalValues();

        changedName = false;

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

        publicNotes = taskDataSource.publicNotes;
        privateNotes = taskDataSource.privateNotes;
    }
}
