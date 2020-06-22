using UnityEngine;
using System.Linq;

public class PhaseSaveCore : GeneralData
{
    private int saveId;
    private int chapterSaveId;
    private int phaseId;

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

    public int ChapterSaveId
    {
        get { return chapterSaveId; }
        set { chapterSaveId = value; }
    }

    public int PhaseId
    {
        get { return phaseId; }
        set { phaseId = value; }
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
        var phaseSaveData = Fixtures.phaseSaveList.Where(x => x.Id == Id).FirstOrDefault();

        if (changedComplete)
            phaseSaveData.complete = complete;
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
        var phaseSaveDataSource = (PhaseSaveElementData)dataSource;

        saveId = phaseSaveDataSource.saveId;
        chapterSaveId = phaseSaveDataSource.chapterSaveId;
        phaseId = phaseSaveDataSource.phaseId;

        complete = phaseSaveDataSource.complete;
    }
}
