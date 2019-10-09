using UnityEngine;
using System.Linq;

public class InteractableCore : GeneralData
{
    private int objectGraphicId;
    private string name;

    public int originalObjectGraphicId;
    public string originalName;

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
        originalName = Name;
        originalObjectGraphicId = ObjectGraphicId;
    }

    public void GetOriginalValues()
    {
        Name = originalName;
        ObjectGraphicId = originalObjectGraphicId;
    }

    public virtual void ClearChanges()
    {
        GetOriginalValues();

        changedObjectGraphicId = false;
        changedName = false;
    }

    public void Delete() { }

    #endregion
}
