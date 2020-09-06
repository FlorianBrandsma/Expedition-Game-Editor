using UnityEngine;
using System.Collections.Generic;

public class GameSaveData
{
    public int Id                               { get; set; }

    public PlayerSaveElementData PlayerSaveData { get; set; }

    public List<ChapterSaveElementData> ChapterSaveDataList         { get; set; } = new List<ChapterSaveElementData>();
    public List<PhaseSaveElementData> PhaseSaveDataList             { get; set; } = new List<PhaseSaveElementData>();
    public List<QuestSaveElementData> QuestSaveDataList             { get; set; } = new List<QuestSaveElementData>();
    public List<ObjectiveSaveElementData> ObjectiveSaveDataList     { get; set; } = new List<ObjectiveSaveElementData>();
    public List<TaskSaveElementData> TaskSaveDataList               { get; set; } = new List<TaskSaveElementData>();
    public List<InteractionSaveElementData> InteractionSaveDataList { get; set; } = new List<InteractionSaveElementData>();

    public virtual void GetOriginalValues(GameSaveData originalData)
    {
        Id              = originalData.Id;

        PlayerSaveData  = originalData.PlayerSaveData;
    }

    public GameSaveData Clone()
    {
        var data = new GameSaveData();
        
        data.Id             = Id;

        data.PlayerSaveData = PlayerSaveData;

        return data;
    }

    public virtual void Clone(GameSaveElementData elementData)
    {
        elementData.Id              = Id;

        elementData.PlayerSaveData  = PlayerSaveData;
    }
}
