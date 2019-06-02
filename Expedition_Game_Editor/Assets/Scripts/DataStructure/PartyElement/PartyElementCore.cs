using UnityEngine;
using System.Linq;

public class PartyElementCore : GeneralData
{
    private int chapterId;
    private int elementId;

    public int originalChapterId;
    public int originalElementId;

    private bool changedChapterId;
    private bool changedElementId;

    public bool Changed
    {
        get
        {
            return changedChapterId || changedElementId;
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

    public int ElementId
    {
        get { return elementId; }
        set
        {
            if (value == elementId) return;

            changedElementId = (value != originalElementId);

            elementId = value;
        }
    }

    #endregion

    #region Methods

    public void Create()
    {

    }

    public virtual void Update()
    {
        var partyElementData = Fixtures.partyElementList.Where(x => x.id == id).FirstOrDefault();

        if (changedElementId)
            partyElementData.elementId = elementId;
    }

    public void UpdateIndex() { }

    public virtual void SetOriginalValues()
    {
        originalChapterId = ChapterId;
        originalElementId = ElementId;
    }

    public void GetOriginalValues()
    {
        ChapterId = originalChapterId;
        ElementId = originalElementId;
    }

    public virtual void ClearChanges()
    {
        GetOriginalValues();

        changedChapterId = false;
        changedElementId = false;
    }

    public void Delete()
    {

    }

    #endregion
}
