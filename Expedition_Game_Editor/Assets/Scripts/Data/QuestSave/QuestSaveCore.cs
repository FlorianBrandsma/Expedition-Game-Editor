using UnityEngine;
using System.Linq;

public class QuestSaveCore : GeneralData
{
    private int saveId;
    private int phaseSaveId;
    private int questId;

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

    public int PhaseSaveId
    {
        get { return phaseSaveId; }
        set { phaseSaveId = value; }
    }

    public int QuestId
    {
        get { return questId; }
        set { questId = value; }
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
        var questSaveData = Fixtures.questSaveList.Where(x => x.Id == Id).FirstOrDefault();

        if (changedComplete)
            questSaveData.complete = complete;
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
        var questSaveDataSource = (QuestSaveElementData)dataSource;

        saveId = questSaveDataSource.saveId;
        phaseSaveId = questSaveDataSource.phaseSaveId;
        questId = questSaveDataSource.questId;

        complete = questSaveDataSource.complete;
    }
}
