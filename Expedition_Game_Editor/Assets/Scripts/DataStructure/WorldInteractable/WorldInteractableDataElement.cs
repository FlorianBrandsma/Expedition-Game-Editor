﻿using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class WorldInteractableDataElement : WorldInteractableCore, IDataElement
{
    public SelectionElement SelectionElement { get; set; }

    public WorldInteractableDataElement() : base()
    {
        DataType = Enums.DataType.WorldInteractable;
    }

    public int terrainTileId;

    public int objectGraphicId;

    public bool isDefault;
    public int taskGroup;

    public string objectGraphicPath;

    public string interactableName;
    public string objectGraphicIconPath;

    public string originalInteractableName;
    public string originalObjectGraphicIconPath;

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

    public Vector2 startPosition;
    
    public int startTime;
    public int endTime;

    public int defaultTime;

    public bool containsActiveTime;

    public override void Update()
    {
        if (!Changed) return;

        base.Update();

        SetOriginalValues();
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

    public IDataElement Clone()
    {
        var dataElement = new WorldInteractableDataElement();
        
        dataElement.SelectionElement = SelectionElement;

        dataElement.terrainTileId = terrainTileId;

        dataElement.objectGraphicId = objectGraphicId;

        dataElement.isDefault = isDefault;
        dataElement.taskGroup = taskGroup;

        dataElement.objectGraphicPath = objectGraphicPath;

        dataElement.interactableName = interactableName;
        dataElement.objectGraphicIconPath = objectGraphicIconPath;

        dataElement.originalInteractableName = originalInteractableName;
        dataElement.originalObjectGraphicIconPath = originalObjectGraphicIconPath;

        dataElement.positionX = positionX;
        dataElement.positionY = positionY;
        dataElement.positionZ = positionZ;

        dataElement.rotationX = rotationX;
        dataElement.rotationY = rotationY;
        dataElement.rotationZ = rotationZ;

        dataElement.height = height;
        dataElement.width = width;
        dataElement.depth = depth;

        dataElement.scaleMultiplier = scaleMultiplier;

        dataElement.animation = animation;

        dataElement.startPosition = startPosition;
        
        dataElement.startTime = startTime;
        dataElement.endTime = endTime;

        dataElement.defaultTime = defaultTime;

        dataElement.containsActiveTime = containsActiveTime;

        CloneCore(dataElement);

        return dataElement;
    }

    public override void Copy(IDataElement dataSource)
    {
        base.Copy(dataSource);

        var worldInteractableDataSource = (WorldInteractableDataElement)dataSource;

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

        startPosition = worldInteractableDataSource.startPosition;
        
        startTime = worldInteractableDataSource.startTime;
        endTime = worldInteractableDataSource.endTime;

        defaultTime = worldInteractableDataSource.defaultTime;

        containsActiveTime = worldInteractableDataSource.containsActiveTime;

        SetOriginalValues();
    }
}
