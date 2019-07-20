using UnityEngine;
using System.Linq;

public class TerrainObjectCore : GeneralData
{
    private int objectGraphicId;
    private int regionId;

    public int originalObjectGraphicId;
    public int originalRegionId;

    private bool changedObjectGraphicId;
    private bool changedRegionId;

    public bool Changed
    {
        get
        {
            return changedObjectGraphicId || changedRegionId;
        }
    }

    #region Properties

    public int Id { get { return id; } }

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

    public int RegionId
    {
        get { return regionId; }
        set
        {
            if (value == regionId) return;

            changedRegionId = (value != originalRegionId);

            regionId = value;
        }
    }

    #endregion

    #region Methods

    public void Create()
    {

    }

    public virtual void Update()
    {
        var terrainObjectData = Fixtures.terrainObjectList.Where(x => x.id == id).FirstOrDefault();

        if (changedObjectGraphicId)
            terrainObjectData.objectGraphicId = objectGraphicId;
    }

    public void UpdateIndex() { }

    public virtual void SetOriginalValues()
    {
        originalObjectGraphicId = objectGraphicId;
        originalRegionId = regionId;
    }

    public void GetOriginalValues()
    {
        objectGraphicId = originalObjectGraphicId;
        regionId = originalRegionId;
    }

    public virtual void ClearChanges()
    {
        GetOriginalValues();

        changedObjectGraphicId = false;
        changedRegionId = false;
    }

    public void Delete()
    {

    }

    #endregion
}
