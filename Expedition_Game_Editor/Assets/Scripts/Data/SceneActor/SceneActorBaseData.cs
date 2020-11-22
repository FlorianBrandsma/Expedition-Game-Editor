using UnityEngine;

public class SceneActorBaseData
{
    public int Id                   { get; set; }

    public int SceneId              { get; set; }
    public int WorldInteractableId  { get; set; }
    public int TerrainId            { get; set; }
    public int TerrainTileId        { get; set; }
    
    public int SpeechMethod         { get; set; }
    public string SpeechText        { get; set; }
    public bool ShowTextBox         { get; set; }

    public int TargetSceneActorId   { get; set; }

    public bool ChangePosition      { get; set; }
    public bool FreezePosition      { get; set; }

    public float PositionX          { get; set; }
    public float PositionY          { get; set; }
    public float PositionZ          { get; set; }

    public bool ChangeRotation      { get; set; }
    public bool FaceTarget          { get; set; }

    public int RotationX            { get; set; }
    public int RotationY            { get; set; }
    public int RotationZ            { get; set; }

    public virtual void GetOriginalValues(SceneActorData originalData)
    {
        Id                  = originalData.Id;

        SceneId             = originalData.SceneId;
        WorldInteractableId = originalData.WorldInteractableId;
        TerrainId           = originalData.TerrainId;
        TerrainTileId       = originalData.TerrainTileId;
        
        SpeechMethod        = originalData.SpeechMethod;
        SpeechText          = originalData.SpeechText;
        ShowTextBox         = originalData.ShowTextBox;

        TargetSceneActorId  = originalData.TargetSceneActorId;

        ChangePosition      = originalData.ChangePosition;
        FreezePosition      = originalData.FreezePosition;

        PositionX           = originalData.PositionX;
        PositionY           = originalData.PositionY;
        PositionZ           = originalData.PositionZ;

        ChangeRotation      = originalData.ChangeRotation;
        FaceTarget          = originalData.FaceTarget;

        RotationX           = originalData.RotationX;
        RotationY           = originalData.RotationY;
        RotationZ           = originalData.RotationZ;
    }

    public virtual void Clone(SceneActorData data)
    {
        data.Id                     = Id;

        data.SceneId                = SceneId;
        data.WorldInteractableId    = WorldInteractableId;
        data.TerrainId              = TerrainId;
        data.TerrainTileId          = TerrainTileId;

        data.SpeechMethod           = SpeechMethod;
        data.SpeechText             = SpeechText;
        data.ShowTextBox            = ShowTextBox;

        data.TargetSceneActorId     = TargetSceneActorId;

        data.ChangePosition         = ChangePosition;
        data.FreezePosition         = FreezePosition;

        data.PositionX              = PositionX;
        data.PositionY              = PositionY;
        data.PositionZ              = PositionZ;

        data.ChangeRotation         = ChangeRotation;
        data.FaceTarget             = FaceTarget;

        data.RotationX              = RotationX;
        data.RotationY              = RotationY;
        data.RotationZ              = RotationZ;
    }
}

