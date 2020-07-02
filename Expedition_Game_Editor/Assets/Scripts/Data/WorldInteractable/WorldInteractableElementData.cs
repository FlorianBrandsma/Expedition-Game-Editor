using UnityEngine;
using System.Collections.Generic;

public class WorldInteractableElementData : WorldInteractableCore, IElementData
{
    public DataElement DataElement { get; set; }

    public WorldInteractableElementData() : base()
    {
        DataType = Enums.DataType.WorldInteractable;
    }

    public Enums.ElementStatus elementStatus;

    public int terrainTileId;

    public int objectGraphicId;

    public bool isDefault;
    public int taskGroup;

    public string objectGraphicPath;

    public string interactableName;
    public string objectGraphicIconPath;
    
    public float positionX;
    public float positionY;
    public float positionZ;

    public int rotationX;
    public int rotationY;
    public int rotationZ;

    public float height;
    public float width;
    public float depth;

    public float scaleMultiplier;

    public int animation;

    public int startTime;
    public int endTime;

    public bool containsActiveTime;

    //Original
    public string originalInteractableName;
    public string originalObjectGraphicIconPath;

    //List
    public List<InteractionElementData> interactionDataList = new List<InteractionElementData>();

    public override void Update()
    {
        if (!Changed) return;

        base.Update();

        SetOriginalValues();
    }

    public override void UpdateSearch()
    {
        base.UpdateSearch();

        originalInteractableName = interactableName;
        originalObjectGraphicIconPath = objectGraphicIconPath;
    }

    public override void SetOriginalValues()
    {
        base.SetOriginalValues();

        originalInteractableName = interactableName;
        originalObjectGraphicIconPath = objectGraphicIconPath;

        ClearChanges();
    }

    public new void GetOriginalValues()
    {
        interactableName = originalInteractableName;
        objectGraphicIconPath = originalObjectGraphicIconPath;
    }

    public override void ClearChanges()
    {
        if (!Changed) return;

        base.ClearChanges();

        GetOriginalValues();
    }

    public IElementData Clone()
    {
        var elementData = new WorldInteractableElementData();

        elementData.DataElement = DataElement;

        elementData.elementStatus = elementStatus;

        elementData.terrainTileId = terrainTileId;

        elementData.objectGraphicId = objectGraphicId;

        elementData.isDefault = isDefault;
        elementData.taskGroup = taskGroup;

        elementData.objectGraphicPath = objectGraphicPath;

        elementData.interactableName = interactableName;
        elementData.objectGraphicIconPath = objectGraphicIconPath;

        elementData.originalInteractableName = originalInteractableName;
        elementData.originalObjectGraphicIconPath = originalObjectGraphicIconPath;

        elementData.positionX = positionX;
        elementData.positionY = positionY;
        elementData.positionZ = positionZ;

        elementData.rotationX = rotationX;
        elementData.rotationY = rotationY;
        elementData.rotationZ = rotationZ;

        elementData.height = height;
        elementData.width = width;
        elementData.depth = depth;

        elementData.scaleMultiplier = scaleMultiplier;

        elementData.animation = animation;
        
        elementData.startTime = startTime;
        elementData.endTime = endTime;

        elementData.containsActiveTime = containsActiveTime;

        CloneCore(elementData);

        return elementData;
    }

    public override void Copy(IElementData dataSource)
    {
        base.Copy(dataSource);

        var worldInteractableDataSource = (WorldInteractableElementData)dataSource;

        elementStatus = worldInteractableDataSource.elementStatus;

        terrainTileId = worldInteractableDataSource.terrainTileId;

        objectGraphicId = worldInteractableDataSource.objectGraphicId;

        isDefault = worldInteractableDataSource.isDefault;
        taskGroup = worldInteractableDataSource.taskGroup;

        objectGraphicPath = worldInteractableDataSource.objectGraphicPath;

        interactableName = worldInteractableDataSource.interactableName;
        objectGraphicIconPath = worldInteractableDataSource.objectGraphicIconPath;

        originalInteractableName = worldInteractableDataSource.originalInteractableName;
        originalObjectGraphicIconPath = worldInteractableDataSource.originalObjectGraphicIconPath;

        positionX = worldInteractableDataSource.positionX;
        positionY = worldInteractableDataSource.positionY;
        positionZ = worldInteractableDataSource.positionZ;

        rotationX = worldInteractableDataSource.rotationX;
        rotationY = worldInteractableDataSource.rotationY;
        rotationZ = worldInteractableDataSource.rotationZ;

        height = worldInteractableDataSource.height;
        width = worldInteractableDataSource.width;
        depth = worldInteractableDataSource.depth;

        scaleMultiplier = worldInteractableDataSource.scaleMultiplier;

        animation = worldInteractableDataSource.animation;

        startTime = worldInteractableDataSource.startTime;
        endTime = worldInteractableDataSource.endTime;

        containsActiveTime = worldInteractableDataSource.containsActiveTime;

        SetOriginalValues();
    }
}
