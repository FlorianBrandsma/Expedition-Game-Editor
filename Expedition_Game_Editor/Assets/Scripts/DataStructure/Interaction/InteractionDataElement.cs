using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class InteractionDataElement : InteractionCore, IDataElement
{
    public SelectionElement SelectionElement { get; set; }

    public InteractionDataElement() : base()
    {
        DataType = Enums.DataType.Interaction;
    }
    
    public int questId;
    public int objectiveId;
    public int worldInteractableId;

    public int objectGraphicId;
    public string objectGraphicPath;

    public string regionName;
    public string objectGraphicIconPath;

    public float height;
    public float width;
    public float depth;

    public Vector2 startPosition;

    public bool timeConflict;
    
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

        ClearChanges();
    }

    public new void GetOriginalValues() { }

    public override void ClearChanges()
    {
        if (!Changed) return;

        base.ClearChanges();

        GetOriginalValues();
    }

    public IDataElement Clone()
    {
        var dataElement = new InteractionDataElement();
        
        dataElement.SelectionElement = SelectionElement;

        dataElement.timeConflict = timeConflict;
        dataElement.containsActiveTime = containsActiveTime;
        dataElement.defaultTime = defaultTime;

        dataElement.worldInteractableId = worldInteractableId;
        dataElement.questId = questId;
        dataElement.objectiveId = objectiveId;

        dataElement.objectGraphicId = objectGraphicId;
        dataElement.objectGraphicPath = objectGraphicPath;

        dataElement.regionName = regionName;
        dataElement.objectGraphicIconPath = objectGraphicIconPath;

        dataElement.height = height;
        dataElement.width = width;
        dataElement.depth = depth;

        dataElement.startPosition = startPosition;

        CopyCore(dataElement);

        return dataElement;
    }

    public override void Copy(IDataElement dataSource)
    {
        base.Copy(dataSource);

        var interactionDataSource = (InteractionDataElement)dataSource;

        timeConflict = interactionDataSource.timeConflict;
        containsActiveTime = interactionDataSource.containsActiveTime;
        defaultTime = interactionDataSource.defaultTime;

        worldInteractableId = interactionDataSource.worldInteractableId;
        questId = interactionDataSource.questId;
        objectiveId = interactionDataSource.objectiveId;

        objectGraphicId = interactionDataSource.objectGraphicId;
        objectGraphicPath = interactionDataSource.objectGraphicPath;

        regionName = interactionDataSource.regionName;
        objectGraphicIconPath = interactionDataSource.objectGraphicIconPath;

        height = interactionDataSource.height;
        width = interactionDataSource.width;
        depth = interactionDataSource.depth;

        startPosition = interactionDataSource.startPosition;

        SetOriginalValues();
    }
}
