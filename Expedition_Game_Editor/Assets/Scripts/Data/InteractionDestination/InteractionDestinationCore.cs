using UnityEngine;
using System.Linq;

public class InteractionDestinationCore : GeneralData
{
    private int interactionId;

    private int regionId;
    private int terrainId;
    private int terrainTileId;

    private float positionX;
    private float positionY;
    private float positionZ;

    private float positionVariance;
    
    private int rotationX;
    private int rotationY;
    private int rotationZ;

    private bool freeRotation;

    private int animation;
    private float patience;

    //Original
    public int originalRegionId;
    public int originalTerrainId;
    public int originalTerrainTileId;

    public float originalPositionX;
    public float originalPositionY;
    public float originalPositionZ;

    public float originalPositionVariance;
    
    public int originalRotationX;
    public int originalRotationY;
    public int originalRotationZ;

    public bool originalFreeRotation;

    public int originalAnimation;
    public float originalPatience;

    //Changed
    private bool changedRegionId;
    private bool changedTerrainId;
    private bool changedTerrainTileId;

    private bool changedPositionX;
    private bool changedPositionY;
    private bool changedPositionZ;

    private bool changedPositionVariance;
    
    private bool changedRotationX;
    private bool changedRotationY;
    private bool changedRotationZ;

    private bool changedFreeRotation;

    private bool changedAnimation;
    private bool changedPatience;

    public bool Changed
    {
        get
        {
            return  changedRegionId     || changedTerrainId || changedTerrainTileId ||
                    changedPositionX    || changedPositionY || changedPositionZ     || changedPositionVariance  ||
                    changedRotationX    || changedRotationY || changedRotationZ     || changedFreeRotation      ||
                    changedAnimation    || changedPatience;
        }
    }

    #region Properties
    public int InteractionId
    {
        get { return interactionId; }
        set { interactionId = value; }
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

    public float PositionVariance
    {
        get { return positionVariance; }
        set
        {
            if (value == positionVariance) return;

            changedPositionVariance = (value != originalPositionVariance);

            positionVariance = value;
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

    public bool FreeRotation
    {
        get { return freeRotation; }
        set
        {
            if (value == freeRotation) return;

            changedFreeRotation = (value != originalFreeRotation);

            freeRotation = value;
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

    public float Patience
    {
        get { return patience; }
        set
        {
            if (value == patience) return;

            changedPatience = (value != originalPatience);

            patience = value;
        }
    }
    #endregion

    #region Methods
    public void Create() { }

    public virtual void Update()
    {
        var interactionDestinationData = Fixtures.interactionDestinationList.Where(x => x.id == Id).FirstOrDefault();

        if (changedRegionId)
            interactionDestinationData.regionId = regionId;

        if (changedTerrainId)
            interactionDestinationData.terrainId = terrainId;

        if (changedTerrainTileId)
            interactionDestinationData.terrainTileId = terrainTileId;

        if (changedPositionX)
            interactionDestinationData.positionX = positionX;

        if (changedPositionY)
            interactionDestinationData.positionY = positionY;

        if (changedPositionZ)
            interactionDestinationData.positionZ = positionZ;

        if (changedPositionVariance)
            interactionDestinationData.positionVariance = positionVariance;

        if (changedRotationX)
            interactionDestinationData.rotationX = rotationX;

        if (changedRotationY)
            interactionDestinationData.rotationY = rotationY;

        if (changedRotationZ)
            interactionDestinationData.rotationZ = rotationZ;

        if (changedFreeRotation)
            interactionDestinationData.freeRotation = freeRotation;

        if (changedAnimation)
            interactionDestinationData.animation = animation;

        if (changedPatience)
            interactionDestinationData.patience = patience;

        SetOriginalValues();
    }

    public void UpdateSearch() { }

    public void UpdateIndex() { }

    public virtual void SetOriginalValues()
    {
        originalRegionId = regionId;
        originalTerrainId = terrainId;
        originalTerrainTileId = terrainTileId;
        
        originalPositionX = positionX;
        originalPositionY = positionY;
        originalPositionZ = positionZ;

        originalPositionVariance = positionVariance;

        originalRotationX = rotationX;
        originalRotationY = rotationY;
        originalRotationZ = rotationZ;

        originalFreeRotation = freeRotation;

        originalAnimation = animation;
        originalPatience = patience;
    }

    public void GetOriginalValues()
    {
        regionId = originalRegionId;
        terrainId = originalTerrainId;
        terrainTileId = originalTerrainTileId;
        
        positionX = originalPositionX;
        positionY = originalPositionY;
        positionZ = originalPositionZ;

        positionVariance = originalPositionVariance;

        rotationX = originalRotationX;
        rotationY = originalRotationY;
        rotationZ = originalRotationZ;

        freeRotation = originalFreeRotation;

        animation = originalAnimation;
        patience = originalPatience;
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

        changedPositionVariance = false;

        changedRotationX = false;
        changedRotationY = false;
        changedRotationZ = false;

        changedFreeRotation = false;

        changedAnimation = false;
        changedPatience = false;
    }

    public void Delete() { }

    public void CloneCore(InteractionDestinationElementData elementData)
    {
        CloneGeneralData(elementData);

        elementData.interactionId = interactionId;

        elementData.regionId = regionId;
        elementData.terrainId = terrainId;
        elementData.terrainTileId = terrainTileId;
        
        elementData.positionX = positionX;
        elementData.positionY = positionY;
        elementData.positionZ = positionZ;

        elementData.positionVariance = positionVariance;

        elementData.rotationX = rotationX;
        elementData.rotationY = rotationY;
        elementData.rotationZ = rotationZ;

        elementData.freeRotation = freeRotation;

        elementData.animation = animation;
        elementData.patience = patience;
        

        //Original
        elementData.originalRegionId = originalRegionId;
        elementData.originalTerrainId = originalTerrainId;
        elementData.originalTerrainTileId = originalTerrainTileId;
        
        elementData.originalPositionX = originalPositionX;
        elementData.originalPositionY = originalPositionY;
        elementData.originalPositionZ = originalPositionZ;

        elementData.originalPositionVariance = originalPositionVariance;

        elementData.originalRotationX = originalRotationX;
        elementData.originalRotationY = originalRotationY;
        elementData.originalRotationZ = originalRotationZ;

        elementData.originalFreeRotation = originalFreeRotation;

        elementData.originalAnimation = originalAnimation;
        elementData.originalPatience = originalPatience;
    }
    #endregion

    new public virtual void Copy(IElementData dataSource)
    {
        var interactionDestinationDataSource = (InteractionDestinationElementData)dataSource;

        interactionId = interactionDestinationDataSource.interactionId;

        regionId = interactionDestinationDataSource.regionId;
        terrainId = interactionDestinationDataSource.terrainId;
        terrainTileId = interactionDestinationDataSource.terrainTileId;

        positionX = interactionDestinationDataSource.positionX;
        positionY = interactionDestinationDataSource.positionY;
        positionZ = interactionDestinationDataSource.positionZ;

        positionVariance = interactionDestinationDataSource.positionVariance;

        rotationX = interactionDestinationDataSource.rotationX;
        rotationY = interactionDestinationDataSource.rotationY;
        rotationZ = interactionDestinationDataSource.rotationZ;

        freeRotation = interactionDestinationDataSource.freeRotation;

        animation = interactionDestinationDataSource.animation;
        patience = interactionDestinationDataSource.patience;
    }
}
