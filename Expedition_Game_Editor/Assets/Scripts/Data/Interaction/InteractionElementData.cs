using System.Collections.Generic;
using System.Linq;

public class InteractionElementData : InteractionCore, IElementData
{
    public DataElement DataElement { get; set; }

    public InteractionElementData() : base()
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

    public IElementData Clone()
    {
        var elementData = new InteractionElementData();
        
        elementData.timeConflict = timeConflict;
        elementData.containsActiveTime = containsActiveTime;
        elementData.defaultTimes = defaultTimes.ToList();

        elementData.worldInteractableId = worldInteractableId;
        elementData.questId = questId;
        elementData.objectiveId = objectiveId;

        elementData.objectGraphicId = objectGraphicId;
        elementData.objectGraphicPath = objectGraphicPath;

        elementData.objectGraphicIconPath = objectGraphicIconPath;
        
        elementData.height = height;
        elementData.width = width;
        elementData.depth = depth;

        elementData.interactableName = interactableName;
        elementData.locationName = locationName;

        CloneCore(elementData);

        return elementData;
    }

    public override void Copy(IElementData dataSource)
    {
        base.Copy(dataSource);

        var interactionDataSource = (InteractionElementData)dataSource;
        
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
