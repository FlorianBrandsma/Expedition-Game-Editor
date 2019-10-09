using UnityEngine;
using System.Linq;

public class PhaseCore : GeneralData
{
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

    public virtual void Create() { }

    public virtual void Update()
    {
        var phaseData = Fixtures.phaseList.Where(x => x.Id == Id).FirstOrDefault();

        if (changedName)
            phaseData.name = name;

        if (changedNotes)
            phaseData.notes = notes;

    }

    public void UpdateSearch() { }

    public void UpdateIndex()
    {
        if (!changedIndex) return;

        var phaseData = Fixtures.phaseList.Where(x => x.Id == Id).FirstOrDefault();

        phaseData.Index = Index;

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
