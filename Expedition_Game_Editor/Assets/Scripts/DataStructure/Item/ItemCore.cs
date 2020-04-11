using UnityEngine;
using System.Linq;

public class ItemCore : GeneralData
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
    public void Create() { }

    public virtual void Update()
    {
        var itemData = Fixtures.itemList.Where(x => x.Id == Id).FirstOrDefault();

        if (changedObjectGraphicId)
            itemData.objectGraphicId = objectGraphicId;

        if (changedName)
            itemData.name = name;
    }

    public void UpdateSearch() { }

    public void UpdateIndex()
    {
        if (!changedIndex) return;

        var itemData = Fixtures.itemList.Where(x => x.Id == Id).FirstOrDefault();
        
        itemData.Index = Index;

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

    new public virtual void Copy(IDataElement dataSource)
    {
        var itemDataSource = (ItemDataElement)dataSource;

        objectGraphicId = itemDataSource.objectGraphicId;

        name = itemDataSource.name;
    }
}
