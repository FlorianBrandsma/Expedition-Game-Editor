using UnityEngine;
using System.Linq;

public class SaveCore : GeneralData
{
    private int gameId;

    public bool Changed
    {
        get
        {
            return false;
        }
    }

    #region Properties
    public int GameId
    {
        get { return gameId; }
        set { gameId = value; }
    }
    #endregion

    #region Methods
    public void Create() { }

    public virtual void Update() { }

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
        var saveDataSource = (SaveElementData)dataSource;

        gameId = saveDataSource.gameId;
    }
}
