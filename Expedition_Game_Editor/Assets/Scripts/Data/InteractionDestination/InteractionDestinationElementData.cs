using UnityEngine;

public class InteractionDestinationElementData : InteractionDestinationCore, IElementData
{
    public DataElement DataElement { get; set; }

    public InteractionDestinationElementData() : base()
    {
        DataType = Enums.DataType.InteractionDestination;
    }

    public int questId;
    public int objectiveId;
    public int worldInteractableId;
    public int taskId;

    public int objectGraphicId;
    public string objectGraphicPath;

    public string objectGraphicIconPath;

    public string interactableName;

    public float height;
    public float width;
    public float depth;
    
    public float scaleMultiplier;

    public string tileIconPath;
    public float tileSize;

    public Vector2 localPosition;

    public string locationName;
    public string interactableStatus;

    public bool isDefault;

    public int startTime;
    public int endTime;

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

    public IElementData Clone()
    {
        var elementData = new InteractionDestinationElementData();

        elementData.questId = questId;
        elementData.objectiveId = objectiveId;
        elementData.worldInteractableId = worldInteractableId;
        elementData.taskId = taskId;

        elementData.objectGraphicId = objectGraphicId;
        elementData.objectGraphicPath = objectGraphicPath;
        
        elementData.objectGraphicIconPath = objectGraphicIconPath;

        elementData.interactableName = interactableName;

        elementData.height = height;
        elementData.width = width;
        elementData.depth = depth;

        elementData.scaleMultiplier = scaleMultiplier;

        elementData.tileIconPath = tileIconPath;
        elementData.tileSize = tileSize;

        elementData.localPosition = localPosition;
        
        elementData.locationName = locationName;
        elementData.interactableStatus = interactableStatus;

        elementData.isDefault = isDefault;

        elementData.startTime = startTime;
        elementData.endTime = endTime;

        elementData.containsActiveTime = containsActiveTime;

        CloneCore(elementData);

        return elementData;
    }

    public override void Copy(IElementData dataSource)
    {
        base.Copy(dataSource);

        var interactionDestinationDataSource = (InteractionDestinationElementData)dataSource;
        
        questId = interactionDestinationDataSource.questId;
        objectiveId = interactionDestinationDataSource.objectiveId;
        worldInteractableId = interactionDestinationDataSource.worldInteractableId;
        taskId = interactionDestinationDataSource.taskId;

        objectGraphicId = interactionDestinationDataSource.objectGraphicId;
        objectGraphicPath = interactionDestinationDataSource.objectGraphicPath;

        objectGraphicIconPath = interactionDestinationDataSource.objectGraphicIconPath;

        interactableName = interactionDestinationDataSource.interactableName;

        height = interactionDestinationDataSource.height;
        width = interactionDestinationDataSource.width;
        depth = interactionDestinationDataSource.depth;

        scaleMultiplier = interactionDestinationDataSource.scaleMultiplier;

        tileIconPath = interactionDestinationDataSource.tileIconPath;
        tileSize = interactionDestinationDataSource.tileSize;

        localPosition = interactionDestinationDataSource.localPosition;
        
        locationName = interactionDestinationDataSource.locationName;
        interactableStatus = interactionDestinationDataSource.interactableStatus;

        isDefault = interactionDestinationDataSource.isDefault;

        startTime = interactionDestinationDataSource.startTime;
        endTime = interactionDestinationDataSource.endTime;

        containsActiveTime = interactionDestinationDataSource.containsActiveTime;

        SetOriginalValues();
    }
}
