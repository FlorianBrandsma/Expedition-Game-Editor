using UnityEngine;
using System.Linq;

public class WorldInteractableCore : GeneralData
{
    private int interactableId;

    //Original
    public int originalInteractableId;

    //Changed
    private bool changedInteractableId;

    public bool Changed
    {
        get
        {
            return changedInteractableId;
        }
    }

    #region Properties
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
        var worldInteractableData = Fixtures.worldInteractableList.Where(x => x.Id == Id).FirstOrDefault();

        if (changedInteractableId)
            worldInteractableData.interactableId = interactableId;
    }

    public void UpdateSearch()
    {
        Update();
    }

    public void UpdateIndex() { }

    public virtual void SetOriginalValues()
    {
        originalInteractableId = InteractableId;
    }

    public void GetOriginalValues()
    {
        interactableId = originalInteractableId;
    }

    public virtual void ClearChanges()
    {
        GetOriginalValues();

        changedInteractableId = false;
    }

    public void Delete() { }

    public void CloneCore(WorldInteractableDataElement dataElement)
    {
        CloneGeneralData(dataElement);
        
        dataElement.InteractableId = InteractableId;

        dataElement.originalInteractableId = originalInteractableId;
    }
    #endregion

    new public virtual void Copy(IDataElement dataSource)
    {
        var worldInteractableDataSource = (WorldInteractableDataElement)dataSource;

        interactableId = worldInteractableDataSource.interactableId;
    }
}
