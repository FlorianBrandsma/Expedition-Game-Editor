using UnityEngine;
using System.Collections.Generic;
using System.Linq;

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

    public string objectGraphicIconPath;

    public string regionName;
    
    public float height;
    public float width;
    public float depth;

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
        
        dataElement.SelectionElement = SelectionElement;

        dataElement.timeConflict = timeConflict;
        dataElement.containsActiveTime = containsActiveTime;
        dataElement.defaultTimes = defaultTimes.ToList();

        dataElement.worldInteractableId = worldInteractableId;
        dataElement.questId = questId;
        dataElement.objectiveId = objectiveId;

        dataElement.objectGraphicId = objectGraphicId;
        dataElement.objectGraphicPath = objectGraphicPath;

        dataElement.objectGraphicIconPath = objectGraphicIconPath;

        dataElement.regionName = regionName;
        
        dataElement.height = height;
        dataElement.width = width;
        dataElement.depth = depth;

        CloneCore(dataElement);

        return dataElement;
    }

    public override void Copy(IDataElement dataSource)
    {
        base.Copy(dataSource);

        var interactionDataSource = (InteractionDataElement)dataSource;

        //Don't copy selection element: interactions can belong to different types

        timeConflict = interactionDataSource.timeConflict;
        containsActiveTime = interactionDataSource.containsActiveTime;
        defaultTimes = interactionDataSource.defaultTimes.ToList();
        
        worldInteractableId = interactionDataSource.worldInteractableId;
        questId = interactionDataSource.questId;
        objectiveId = interactionDataSource.objectiveId;

        objectGraphicId = interactionDataSource.objectGraphicId;
        objectGraphicPath = interactionDataSource.objectGraphicPath;

        objectGraphicIconPath = interactionDataSource.objectGraphicIconPath;

        regionName = interactionDataSource.regionName;
        
        height = interactionDataSource.height;
        width = interactionDataSource.width;
        depth = interactionDataSource.depth;

        SetOriginalValues();
    }
}
