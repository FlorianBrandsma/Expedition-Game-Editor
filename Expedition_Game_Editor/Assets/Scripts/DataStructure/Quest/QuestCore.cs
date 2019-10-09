using UnityEngine;
using System.Linq;

public class QuestCore : GeneralData
{
    private int phaseId;
    private string name;
    private string notes;

    public string originalName;
    public string originalNotes;

    private bool changedName;
    private bool changedNotes;

    public bool Changed
    {
        get
        {
            return changedName || changedNotes;
        }
    }

    #region Properties

    public int PhaseId
    {
        get { return phaseId; }
        set { phaseId = value; }
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

        var questData = Fixtures.questList.Where(x => x.Id == Id).FirstOrDefault();

        if (changedName)
            questData.name = name;

        if (changedNotes)
            questData.notes = notes;

        SetOriginalValues();
    }

    public void UpdateSearch() { }

    public void UpdateIndex()
    {
        if (!changedIndex) return;

        var questData = Fixtures.questList.Where(x => x.Id == Id).FirstOrDefault();

        questData.Index = Index;

        changedIndex = false;
    }

    public virtual void SetOriginalValues()
    {
        originalName = name;
        originalNotes = notes;
    }

    public void GetOriginalValues()
    {
        name = originalName;
        notes = originalNotes;
    }

    public virtual void ClearChanges()
    {
        GetOriginalValues();

        changedName = false;
        changedNotes = false;
    }

    public void Delete() { }

    #endregion
}
