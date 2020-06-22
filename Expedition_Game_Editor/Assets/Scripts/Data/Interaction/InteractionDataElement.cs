using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class InteractionDataElement : InteractionCore, IDataElement
{
    public DataElement DataElement { get; set; }

    public InteractionDataElement() : base()
    {
        DataType = Enums.DataType.Interaction;
    }
    
    public int questId;
    public int objectiveId;
    public int worldInteractableId;

    public int objectGraphicId;
    public string objectGraphicPath;

    public string objectGraphicIconPath;
    
    public float height;
    public float width;
    public float depth;

    public string interactableName;
    public string locationName;

    public bool timeConflict;
    
    public List<int> defaultTimes;

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
        
        dataElement.timeConflict = timeConflict;
        dataElement.containsActiveTime = containsActiveTime;
        dataElement.defaultTimes = defaultTimes.ToList();

        dataElement.worldInteractableId = worldInteractableId;
        dataElement.questId = questId;
        dataElement.objectiveId = objectiveId;

        dataElement.objectGraphicId = objectGraphicId;
        dataElement.objectGraphicPath = objectGraphicPath;

        dataElement.objectGraphicIconPath = objectGraphicIconPath;
        
        dataElement.height = height;
        dataElement.width = width;
        dataElement.depth = depth;

        dataElement.interactableName = interactableName;
        dataElement.locationName = locationName;

        CloneCore(dataElement);

        return dataElement;
    }

    public override void Copy(IDataElement dataSource)
    {
        base.Copy(dataSource);

        var interactionDataSource = (InteractionDataElement)dataSource;
        
        timeConflict = interactionDataSource.timeConflict;
        containsActiveTime = interactionDataSource.containsActiveTime;
        defaultTimes = interactionDataSource.defaultTimes.ToList();
        
        worldInteractableId = interactionDataSource.worldInteractableId;
        questId = interactionDataSource.questId;
        objectiveId = interactionDataSource.objectiveId;

        objectGraphicId = interactionDataSource.objectGraphicId;
        objectGraphicPath = interactionDataSource.objectGraphicPath;

        objectGraphicIconPath = interactionDataSource.objectGraphicIconPath;
        
        height = interactionDataSource.height;
        width = interactionDataSource.width;
        depth = interactionDataSource.depth;

        interactableName = interactionDataSource.interactableName;
        locationName = interactionDataSource.locationName;

        SetOriginalValues();
    }
}
