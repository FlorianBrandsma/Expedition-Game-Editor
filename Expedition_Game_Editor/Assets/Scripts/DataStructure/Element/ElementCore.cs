using UnityEngine;
using System.Linq;

public class ElementCore : GeneralData
{
    private int objectGraphicId;
    private string name;
    
    public int originalIndex;
    public int originalObjectGraphicId;
    public string originalName;

    public bool changed;
    private bool changedIndex;
    private bool changedObjectGraphicId;
    private bool changedName;

    #region Properties

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

    public string Name
    {
        get { return name; }
        set
        {
            if (value == name) return;

            changed = true;
            changedName = true;

            name = value;
        }
    }

    public int ObjectGraphicId
    {
        get { return objectGraphicId; }
        set
        {
            if (value == objectGraphicId) return;

            changed = true;
            changedObjectGraphicId = true;

            objectGraphicId = value;
        }
    }
    
    #endregion

    #region Methods

    public void Create()
    {

    }

    public virtual void Update()
    {
        var elementData = Fixtures.elementList.Where(x => x.id == id).FirstOrDefault();

        if (changedName)
            elementData.name = name;

        if (changedObjectGraphicId)
            elementData.objectGraphicId = objectGraphicId;
    }

    public void UpdateIndex()
    {
        var elementData = Fixtures.elementList.Where(x => x.id == id).FirstOrDefault();

        if (changedIndex)
        {
            elementData.index = index;

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

        changed = false;
        changedIndex = false;
        changedObjectGraphicId = false;
        changedName = false;
    }

    public void Delete()
    {

    }

    #endregion
}
