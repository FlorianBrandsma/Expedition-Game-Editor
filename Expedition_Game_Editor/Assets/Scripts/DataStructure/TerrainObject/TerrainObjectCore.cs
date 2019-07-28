using UnityEngine;
using System.Linq;

public class TerrainObjectCore : GeneralData
{
    private int objectGraphicId;
    private int regionId;

    private bool boundToTile;

    private float positionX;
    private float positionY;
    private float positionZ;

    private int rotationX;
    private int rotationY;
    private int rotationZ;

    private float scaleMultiplier;

    private int animation;


    public int originalObjectGraphicId;
    public int originalRegionId;

    public bool originalBoundToTile;

    public float originalPositionX;
    public float originalPositionY;
    public float originalPositionZ;

    public int originalRotationX;
    public int originalRotationY;
    public int originalRotationZ;

    public float originalScaleMultiplier;

    public int originalAnimation;


    private bool changedObjectGraphicId;
    private bool changedRegionId;

    private bool changedBoundToTile;

    private bool changedPositionX;
    private bool changedPositionY;
    private bool changedPositionZ;

    private bool changedRotationX;
    private bool changedRotationY;
    private bool changedRotationZ;

    private bool changedScaleMultiplier;

    private bool changedAnimation;

    public bool Changed
    {
        get
        {
            return  changedObjectGraphicId  || changedRegionId  || changedBoundToTile   || changedPositionX || changedPositionY         ||
                    changedPositionZ        || changedRotationX || changedRotationY     || changedRotationZ || changedScaleMultiplier   ||
                    changedAnimation;
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

    public bool BoundToTile
    {
        get { return boundToTile; }
        set
        {
            if (value == boundToTile) return;

            changedBoundToTile = (value != originalBoundToTile);

            boundToTile = value;
        }
    }

    public float PositionX
    {
        get { return positionX; }
        set
        {
            if (value == positionX) return;

            changedPositionX = (value != originalPositionX);

            positionX = value;
        }
    }

    public float PositionY
    {
        get { return positionY; }
        set
        {
            if (value == positionY) return;

            changedPositionY = (value != originalPositionY);

            positionY = value;
        }
    }

    public float PositionZ
    {
        get { return positionZ; }
        set
        {
            if (value == positionZ) return;

            changedPositionZ = (value != originalPositionZ);

            positionZ = value;
        }
    }

    public int RotationX
    {
        get { return rotationX; }
        set
        {
            if (value == rotationX) return;

            changedRotationX = (value != originalRotationX);

            rotationX = value;
        }
    }

    public int RotationY
    {
        get { return rotationY; }
        set
        {
            if (value == rotationY) return;

            changedRotationY = (value != originalRotationY);

            rotationY = value;
        }
    }

    public int RotationZ
    {
        get { return rotationZ; }
        set
        {
            if (value == rotationZ) return;

            changedRotationZ = (value != originalRotationZ);

            rotationZ = value;
        }
    }

    public float ScaleMultiplier
    {
        get { return scaleMultiplier; }
        set
        {
            if (value == scaleMultiplier) return;

            changedScaleMultiplier = (value != originalScaleMultiplier);

            scaleMultiplier = value;
        }
    }

    public int Animation
    {
        get { return animation; }
        set
        {
            if (value == animation) return;

            changedAnimation = (value != originalAnimation);

            animation = value;
        }
    }

    #endregion

    #region Methods

    public void Create()
    {

    }

    public virtual void Update()
    {
        if (!Changed) return;

        var terrainObjectData = Fixtures.terrainObjectList.Where(x => x.id == id).FirstOrDefault();

        if (changedBoundToTile)
            terrainObjectData.boundToTile = boundToTile;

        if (changedPositionX)
            terrainObjectData.positionX = positionX;

        if (changedPositionY)
            terrainObjectData.positionY = positionY;

        if (changedPositionZ)
            terrainObjectData.positionZ = positionZ;

        if (changedRotationX)
            terrainObjectData.rotationX = rotationX;

        if (changedRotationY)
            terrainObjectData.rotationY = rotationY;

        if (changedRotationZ)
            terrainObjectData.rotationZ = rotationZ;

        if (changedScaleMultiplier)
            terrainObjectData.scaleMultiplier = scaleMultiplier;

        if (changedAnimation)
            terrainObjectData.animation = animation;
    }

    public void UpdateSearch()
    {
        var terrainObjectData = Fixtures.terrainObjectList.Where(x => x.id == id).FirstOrDefault();

        if (changedObjectGraphicId)
            terrainObjectData.objectGraphicId = objectGraphicId;
    }

    public virtual void SetOriginalValues()
    {
        originalObjectGraphicId = objectGraphicId;
        originalRegionId = regionId;

        originalBoundToTile = boundToTile;

        originalPositionX = positionX;
        originalPositionY = positionY;
        originalPositionZ = positionZ;

        originalRotationX = rotationX;
        originalRotationY = rotationY;
        originalRotationZ = rotationZ;

        originalScaleMultiplier = scaleMultiplier;

        originalAnimation = animation;
    }

    public void GetOriginalValues()
    {
        objectGraphicId = originalObjectGraphicId;
        regionId = originalRegionId;

        boundToTile = originalBoundToTile;

        positionX = originalPositionX;
        positionY = originalPositionY;
        positionZ = originalPositionZ;

        rotationX = originalRotationX;
        rotationY = originalRotationY;
        rotationZ = originalRotationZ;

        scaleMultiplier = originalScaleMultiplier;

        animation = originalAnimation;
    }

    public virtual void ClearChanges()
    {
        GetOriginalValues();

        changedObjectGraphicId = false;
        changedRegionId = false;

        changedBoundToTile = false;

        changedPositionX = false;
        changedPositionY = false;
        changedPositionZ = false;

        changedRotationX = false;
        changedRotationY = false;
        changedRotationZ = false;

        changedScaleMultiplier = false;

        changedAnimation = false;
    }

    public void Delete()
    {

    }

    #endregion
}
