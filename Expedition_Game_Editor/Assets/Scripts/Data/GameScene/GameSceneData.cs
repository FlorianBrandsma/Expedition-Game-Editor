using UnityEngine;
using System.Collections.Generic;

public class GameSceneData
{
    public int Id                   { get; set; }

    public int RegionId             { get; set; }

    public bool FreezeTime          { get; set; }
    public bool AutoContinue        { get; set; }
    public bool SetActorsInstantly  { get; set; }

    public float SceneDuration      { get; set; }
    public float ShotDuration       { get; set; }
    
    public List<GameSceneShotElementData> SceneShotDataList     { get; set; } = new List<GameSceneShotElementData>();
    public List<GameSceneActorElementData> SceneActorDataList   { get; set; } = new List<GameSceneActorElementData>();
    public List<GameScenePropElementData> ScenePropDataList     { get; set; } = new List<GameScenePropElementData>();

    public virtual void GetOriginalValues(GameSceneData originalData)
    {
        Id                  = originalData.Id;

        RegionId            = originalData.RegionId;

        FreezeTime          = originalData.FreezeTime;
        AutoContinue        = originalData.AutoContinue;
        SetActorsInstantly  = originalData.SetActorsInstantly;

        SceneDuration       = originalData.SceneDuration;
        ShotDuration        = originalData.ShotDuration;
    }

    public GameSceneData Clone()
    {
        var data = new GameSceneData();
        
        data.Id                 = Id;

        data.RegionId           = RegionId;

        data.FreezeTime         = FreezeTime;
        data.AutoContinue       = AutoContinue;
        data.SetActorsInstantly = SetActorsInstantly;

        data.SceneDuration      = SceneDuration;
        data.ShotDuration       = ShotDuration;

        SceneShotDataList.ForEach(x => x.SetOriginalValues());
        SceneActorDataList.ForEach(x => x.SetOriginalValues());
        ScenePropDataList.ForEach(x => x.SetOriginalValues());

        return data;
    }

    public virtual void Clone(GameSceneElementData elementData)
    {
        elementData.Id                  = Id;

        elementData.RegionId            = RegionId;

        elementData.FreezeTime          = FreezeTime;
        elementData.AutoContinue        = AutoContinue;
        elementData.SetActorsInstantly  = SetActorsInstantly;

        elementData.SceneDuration       = SceneDuration;
        elementData.ShotDuration        = ShotDuration;
    }
}
