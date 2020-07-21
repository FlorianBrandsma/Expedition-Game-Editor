using UnityEngine;
using System.Linq;

public class InteractableSaveCore : GeneralData
{
    private int saveId;
    private int interactableId;

    public bool Changed
    {
        get
        {
            return false;
        }
    }

    #region Properties
    public int SaveId
    {
        get { return saveId; }
        set { saveId = value; }
    }

    public int InteractableId
    {
        get { return interactableId; }
        set { interactableId = value; }
    }
    #endregion

    #region Methods
    public void Create() { }

    public virtual void Update()
    {
        var interactableSaveData = Fixtures.interactableSaveList.Where(x => x.Id == Id).FirstOrDefault();
    }

    public void UpdateSearch() { }

    public void UpdateIndex() { }

    public virtual void SetOriginalValues() { }

    public void GetOriginalValues() { }

    public virtual void ClearChanges()
    {
        GetOriginalValues();
    }

    public void Delete() { }
    #endregion

    new public virtual void Copy(IElementData dataSource)
    {
        var interactableSaveDataSource = (InteractableSaveElementData)dataSource;

        saveId = interactableSaveDataSource.saveId;
        interactableId = interactableSaveDataSource.interactableId;
    }
}
