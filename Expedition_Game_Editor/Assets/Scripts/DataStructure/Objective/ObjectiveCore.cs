using UnityEngine;
using System.Linq;

public class ObjectiveCore : GeneralData
{
    private int questId;
    private string name;
    private string journal;
    private string notes;

    public int originalIndex;
    public string originalName;
    public string originalJournal;
    public string originalNotes;

    private bool changedIndex;
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

    public void Create()
    {

    }

    public void Update()
    {
        if (!Changed) return;

        var objectiveData = Fixtures.objectiveList.Where(x => x.id == id).FirstOrDefault();

        if(changedName)
            objectiveData.name = name;

        if (changedJournal)
            objectiveData.journal = journal;

        if (changedNotes)
            objectiveData.notes = notes;
        
        SetOriginalValues();
    }

    public void UpdateIndex()
    {
        var objectiveData = Fixtures.objectiveList.Where(x => x.id == id).FirstOrDefault();

        if (changedIndex)
        {
            objectiveData.index = index;

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
