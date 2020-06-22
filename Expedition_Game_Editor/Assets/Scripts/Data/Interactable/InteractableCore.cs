using UnityEngine;
using System.Linq;

public class InteractableCore : GeneralData
{
    private int type;
    private int objectGraphicId;
    private string name;

    //Original
    public int originalObjectGraphicId;
    public string originalName;

    //Changed
    private bool changedObjectGraphicId;
    private bool changedName;

    public bool Changed
    {
        get
        {
            return changedObjectGraphicId || changedName;
        }
    }

    #region Properties
    public int Type
    {
        get { return type; }
        set { type = value; }
    }

    public string Name
    {
        get { return name; }
        set
        {
            if (value == name) return;

            changedName = (value != originalName);

            name = value;
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
        var interactableData = Fixtures.interactableList.Where(x => x.Id == Id).FirstOrDefault();

        if (changedName)
            interactableData.name = name;

        if (changedObjectGraphicId)
            interactableData.objectGraphicId = objectGraphicId;
    }

    public void UpdateSearch() { }

    public void UpdateIndex()
    {
        if (!changedIndex) return;

        var interactableData = Fixtures.interactableList.Where(x => x.Id == Id).FirstOrDefault();

        interactableData.Index = Index;

        changedIndex = false;
    }

    public virtual void SetOriginalValues()
    {
        originalObjectGraphicId = ObjectGraphicId;
        originalName = Name;
    }

    public void GetOriginalValues()
    {
        objectGraphicId = originalObjectGraphicId;
        name = originalName;
    }

    public virtual void ClearChanges()
    {
        GetOriginalValues();

        changedObjectGraphicId = false;
        changedName = false;
    }

    public void Delete() { }
    #endregion

    new public virtual void Copy(IElementData dataSource)
    {
        var interactableDataSource = (InteractableElementData)dataSource;

        objectGraphicId = interactableDataSource.objectGraphicId;
        name = interactableDataSource.name;
    }
}
