using UnityEngine;

public class GameSceneActorData
{
    public int Id                           { get; set; }

    public int WorldInteractableId          { get; set; }
    public int TerrainTileId                { get; set; }
    
    public int SpeechMethod                 { get; set; }
    public string SpeechText                { get; set; }
    public bool ShowTextBox                 { get; set; }

    public int TargetWorldInteractableId    { get; set; }

    public bool ChangePosition              { get; set; }
    public bool FreezePosition              { get; set; }

    public float PositionX                  { get; set; }
    public float PositionY                  { get; set; }
    public float PositionZ                  { get; set; }

    public bool ChangeRotation              { get; set; }
    public bool FaceTarget                  { get; set; }

    public int RotationX                    { get; set; }
    public int RotationY                    { get; set; }
    public int RotationZ                    { get; set; }
    
    public virtual void GetOriginalValues(GameSceneActorData originalData)
    {
        Id                          = originalData.Id;

        WorldInteractableId         = originalData.WorldInteractableId;
        TerrainTileId               = originalData.TerrainTileId;
        
        SpeechMethod                = originalData.SpeechMethod;
        SpeechText                  = originalData.SpeechText;
        ShowTextBox                 = originalData.ShowTextBox;

        TargetWorldInteractableId   = originalData.TargetWorldInteractableId;

        ChangePosition              = originalData.ChangePosition;
        FreezePosition              = originalData.FreezePosition;

        PositionX                   = originalData.PositionX;
        PositionY                   = originalData.PositionY;
        PositionZ                   = originalData.PositionZ;

        ChangeRotation              = originalData.ChangeRotation;
        FaceTarget                  = originalData.FaceTarget;

        RotationX                   = originalData.RotationX;
        RotationY                   = originalData.RotationY;
        RotationZ                   = originalData.RotationZ;
    }

    public GameSceneActorData Clone()
    {
        var data = new GameSceneActorData();
        
        data.Id                         = Id;
        
        data.WorldInteractableId        = WorldInteractableId;
        data.TerrainTileId              = TerrainTileId;

        data.SpeechMethod               = SpeechMethod;
        data.SpeechText                 = SpeechText;
        data.ShowTextBox                = ShowTextBox;

        data.TargetWorldInteractableId  = TargetWorldInteractableId;

        data.ChangePosition             = ChangePosition;
        data.FreezePosition             = FreezePosition;

        data.PositionX                  = PositionX;
        data.PositionY                  = PositionY;
        data.PositionZ                  = PositionZ;

        data.ChangeRotation             = ChangeRotation;
        data.FaceTarget                 = FaceTarget;

        data.RotationX                  = RotationX;
        data.RotationY                  = RotationY;
        data.RotationZ                  = RotationZ;

        return data;
    }

    public virtual void Clone(GameSceneActorElementData elementData)
    {
        elementData.Id = Id;

        elementData.WorldInteractableId         = WorldInteractableId;
        elementData.TerrainTileId               = TerrainTileId;

        elementData.SpeechMethod                = SpeechMethod;
        elementData.SpeechText                  = SpeechText;
        elementData.ShowTextBox                 = ShowTextBox;

        elementData.TargetWorldInteractableId   = TargetWorldInteractableId;

        elementData.ChangePosition              = ChangePosition;
        elementData.FreezePosition              = FreezePosition;

        elementData.PositionX                   = PositionX;
        elementData.PositionY                   = PositionY;
        elementData.PositionZ                   = PositionZ;

        elementData.ChangeRotation              = ChangeRotation;
        elementData.FaceTarget                  = FaceTarget;

        elementData.RotationX                   = RotationX;
        elementData.RotationY                   = RotationY;
        elementData.RotationZ                   = RotationZ;
    }
}
