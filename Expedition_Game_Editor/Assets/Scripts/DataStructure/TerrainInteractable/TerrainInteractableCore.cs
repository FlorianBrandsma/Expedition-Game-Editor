using UnityEngine;
using System.Linq;

public class TerrainInteractableCore : GeneralData
{
    private int chapterId;
    private int interactableId;

    public int originalChapterId;
    public int originalInteractableId;

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

    public int Id { get { return id; } }

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

    public void Create()
    {

    }

    public virtual void Update()
    {
        var terrainElementData = Fixtures.terrainInteractableList.Where(x => x.id == id).FirstOrDefault();

        if (changedInteractableId)
            terrainElementData.interactableId = interactableId;
    }

    public void UpdateIndex() { }

    public virtual void SetOriginalValues()
    {
        originalChapterId = ChapterId;
        originalInteractableId = InteractableId;
    }

    public void GetOriginalValues()
    {
        ChapterId = originalChapterId;
        InteractableId = originalInteractableId;
    }

    public virtual void ClearChanges()
    {
        GetOriginalValues();

        changedChapterId = false;
        changedInteractableId = false;
    }

    public void Delete()
    {

    }

    #endregion
}
