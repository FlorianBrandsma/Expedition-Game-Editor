using UnityEngine;
using System.Linq;

public class ObjectiveCore : GeneralData
{
    private int questId;

    private string name;
    private string journal;

    private string publicNotes;
    private string privateNotes;

    //Original
    public string originalName;
    public string originalJournal;

    public string originalPublicNotes;
    public string originalPrivateNotes;

    //Changed
    private bool changedName;
    private bool changedJournal;

    private bool changedPublicNotes;
    private bool changedPrivateNotes;

    public bool Changed
    {
        get
        {
            return changedName || changedJournal || changedPublicNotes || changedPrivateNotes;
        }
    }

    #region Properties
    public int QuestId
    {
        get { return questId; }
        set { questId = value; }
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

    public string Journal
    {
        get { return journal; }
        set
        {
            if (value == journal) return;

            changedJournal = (value != originalJournal);

            journal = value;
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
        var objectiveData = Fixtures.objectiveList.Where(x => x.id == Id).FirstOrDefault();

        if(changedName)
            objectiveData.name = name;

        if (changedJournal)
            objectiveData.journal = journal;

        if (changedPublicNotes)
            objectiveData.publicNotes = publicNotes;

        if (changedPrivateNotes)
            objectiveData.privateNotes = privateNotes;
    }

    public void UpdateSearch() { }

    public void UpdateIndex()
    {
        if (!changedIndex) return;

        var objectiveData = Fixtures.objectiveList.Where(x => x.id == Id).FirstOrDefault();

        objectiveData.index = Index;

        changedIndex = false;
    }

    public virtual void SetOriginalValues()
    {
        originalName = name;
        originalJournal = journal;
        originalPublicNotes = publicNotes;
        originalPrivateNotes = privateNotes;
    }

    public void GetOriginalValues()
    {
        name = originalName;
        journal = originalJournal;
        publicNotes = originalPublicNotes;
        privateNotes = originalPrivateNotes;
    }

    public virtual void ClearChanges()
    {
        GetOriginalValues();

        changedName = false;
        changedJournal = false;
        changedPublicNotes = false;
        changedPrivateNotes = false;
    }

    public void Delete() { }
    #endregion

    new public virtual void Copy(IElementData dataSource)
    {
        var objectiveDataSource = (ObjectiveElementData)dataSource;

        questId = objectiveDataSource.questId;

        name = objectiveDataSource.name;
        journal = objectiveDataSource.journal;

        publicNotes = objectiveDataSource.publicNotes;
        privateNotes = objectiveDataSource.privateNotes;
    }
}
