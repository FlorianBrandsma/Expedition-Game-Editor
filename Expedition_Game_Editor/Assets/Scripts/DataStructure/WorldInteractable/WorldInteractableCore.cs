using UnityEngine;
using System.Linq;

public class WorldInteractableCore : GeneralData
{
    private int type;

    private int interactableId;
    private int objectGraphicId;

    //Original
    public int originalInteractableId;
    public int originalObjectGraphicId;

    //Changed
    private bool changedInteractableId;
    private bool changedObjectGraphicId;

    public bool Changed
    {
        get
        {
            return changedInteractableId || changedObjectGraphicId;
        }
    }

    #region Properties
    public int Type
    {
        get { return type; }
        set { type = value; }
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

    public int ObjectGraphicId
    {
        get { return objectGraphicId; }
        set
        {
            if (value == objectGraphicId) return;

            changedObjectGraphicId = (value != originalObjectGraphicId);

            objectGraphicId = value;
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

        if (changedObjectGraphicId)
            worldInteractableData.objectGraphicId = objectGraphicId;
    }

    public void UpdateSearch()
    {
        Update();
    }

    public void UpdateIndex() { }

    public virtual void SetOriginalValues()
    {
        originalInteractableId = interactableId;
        originalObjectGraphicId = objectGraphicId;
    }

    public void GetOriginalValues()
    {
        interactableId = originalInteractableId;
        objectGraphicId = originalObjectGraphicId;
    }

    public virtual void ClearChanges()
    {
        GetOriginalValues();

        changedInteractableId = false;
        changedObjectGraphicId = false;
    }

    public void Delete() { }

    public void CloneCore(WorldInteractableDataElement dataElement)
    {
        CloneGeneralData(dataElement);

        dataElement.type = type;

        dataElement.InteractableId = interactableId;
        dataElement.ObjectGraphicId = objectGraphicId;

        dataElement.originalInteractableId = originalInteractableId;
        dataElement.originalObjectGraphicId = originalObjectGraphicId;
    }
    #endregion

    new public virtual void Copy(IDataElement dataSource)
    {
        var worldInteractableDataSource = (WorldInteractableDataElement)dataSource;

        type = worldInteractableDataSource.type;

        interactableId = worldInteractableDataSource.interactableId;
        objectGraphicId = worldInteractableDataSource.objectGraphicId;
    }
}
