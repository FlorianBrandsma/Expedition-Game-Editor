using UnityEngine;

public class GameSceneShotData
{
    public int Id                                   { get; set; }

    public int Type                                 { get; set; }

    public bool ChangePosition                      { get; set; }

    public float PositionX                          { get; set; }
    public float PositionY                          { get; set; }
    public float PositionZ                          { get; set; }

    public int PositionTargetWorldInteractableId    { get; set; }

    public bool ChangeRotation                      { get; set; }

    public int RotationX                            { get; set; }
    public int RotationY                            { get; set; }
    public int RotationZ                            { get; set; }

    public int RotationTargetWorldInteractableId    { get; set; } 

    public string CameraFilterPath                  { get; set; }

    public virtual void GetOriginalValues(GameSceneShotData originalData)
    {
        Id                                  = originalData.Id;

        Type                                = originalData.Type;

        ChangePosition                      = originalData.ChangePosition;

        PositionX                           = originalData.PositionX;
        PositionY                           = originalData.PositionY;
        PositionZ                           = originalData.PositionZ;

        PositionTargetWorldInteractableId   = originalData.PositionTargetWorldInteractableId;

        ChangeRotation                      = originalData.ChangeRotation;

        RotationX                           = originalData.RotationX;
        RotationY                           = originalData.RotationY;
        RotationZ                           = originalData.RotationZ;

        RotationTargetWorldInteractableId   = originalData.RotationTargetWorldInteractableId;

        CameraFilterPath                    = originalData.CameraFilterPath;
    }

    public GameSceneShotData Clone()
    {
        var data = new GameSceneShotData();

        data.Id                                 = Id;

        data.Type                               = Type;

        data.ChangePosition                     = ChangePosition;

        data.PositionX                          = PositionX;
        data.PositionY                          = PositionY;
        data.PositionZ                          = PositionZ;

        data.PositionTargetWorldInteractableId  = PositionTargetWorldInteractableId;

        data.ChangeRotation                     = ChangeRotation;

        data.RotationX                          = RotationX;
        data.RotationY                          = RotationY;
        data.RotationZ                          = RotationZ;

        data.RotationTargetWorldInteractableId  = RotationTargetWorldInteractableId;

        data.CameraFilterPath                   = CameraFilterPath;

        return data;
    }

    public virtual void Clone(GameSceneShotElementData elementData)
    {
        elementData.Id                                  = Id;

        elementData.Type                                = Type;

        elementData.ChangePosition                      = ChangePosition;

        elementData.PositionX                           = PositionX;
        elementData.PositionY                           = PositionY;
        elementData.PositionZ                           = PositionZ;

        elementData.PositionTargetWorldInteractableId   = PositionTargetWorldInteractableId;

        elementData.ChangeRotation                      = ChangeRotation;

        elementData.RotationX                           = RotationX;
        elementData.RotationY                           = RotationY;
        elementData.RotationZ                           = RotationZ;

        elementData.RotationTargetWorldInteractableId   = RotationTargetWorldInteractableId;

        elementData.CameraFilterPath                    = CameraFilterPath;
    }
}
