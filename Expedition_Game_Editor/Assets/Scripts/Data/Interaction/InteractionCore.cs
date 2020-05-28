using UnityEngine;
using System.Linq;

public class InteractionCore : GeneralData
{
    private int taskId;
    private int regionId;
    private int terrainId;
    private int terrainTileId;

    private bool isDefault;

    private int startTime;
    private int endTime;

    private string publicNotes;
    private string privateNotes;

    private float positionX;
    private float positionY;
    private float positionZ;

    private int rotationX;
    private int rotationY;
    private int rotationZ;

    private float scaleMultiplier;

    private int animation;
    
    //Original
    public int originalRegionId;
    public int originalTerrainId;
    public int originalTerrainTileId;

    public int originalStartTime;
    public int originalEndTime;

    public string originalPublicNotes;
    public string originalPrivateNotes;

    public float originalPositionX;
    public float originalPositionY;
    public float originalPositionZ;

    public int originalRotationX;
    public int originalRotationY;
    public int originalRotationZ;

    public float originalScaleMultiplier;

    public int originalAnimation;

    //Original
    private bool changedRegionId;
    private bool changedTerrainId;
    private bool changedTerrainTileId;

    public bool changedStartTime;
    public bool changedEndTime;

    private bool changedPublicNotes;
    private bool changedPrivateNotes;

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
            return  changedRegionId     || changedTerrainId         || changedTerrainTileId || changedStartTime ||
                    changedEndTime      || changedPublicNotes       || changedPrivateNotes  || changedPositionX || 
                    changedPositionY    || changedPositionZ         || changedRotationX     || changedRotationY || 
                    changedRotationZ    || changedScaleMultiplier   || changedAnimation;
        }
    }

    #region Properties
    public int TaskId
    {
        get { return taskId; }
        set { taskId = value; }
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

    public bool Default
    {
        get { return isDefault; }
        set { isDefault = value; }
    }

    public int StartTime
    {
        get { return startTime; }
        set
        {
            if (value == startTime) return;

            changedStartTime = (value != originalStartTime);

            startTime = value;
        }
    }

    public int EndTime
    {
        get { return endTime; }
        set
        {
            if (value == endTime) return;

            changedEndTime = (value != originalEndTime);

            endTime = value;
        }
    }

    public string PublicNotes
    {
        get { return publicNotes; }
        set
        {
            if (value == publicNotes) return;

            changedPublicNotes = (value != originalPublicNotes);

            publicNotes = value;
        }
    }

    public string PrivateNotes
    {
        get { return privateNotes; }
        set
        {
            if (value == privateNotes) return;

            changedPrivateNotes = (value != originalPrivateNotes);

            privateNotes = value;
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

        if (changedStartTime)
            interactionData.startTime = startTime;

        if (changedEndTime)
            interactionData.endTime = endTime;

        if (changedPublicNotes)
            interactionData.publicNotes = publicNotes;

        if (changedPrivateNotes)
            interactionData.privateNotes = privateNotes;

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

        originalStartTime = startTime;
        originalEndTime = endTime;

        originalPublicNotes = publicNotes;
        originalPrivateNotes = privateNotes;

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

        startTime = originalStartTime;
        endTime = originalEndTime;

        publicNotes = originalPublicNotes;
        privateNotes = originalPrivateNotes;

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

        changedStartTime = false;
        changedEndTime = false;

        changedPublicNotes = false;
        changedPrivateNotes = false;

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

    public void CloneCore(InteractionDataElement dataElement)
    {
        CloneGeneralData(dataElement);

        dataElement.taskId = taskId;
        dataElement.regionId = regionId;
        dataElement.terrainId = terrainId;
        dataElement.terrainTileId = terrainTileId;

        dataElement.isDefault = isDefault;

        dataElement.startTime = startTime;
        dataElement.endTime = endTime;

        dataElement.publicNotes = publicNotes;
        dataElement.privateNotes = privateNotes;

        dataElement.positionX = positionX;
        dataElement.positionY = positionY;
        dataElement.positionZ = positionZ;

        dataElement.rotationX = rotationX;
        dataElement.rotationY = rotationY;
        dataElement.rotationZ = rotationZ;

        dataElement.scaleMultiplier = scaleMultiplier;

        dataElement.animation = animation;

        //Original
        dataElement.originalRegionId = originalRegionId;
        dataElement.originalTerrainId = originalTerrainId;
        dataElement.originalTerrainTileId = originalTerrainTileId;

        dataElement.originalStartTime = originalStartTime;
        dataElement.originalEndTime = originalEndTime;

        dataElement.originalPublicNotes = originalPublicNotes;
        dataElement.originalPrivateNotes = originalPrivateNotes;

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

    new public virtual void Copy(IDataElement dataSource)
    {
        var interactionDataSource = (InteractionDataElement)dataSource;

        taskId = interactionDataSource.taskId;
        regionId = interactionDataSource.regionId;
        terrainId = interactionDataSource.terrainId;
        terrainTileId = interactionDataSource.terrainTileId;

        isDefault = interactionDataSource.isDefault;

        startTime = interactionDataSource.startTime;
        endTime = interactionDataSource.endTime;

        publicNotes = interactionDataSource.publicNotes;
        privateNotes = interactionDataSource.privateNotes;

        positionX = interactionDataSource.positionX;
        positionY = interactionDataSource.positionY;
        positionZ = interactionDataSource.positionZ;

        rotationX = interactionDataSource.rotationX;
        rotationY = interactionDataSource.rotationY;
        rotationZ = interactionDataSource.rotationZ;

        scaleMultiplier = interactionDataSource.scaleMultiplier;

        animation = interactionDataSource.animation;
    }
}
