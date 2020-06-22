using UnityEngine;
using System.Linq;

public class OutcomeCore : GeneralData
{
    public int type;

    private int interactionId;
    
    //Original

    //Changed

    public bool Changed
    {
        get
        {
            return false;
        }
    }

    #region Properties
    public int Type
    {
        get { return type; }
        set { type = value; }
    }

    public int InteractionId
    {
        get { return interactionId; }
        set { interactionId = value; }
    }
    #endregion

    #region Methods
    public void Create() { }

    public virtual void Update()
    {
        var outcomeData = Fixtures.outcomeList.Where(x => x.Id == Id).FirstOrDefault();
        
        SetOriginalValues();
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
        var outcomeDataSource = (OutcomeElementData)dataSource;

        type = outcomeDataSource.type;

        interactionId = outcomeDataSource.interactionId;
    }
}
