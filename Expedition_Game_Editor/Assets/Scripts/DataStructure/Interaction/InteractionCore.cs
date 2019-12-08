using UnityEngine;
using System.Linq;

public class InteractionCore : GeneralData
{
    private int sceneInteractableId;
    private int regionId;
    private int terrainId;
    private int terrainTileId;

    private string description;

    private float positionX;
    private float positionY;
    private float positionZ;

    private int rotationX;
    private int rotationY;
    private int rotationZ;

    private float scaleMultiplier;

    private int animation;
    

    public int originalRegionId;
    public int originalTerrainId;
    public int originalTerrainTileId;

    public string originalDescription;

    public float originalPositionX;
    public float originalPositionY;
    public float originalPositionZ;

    public int originalRotationX;
    public int originalRotationY;
    public int originalRotationZ;

    public float originalScaleMultiplier;

    public int originalAnimation;


    private bool changedRegionId;
    private bool changedTerrainId;
    private bool changedTerrainTileId;

    private bool changedDescription;

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
            return  changedRegionId     || changedTerrainId || changedTerrainTileId     || changedDescription   || 
                    changedPositionX    || changedPositionY || changedPositionZ         || changedRotationX     || 
                    changedRotationY    || changedRotationZ || changedScaleMultiplier   || changedAnimation;
        }
    }

    #region Properties

    public int SceneInteractableId
    {
        get { return sceneInteractableId; }
        set { sceneInteractableId = value; }
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

    public string Description
    {
        get { return description; }
        set
        {
            if (value == description) return;

            changedDescription = (value != originalDescription);

            description = value;
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
        var interactionData = Fixtures.interactionList.Where(x => x.Id == Id).FirstOrDefault();
        
        if (changedRegionId)
            interactionData.regionId = regionId;

        if (changedTerrainId)
            interactionData.terrainId = terrainId;
        
        if (changedTerrainTileId)
            interactionData.terrainTileId = terrainTileId;

        if (changedDescription)
            interactionData.description = description;

        if (changedPositionX)
            interactionData.positionX = positionX;

        if (changedPositionY)
            interactionData.positionY = positionY;

        if (changedPositionZ)
            interactionData.positionZ = positionZ;

        if (changedRotationX)
            interactionData.rotationX = rotationX;

        if (changedRotationY)
            interactionData.rotationY = rotationY;

        if (changedRotationZ)
            interactionData.rotationZ = rotationZ;

        if (changedScaleMultiplier)
            interactionData.scaleMultiplier = scaleMultiplier;

        if (changedAnimation)
            interactionData.animation = animation;
    }

    public void UpdateSearch() { }

    public void UpdateIndex()
    {
        if (!changedIndex) return;

        var interactionData = Fixtures.interactionList.Where(x => x.Id == Id).FirstOrDefault();

        interactionData.Index = Index;

        changedIndex = false;
    }

    public virtual void SetOriginalValues()
    {
        originalRegionId = regionId;
        originalTerrainId = terrainId;
        originalTerrainTileId = terrainTileId;

        originalDescription = description;

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

        description = originalDescription;

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

        changedDescription = false;

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

    public void CopyCore(InteractionDataElement dataElement)
    {
        CopyGeneralData(dataElement);

        dataElement.sceneInteractableId = sceneInteractableId;
        dataElement.regionId = regionId;
        dataElement.terrainId = terrainId;
        dataElement.terrainTileId = terrainTileId;

        dataElement.description = description;

        dataElement.positionX = positionX;
        dataElement.positionY = positionY;
        dataElement.positionZ = positionZ;

        dataElement.rotationX = rotationX;
        dataElement.rotationY = rotationY;
        dataElement.rotationZ = rotationZ;

        dataElement.scaleMultiplier = scaleMultiplier;

        dataElement.animation = animation;


        dataElement.originalRegionId = originalRegionId;
        dataElement.originalTerrainId = originalTerrainId;
        dataElement.originalTerrainTileId = originalTerrainTileId;

        dataElement.originalDescription = originalDescription;

        dataElement.originalPositionX = originalPositionX;
        dataElement.originalPositionY = originalPositionY;
        dataElement.originalPositionZ = originalPositionZ;

        dataElement.originalRotationX = originalRotationX;
        dataElement.originalRotationY = originalRotationY;
        dataElement.originalRotationZ = originalRotationZ;

        dataElement.originalScaleMultiplier = originalScaleMultiplier;

        dataElement.originalAnimation = originalAnimation;

    }
    #endregion
}
