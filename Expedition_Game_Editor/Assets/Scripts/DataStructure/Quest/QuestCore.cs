using UnityEngine;
using System.Linq;

public class QuestCore : GeneralData
{
    private int phaseId;
    private string name;
    private string notes;

    public int originalIndex;
    public string originalName;
    public string originalNotes;

    private bool changedIndex;
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

    public int Id { get { return id; } }

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

    public void Create()
    {

    }

    public void Update()
    {
        if (!Changed) return;

        var questData = Fixtures.questList.Where(x => x.id == id).FirstOrDefault();

        if (changedName)
            questData.name = name;

        if (changedNotes)
            questData.notes = notes;

        SetOriginalValues();
    }

    public void UpdateIndex()
    {
        var questData = Fixtures.questList.Where(x => x.id == id).FirstOrDefault();

        if (changedIndex)
        {
            questData.index = index;

            changedIndex = false;
        }
    }

    public void SetOriginalValues()
    {
        originalName = name;
        originalNotes = notes;

        ClearChanges();
    }

    public void GetOriginalValues()
    {
        name = originalName;
        notes = originalNotes;
    }

    public void ClearChanges()
    {
        GetOriginalValues();

        changedIndex = false;
        changedName = false;
        changedNotes = false;
    }

    public void Delete()
    {

    }

    #endregion
}
