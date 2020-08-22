using UnityEngine;
using System.Linq;

public class ChapterSaveCore : GeneralData
{
    private int saveId;
    private int chapterId;

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

    public int ChapterId
    {
        get { return chapterId; }
        set { chapterId = value; }
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
        var chapterSaveData = Fixtures.chapterSaveList.Where(x => x.id == Id).FirstOrDefault();

        if (changedComplete)
            chapterSaveData.complete = complete;
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
        var chapterSaveDataSource = (ChapterSaveElementData)dataSource;

        saveId = chapterSaveDataSource.saveId;
        chapterId = chapterSaveDataSource.chapterId;

        complete = chapterSaveDataSource.complete;
    }
}
