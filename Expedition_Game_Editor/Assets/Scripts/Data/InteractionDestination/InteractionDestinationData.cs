using UnityEngine;

public class InteractionDestinationData : InteractionDestinationBaseData
{
    public int TaskId                   { get; set; }
    public int WorldInteractableId      { get; set; }
    public int ObjectiveId              { get; set; }
    public int QuestId                  { get; set; }
    
    public int ModelId                  { get; set; }
    public string ModelPath             { get; set; }

    public string ModelIconPath         { get; set; }

    public string InteractableName      { get; set; }

    public float Height                 { get; set; }
    public float Width                  { get; set; }
    public float Depth                  { get; set; }

    public float Scale                  { get; set; }

    public string TileIconPath          { get; set; }
    public float TileSize               { get; set; }

    public Vector2 LocalPosition        { get; set; }

    public string LocationName          { get; set; }
    public string InteractableStatus    { get; set; }

    public bool DefaultInteraction      { get; set; }

    public int StartTime                { get; set; }
    public int EndTime                  { get; set; }

    public bool ContainsActiveTime      { get; set; }

    public override void GetOriginalValues(InteractionDestinationData originalData)
    {
        TaskId              = originalData.TaskId;
        WorldInteractableId = originalData.WorldInteractableId;
        ObjectiveId         = originalData.ObjectiveId;
        QuestId             = originalData.QuestId;
        
        ModelId             = originalData.ModelId;
        ModelPath           = originalData.ModelPath;

        ModelIconPath       = originalData.ModelIconPath;

        InteractableName    = originalData.InteractableName;

        Height              = originalData.Height;
        Width               = originalData.Width;
        Depth               = originalData.Depth;

        Scale               = originalData.Scale;

        TileIconPath        = originalData.TileIconPath;
        TileSize            = originalData.TileSize;

        LocalPosition       = originalData.LocalPosition;

        LocationName        = originalData.LocationName;
        InteractableStatus  = originalData.InteractableStatus;

        DefaultInteraction  = originalData.DefaultInteraction;

        StartTime           = originalData.StartTime;
        EndTime             = originalData.EndTime;

        ContainsActiveTime  = originalData.ContainsActiveTime;

        base.GetOriginalValues(originalData);
    }

    public InteractionDestinationData Clone()
    {
        var data = new InteractionDestinationData();
        
        data.TaskId                 = TaskId;
        data.WorldInteractableId    = WorldInteractableId;
        data.ObjectiveId            = ObjectiveId;
        data.QuestId                = QuestId;
        
        data.ModelId                = ModelId;
        data.ModelPath              = ModelPath;

        data.ModelIconPath          = ModelIconPath;

        data.InteractableName       = InteractableName;

        data.Height                 = Height;
        data.Width                  = Width;
        data.Depth                  = Depth;

        data.Scale                  = Scale;

        data.TileIconPath           = TileIconPath;
        data.TileSize               = TileSize;

        data.LocalPosition          = LocalPosition;

        data.LocationName           = LocationName;
        data.InteractableStatus     = InteractableStatus;

        data.DefaultInteraction     = DefaultInteraction;

        data.StartTime              = StartTime;
        data.EndTime                = EndTime;

        data.ContainsActiveTime     = ContainsActiveTime;

        base.Clone(data);

        return data;
    }

    public virtual void Clone(InteractionDestinationElementData elementData)
    {
        elementData.TaskId              = TaskId;
        elementData.WorldInteractableId = WorldInteractableId;
        elementData.ObjectiveId         = ObjectiveId;
        elementData.QuestId             = QuestId;
        
        elementData.ModelId             = ModelId;
        elementData.ModelPath           = ModelPath;

        elementData.ModelIconPath       = ModelIconPath;

        elementData.InteractableName    = InteractableName;

        elementData.Height              = Height;
        elementData.Width               = Width;
        elementData.Depth               = Depth;

        elementData.Scale               = Scale;

        elementData.TileIconPath        = TileIconPath;
        elementData.TileSize            = TileSize;

        elementData.LocalPosition       = LocalPosition;

        elementData.LocationName        = LocationName;
        elementData.InteractableStatus  = InteractableStatus;

        elementData.DefaultInteraction  = DefaultInteraction;

        elementData.StartTime           = StartTime;
        elementData.EndTime             = EndTime;

        elementData.ContainsActiveTime  = ContainsActiveTime;

        base.Clone(elementData);
    }
}
