using UnityEngine;
using System.Linq;

public class WorldObjectCore : GeneralData
{
    private int objectGraphicId;
    private int regionId;
    private int terrainId;
    private int terrainTileId;

    private float positionX;
    private float positionY;
    private float positionZ;

    private int rotationX;
    private int rotationY;
    private int rotationZ;

    private float scaleMultiplier;

    private int animation;

    //Original
    public int originalObjectGraphicId;
    public int originalRegionId;
    public int originalTerrainId;
    public int originalTerrainTileId;

    public float originalPositionX;
    public float originalPositionY;
    public float originalPositionZ;

    public int originalRotationX;
    public int originalRotationY;
    public int originalRotationZ;

    public float originalScaleMultiplier;

    public int originalAnimation;

    //Changed
    private bool changedObjectGraphicId;
    private bool changedRegionId;
    private bool changedTerrainId;
    private bool changedTerrainTileId;

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
            return  changedRegionId         || changedTerrainId || changedTerrainTileId || 
                    changedPositionX        || changedPositionY || changedPositionZ     || 
                    changedRotationX        || changedRotationY || changedRotationZ     || 
                    changedScaleMultiplier  || changedAnimation;
        }
    }

    #region Properties
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

    public int TerrainId
    {
        get { return terrainId; }
        set
        {
            if (value == terrainId) return;

            changedTerrainId = (value != originalTerrainId);

            terrainId = value;
        }
    }

    public int TerrainTileId
    {
        get { return terrainTileId; }
        set
        {
            if (value == terrainTileId) return;
            
            changedTerrainTileId = (value != originalTerrainTileId);

            terrainTileId = value;
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
    public void Create() { }

    public virtual void Update()
    {
        var worldObjectData = Fixtures.worldObjectList.Where(x => x.Id == Id).FirstOrDefault();

        if (changedRegionId)
            worldObjectData.regionId = regionId;

        if (changedTerrainId)
            worldObjectData.terrainId = terrainId;
        
        if (changedTerrainTileId)
            worldObjectData.terrainTileId = terrainTileId;

        if (changedPositionX)
            worldObjectData.positionX = positionX;

        if (changedPositionY)
            worldObjectData.positionY = positionY;

        if (changedPositionZ)
            worldObjectData.positionZ = positionZ;

        if (changedRotationX)
            worldObjectData.rotationX = rotationX;

        if (changedRotationY)
            worldObjectData.rotationY = rotationY;

        if (changedRotationZ)
            worldObjectData.rotationZ = rotationZ;

        if (changedScaleMultiplier)
            worldObjectData.scaleMultiplier = scaleMultiplier;

        if (changedAnimation)
            worldObjectData.animation = animation;
    }

    public virtual void UpdateSearch()
    {
        var worldObjectData = Fixtures.worldObjectList.Where(x => x.Id == Id).FirstOrDefault();

        if (changedObjectGraphicId)
            worldObjectData.objectGraphicId = objectGraphicId;

        originalObjectGraphicId = objectGraphicId;

        changedObjectGraphicId = false;
    }
    
    public void UpdateIndex() { }

    public virtual void SetOriginalValues()
    {
        originalRegionId = regionId;
        originalTerrainId = terrainId;
        originalTerrainTileId = terrainTileId;

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
        regionId = originalRegionId;
        terrainId = originalTerrainId;
        terrainTileId = originalTerrainTileId;

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

        changedRegionId = false;
        changedTerrainId = false;
        changedTerrainTileId = false;

        changedPositionX = false;
        changedPositionY = false;
        changedPositionZ = false;

        changedRotationX = false;
        changedRotationY = false;
        changedRotationZ = false;

        changedScaleMultiplier = false;

        changedAnimation = false;
    }

    public void Delete() { }
    #endregion

    new public virtual void Copy(IElementData dataSource)
    {
        var worldObjectDataSource = (WorldObjectElementData)dataSource;

        objectGraphicId = worldObjectDataSource.objectGraphicId;
        regionId = worldObjectDataSource.regionId;
        terrainId = worldObjectDataSource.terrainId;
        terrainTileId = worldObjectDataSource.terrainTileId;

        positionX = worldObjectDataSource.positionX;
        positionY = worldObjectDataSource.positionY;
        positionZ = worldObjectDataSource.positionZ;

        rotationX = worldObjectDataSource.rotationX;
        rotationY = worldObjectDataSource.rotationY;
        rotationZ = worldObjectDataSource.rotationZ;

        scaleMultiplier = worldObjectDataSource.scaleMultiplier;

        animation = worldObjectDataSource.animation;
    }
}
