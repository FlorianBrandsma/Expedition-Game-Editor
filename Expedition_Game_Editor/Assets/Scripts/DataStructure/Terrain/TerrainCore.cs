using UnityEngine;
using System.Collections;

public class TerrainCore : GeneralData
{
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
        //var elementData = Fixtures.elementList.Where(x => x.id == id).FirstOrDefault();

        //if (changedName)
        //    elementData.name = name;

        //if (changedObjectGraphicId)
        //    elementData.objectGraphicId = objectGraphicId;
    }

    public void UpdateIndex()
    {
        //var elementData = Fixtures.elementList.Where(x => x.id == id).FirstOrDefault();

        //if (changedIndex)
        //{
        //    elementData.index = index;

        //    changedIndex = false;
        //}
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

    public void Delete()
    {

    }

    #endregion
}
