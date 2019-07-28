using UnityEngine;
using System.Linq;

public class ItemCore : GeneralData
{
    private int type;
    private int objectGraphicId;
    private string name;

    public int originalIndex;
    public int originalObjectGraphicId;
    public string originalName;

    private bool changedIndex;
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

    public int Id { get { return id; } }

    public int Index
    {
        get { return index; }
        set
        {
            if (value == index) return;

            changedIndex = true;

            index = value;
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

    #endregion

    #region Methods

    public void Create()
    {

    }

    public virtual void Update()
    {
        var itemData = Fixtures.itemList.Where(x => x.id == id).FirstOrDefault();

        if (changedObjectGraphicId)
            itemData.objectGraphicId = objectGraphicId;

        if (changedName)
            itemData.name = name;
    }

    public void UpdateSearch() { }

    public void UpdateIndex()
    {
        var itemData = Fixtures.itemList.Where(x => x.id == id).FirstOrDefault();

        if (changedIndex)
        {
            itemData.index = index;

            changedIndex = false;
        }
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

        changedIndex = false;
        changedObjectGraphicId = false;
        changedName = false;
    }

    public void Delete()
    {

    }

    #endregion
}
