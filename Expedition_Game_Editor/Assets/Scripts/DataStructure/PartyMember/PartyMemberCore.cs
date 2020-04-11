using UnityEngine;
using System.Linq;

public class PartyMemberCore : GeneralData
{
    private int chapterId;
    private int interactableId;

    //Original
    public int originalChapterId;
    public int originalInteractableId;

    //Changed
    private bool changedChapterId;
    private bool changedInteractableId;

    public bool Changed
    {
        get
        {
            return changedChapterId || changedInteractableId;
        }
    }

    #region Properties
    public int ChapterId
    {
        get { return chapterId; }
        set
        {
            if (value == chapterId) return;

            changedChapterId = (value != originalChapterId);

            chapterId = value;
        }
    }

    public int InteractableId
    {
        get { return interactableId; }
        set
        {
            if (value == interactableId) return;

            changedInteractableId = (value != originalInteractableId);

            interactableId = value;
        }
    }
    #endregion

    #region Methods
    public void Create() { }

    public virtual void Update()
    {
        var partyElementData = Fixtures.partyMemberList.Where(x => x.Id == Id).FirstOrDefault();

        if (changedInteractableId)
            partyElementData.interactableId = interactableId;
    }

    public void UpdateSearch() { }

    public void UpdateIndex() { }

    public virtual void SetOriginalValues()
    {
        originalChapterId = ChapterId;
        originalInteractableId = InteractableId;
    }

    public void GetOriginalValues()
    {
        chapterId = originalChapterId;
        interactableId = originalInteractableId;
    }

    public virtual void ClearChanges()
    {
        GetOriginalValues();

        changedChapterId = false;
        changedInteractableId = false;
    }

    public void Delete() { }
    #endregion

    new public virtual void Copy(IDataElement dataSource)
    {
        var partyMemberDataSource = (PartyMemberDataElement)dataSource;

        chapterId = partyMemberDataSource.chapterId;
        interactableId = partyMemberDataSource.interactableId;
    }
}
