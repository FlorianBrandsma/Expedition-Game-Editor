using UnityEngine;

public class SceneShotBaseData
{
    public int Id                           { get; set; }

    public int SceneId                      { get; set; }

    public int Type                         { get; set; }

    public bool ChangePosition              { get; set; }

    public float PositionX                  { get; set; }
    public float PositionY                  { get; set; }
    public float PositionZ                  { get; set; }

    public int PositionTargetSceneActorId   { get; set; }

    public bool ChangeRotation              { get; set; }

    public int RotationX                    { get; set; }
    public int RotationY                    { get; set; }
    public int RotationZ                    { get; set; }

    public int RotationTargetSceneActorId   { get; set; } 

    public virtual void GetOriginalValues(SceneShotData originalData)
    {
        Id                          = originalData.Id;

        SceneId                     = originalData.SceneId;

        Type                        = originalData.Type;

        ChangePosition              = originalData.ChangePosition;

        PositionX                   = originalData.PositionX;
        PositionY                   = originalData.PositionY;
        PositionZ                   = originalData.PositionZ;

        PositionTargetSceneActorId  = originalData.PositionTargetSceneActorId;

        ChangeRotation              = originalData.ChangeRotation;

        RotationX                   = originalData.RotationX;
        RotationY                   = originalData.RotationY;
        RotationZ                   = originalData.RotationZ;

        RotationTargetSceneActorId  = originalData.RotationTargetSceneActorId;
    }

    public virtual void Clone(SceneShotData data)
    {
        data.Id                         = Id;

        data.SceneId                    = SceneId;

        data.Type                       = Type;

        data.ChangePosition             = ChangePosition;

        data.PositionX                  = PositionX;
        data.PositionY                  = PositionY;
        data.PositionZ                  = PositionZ;

        data.PositionTargetSceneActorId = PositionTargetSceneActorId;

        data.ChangeRotation             = ChangeRotation;

        data.RotationX                  = RotationX;
        data.RotationY                  = RotationY;
        data.RotationZ                  = RotationZ;

        data.RotationTargetSceneActorId = RotationTargetSceneActorId;
    }
}
