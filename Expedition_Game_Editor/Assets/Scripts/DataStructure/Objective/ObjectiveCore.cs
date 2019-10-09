using UnityEngine;
using System.Linq;

public class ObjectiveCore : GeneralData
{
    private int questId;
    private string name;
    private string journal;
    private string notes;

    public string originalName;
    public string originalJournal;
    public string originalNotes;

    private bool changedName;
    private bool changedJournal;
    private bool changedNotes;

    public bool Changed
    {
        get
        {
            return changedName || changedJournal || changedNotes;
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

    public string Notes
    {
        get { return notes; }
        set
        {
            if (value == notes) return;

            changedNotes = (value != originalNotes);

            notes = value;
        }
    }

    #endregion

    #region Methods

    public void Create() { }

    public virtual void Update()
    {
        var objectiveData = Fixtures.objectiveList.Where(x => x.Id == Id).FirstOrDefault();

        if(changedName)
            objectiveData.name = name;

        if (changedJournal)
            objectiveData.journal = journal;

        if (changedNotes)
            objectiveData.notes = notes;
    }

    public void UpdateSearch() { }

    public void UpdateIndex()
    {
        if (!changedIndex) return;

        var objectiveData = Fixtures.objectiveList.Where(x => x.Id == Id).FirstOrDefault();

        objectiveData.Index = Index;

        changedIndex = false;
    }

    public virtual void SetOriginalValues()
    {
        originalName = name;
        originalJournal = journal;
        originalNotes = notes;
    }

    public void GetOriginalValues()
    {
        name = originalName;
        journal = originalJournal;
        notes = originalNotes;
    }

    public virtual void ClearChanges()
    {
        GetOriginalValues();

        changedName = false;
        changedJournal = false;
        changedNotes = false;
    }

    public void Delete() { }

    #endregion
}
